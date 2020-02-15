using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class ToolDetail
    {
        public ToolDetail()
        {
            Tool = new HashSet<Tool>();
        }

        public int ToolDetailId { get; set; }
        public int? ToolClassificationId { get; set; }
        public string ToolBrand { get; set; }
        public string TradeName { get; set; }

        public ICollection<Tool> Tool { get; set; }
    }
}
