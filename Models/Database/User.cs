using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class User
    {
        public User()
        {
            Rental = new HashSet<Rental>();
        }

        public int UserId { get; set; }

        public ICollection<Rental> Rental { get; set; }
    }
}
