using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class BaseNode : Node
    {
        public string Guid { get; protected set; } = System.Guid.NewGuid().ToString();

        protected GraphView _graphView = null;

        public BaseNode(GraphView graphView, StyleSheet styleSheet, Vector2 position)
        {
            _graphView = graphView;
            styleSheets.Add(styleSheet);
            SetPosition(new Rect(position, Vector2.zero));
        }

        protected Port AddPort(string name, Direction direction,
            Port.Capacity capacity = Port.Capacity.Multi, System.Type type = null)
        {
            Port port = GetPort(direction, capacity, type ?? typeof(float));
            port.portName = name;
            (direction == Direction.Input ? inputContainer : outputContainer).Add(port);
            RefreshExpandedState();
            RefreshPorts();
            return port;
        }

        protected Port GetPort(Direction direction, Port.Capacity capacity = Port.Capacity.Multi,
            System.Type type = null)
        {
            return InstantiatePort(Orientation.Horizontal, direction, capacity, type ?? typeof(float));
        }

        protected void RemovePort(Port port)
        {
            List<Edge> connections = new List<Edge>(port.connections);

            foreach (Edge connection in connections)
            {
                connection.input.Disconnect(connection);
                connection.output.Disconnect(connection);
                _graphView.RemoveElement(connection);
            }

            (port.direction == Direction.Input ? inputContainer : outputContainer).Remove(port);
            RefreshExpandedState();
            RefreshPorts();
        }
    }
}
