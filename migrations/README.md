### V002

**Migration file:** `V002__books_need_authors.sql`  
**Purpose:** Adds `Authors` and `BookAuthors` tables and backfills existing books by linking them to a single **Unknown Author** record.

#### API impact
- `GET /api/books` now includes a new `authors` field.
- Existing fields (`bookId`, `title`, `isbn`, `publicationYear`) are unchanged.

#### Deployment notes
1. Apply the database migration first.
2. Deploy the updated API after.
   - The old API will continue to work (it ignores the new tables).
   - The new API expects the new tables to exist and expects legacy books to be backfilled.

#### Decision: “Unknown Author” backfill
We chose the **Unknown Author** approach so legacy data immediately complies with the new requirement: every book has at least one author.

**Benefits**
- Immediately satisfies the “at least one author per book” requirement.
- Prevents author-less books going forward and keeps the database consistent.

**Tradeoffs**
- Introduces placeholder/fake data (“Unknown Author” is not a real author), which can be misleading in UI, reports, and analytics.

**Mitigation (optional)**
- Add a flag such as `Authors.IsPlaceholder` to clearly identify placeholder authors and allow clients/UI to handle them differently.