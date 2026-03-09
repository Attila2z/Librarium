## V001 - Initial Schema

The initial schema establishes the foundational tables: Books, Members, and Loans required to support a library management platform.

**Type of change:** Additive (non-breaking)

**API impact:**
Four new endpoints are introduced: GET /api/books, GET /api/members, POST /api/loans, and GET /api/loans/{memberId}. No existing API contract exists to break.

**Deployment notes:**
Initial schema migration applied when bootstrapping teh system. Both code and schema deploy simultaneosly.

**Decisions and tradeoffs:**
The schema enforces critical constraints at teh database level: unique constraints on ISBN and Email, and ON DELETE RESTRICT for loans to preserve audit history. This trades flexibility for confidence — books and members with loans cannot be deleted, preventing accidental data loss. Validation at the database boundary ensures these garantees apply regardless of application bugs or future code changes.


## V002 — Books Need Authors

**Type of change:** Additive (potentially breaking)

**API impact:**
`GET /api/books` now includes a new `authors` array in each response object. 
The existing fields (`bookId`, `title`, `isbn`, `publicationYear`) are 
unchanged. Adding a new field to a JSON response is generally considered 
a non-breaking change — well-written clients ignore unknown fields — so we 
chose backwards compatibility over introducing a v2 endpoint. The cost of 
maintaining two API versions indefinitely was not justified here.

**Deployment notes:**
The migration must be applied before the new API code is deployed. During 
the window where the old code is still running against the new schema, 
the old `GET /api/books` ignores the new `Author` and `AuthorBook` tables 
entirely, so nothing breaks. The new code depends on those tables existing, 
which is why the migration must come first.

**Decisions and tradeoffs:**
The main challenge was deciding what to do with books that already exist in 
the database and have no author. We considered making the author relationship 
optional (nullable foreign key), which would be the simplest solution, but 
that would mean the "books must have authors" requirement is never actually 
enforced at the database level — it would rely entirely on application logic. 
Instead we chose to backfill: we insert a single placeholder "Unknown Author" 
record and link all existing authorless books to it. This keeps the constraint 
meaningful and the data consistent from the moment the migration runs. The 
tradeoff is that "Unknown Author" is not real data, which could be misleading 
in a UI or skew author statistics. A future improvement would be to add an 
`IsPlaceholder` boolean to the `Authors` table so clients can filter or 
display these records differently.


## V003 — Member Profile Expanding

**Type of change:** Requires coordination

**API impact:**
`GET /api/members` does not currently return `PhoneNumber` — the field exists 
in the database but is not yet exposed in the response. This was a deliberate 
choice: since existing members have no phone number (the column is nullable), 
returning it immediately would expose `null` values to clients that don't 
expect them. A future API update can add `phoneNumber` to the response once 
data is populated. No endpoint versioning was needed at this stage.

**Deployment notes:**
The migration must be applied before the new code is deployed. During the 
window where the old application runs against the new schema, the old code 
simply ignores the new `PhoneNumber` column — nothing breaks. The new 
registration form enforces phone number at the application level, but the 
database column is nullable to handle the existing rows that were created 
before this requirement existed.

**Decisions and tradeoffs:**
Requirement 2 asked for two changes: enforce email uniqueness and add a 
mandatory phone number. Email uniqueness was already enforced from V001 
(we added the unique index preemptively), so no schema change was needed 
there — only a note that it was already handled. The harder problem was 
phone number. The requirement says it is mandatory on the new registration 
form, but the database already contains member rows with no phone number. 
Adding a `NOT NULL` column to a table with existing data would fail 
immediately unless we supply a default value, and there is no sensible 
default for a phone number. We chose to add the column as nullable at the 
database level and enforce the "mandatory" constraint in the application 
layer for new registrations only. This is a deliberate tradeoff: the 
database is slightly more permissive than the business rule, but it avoids 
polluting existing records with fake placeholder data.


## V004 - Loans Need Status

The loan workflow becomes more explicit by introducing a Status column to Loans. Previosly, loan state was inferred from ReturnDate; now Status (Active, Returned, Overdue, Lost) explicitly tracks state.

**Type of change:** Additive (potentially breaking)

**API impact:**
GET /api/loans/{memberId} includes a new status field alongside bookTitle, loanDate, and returnDate. The old fields are preserved for backward compatibility. The shift from implicit to explicit state could be breaking if clients relied on returnDate logic.

**Deployment notes:**
The migration must be applied before the new API code deploys. During transition, the old code continues inferring state from ReturnDate — nothing breaks.

**Decisions and tradeoffs:**
Existing loans were populated automaticaly: null ReturnDate becomes Active, otherwise Returned. This derives historical state from existing data without manual backfill. The tradeoff is that Overdue and Lost cannot be distinguished for historical loans. Backwards compatibility in the API response delays the breaking change until a future version.


## V005 - Books Can Be Retired

An IsRetired boolean column is added to Books to suport marking books as retired from the catalogue while preserving loan history for auditing.

**Type of change:** Additive (non-breaking)

**API impact:**
GET /api/books response structure remains unchanged. Retired books (IsRetired = true) are filtered out, so existing clients receive a shorter list without format changes.

**Deployment notes:**
The migration must be applied before the new API code deploys. During transition, the old code returns all books; the new code filters retired ones. No data loss occurs.

**Decisions and tradeoffs:**
A developer proposed using an `IsDeleted` flag with a WHERE clause filter. This approach was reviewed and refined: the column was renamed to `IsRetired` for semantic clarity (books are retired, not deleted), and filtering occurs at the API layer to prevent null book references in loan responses. IsRetired was implemented as a boolean flag rather than a status field for simplicity — the requierment asked only for retired or in-circulation states. Loan history is preserved automaticaly via ON DELETE RESTRICT. The tradeoff is that circulation counts require checking IsRetired in the application rather than the database.


## V006 - ISBN Column Type Change

The ISBN column type is converted from integr to varchar to properly store formatted ISBNs. The existing integer data is truncated and irrecoverable.

**Type of change:** Requires coordination

**API impact:**
GET /api/books continues to return an isbn field, but all existing books return isbn: null. This is a breaking change requiring client notification before production deployment. New books will have valid ISBNs after migration.

**Deployment notes:**
The migration must be applied before the new API code deploys. During transition, the old code queries Isbn without knowing it returns null. If ISBN is used for business logic, this is a visible breaking change that must be communicated.

**Decisions and tradeoffs:**
Three approaches were considerd: drop the column (losing audit trail), preserve old data separately, or add placeholder ISBNs. The second option was selected: create IsbnHistorical to preserve the corrupted values for auditing while allowing new data to be corect. The tradeoff is two ISBN-related columns and temporary null values in responses.