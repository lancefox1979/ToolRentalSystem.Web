using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class ToolCondition
    {
        public ToolCondition()
        {
            Tool = new HashSet<Tool>();
        }

        public int ToolConditionId { get; set; }
        public string ToolCondition1 { get; set; }

        public ICollection<Tool> Tool { get; set; }
    }
}
