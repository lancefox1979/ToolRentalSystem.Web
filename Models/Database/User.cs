using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class User
    {
        public User()
        {
            Rented = new HashSet<Rented>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? UserTypeId { get; set; }

        public UserType UserType { get; set; }
        public Login Login { get; set; }
        public ICollection<Rented> Rented { get; set; }
    }
}
