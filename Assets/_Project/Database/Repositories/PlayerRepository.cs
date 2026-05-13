using System.Collections.Generic;
using NBAManager.Models;

namespace NBAManager.Database.Repositories
{
    /// <summary>
    /// Único punto de acceso a la tabla Players.
    /// </summary>
    public class PlayerRepository
    {
        private readonly DatabaseManager _db;

        public PlayerRepository(DatabaseManager db)
        {
            _db = db;
        }

        // ── Lectura ───────────────────────────────────────────────────────

        public Player GetById(int id)
        {
            return _db.Connection.Table<Player>()
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Player> GetAll()
        {
            return new List<Player>(_db.Connection.Table<Player>());
        }

        public List<Player> GetByTeam(int teamId)
        {
            return new List<Player>(
                _db.Connection.Table<Player>()
                    .Where(p => p.TeamId == teamId));
        }

        public List<Player> GetFreeAgents()
        {
            return new List<Player>(
                _db.Connection.Table<Player>()
                    .Where(p => p.TeamId == 0));
        }

        public List<Player> GetByPosition(string position)
        {
            return new List<Player>(
                _db.Connection.Table<Player>()
                    .Where(p => p.Position == position));
        }

        public int GetCount()
        {
            return _db.Connection.Table<Player>().Count();
        }

        // ── Escritura ─────────────────────────────────────────────────────

        public void Insert(Player player)
        {
            _db.Connection.Insert(player);
        }

        public void InsertAll(IEnumerable<Player> players)
        {
            _db.Connection.InsertAll(players);
        }

        public void Update(Player player)
        {
            _db.Connection.Update(player);
        }

        public void UpdateFatigue(int playerId, int fatigue)
        {
            _db.Execute(
                "UPDATE Players SET Fatigue = ? WHERE Id = ?",
                fatigue, playerId);
        }

        public void UpdateMorale(int playerId, int morale)
        {
            _db.Execute(
                "UPDATE Players SET Morale = ? WHERE Id = ?",
                morale, playerId);
        }

        public void UpdateInjury(int playerId, int daysInjured, string injuryType)
        {
            _db.Execute(
                "UPDATE Players SET InjuryStatus = ?, InjuryType = ? WHERE Id = ?",
                daysInjured, injuryType, playerId);
        }

        public void AssignToTeam(int playerId, int teamId)
        {
            _db.Execute(
                "UPDATE Players SET TeamId = ? WHERE Id = ?",
                teamId, playerId);
        }

        public void Delete(int id)
        {
            _db.Connection.Delete<Player>(id);
        }
    }
}