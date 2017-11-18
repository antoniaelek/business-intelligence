using System.Collections.Generic;

namespace DWClient.Models
{
    public class TableMetadata
    {
        public DatabaseObject<int> ID { get; set; } = new DatabaseObject<int>("sifTablica");
        public DatabaseObject<string> Name { get; set; } = new DatabaseObject<string>("nazTablica");
        public DatabaseObject<string> SqlName { get; set; } = new DatabaseObject<string>("nazSQLTablica");
        public DatabaseObject<int> Type { get; set; } = new DatabaseObject<int>("sifTipTablica");

        public TableMetadata()
        {
            
        }

        public TableMetadata(Dictionary<string, string> result)
        {
            ID.Value = int.Parse(result[ID.SqlName]);
            Name.Value = result[Name.SqlName];
            SqlName.Value = result[SqlName.SqlName];
            Type.Value = int.Parse(result[Type.SqlName]);
        }
    }
}
