using NBAManager.Models;

namespace NBAManager.Database.Repositories
{
    /// <summary>
    /// Acceso a SaveMeta — siempre una sola fila (Id = 1).
    /// </summary>
    public class SaveMetaRepository
    {
        private readonly DatabaseManager _db;

        public SaveMetaRepository(DatabaseManager db)
        {
            _db = db;
        }

        public SaveMeta Get()
        {
            return _db.Connection.Table<SaveMeta>()
                .FirstOrDefault(s => s.Id == 1);
        }

        public void Update(SaveMeta meta)
        {
            meta.Id = 1;
            _db.Connection.InsertOrReplace(meta);
        }

        public void AdvanceDay()
        {
            _db.Execute(
                "UPDATE SaveMeta SET CurrentDay = CurrentDay + 1 WHERE Id = 1");
        }

        public void SetInGameDate(string isoDate)
        {
            _db.Execute(
                "UPDATE SaveMeta SET InGameDate = ? WHERE Id = 1", isoDate);
        }

        public void AddPlayTime(int seconds)
        {
            _db.Execute(
                "UPDATE SaveMeta SET PlayTimeSeconds = PlayTimeSeconds + ?, " +
                "LastPlayedRealDate = datetime('now') WHERE Id = 1", seconds);
        }
    }
}