using System.Collections.Generic;

namespace RealmSchema
{
    [System.Serializable]
    public class Schema : NodeData
    {
        public string CollectionName = "";
        public List<SchemaField> Fields = new List<SchemaField>();

        public int NextFieldIndex = 0;

        public Schema(string name)
        {
            CollectionName = name;
        }

        public Schema(Schema schema)
            : base(schema.Guid, schema.Position)
        {
            CollectionName = schema.CollectionName;
            Fields = new List<SchemaField>();

            NextFieldIndex = schema.NextFieldIndex;

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
