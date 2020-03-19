using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class Rental
    {
        public int RentalId { get; set; }
        public int? UserId { get; set; }
        public int? ToolId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string RentalStatus { get; set; }

        public Tool Tool { get; set; }
        public User User { get; set; }
    }
}
