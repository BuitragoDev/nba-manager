using UnityEngine;
using UnityEngine.UIElements;

namespace NBAManager.UI.Navigation
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset layoutAsset;

        protected VisualElement Root;

        protected virtual void Start()
        {
            if (layoutAsset == null)
            {
                Debug.LogError($"[{GetType().Name}] No tiene asignado un VisualTreeAsset (UXML).");
                return;
            }

            Root = layoutAsset.Instantiate();
            Root.style.flexGrow = 1;
            Root.style.display = DisplayStyle.None;

            NavigationManager.Instance.Root.Add(Root);
            RegisterScreen();
            OnInit();
        }

        private void RegisterScreen()
        {
            NavigationManager.Instance.RegisterScreen(GetScreenId(), this);
        }

        /// <summary>Llamado una sola vez tras crear la UI. Aquí haces Q() y registras callbacks.</summary>
        protected virtual void OnInit() { }

        protected abstract ScreenId GetScreenId();

        public virtual void Show()
        {
            Root.style.display = DisplayStyle.Flex;
            OnShow();
        }

        public virtual void Hide()
        {
            Root.style.display = DisplayStyle.None;
            OnHide();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        /// <summary>Shortcut para Query por name.</summary>
        protected T Q<T>(string name) where T : VisualElement
        {
            return Root.Q<T>(name);
        }
    }
}