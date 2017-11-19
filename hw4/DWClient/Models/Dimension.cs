using System.Collections.Generic;

namespace DWClient.Models
{
    public class Dimension
    {
        public AttributeMetadata TableAttributeMetadata { get; set; }

        public string DimTableName { get; set; }
        public string DimTableSqlName { get; set; }
        public string FactTableSqlName { get; set; }
        public string FactTableAttributeSqlName { get; set; }
        public string DimTableAttributeSqlName { get; set; }
    }
}
