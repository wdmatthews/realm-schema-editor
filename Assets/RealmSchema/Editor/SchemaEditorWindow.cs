using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class SchemaEditorWindow : EditorWindow
    {
        private EditorDataSO _data = null;
        private SchemaGraphView _graphView = null;
        private StyleSheet _styleSheet = null;

        private Toolbar _toolbar = null;
        private ToolbarButton _addSchemaButton = null;

        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceId, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (!(asset is EditorDataSO)) return false;

            SchemaEditorWindow window = GetWindow<SchemaEditorWindow>("Schema Editor");
            window._data = (EditorDataSO)asset;
            window.minSize = new Vector2(512, 512);

            return false;
        }

        private void OnEnable()
        {
            _styleSheet = Resources.Load<StyleSheet>("Style Sheet");
            rootVisualElement.styleSheets.Add(_styleSheet);
            _graphView = new SchemaGraphView(_styleSheet);
            rootVisualElement.Add(_graphView);

            _toolbar = new Toolbar();
            _addSchemaButton = new ToolbarButton(AddSchema);
            _addSchemaButton.text = "Add Schema";
            _toolbar.Add(_addSchemaButton);
            rootVisualElement.Add(_toolbar);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
            rootVisualElement.Remove(_toolbar);
        }

        private void AddSchema()
        {
            _graphView.AddSchema(Vector2.zero);
        }
    }
}
