using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
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
        private bool _wasLoaded = false;

        private Toolbar _toolbar = null;
        private ToolbarButton _saveButton = null;
        private ToolbarButton _addRoleButton = null;
        private ToolbarButton _addSchemaButton = null;

        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceId, int line)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (!(asset is EditorDataSO)) return false;

            SchemaEditorWindow window = GetWindow<SchemaEditorWindow>("Schema Editor");
            window._data = (EditorDataSO)asset;
            window._wasLoaded = false;
            window.minSize = new Vector2(512, 512);

            return false;
        }

        private void OnGUI()
        {
            if (!_wasLoaded) Load();
        }

        private void OnEnable()
        {
            _styleSheet = Resources.Load<StyleSheet>("Style Sheet");
            rootVisualElement.styleSheets.Add(_styleSheet);
            _graphView = new SchemaGraphView(_styleSheet);
            rootVisualElement.Add(_graphView);

            _toolbar = new Toolbar();

            _saveButton = new ToolbarButton(Save);
            _saveButton.text = "Save";
            _toolbar.Add(_saveButton);

            _addRoleButton = new ToolbarButton(() => _graphView.AddRole(Vector2.zero));
            _addRoleButton.text = "Add Role";
            _toolbar.Add(_addRoleButton);

            _addSchemaButton = new ToolbarButton(() => _graphView.AddSchema(Vector2.zero));
            _addSchemaButton.text = "Add Schema";
            _toolbar.Add(_addSchemaButton);

            rootVisualElement.Add(_toolbar);

            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
            rootVisualElement.Remove(_toolbar);
        }

        private void Load()
        {
            if (!_data) return;
            _graphView.NextRoleIndex = _data.NextRoleIndex;
            _graphView.NextSchemaIndex = _data.NextSchemaIndex;

            foreach (Role role in _data.Roles)
            {
                _graphView.AddRole(role.Position, role);
            }

            foreach (Schema schema in _data.Schemas)
            {
                _graphView.AddSchema(schema.Position, schema);
            }

            foreach (Connection connection in _data.Connections)
            {
                BaseNode inputNode = (BaseNode)_graphView.nodes.Where(
                    (Node node) => ((BaseNode)node).Guid == connection.InputNodeGuid).First();
                BaseNode outputNode = (BaseNode)_graphView.nodes.Where(
                    (Node node) => ((BaseNode)node).Guid == connection.OutputNodeGuid).First();

                Port inputPort = (Port)inputNode.inputContainer[connection.InputPortIndex];
                Port outputPort = (Port)outputNode.outputContainer[connection.OutputPortIndex];

                Edge edge = inputPort.ConnectTo(outputPort);
                _graphView.AddElement(edge);
            }

            _wasLoaded = true;
        }

        private void Save()
        {
            _data.Roles.Clear();
            _data.Schemas.Clear();
            _data.Connections.Clear();
            _data.NextRoleIndex = _graphView.NextRoleIndex;
            _data.NextSchemaIndex = _graphView.NextSchemaIndex;

            foreach (Node node in _graphView.nodes)
            {
                if (node is RoleNode)
                {
                    RoleNode roleNode = (RoleNode)node;
                    Role role = roleNode.Role;
                    role.Guid = roleNode.Guid;
                    role.Position = roleNode.GetPosition().position;
                    _data.Roles.Add(role);
                }
                else if (node is SchemaNode)
                {
                    SchemaNode schemaNode = (SchemaNode)node;
                    Schema schema = schemaNode.Schema;
                    schema.Guid = schemaNode.Guid;
                    schema.Position = schemaNode.GetPosition().position;
                    schema.NextFieldIndex = schemaNode.NextFieldIndex;
                    _data.Schemas.Add(schema);
                }
            }

            foreach (Edge connection in _graphView.edges)
            {
                BaseNode inputNode = (BaseNode)connection.input.node;
                BaseNode outputNode = (BaseNode)connection.output.node;

                _data.Connections.Add(new Connection
                {
                    InputNodeGuid = inputNode.Guid,
                    InputPortIndex = inputNode.inputContainer.IndexOf(connection.input),
                    OutputNodeGuid = outputNode.Guid,
                    OutputPortIndex = outputNode.outputContainer.IndexOf(connection.output),
                });
            }
        }
    }
}
