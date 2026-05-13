namespace NBAManager.Models
{
    /// <summary>
    /// Representa una franquicia NBA.
    /// Mapeado directamente a la tabla Teams en SQLite.
    /// </summary>
    /// 
    [SQLite.Table("Teams")]
    public class Team
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int    Id             { get; set; }
        public string Abbreviation   { get; set; }  // LAL, BOS, GSW…
        public string FullName       { get; set; }  // Los Angeles Lakers
        public string City           { get; set; }  // Los Angeles
        public string Nickname       { get; set; }  // Lakers
        public string Conference     { get; set; }  // East / West
        public string Division       { get; set; }  // Pacific, Atlantic…
        public int    PrestigeLevel  { get; set; }  // 1-5, afecta free agency
        public int    ArenaCapacity  { get; set; }
        public string PrimaryColor   { get; set; }  // hex #552583
        public string SecondaryColor { get; set; }  // hex #FDB927
        // Filosofía IA
        public string AIPhilosophy   { get; set; }  // Rebuild / Contender / Tanking
    }
}