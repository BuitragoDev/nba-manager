namespace NBAManager.Models
{
    /// <summary>
    /// Jugador NBA con todos los atributos definidos en el GDD sección 6.
    /// Mapeado directamente a la tabla Players en SQLite.
    /// </summary>
    /// 
    [SQLite.Table("Players")]
    public class Player
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int    Id            { get; set; }
        public string FirstName     { get; set; }
        public string LastName      { get; set; }
        public int    Age           { get; set; }
        public string Nationality  { get; set; } = "USA";
        public string Position     { get; set; } = "SG";
        public int    JerseyNumber  { get; set; }
        public int    TeamId        { get; set; }  // FK → Teams.Id (0 = free agent)
        public int    YearsInLeague { get; set; }

        // ── Técnicos ──────────────────────────────────────────────────────
        public int InsideScoring    { get; set; }
        public int MidRange         { get; set; }
        public int ThreePoint       { get; set; }
        public int FreeThrow        { get; set; }
        public int Passing          { get; set; }
        public int BallHandling     { get; set; }
        public int PostMoves        { get; set; }
        public int OffensiveRebounding { get; set; }
        public int DefensiveRebounding { get; set; }
        public int PerimeterDefense   { get; set; }
        public int InteriorDefense     { get; set; }
        public int Stealing         { get; set; }
        public int Blocking         { get; set; }

        // ── Físicos ───────────────────────────────────────────────────────
        public int Speed            { get; set; }
        public int Strength         { get; set; }
        public int Vertical         { get; set; }
        public int Stamina          { get; set; }
        public int Durability       { get; set; }
        public int HeightCm         { get; set; }
        public int WeightKg         { get; set; }
        public int WingspanCm       { get; set; }

        // ── Mentales ──────────────────────────────────────────────────────
        public int OffensiveIQ      { get; set; }
        public int DefensiveIQ      { get; set; }
        public int Clutch           { get; set; }
        public int Leadership       { get; set; }
        public int WorkEthic        { get; set; }
        public int Ego              { get; set; }
        public int Coachability     { get; set; }
        public int Consistency      { get; set; }

        // ── Tendencias (peso en el motor de simulación) ───────────────────
        public int TendencyShot     { get; set; }  // 0-100
        public int TendencyDrive    { get; set; }
        public int TendencyPass     { get; set; }
        public int TendencyPost     { get; set; }
        public int TendencyThree    { get; set; }
        public int TendencyFoul     { get; set; }
        public int TendencyPace     { get; set; }

        // ── Potencial (oculto al jugador, visible con scouting) ───────────
        public int Overall          { get; set; }  // calculado, no editable
        public int Potential        { get; set; }  // 1-99, oculto
        public int PotentialVisible { get; set; }  // 0 = oculto, 1 = descubierto

        // ── Estados dinámicos ─────────────────────────────────────────────
        public int    Fatigue       { get; set; }  // 0-100 (100 = fresco)
        public int    Morale        { get; set; }  // 0-100
        public int    Form          { get; set; }  // -3 a +3
        public int    InjuryStatus  { get; set; }  // 0 = sano, >0 = días lesionado
        public string InjuryType   { get; set; } = null;  // null si sano
    }
}