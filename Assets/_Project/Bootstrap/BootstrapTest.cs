using UnityEngine;
using NBAManager.SaveLoad;
using NBAManager.Database;

public class BootstrapTest : MonoBehaviour
{
    void Start()
    {
        GameSession.StartNew("Lakers_Test", userTeamId: 1);

        var db = DatabaseManager.Current;

        // Verificar que las tablas existen
        int teamCount = db.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Teams'");
        int playerCount = db.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Players'");
        int saveMetaCount = db.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='SaveMeta'");

        Debug.Log($"[BootstrapTest] Tablas creadas — " +
                  $"SaveMeta: {saveMetaCount} | Teams: {teamCount} | Players: {playerCount}");

        // Insertar equipo de prueba
        db.Execute(@"INSERT INTO Teams
            (Abbreviation, FullName, City, Nickname, Conference, Division, PrestigeLevel)
            VALUES ('LAL', 'Los Angeles Lakers', 'Los Angeles', 'Lakers', 'West', 'Pacific', 5)");

        int insertedTeamId = db.ExecuteScalar<int>("SELECT Id FROM Teams WHERE Abbreviation = 'LAL'");
        Debug.Log($"[BootstrapTest] ✅ Equipo insertado — Id: {insertedTeamId}");

        // Insertar jugador de prueba
        db.Execute(@"INSERT INTO Players
            (FirstName, LastName, Age, Position, TeamId, Overall, Potential)
            VALUES ('LeBron', 'James', 40, 'SF', ?, 91, 91)", insertedTeamId);

        string playerName = db.ExecuteScalar<string>(
            "SELECT FirstName || ' ' || LastName FROM Players WHERE TeamId = ?", insertedTeamId);
        Debug.Log($"[BootstrapTest] ✅ Jugador insertado — {playerName}");

        GameSession.Close();
        Debug.Log("[BootstrapTest] ✅ Paso 3 completado.");
    }
}