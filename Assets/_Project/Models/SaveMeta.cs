namespace NBAManager.Models
{
    [SQLite.Table("SaveMeta")]
    public class SaveMeta
    {
        [SQLite.PrimaryKey]
        public int    Id                 { get; set; }
        public string SaveName           { get; set; }
        public int    UserTeamId         { get; set; }
        public int    CurrentSeason      { get; set; }
        public int    CurrentDay         { get; set; }
        public string InGameDate         { get; set; }
        public int    PlayTimeSeconds    { get; set; }
        public string LastPlayedRealDate { get; set; }
        public string GameVersion        { get; set; }
    }
}