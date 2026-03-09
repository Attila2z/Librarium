START TRANSACTION;

ALTER TABLE "Members" ADD "PhoneNumber" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260304143757_V003_AddMemberPhoneNumber', '10.0.3');

COMMIT;