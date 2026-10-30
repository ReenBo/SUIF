using SUIF.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.ViewSystems
{
    [RequireComponent(typeof(UIDocument))]
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField] private UIDocument _uiDocument;

        public VisualElement Container => _uiDocument != null ? _uiDocument.rootVisualElement : null;

        private void Awake()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            if (_uiDocument != null && _uiDocument.rootVisualElement != null)
            {
                var viewBox = _uiDocument.rootVisualElement.Q("ViewBox");
                if (viewBox != null)
                {
                    viewBox.style.flexGrow = 1;
                    viewBox.style.width = Length.Percent(100);
                    viewBox.style.height = Length.Percent(100);
                    foreach (var child in viewBox.Children())
                    {
                        child.style.position = Position.Absolute;
                        child.style.left = 0;
                        child.style.top = 0;
                        child.style.right = 0;
                        child.style.bottom = 0;
                        child.style.flexGrow = 1;
                    }
                }
            }
        }
    }
}
