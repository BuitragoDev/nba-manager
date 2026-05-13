using System.Collections.Generic;
using NBAManager.Database;
using NBAManager.Database.Repositories;
using NBAManager.Models;
using UnityEngine;

namespace NBAManager.SaveLoad
{
    /// <summary>
    /// Puebla una nueva partida con los datos reales de la NBA.
    /// Se ejecuta una sola vez al crear una nueva partida, después de
    /// aplicar las migraciones.
    /// </summary>
    public static class LeagueSeeder
    {
        public static void Seed(DatabaseManager db)
        {
            var teamRepo = new TeamRepository(db);

            if (teamRepo.GetCount() > 0)
            {
                Debug.Log("[LeagueSeeder] La liga ya está poblada, omitiendo seed.");
                return;
            }

            db.RunInTransaction(() =>
            {
                teamRepo.InsertAll(GetAllTeams());
            });

            Debug.Log($"[LeagueSeeder] ✅ {teamRepo.GetCount()} equipos insertados.");
        }

        private static List<Team> GetAllTeams()
        {
            return new List<Team>
            {
                // ── EASTERN CONFERENCE ────────────────────────────────────

                // Atlantic
                new Team { Abbreviation="BOS", FullName="Boston Celtics",
                    City="Boston", Nickname="Celtics",
                    Conference="East", Division="Atlantic",
                    PrestigeLevel=5, ArenaCapacity=19156,
                    PrimaryColor="#007A33", SecondaryColor="#BA9653",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="BKN", FullName="Brooklyn Nets",
                    City="Brooklyn", Nickname="Nets",
                    Conference="East", Division="Atlantic",
                    PrestigeLevel=3, ArenaCapacity=17732,
                    PrimaryColor="#000000", SecondaryColor="#FFFFFF",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="NYK", FullName="New York Knicks",
                    City="New York", Nickname="Knicks",
                    Conference="East", Division="Atlantic",
                    PrestigeLevel=4, ArenaCapacity=19812,
                    PrimaryColor="#006BB6", SecondaryColor="#F58426",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="PHI", FullName="Philadelphia 76ers",
                    City="Philadelphia", Nickname="76ers",
                    Conference="East", Division="Atlantic",
                    PrestigeLevel=4, ArenaCapacity=20478,
                    PrimaryColor="#006BB6", SecondaryColor="#ED174C",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="TOR", FullName="Toronto Raptors",
                    City="Toronto", Nickname="Raptors",
                    Conference="East", Division="Atlantic",
                    PrestigeLevel=3, ArenaCapacity=19800,
                    PrimaryColor="#CE1141", SecondaryColor="#000000",
                    AIPhilosophy="Rebuild" },

                // Southeast
                new Team { Abbreviation="CHI", FullName="Chicago Bulls",
                    City="Chicago", Nickname="Bulls",
                    Conference="East", Division="Central",
                    PrestigeLevel=4, ArenaCapacity=20917,
                    PrimaryColor="#CE1141", SecondaryColor="#000000",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="CLE", FullName="Cleveland Cavaliers",
                    City="Cleveland", Nickname="Cavaliers",
                    Conference="East", Division="Central",
                    PrestigeLevel=3, ArenaCapacity=19432,
                    PrimaryColor="#860038", SecondaryColor="#FDBB30",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="DET", FullName="Detroit Pistons",
                    City="Detroit", Nickname="Pistons",
                    Conference="East", Division="Central",
                    PrestigeLevel=2, ArenaCapacity=20491,
                    PrimaryColor="#C8102E", SecondaryColor="#006BB6",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="IND", FullName="Indiana Pacers",
                    City="Indianapolis", Nickname="Pacers",
                    Conference="East", Division="Central",
                    PrestigeLevel=3, ArenaCapacity=17923,
                    PrimaryColor="#002D62", SecondaryColor="#FDBB30",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="MIL", FullName="Milwaukee Bucks",
                    City="Milwaukee", Nickname="Bucks",
                    Conference="East", Division="Central",
                    PrestigeLevel=4, ArenaCapacity=17341,
                    PrimaryColor="#00471B", SecondaryColor="#EEE1C6",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="ATL", FullName="Atlanta Hawks",
                    City="Atlanta", Nickname="Hawks",
                    Conference="East", Division="Southeast",
                    PrestigeLevel=3, ArenaCapacity=16600,
                    PrimaryColor="#E03A3E", SecondaryColor="#C1D32F",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="CHA", FullName="Charlotte Hornets",
                    City="Charlotte", Nickname="Hornets",
                    Conference="East", Division="Southeast",
                    PrestigeLevel=2, ArenaCapacity=19077,
                    PrimaryColor="#1D1160", SecondaryColor="#00788C",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="MIA", FullName="Miami Heat",
                    City="Miami", Nickname="Heat",
                    Conference="East", Division="Southeast",
                    PrestigeLevel=5, ArenaCapacity=19600,
                    PrimaryColor="#98002E", SecondaryColor="#F9A01B",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="ORL", FullName="Orlando Magic",
                    City="Orlando", Nickname="Magic",
                    Conference="East", Division="Southeast",
                    PrestigeLevel=3, ArenaCapacity=18846,
                    PrimaryColor="#0077C0", SecondaryColor="#C4CED4",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="WAS", FullName="Washington Wizards",
                    City="Washington", Nickname="Wizards",
                    Conference="East", Division="Southeast",
                    PrestigeLevel=2, ArenaCapacity=20356,
                    PrimaryColor="#002B5C", SecondaryColor="#E31837",
                    AIPhilosophy="Tanking" },

                // ── WESTERN CONFERENCE ────────────────────────────────────

                // Northwest
                new Team { Abbreviation="DEN", FullName="Denver Nuggets",
                    City="Denver", Nickname="Nuggets",
                    Conference="West", Division="Northwest",
                    PrestigeLevel=5, ArenaCapacity=19520,
                    PrimaryColor="#0E2240", SecondaryColor="#FEC524",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="MIN", FullName="Minnesota Timberwolves",
                    City="Minneapolis", Nickname="Timberwolves",
                    Conference="West", Division="Northwest",
                    PrestigeLevel=3, ArenaCapacity=18978,
                    PrimaryColor="#0C2340", SecondaryColor="#236192",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="OKC", FullName="Oklahoma City Thunder",
                    City="Oklahoma City", Nickname="Thunder",
                    Conference="West", Division="Northwest",
                    PrestigeLevel=4, ArenaCapacity=18203,
                    PrimaryColor="#007AC1", SecondaryColor="#EF3B24",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="POR", FullName="Portland Trail Blazers",
                    City="Portland", Nickname="Trail Blazers",
                    Conference="West", Division="Northwest",
                    PrestigeLevel=3, ArenaCapacity=19393,
                    PrimaryColor="#E03A3E", SecondaryColor="#000000",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="UTA", FullName="Utah Jazz",
                    City="Salt Lake City", Nickname="Jazz",
                    Conference="West", Division="Northwest",
                    PrestigeLevel=3, ArenaCapacity=18306,
                    PrimaryColor="#002B5C", SecondaryColor="#00471B",
                    AIPhilosophy="Rebuild" },

                // Pacific
                new Team { Abbreviation="GSW", FullName="Golden State Warriors",
                    City="San Francisco", Nickname="Warriors",
                    Conference="West", Division="Pacific",
                    PrestigeLevel=5, ArenaCapacity=18064,
                    PrimaryColor="#1D428A", SecondaryColor="#FFC72C",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="LAC", FullName="Los Angeles Clippers",
                    City="Los Angeles", Nickname="Clippers",
                    Conference="West", Division="Pacific",
                    PrestigeLevel=4, ArenaCapacity=19068,
                    PrimaryColor="#C8102E", SecondaryColor="#1D428A",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="LAL", FullName="Los Angeles Lakers",
                    City="Los Angeles", Nickname="Lakers",
                    Conference="West", Division="Pacific",
                    PrestigeLevel=5, ArenaCapacity=18997,
                    PrimaryColor="#552583", SecondaryColor="#FDB927",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="PHX", FullName="Phoenix Suns",
                    City="Phoenix", Nickname="Suns",
                    Conference="West", Division="Pacific",
                    PrestigeLevel=4, ArenaCapacity=18055,
                    PrimaryColor="#1D1160", SecondaryColor="#E56020",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="SAC", FullName="Sacramento Kings",
                    City="Sacramento", Nickname="Kings",
                    Conference="West", Division="Pacific",
                    PrestigeLevel=3, ArenaCapacity=17583,
                    PrimaryColor="#5A2D81", SecondaryColor="#63727A",
                    AIPhilosophy="Contender" },

                // Southwest
                new Team { Abbreviation="DAL", FullName="Dallas Mavericks",
                    City="Dallas", Nickname="Mavericks",
                    Conference="West", Division="Southwest",
                    PrestigeLevel=4, ArenaCapacity=19200,
                    PrimaryColor="#00538C", SecondaryColor="#002F5F",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="HOU", FullName="Houston Rockets",
                    City="Houston", Nickname="Rockets",
                    Conference="West", Division="Southwest",
                    PrestigeLevel=3, ArenaCapacity=18055,
                    PrimaryColor="#CE1141", SecondaryColor="#000000",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="MEM", FullName="Memphis Grizzlies",
                    City="Memphis", Nickname="Grizzlies",
                    Conference="West", Division="Southwest",
                    PrestigeLevel=3, ArenaCapacity=17794,
                    PrimaryColor="#5D76A9", SecondaryColor="#12173F",
                    AIPhilosophy="Contender" },

                new Team { Abbreviation="NOP", FullName="New Orleans Pelicans",
                    City="New Orleans", Nickname="Pelicans",
                    Conference="West", Division="Southwest",
                    PrestigeLevel=3, ArenaCapacity=16867,
                    PrimaryColor="#0C2340", SecondaryColor="#C8102E",
                    AIPhilosophy="Rebuild" },

                new Team { Abbreviation="SAS", FullName="San Antonio Spurs",
                    City="San Antonio", Nickname="Spurs",
                    Conference="West", Division="Southwest",
                    PrestigeLevel=4, ArenaCapacity=18418,
                    PrimaryColor="#C4CED4", SecondaryColor="#000000",
                    AIPhilosophy="Rebuild" },
            };
        }
    }
}