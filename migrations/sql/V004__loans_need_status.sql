START TRANSACTION;

ALTER TABLE "Loans" ADD "Status" integer NOT NULL DEFAULT 0;

UPDATE "Loans" SET "Status" = CASE WHEN "ReturnDate" IS NULL THEN 0 ELSE 1 END;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260305003855_V004_LoansNeedStatus', '10.0.3');

COMMIT;
