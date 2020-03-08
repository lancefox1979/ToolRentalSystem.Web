using System.Collections.Generic;
using ToolRentalSystem.Web.Models.Database;

namespace ToolRentalSystem.Web.ViewModels
{
    public class ToolModel
    {
        public int? DetailID { get; set; }
        public int? ClassificationID { get; set; }
        public string Type { get; set; }  // This property refers to the 'tool_classification' field.
        public List<ToolClassification> TypeList { get; set; }
        public string Brand { get; set; }
        public string TradeName { get; set; }
        
    }
}