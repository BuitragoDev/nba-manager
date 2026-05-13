using System;
using UnityEngine;

namespace NBAManager.Database
{
    /// <summary>
    /// Gestiona la creación inicial del esquema y migraciones futuras.
    /// Se ejecuta siempre al abrir una DB, antes de cualquier operación.
    /// </summary>
    public static class DatabaseMigrator
    {
        public static void ApplyMigrations(DatabaseManager db)
        {
            EnsureMetaTable(db);
            int currentVersion = GetSchemaVersion(db);

            Debug.Log($"[DatabaseMigrator] Versión DB: {currentVersion}, " +
                      $"Versión requerida: {SchemaVersion.Current}");

            if (currentVersion < 1) ApplyMigration_001(db);

            // Futuras migraciones:
            // if (currentVersion < 2) ApplyMigration_002(db);
            // if (currentVersion < 3) ApplyMigration_003(db);
        }

        // ─── Tabla de control de versión ──────────────────────────────────
        private static void EnsureMetaTable(DatabaseManager db)
        {
            db.Execute(@"
                CREATE TABLE IF NOT EXISTS SchemaInfo (
                    Key     TEXT PRIMARY KEY,
                    Value   TEXT NOT NULL
                )");
        }

        private static int GetSchemaVersion(DatabaseManager db)
        {
            try
            {
                string val = db.ExecuteScalar<string>(
                    "SELECT Value FROM SchemaInfo WHERE Key = 'version'");
                return int.TryParse(val, out int v) ? v : 0;
            }
            catch { return 0; }
        }

        private static void SetSchemaVersion(DatabaseManager db, int version)
        {
            db.Execute(@"
                INSERT OR REPLACE INTO SchemaInfo (Key, Value)
                VALUES ('version', ?)", version.ToString());
        }

        // ─── Migración 001 — Esquema base ─────────────────────────────────
        private static void ApplyMigration_001(DatabaseManager db)
        {
            Debug.Log("[DatabaseMigrator] Aplicando migración 001: esquema base...");

            db.RunInTransaction(() =>
            {
                // ── SaveMeta ──────────────────────────────────────────────────
                db.Execute(@"
                    CREATE TABLE IF NOT EXISTS SaveMeta (
                        Id                  INTEGER PRIMARY KEY CHECK (Id = 1),
                        SaveName            TEXT    NOT NULL,
                        UserTeamId          INTEGER NOT NULL DEFAULT 0,
                        CurrentSeason       INTEGER NOT NULL DEFAULT 2025,
                        CurrentDay          INTEGER NOT NULL DEFAULT 1,
                        InGameDate          TEXT    NOT NULL DEFAULT '2025-10-01',
                        PlayTimeSeconds     INTEGER NOT NULL DEFAULT 0,
                        LastPlayedRealDate  TEXT    NOT NULL,
                        GameVersion         TEXT    NOT NULL DEFAULT '0.1.0'
                    )");

                // ── Teams ─────────────────────────────────────────────────────
                db.Execute(@"
                    CREATE TABLE IF NOT EXISTS Teams (
                        Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                        Abbreviation    TEXT    NOT NULL UNIQUE,
                        FullName        TEXT    NOT NULL,
                        City            TEXT    NOT NULL,
                        Nickname        TEXT    NOT NULL,
                        Conference      TEXT    NOT NULL,
                        Division        TEXT    NOT NULL,
                        PrestigeLevel   INTEGER NOT NULL DEFAULT 3,
                        ArenaCapacity   INTEGER NOT NULL DEFAULT 18000,
                        PrimaryColor    TEXT    NOT NULL DEFAULT '#000000',
                        SecondaryColor  TEXT    NOT NULL DEFAULT '#FFFFFF',
                        AIPhilosophy    TEXT    NOT NULL DEFAULT 'Contender'
                    )");

                // ── Players ───────────────────────────────────────────────────
                db.Execute(@"
                    CREATE TABLE IF NOT EXISTS Players (
                        Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName           TEXT    NOT NULL,
                        LastName            TEXT    NOT NULL,
                        Age                 INTEGER NOT NULL DEFAULT 22,
                        Position            TEXT    NOT NULL DEFAULT 'SG',
                        Nationality         TEXT    NOT NULL DEFAULT 'USA',
                        JerseyNumber        INTEGER NOT NULL DEFAULT 0,
                        TeamId              INTEGER NOT NULL DEFAULT 0,
                        YearsInLeague       INTEGER NOT NULL DEFAULT 0,
                        -- Técnicos
                        InsideScoring       INTEGER NOT NULL DEFAULT 50,
                        MidRange            INTEGER NOT NULL DEFAULT 50,
                        ThreePoint          INTEGER NOT NULL DEFAULT 50,
                        FreeThrow           INTEGER NOT NULL DEFAULT 50,
                        Passing             INTEGER NOT NULL DEFAULT 50,
                        BallHandling        INTEGER NOT NULL DEFAULT 50,
                        PostMoves           INTEGER NOT NULL DEFAULT 50,
                        OffensiveRebounding INTEGER NOT NULL DEFAULT 50,
                        DefensiveRebounding INTEGER NOT NULL DEFAULT 50,
                        PerimeterDefense    INTEGER NOT NULL DEFAULT 50,
                        InteriorDefense     INTEGER NOT NULL DEFAULT 50,
                        Stealing            INTEGER NOT NULL DEFAULT 50,
                        Blocking            INTEGER NOT NULL DEFAULT 50,
                        -- Físicos
                        Speed               INTEGER NOT NULL DEFAULT 50,
                        Strength            INTEGER NOT NULL DEFAULT 50,
                        Vertical            INTEGER NOT NULL DEFAULT 50,
                        Stamina             INTEGER NOT NULL DEFAULT 50,
                        Durability          INTEGER NOT NULL DEFAULT 50,
                        HeightCm            INTEGER NOT NULL DEFAULT 198,
                        WeightKg            INTEGER NOT NULL DEFAULT 95,
                        WingspanCm          INTEGER NOT NULL DEFAULT 205,
                        -- Mentales
                        OffensiveIQ         INTEGER NOT NULL DEFAULT 50,
                        DefensiveIQ         INTEGER NOT NULL DEFAULT 50,
                        Clutch              INTEGER NOT NULL DEFAULT 50,
                        Leadership          INTEGER NOT NULL DEFAULT 50,
                        WorkEthic           INTEGER NOT NULL DEFAULT 50,
                        Ego                 INTEGER NOT NULL DEFAULT 50,
                        Coachability        INTEGER NOT NULL DEFAULT 50,
                        Consistency         INTEGER NOT NULL DEFAULT 50,
                        -- Tendencias
                        TendencyShot        INTEGER NOT NULL DEFAULT 50,
                        TendencyDrive       INTEGER NOT NULL DEFAULT 50,
                        TendencyPass        INTEGER NOT NULL DEFAULT 50,
                        TendencyPost        INTEGER NOT NULL DEFAULT 50,
                        TendencyThree       INTEGER NOT NULL DEFAULT 50,
                        TendencyFoul        INTEGER NOT NULL DEFAULT 50,
                        TendencyPace        INTEGER NOT NULL DEFAULT 50,
                        -- Potencial
                        Overall             INTEGER NOT NULL DEFAULT 50,
                        Potential           INTEGER NOT NULL DEFAULT 50,
                        PotentialVisible    INTEGER NOT NULL DEFAULT 0,
                        -- Estados dinámicos
                        Fatigue             INTEGER NOT NULL DEFAULT 100,
                        Morale              INTEGER NOT NULL DEFAULT 75,
                        Form                INTEGER NOT NULL DEFAULT 0,
                        InjuryStatus        INTEGER NOT NULL DEFAULT 0,
                        InjuryType          TEXT,
                        -- FK
                        FOREIGN KEY (TeamId) REFERENCES Teams(Id)
                    )");

                // ── UserManager ───────────────────────────────────────────
                db.Execute(@"
                    CREATE TABLE IF NOT EXISTS UserManager (
                        Id              INTEGER PRIMARY KEY CHECK (Id = 1),
                        FirstName       TEXT    NOT NULL,
                        LastName        TEXT    NOT NULL,
                        Nationality     TEXT    NOT NULL DEFAULT 'USA',
                        BirthDate       TEXT    NOT NULL DEFAULT '1980-01-01',
                        FavoriteStyle   TEXT    NOT NULL DEFAULT 'Balanced'
                    )");

                SetSchemaVersion(db, 1);
            });

            Debug.Log("[DatabaseMigrator] Migración 001 completada.");
        }
    }
}