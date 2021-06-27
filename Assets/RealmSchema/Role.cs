namespace RealmSchema
{
    [System.Serializable]
    public class Role : NodeData
    {
        public string Name;

        public Role(string name)
        {
            Name = name;
        }

        public Role(Role role)
            : base(role.Guid, role.Position)
        {
            Name = role.Name;
        }
    }
}
