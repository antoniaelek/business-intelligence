using System.Collections.Generic;

namespace DWClient.Models
{
    public class AttributeMetadata
    {
        public DatabaseObject<int> OrdinalNum { get; set; }
        public DatabaseObject<string> SqlName { get; set; }
        public DatabaseObject<int> TypeID { get; set; }
        public DatabaseObject<string> Name { get; set; }

        public AttributeMetadata(string metadataTableName)
        {
            OrdinalNum = new DatabaseObject<int>($"{metadataTableName}.rbrAtrib");
            SqlName = new DatabaseObject<string>($"{metadataTableName}.imeSQLAtrib");
            TypeID = new DatabaseObject<int>($"{metadataTableName}.sifTipAtrib");
            Name = new DatabaseObject<string>($"{metadataTableName}.imeAtrib");
        }

        public AttributeMetadata(string metadataTableName, Dictionary<string, string> result) : this(metadataTableName)
        {
            OrdinalNum.Value = int.Parse(result[OrdinalNum.SqlName]);
            SqlName.Value = result[SqlName.SqlName];
            TypeID.Value = int.Parse(result[TypeID.SqlName]);
            Name.Value = result[Name.SqlName];
        }
    }
}
