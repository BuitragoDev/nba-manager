using UnityEngine;
using UnityEngine.UIElements;
using NBAManager.UI.Navigation;
using NBAManager.Database;
using NBAManager.Models;

namespace NBAManager.UI.Screens
{
    public class CreateManagerScreen : BaseScreen
    {
        protected override ScreenId GetScreenId() => ScreenId.CreateManager;

        private TextField _firstName;
        private TextField _lastName;
        private TextField _nationality;
        private TextField _birthDate;
        private Label     _errorLabel;
        private Label     _styleDesc;
        private string    _selectedStyle = "Balanced";

        private readonly string[] _styles = { "Balanced", "Offensive", "Defensive", "Analytics" };
        private readonly string[] _styleNames = { "style-balanced", "style-offensive", "style-defensive", "style-analytics" };
        private readonly string[] _styleDescs =
        {
            "Prioriza el equilibrio entre ataque y defensa. Rotaciones amplias y gestión conservadora del cap.",
            "Máxima anotación y ritmo alto. Prioriza scorers y pace ofensivo sobre la defensa.",
            "La defensa gana campeonatos. Prioriza DRTG, rebote y control del ritmo.",
            "Decisiones basadas en estadísticas avanzadas. Eficiencia, espaciado y valor de contrato."
        };

        protected override void OnInit()
        {
            _firstName   = Q<TextField>("input-firstname");
            _lastName    = Q<TextField>("input-lastname");
            _nationality = Q<TextField>("input-nationality");
            _birthDate   = Q<TextField>("input-birthdate");
            _errorLabel  = Q<Label>("error-label");
            _styleDesc   = Q<Label>("style-description");

            Q<Button>("btn-back").clicked += () =>
                NavigationManager.Instance.ShowScreen(ScreenId.NewGame);

            Q<Button>("btn-confirm").clicked += OnConfirm;

            // Estilo botones
            for (int i = 0; i < _styleNames.Length; i++)
            {
                int index = i;
                Q<Button>(_styleNames[i]).clicked += () => SelectStyle(index);
            }
        }

        private void SelectStyle(int index)
        {
            _selectedStyle = _styles[index];
            _styleDesc.text = _styleDescs[index];

            for (int i = 0; i < _styleNames.Length; i++)
            {
                var btn = Q<Button>(_styleNames[i]);
                if (i == index)
                    btn.AddToClassList("cm-style-btn--active");
                else
                    btn.RemoveFromClassList("cm-style-btn--active");
            }
        }

        private void OnConfirm()
        {
            _errorLabel.text = "";

            string first = _firstName.value.Trim();
            string last  = _lastName.value.Trim();

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                _errorLabel.text = "El nombre y apellido son obligatorios.";
                return;
            }

            var manager = new UserManager
            {
                Id            = 1,
                FirstName     = first,
                LastName      = last,
                Nationality   = _nationality.value.Trim(),
                BirthDate     = _birthDate.value.Trim(),
                FavoriteStyle = _selectedStyle
            };

            DatabaseManager.Current.Connection.InsertOrReplace(manager);
            Debug.Log($"[CreateManager] Manager creado: {first} {last} | {_selectedStyle}");

            NavigationManager.Instance.ShowScreen(ScreenId.Dashboard);
        }
    }
}