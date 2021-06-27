using System.Collections.Generic;
using UnityEngine;

namespace RealmSchema
{
    [CreateAssetMenu(fileName = "Data", menuName = "realm-schema-editor/Data")]
    public class EditorDataSO : ScriptableObject
    {
        public List<Role> Roles = new List<Role>();
        public List<Schema> Schemas = new List<Schema>();
        public List<Connection> Connections = new List<Connection>();

        public int NextRoleIndex = 0;
        public int NextSchemaIndex = 0;
    }
}
