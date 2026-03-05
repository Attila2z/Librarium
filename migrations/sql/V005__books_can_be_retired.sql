START TRANSACTION;

ALTER TABLE "Books" ADD "IsRetired" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260305123207_V005_BooksCanBeRetired', '10.0.3');

COMMIT;
