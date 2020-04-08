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
        public string ToolType { get; set; }
        public string ToolBrand { get; set; }
        public string TradeName { get; set; }
        public string ToolCondition { get; set; }
        public string ToolStatus { get; set; }
        public decimal? ToolPrice { get; set; }
        public DateTime? ToolLastUpdated { get; set; }
        public string ToolUpdatedBy { get; set; }

        public ICollection<Rental> Rental { get; set; }
    }
}
