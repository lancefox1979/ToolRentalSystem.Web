using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class Tool
    {
        public Tool()
        {
            Rented = new HashSet<Rented>();
        }

        public int ToolId { get; set; }
        public int? ToolDetailId { get; set; }
        public int? ToolConditionId { get; set; }

        public ToolCondition ToolCondition { get; set; }
        public ToolDetail ToolDetail { get; set; }
        public ICollection<Rented> Rented { get; set; }
    }
}
