using System.Collections.Generic;

namespace DWClient.Models
{
    public class TableMetadata
    {
        public DatabaseObject<int> ID { get; set; }
        public DatabaseObject<string> Name { get; set; }
        public DatabaseObject<string> SqlName { get; set; }
        public DatabaseObject<int> Type { get; set; }

        public TableMetadata(string metadataTableName)
        {
            ID = new DatabaseObject<int>($"{metadataTableName}.sifTablica");
            Name = new DatabaseObject<string>($"{metadataTableName}.nazTablica");
            SqlName = new DatabaseObject<string>($"{metadataTableName}.nazSQLTablica");
            Type = new DatabaseObject<int>($"{metadataTableName}.sifTipTablica");
        }

        public TableMetadata(string metadataTableName, Dictionary<string, string> result) : this(metadataTableName)
        {
            ID.Value = int.Parse(result[ID.SqlName]);
            Name.Value = result[Name.SqlName];
            SqlName.Value = result[SqlName.SqlName];
            Type.Value = int.Parse(result[Type.SqlName]);
        }
    }
}
