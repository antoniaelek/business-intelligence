using System.Collections.Generic;

namespace DWClient.Models
{
    public class Measurement
    {
        public TableMetadata TableMetadata { get; set; }
        public AttributeMetadata AttributeMetadata { get; set; }
        public AggregateFunctionMetadata AggrFunMetadata { get; set; }
 
        public DatabaseObject<string> AttributeAggrFunName { get; set; }

        public Measurement(string tableMetadataTableName, string attributeMetadataTableName,
            string aggrFunMetadataTableName, string attributeAggFun, Dictionary<string, string> result)
        {
            TableMetadata = new TableMetadata(tableMetadataTableName, result);
            AttributeMetadata = new AttributeMetadata(attributeMetadataTableName, result);
            AggrFunMetadata = new AggregateFunctionMetadata(aggrFunMetadataTableName, result);
            AttributeAggrFunName = new DatabaseObject<string>($"{attributeAggFun}.imeAtrib");
            AttributeAggrFunName.Value = result[AttributeAggrFunName.SqlName];
        }
    }
}
