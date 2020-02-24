using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class ToolClassification
    {
        public ToolClassification()
        {
            ToolDetail = new HashSet<ToolDetail>();
        }

        public int ToolClassificationId { get; set; }
        public string ToolClassification1 { get; set; }

        public ICollection<ToolDetail> ToolDetail { get; set; }
    }
}
