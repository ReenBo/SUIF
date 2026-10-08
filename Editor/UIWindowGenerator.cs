using System.IO;
using UnityEditor;
using UnityEngine;

namespace SUIF.Editor
{
    public class UIWindowGenerator : EditorWindow
    {
        private string _windowName = "NewWindow";
        private string _targetFolder = "Assets/_project/Scripts/UI/Windows";
        private string _layerName = "Windows";
        private bool _isModal = false;

        [MenuItem("Tools/SUIF/Create UI Window Wizard...")]
        [MenuItem("Assets/Create/SUIF/UI Window...", false, 80)]
        public static void Open()
        {
            var window = GetWindow<UIWindowGenerator>("SUIF Window Wizard");
            window.minSize = new Vector2(400, 260);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("SUIF Window Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _windowName = EditorGUILayout.TextField("Window Name:", _windowName);
            _targetFolder = EditorGUILayout.TextField("Target Directory:", _targetFolder);
            _layerName = EditorGUILayout.TextField("UI Layer:", _layerName);
            _isModal = EditorGUILayout.Toggle("Is Modal Window:", _isModal);

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Window (View, ViewModel, UXML, USS)", GUILayout.Height(36)))
            {
                Generate();
            }
        }

        private void Generate()
        {
            if (string.IsNullOrWhiteSpace(_windowName))
            {
                EditorUtility.DisplayDialog("Error", "Please enter a valid Window name.", "OK");
                return;
            }

            if (!Directory.Exists(_targetFolder))
            {
                Directory.CreateDirectory(_targetFolder);
            }

            var viewPath = Path.Combine(_targetFolder, $"{_windowName}View.cs");
            var vmPath = Path.Combine(_targetFolder, $"{_windowName}ViewModel.cs");
            var uxmlPath = Path.Combine(_targetFolder, $"{_windowName}.uxml");
            var ussPath = Path.Combine(_targetFolder, $"{_windowName}.uss");

            var viewTemplate = $@"using SUIF.Attributes;
using SUIF.Base;
using UnityEngine.UIElements;

namespace Project.UI
{{
    [UIView(""{_windowName}"", SUIF.API.UILayer.{_layerName}, isModal: {(_isModal ? "true" : "false")})]
    public class {_windowName}View : BaseView<{_windowName}ViewModel>
    {{
        public {_windowName}View(VisualElement root) : base(root) {{ }}

        protected override void Bind()
        {{
            // Bind view elements using Binder
        }}
    }}
}}";

            var vmTemplate = $@"using SUIF.Base;

namespace Project.UI
{{
    public class {_windowName}ViewModel : BaseViewModel
    {{
        public {_windowName}ViewModel()
        {{
        }}
    }}
}}";

            var uxmlTemplate = $@"<ui:UXML xmlns:ui=""UnityEngine.UIElements"" xmlns:uie=""UnityEditor.UIElements"">
    <ui:Style src=""{_windowName}.uss"" />
    <ui:VisualElement class=""c-window"" picking-mode=""Ignore"">
        <ui:Label text=""{_windowName} Title"" class=""c-window__title"" />
    </ui:VisualElement>
</ui:UXML>";

            var ussTemplate = $@".c-window {{
    flex-grow: 1;
    justify-content: center;
    align-items: center;
}}

.c-window__title {{
    font-size: 24px;
    -unity-font-style: bold;
}}";

            File.WriteAllText(viewPath, viewTemplate);
            File.WriteAllText(vmPath, vmTemplate);
            File.WriteAllText(uxmlPath, uxmlTemplate);
            File.WriteAllText(ussPath, ussTemplate);

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success", $"SUIF Window '{_windowName}' generated in {_targetFolder}!", "OK");
            Close();
        }
    }
}
