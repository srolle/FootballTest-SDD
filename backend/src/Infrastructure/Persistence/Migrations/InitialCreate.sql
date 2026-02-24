CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Matchdays" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Matchdays" PRIMARY KEY,
    "Number" INTEGER NOT NULL,
    "Status" TEXT NOT NULL,
    "StartDate" TEXT NULL,
    "EndDate" TEXT NULL
);

CREATE TABLE "ResultChangeLogs" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ResultChangeLogs" PRIMARY KEY,
    "MatchId" TEXT NOT NULL,
    "ChangeType" TEXT NOT NULL,
    "ChangedAt" TEXT NOT NULL
);

CREATE TABLE "StandingEntries" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_StandingEntries" PRIMARY KEY,
    "MatchdayId" TEXT NOT NULL,
    "TeamId" TEXT NOT NULL,
    "Played" INTEGER NOT NULL,
    "Won" INTEGER NOT NULL,
    "Drawn" INTEGER NOT NULL,
    "Lost" INTEGER NOT NULL,
    "GoalsFor" INTEGER NOT NULL,
    "GoalsAgainst" INTEGER NOT NULL,
    "GoalDifference" INTEGER NOT NULL,
    "Points" INTEGER NOT NULL,
    "Position" INTEGER NOT NULL
);

CREATE TABLE "Teams" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Teams" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL
);

CREATE TABLE "Matches" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Matches" PRIMARY KEY,
    "MatchdayId" TEXT NOT NULL,
    "HomeTeamId" TEXT NOT NULL,
    "AwayTeamId" TEXT NOT NULL,
    "HomeGoals" INTEGER NULL,
    "AwayGoals" INTEGER NULL,
    "Status" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Matches_Matchdays_MatchdayId" FOREIGN KEY ("MatchdayId") REFERENCES "Matchdays" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_Matchdays_Number" ON "Matchdays" ("Number");

CREATE INDEX "IX_Matches_MatchdayId" ON "Matches" ("MatchdayId");

CREATE UNIQUE INDEX "IX_StandingEntries_MatchdayId_TeamId" ON "StandingEntries" ("MatchdayId", "TeamId");

CREATE UNIQUE INDEX "IX_Teams_Name" ON "Teams" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260224180556_InitialCreate', '9.0.13');

COMMIT;

