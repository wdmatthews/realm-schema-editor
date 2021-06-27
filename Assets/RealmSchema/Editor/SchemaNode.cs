using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class SchemaNode : BaseNode
    {
        private static readonly Vector2 _defaultSize = new Vector2();

        public Schema Schema { get; private set; } = null;

        private int _nextFieldIndex = 0;
        private List<Port> _inputs = new List<Port>();
        private List<Port> _outputs = new List<Port>();
        private List<GroupBox> _fieldGroups = new List<GroupBox>();
        private TextField _collectionNameField = null;
        private Button _addFieldButton = null;

        public SchemaNode(GraphView graphView, StyleSheet styleSheet, Vector2 position, Schema schema = null)
            : base(graphView, styleSheet, position, _defaultSize)
        {
            Schema = schema ?? new Schema("collection");
            title = Schema.CollectionName;

            _collectionNameField = new TextField { label = "Collection", value = Schema.CollectionName };
            _collectionNameField.RegisterValueChangedCallback((value) =>
            {
                Schema.CollectionName = value.newValue;
                title = Schema.CollectionName;
            });
            mainContainer.Add(_collectionNameField);

            _addFieldButton = new Button(() => AddField());
            _addFieldButton.text = "Add Field";
            mainContainer.Add(_addFieldButton);
        }

        private void AddField(string name = null)
        {
            SchemaField field = Schema.AddField(name ?? $"field_{_nextFieldIndex}", "string");
            Port input = AddPort(field.Name, Direction.Input);

            _inputs.Add(input);
            _outputs.Add(AddPort(field.Name, Direction.Output));

            if (name == null) _nextFieldIndex++;

            GroupBox groupBox = new GroupBox();
            groupBox.AddToClassList("field-group");

            TextField textField = new TextField { value = field.Name };
            textField.RegisterValueChangedCallback((value) =>
            {
                int fieldIndex = Schema.Fields.IndexOf(field);
                Schema.Fields[fieldIndex].Name = value.newValue;
            });
            groupBox.Add(textField);

            DropdownField typeField = new DropdownField(SchemaField.Types, 0);
            typeField.RegisterValueChangedCallback((value) =>
            {
                int fieldIndex = Schema.Fields.IndexOf(field);
                Schema.Fields[fieldIndex].Type = value.newValue;
            });
            groupBox.Add(typeField);

            Button removeButton = new Button(() =>
            {
                int fieldIndex = Schema.Fields.IndexOf(field);
                RemoveField(fieldIndex);
            });
            removeButton.text = "Remove";
            groupBox.Add(removeButton);

            mainContainer.Add(groupBox);
            _fieldGroups.Add(groupBox);
        }

        private void RemoveField(int index)
        {
            Schema.RemoveField(index);
            RemovePort(_inputs[index]);
            RemovePort(_outputs[index]);
            mainContainer.Remove(_fieldGroups[index]);
            _inputs.RemoveAt(index);
            _outputs.RemoveAt(index);
            _fieldGroups.RemoveAt(index);
        }
    }
}
