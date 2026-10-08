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
        }
    }
}
