using System;
using System.IO;
using UnityEngine;
using NBAManager.Database;

namespace NBAManager.SaveLoad
{
    /// <summary>
    /// Controla el ciclo de vida de una partida:
    /// crear nueva, cargar existente, cerrar.
    /// Es el único sistema autorizado a llamar a DatabaseManager.Open().
    /// </summary>
    public static class GameSession
    {
        // ─── Rutas ────────────────────────────────────────────────────────
        public static string SavesDirectory
        {
            get
            {
                string path = Path.Combine(Application.persistentDataPath, "Saves");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        // ─── Nueva partida ────────────────────────────────────────────────
        public static void StartNew(string saveName, int userTeamId)
        {
            string fileName = SanitizeFileName(saveName) + ".db";
            string dbPath   = Path.Combine(SavesDirectory, fileName);

            if (File.Exists(dbPath))
            {
                string stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                fileName = $"{SanitizeFileName(saveName)}_{stamp}.db";
                dbPath   = Path.Combine(SavesDirectory, fileName);
            }

            var db = DatabaseManager.Open(dbPath);
            DatabaseMigrator.ApplyMigrations(db);
            WriteSaveMeta(db, saveName, userTeamId);
            LeagueSeeder.Seed(db);              // ← añadir esta línea

            Debug.Log($"[GameSession] Nueva partida creada: {dbPath}");
        }

        // ─── Cargar partida ───────────────────────────────────────────────
        public static void Load(string dbPath)
        {
            if (!File.Exists(dbPath))
                throw new FileNotFoundException($"[GameSession] No se encontró la partida: {dbPath}");

            var db = DatabaseManager.Open(dbPath);
            DatabaseMigrator.ApplyMigrations(db); // aplica migraciones pendientes al cargar
            Debug.Log($"[GameSession] Partida cargada: {dbPath}");
        }

        // ─── Cerrar partida ───────────────────────────────────────────────
        public static void Close()
        {
            if (DatabaseManager.HasActiveSession)
                DatabaseManager.Current.Dispose();

            Debug.Log("[GameSession] Sesión cerrada.");
        }

        // ─── Listar partidas guardadas ─────────────────────────────────────
        public static SaveMetaInfo[] GetAllSaves()
        {
            string[] files = Directory.GetFiles(SavesDirectory, "*.db");
            var results = new System.Collections.Generic.List<SaveMetaInfo>();

            foreach (string file in files)
            {
                try
                {
                    // Leer SaveMeta sin abrir sesión completa
                    var tempDb = new SQLite.SQLiteConnection(file,
                        SQLite.SQLiteOpenFlags.ReadOnly);
                    var row = tempDb.ExecuteScalar<string>(
                        "SELECT SaveName || '|' || UserTeamId || '|' || " +
                        "CurrentSeason || '|' || InGameDate || '|' || " +
                        "LastPlayedRealDate FROM SaveMeta WHERE Id = 1");
                    tempDb.Close();

                    if (row != null)
                    {
                        var parts = row.Split('|');
                        results.Add(new SaveMetaInfo
                        {
                            FilePath         = file,
                            SaveName         = parts[0],
                            UserTeamId       = int.Parse(parts[1]),
                            CurrentSeason    = int.Parse(parts[2]),
                            InGameDate       = parts[3],
                            LastPlayedDate   = parts[4]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[GameSession] No se pudo leer {file}: {ex.Message}");
                }
            }

            return results.ToArray();
        }

        // ─── Helpers ──────────────────────────────────────────────────────
        private static void WriteSaveMeta(DatabaseManager db, string saveName, int userTeamId)
        {
            db.Execute(@"
                INSERT OR REPLACE INTO SaveMeta
                    (Id, SaveName, UserTeamId, CurrentSeason, CurrentDay,
                     InGameDate, PlayTimeSeconds, LastPlayedRealDate, GameVersion)
                VALUES (1, ?, ?, 2025, 1, '2025-10-01', 0, ?, '0.1.0')",
                saveName,
                userTeamId,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }
    }

    // ─── DTO de partida guardada ───────────────────────────────────────────
    public struct SaveMetaInfo
    {
        public string FilePath;
        public string SaveName;
        public int    UserTeamId;
        public int    CurrentSeason;
        public string InGameDate;
        public string LastPlayedDate;
    }
}