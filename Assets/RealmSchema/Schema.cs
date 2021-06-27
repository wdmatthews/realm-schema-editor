using System.Collections.Generic;
using UnityEngine;

namespace RealmSchema
{
    [System.Serializable]
    public class Schema
    {
        public string CollectionName = "";
        public List<SchemaField> Fields = new List<SchemaField>();

        public string Guid = "";
        public Vector2 Position = Vector2.zero;
        public int NextFieldIndex = 0;

        public Schema(string name)
        {
            CollectionName = name;
        }

        public Schema(Schema schema)
        {
            CollectionName = schema.CollectionName;
            Guid = schema.Guid;
            Position = schema.Position;
            NextFieldIndex = schema.NextFieldIndex;
            Fields = new List<SchemaField>();

            foreach (SchemaField field in schema.Fields)
            {
                Fields.Add(new SchemaField(field.Name, field.Type));
            }
        }

        public SchemaField AddField(string name, string type)
        {
            SchemaField field = new SchemaField(name, type);
            Fields.Add(field);
            return field;
        }

        public int GetFieldIndex(SchemaField field)
        {
            for (int i = Fields.Count - 1; i >= 0; i--)
            {
                if (Fields[i].Name == field.Name) return i;
            }

            return -1;
        }

        public void RemoveField(int index)
        {
            Fields.RemoveAt(index);
        }
    }
}
