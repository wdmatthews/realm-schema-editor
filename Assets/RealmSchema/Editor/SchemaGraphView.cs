using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RealmSchema.Editor
{
    public class SchemaGraphView : GraphView
    {
        private GridBackground _background = null;
        private StyleSheet _styleSheet = null;

        public SchemaGraphView(StyleSheet styleSheet)
        {
            _styleSheet = styleSheet;
            styleSheets.Add(_styleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
            
            _background = new GridBackground();
            Insert(0, _background);
            _background.StretchToParentSize();
            this.StretchToParentSize();
        }

        public void AddSchema(Vector2 position, Schema schema = null)
        {
            AddElement(new SchemaNode(this, _styleSheet, position, schema));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            foreach (Port port in ports)
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }
    }
}
