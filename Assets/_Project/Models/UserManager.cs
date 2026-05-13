namespace NBAManager.Models
{
    [SQLite.Table("UserManager")]
    public class UserManager
    {
        [SQLite.PrimaryKey]
        public int    Id            { get; set; } = 1;
        public string FirstName     { get; set; }
        public string LastName      { get; set; }
        public string Nationality   { get; set; } = "USA";
        public string BirthDate     { get; set; } = "1980-01-01";
        public string FavoriteStyle { get; set; } = "Balanced";
    }
}