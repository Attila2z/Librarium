START TRANSACTION;

ALTER TABLE "Books" ADD COLUMN "IsbnHistorical" text;

UPDATE "Books" SET "IsbnHistorical" = "Isbn" WHERE "Isbn" IS NOT NULL;

ALTER TABLE "Books" ALTER COLUMN "Isbn" DROP NOT NULL;

UPDATE "Books" SET "Isbn" = NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260308160351_V006_IsbnColumnTypeChange', '10.0.3');

COMMIT;
