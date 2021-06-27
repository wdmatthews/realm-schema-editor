using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class RoleNode : BaseNode
    {
        public Role Role { get; private set; } = null;

        public int NextFieldIndex { get; private set; } = 0;
        private TextField _nameField = null;

        public RoleNode(GraphView graphView, StyleSheet styleSheet, Vector2 position, Role role = null, int index = 0)
            : base(graphView, styleSheet, position)
        {
            Role = role != null ? new Role(role) : new Role($"role_{index}");
            title = Role.Name;

            _nameField = new TextField { value = Role.Name };
            _nameField.RegisterValueChangedCallback((value) =>
            {
                Role.Name = value.newValue;
                title = Role.Name;
            });
            mainContainer.Add(_nameField);

            if (role != null)
            {
                Guid = role.Guid;
            }

            AddPort("insert", Direction.Output, Port.Capacity.Multi, typeof(int));
            AddPort("delete", Direction.Output, Port.Capacity.Multi, typeof(int));
            AddPort("read", Direction.Output, Port.Capacity.Multi, typeof(Vector2));
            AddPort("write", Direction.Output, Port.Capacity.Multi, typeof(Vector2));
            AddPort("search", Direction.Output, Port.Capacity.Multi, typeof(Vector2));
        }
    }
}
