using UnityEngine;
using UnityEngine.UIElements;
using NBAManager.UI.Navigation;
using NBAManager.SaveLoad;

namespace NBAManager.UI.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        protected override ScreenId GetScreenId() => ScreenId.MainMenu;

        protected override void OnInit()
        {
            Q<Button>("btn-new-game").clicked += () =>
                NavigationManager.Instance.ShowScreen(ScreenId.NewGame);

            Q<Button>("btn-load-game").clicked += OnLoadGame;
            Q<Button>("btn-quit").clicked       += OnQuit;
        }

        private void OnLoadGame()
        {
            var saves = GameSession.GetAllSaves();
            if (saves.Length == 0)
            {
                Debug.Log("[MainMenu] No hay partidas guardadas.");
                return;
            }
            GameSession.Load(saves[0].FilePath);
            NavigationManager.Instance.ShowScreen(ScreenId.Dashboard);
        }

        private void OnQuit()
        {
            Application.Quit();
        }
    }
}