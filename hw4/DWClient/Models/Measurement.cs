using System.Collections.Generic;

namespace DWClient.Models
{
    public class Measurement
    {
        public TableMetadata TableMetadata { get; set; }
        public AttributeMetadata AttributeMetadata { get; set; }
        public AggregateFunctionMetadata AggrFunMetadata { get; set; }
 
        public DatabaseObject<string> AttributeAggrFunName { get; set; }
    }
}
