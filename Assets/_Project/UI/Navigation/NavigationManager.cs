using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NBAManager.UI.Navigation
{
    /// <summary>
    /// Gestiona qué pantalla está visible en cada momento.
    /// Todas las pantallas viven en el mismo UIDocument.
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        public static NavigationManager Instance { get; private set; }

        [SerializeField] private UIDocument uiDocument;

        private readonly Dictionary<ScreenId, BaseScreen> _screens = new();
        private BaseScreen _currentScreen;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterScreen(ScreenId id, BaseScreen screen)
        {
            _screens[id] = screen;
        }

        public void ShowScreen(ScreenId id)
        {
            if (_currentScreen != null)
                _currentScreen.Hide();

            if (_screens.TryGetValue(id, out var screen))
            {
                _currentScreen = screen;
                _currentScreen.Show();
            }
            else
            {
                Debug.LogWarning($"[NavigationManager] Pantalla no registrada: {id}");
            }
        }

        public VisualElement Root => uiDocument.rootVisualElement;
    }
}