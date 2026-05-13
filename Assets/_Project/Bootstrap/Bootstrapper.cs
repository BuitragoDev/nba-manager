using UnityEngine;
using NBAManager.UI.Navigation;

namespace NBAManager.Bootstrap
{
    /// <summary>
    /// Punto de entrada del juego. Solo inicializa la navegación.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        void Start()
        {
            NavigationManager.Instance.ShowScreen(ScreenId.MainMenu);
        }
    }
}