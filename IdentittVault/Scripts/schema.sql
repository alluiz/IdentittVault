CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `User` (
    `Id` varbinary(16) NOT NULL,
    `Email` varchar(767) NOT NULL,
    `HashPassword` text NOT NULL,
    `PrivateKeyCrypt` varbinary(4000) NOT NULL,
    `PublicKeyPlain` varbinary(4000) NOT NULL,
    `PrivateKeyPlainHash` varchar(767) NOT NULL,
    `PublicKeyPlainHash` varchar(767) NOT NULL,
    `Name` varchar(50) NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Account` (
    `Id` varbinary(16) NOT NULL,
    `UserId` varbinary(16) NOT NULL,
    `Username` text NOT NULL,
    `CypherPassword` text NOT NULL,
    `Name` varchar(50) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Account_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_Account_UserId` ON `Account` (`UserId`);

CREATE UNIQUE INDEX `IX_User_Email` ON `User` (`Email`);

CREATE UNIQUE INDEX `IX_User_PrivateKeyPlainHash` ON `User` (`PrivateKeyPlainHash`);

CREATE UNIQUE INDEX `IX_User_PublicKeyPlainHash` ON `User` (`PublicKeyPlainHash`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211107074309_initial', '5.0.11');

COMMIT;

