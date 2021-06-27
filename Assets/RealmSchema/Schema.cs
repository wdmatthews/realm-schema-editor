using System.Collections.Generic;

namespace RealmSchema
{
    [System.Serializable]
    public class Schema
    {
        public string CollectionName = "";
        public List<SchemaField> Fields = new List<SchemaField>();

        public Schema(string name)
        {
            CollectionName = name;
        }

        public SchemaField AddField(string name, string type)
        {
            SchemaField field = new SchemaField(name, type);
            Fields.Add(field);
            return field;
        }

        public void RemoveField(int index)
        {
            Fields.RemoveAt(index);
        }
    }
}
