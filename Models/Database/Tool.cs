using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class Tool
    {
        public Tool()
        {
            Rental = new HashSet<Rental>();
        }

        public int ToolId { get; set; }
        public string ToolClassification { get; set; }
        public string ToolBrand { get; set; }
        public string TradeName { get; set; }
        public string ToolCondition { get; set; }
        public string ToolStatus { get; set; }

        public ICollection<Rental> Rental { get; set; }
    }
}
