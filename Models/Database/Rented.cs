using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class Rented
    {
        public int UserId { get; set; }
        public int ToolId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? RentalStatus { get; set; }

        public Tool Tool { get; set; }
        public User User { get; set; }
    }
}
