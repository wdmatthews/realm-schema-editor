using System.Collections.Generic;
using UnityEngine;

namespace RealmSchema
{
    [CreateAssetMenu(fileName = "Data", menuName = "realm-schema-editor/Data")]
    public class EditorDataSO : ScriptableObject
    {
        public List<Schema> Schemas = new List<Schema>();
    }
}
