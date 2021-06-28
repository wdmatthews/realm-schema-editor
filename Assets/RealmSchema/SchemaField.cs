using System.Collections.Generic;

namespace RealmSchema
{
    [System.Serializable]
    public class SchemaField
    {
        public static readonly List<string> Types = new List<string>
        {
            "objectId",
            "string",
            "bool",
            "double",
            "int",
            "timestamp",
            "date",
            "array",
            "object",
            "decimal",
            "long",
            "regex",
            "uuid",
            "mixed",
            "null",
        };

        public string Name = "";
        public string Type = "";

        public SchemaField(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
