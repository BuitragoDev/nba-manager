using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using NBAManager.UI.Navigation;
using NBAManager.SaveLoad;
using NBAManager.Database;
using NBAManager.Database.Repositories;
using NBAManager.Models;

namespace NBAManager.UI.Screens
{
    public class NewGameScreen : BaseScreen
    {
        protected override ScreenId GetScreenId() => ScreenId.NewGame;

        private List<Team> _allTeams = new();
        private Team _selectedTeam;

        // Referencias UI
        private VisualElement _teamListContainer;
        private VisualElement _colorSwatch;
        private Label _teamName;
        private Label _teamCity;
        private Label _statConference;
        private Label _statDivision;
        private Label _statPrestige;
        private Label _statPhilosophy;
        private Button _btnStart;

        protected override void OnInit()
        {
            _teamListContainer = Q<VisualElement>("team-list-container");
            _colorSwatch       = Q<VisualElement>("color-swatch");
            _teamName          = Q<Label>("team-name");
            _teamCity          = Q<Label>("team-city");
            _statConference    = Q<Label>("stat-conference");
            _statDivision      = Q<Label>("stat-division");
            _statPrestige      = Q<Label>("stat-prestige");
            _statPhilosophy    = Q<Label>("stat-philosophy");
            _btnStart          = Q<Button>("btn-start");

            Q<Button>("btn-back").clicked += () =>
                NavigationManager.Instance.ShowScreen(ScreenId.MainMenu);

            _btnStart.clicked += OnStartGame;
            _btnStart.SetEnabled(false);
        }

        protected override void OnShow()
        {
            LoadTeams();
        }

        private void LoadTeams()
        {
            _teamListContainer.Clear();
            _allTeams.Clear();

            if (!DatabaseManager.HasActiveSession)
                GameSession.StartNew("__preview__", 0);

            var repo = new TeamRepository(DatabaseManager.Current);
            _allTeams = repo.GetAll();

            BuildConferenceSection("EAST", _allTeams.FindAll(t => t.Conference == "East"));
            BuildConferenceSection("WEST", _allTeams.FindAll(t => t.Conference == "West"));
        }

        private void BuildConferenceSection(string title, List<Team> teams)
        {
            var header = new Label(title);
            header.AddToClassList("ng-conference-header");
            _teamListContainer.Add(header);

            foreach (var team in teams)
            {
                var row = new Button(() => SelectTeam(team));
                row.AddToClassList("ng-team-row");

                var dot = new VisualElement();
                dot.AddToClassList("ng-team-dot");
                dot.style.backgroundColor = HexToColor(team.PrimaryColor);

                var abbr = new Label(team.Abbreviation);
                abbr.AddToClassList("ng-team-abbr");

                var name = new Label(team.Nickname);
                name.AddToClassList("ng-team-name");

                row.Add(dot);
                row.Add(abbr);
                row.Add(name);
                _teamListContainer.Add(row);
            }
        }

        private void SelectTeam(Team team)
        {
            _selectedTeam = team;
            _colorSwatch.style.backgroundColor = HexToColor(team.PrimaryColor);
            _teamName.text        = team.Nickname.ToUpper();
            _teamCity.text        = team.City;
            _statConference.text  = team.Conference;
            _statDivision.text    = team.Division;
            _statPrestige.text    = new string('★', team.PrestigeLevel) +
                                    new string('☆', 5 - team.PrestigeLevel);
            _statPhilosophy.text  = team.AIPhilosophy;
            _btnStart.SetEnabled(true);
        }

        private void OnStartGame()
        {
            if (_selectedTeam == null) return;
            GameSession.Close();
            GameSession.StartNew($"{_selectedTeam.Abbreviation}_2025", _selectedTeam.Id);
            NavigationManager.Instance.ShowScreen(ScreenId.CreateManager);
        }

        private static Color HexToColor(string hex)
        {
            hex = hex.TrimStart('#');
            if (hex.Length != 6) return Color.white;
            float r = System.Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
            float g = System.Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
            float b = System.Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
            return new Color(r, g, b);
        }
    }
}