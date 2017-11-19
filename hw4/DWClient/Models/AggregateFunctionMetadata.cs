using System.Collections.Generic;

namespace DWClient.Models
{
    public class AggregateFunctionMetadata
    {
        public DatabaseObject<int> ID { get; set; }
        public DatabaseObject<string> Name { get; set; }

        public AggregateFunctionMetadata(string metadataTableName)
        {
            ID = new DatabaseObject<int>($"{metadataTableName}.sifAgrFun");
            Name = new DatabaseObject<string>($"{metadataTableName}.nazAgrFun");
        }

        public AggregateFunctionMetadata(string metadataTableName, Dictionary<string, string> result) : this(metadataTableName)
        {
            ID.Value = int.Parse(result[ID.SqlName]);
            Name.Value = result[Name.SqlName];
        }
    }
}
