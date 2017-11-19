using System.Collections.Generic;

namespace DWClient.Models
{
    public class TableMetadata
    {
        private string metadataTableName;
        public DatabaseObject<int> ID { get; set; } = new DatabaseObject<int>("sifTablica");
        public DatabaseObject<string> Name { get; set; } = new DatabaseObject<string>("nazTablica");
        public DatabaseObject<string> SqlName { get; set; } = new DatabaseObject<string>("nazSQLTablica");
        public DatabaseObject<int> Type { get; set; } = new DatabaseObject<int>("sifTipTablica");

        public TableMetadata(string metadataTableName)
        {
            this.metadataTableName = metadataTableName;
        }

        public TableMetadata(string metadataTableName, Dictionary<string, string> result) : this(metadataTableName)
        {
            ID.Value = int.Parse(result[metadataTableName + "." + ID.SqlName]);
            Name.Value = result[metadataTableName + "." + Name.SqlName];
            SqlName.Value = result[metadataTableName + "." + SqlName.SqlName];
            Type.Value = int.Parse(result[metadataTableName + "." + Type.SqlName]);
        }
    }
}
