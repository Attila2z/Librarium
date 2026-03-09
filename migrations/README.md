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