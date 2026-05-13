using System.Collections.Generic;
using NBAManager.Models;

namespace NBAManager.Database.Repositories
{
    /// <summary>
    /// Único punto de acceso a la tabla Teams.
    /// Ningún sistema externo escribe SQL sobre Teams directamente.
    /// </summary>
    public class TeamRepository
    {
        private readonly DatabaseManager _db;

        public TeamRepository(DatabaseManager db)
        {
            _db = db;
        }

        // ── Lectura ───────────────────────────────────────────────────────

        public Team GetById(int id)
        {
            return _db.Connection.Table<Team>()
                .FirstOrDefault(t => t.Id == id);
        }

        public Team GetByAbbreviation(string abbreviation)
        {
            return _db.Connection.Table<Team>()
                .FirstOrDefault(t => t.Abbreviation == abbreviation);
        }

        public List<Team> GetAll()
        {
            return new List<Team>(_db.Connection.Table<Team>());
        }

        public List<Team> GetByConference(string conference)
        {
            return new List<Team>(
                _db.Connection.Table<Team>()
                    .Where(t => t.Conference == conference));
        }

        public List<Team> GetByDivision(string division)
        {
            return new List<Team>(
                _db.Connection.Table<Team>()
                    .Where(t => t.Division == division));
        }

        public int GetCount()
        {
            return _db.Connection.Table<Team>().Count();
        }

        // ── Escritura ─────────────────────────────────────────────────────

        public void Insert(Team team)
        {
            _db.Connection.Insert(team);
        }

        public void InsertAll(IEnumerable<Team> teams)
        {
            _db.Connection.InsertAll(teams);
        }

        public void Update(Team team)
        {
            _db.Connection.Update(team);
        }

        public void Delete(int id)
        {
            _db.Connection.Delete<Team>(id);
        }
    }
}