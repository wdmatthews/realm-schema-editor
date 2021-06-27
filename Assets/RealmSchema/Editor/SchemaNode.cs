using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class SchemaNode : BaseNode
    {
        public Schema Schema { get; private set; } = null;

        public int NextFieldIndex { get; private set; } = 0;
        private List<Port> _inputs = new List<Port>();
        private List<Port> _outputs = new List<Port>();
        private List<GroupBox> _fieldGroups = new List<GroupBox>();
        private TextField _collectionNameField = null;
        private Button _addFieldButton = null;

        public SchemaNode(GraphView graphView, StyleSheet styleSheet, Vector2 position, Schema schema = null, int index = 0)
            : base(graphView, styleSheet, position)
        {
            Schema = schema != null ? new Schema(schema) : new Schema($"collection_{index}");
            title = Schema.CollectionName;

            _collectionNameField = new TextField { value = Schema.CollectionName };
            _collectionNameField.RegisterValueChangedCallback((value) =>
            {
                Schema.CollectionName = value.newValue;
                title = Schema.CollectionName;
            });
            mainContainer.Add(_collectionNameField);

            _addFieldButton = new Button(() => AddField());
            _addFieldButton.text = "Add Field";
            mainContainer.Add(_addFieldButton);

            AddPort("Document", Direction.Input, Port.Capacity.Multi, typeof(int));
            Port documentOutput = AddPort("Document", Direction.Output, Port.Capacity.Multi, typeof(int));
            documentOutput.portColor = documentOutput.disabledPortColor;

            if (schema != null)
            {
                Guid = schema.Guid;
                NextFieldIndex = Schema.NextFieldIndex;

                foreach (SchemaField field in schema.Fields)
                {
                    AddField(field);
                }
            }
        }

        private void AddField(SchemaField newField = null)
        {
            SchemaField field = newField ?? Schema.AddField($"field_{NextFieldIndex}", "string");
            Port input = AddPort(field.Name, Direction.Input);

            _inputs.Add(input);
            _outputs.Add(AddPort(field.Name, Direction.Output));

            if (newField == null) NextFieldIndex++;

            GroupBox groupBox = new GroupBox();
            groupBox.AddToClassList("field-group");

            TextField textField = new TextField { value = field.Name };
            textField.RegisterValueChangedCallback((value) =>
            {
                int fieldIndex = Schema.GetFieldIndex(field);
                Schema.Fields[fieldIndex].Name = value.newValue;
                _inputs[fieldIndex].portName = value.newValue;
                _outputs[fieldIndex].portName = value.newValue;
            });
            groupBox.Add(textField);

            DropdownField typeField = new DropdownField(SchemaField.Types, 0);
            typeField.RegisterValueChangedCallback((value) =>
            {
                int fieldIndex = Schema.GetFieldIndex(field);
                Schema.Fields[fieldIndex].Type = value.newValue;
            });
            groupBox.Add(typeField);

            Button removeButton = new Button(() =>
            {
                int fieldIndex = Schema.GetFieldIndex(field);
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
