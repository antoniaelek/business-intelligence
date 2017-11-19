namespace DWClient.Models
{
    class DatabaseAttribute
    {
        public string QueryName { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public DatabaseAttribute(string queryName, string name, string alias)
        {
            QueryName = queryName;
            Name = name;
            Alias = alias;
        }
    }
}
