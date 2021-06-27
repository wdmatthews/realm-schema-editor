using UnityEngine;

namespace RealmSchema
{
    [System.Serializable]
    public class NodeData
    {
        public string Guid = "";
        public Vector2 Position = Vector2.zero;

        public NodeData() { }

        public NodeData(string guid, Vector2 position)
        {
            Guid = guid;
            Position = position;
        }
    }
}
