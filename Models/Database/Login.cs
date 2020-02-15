using System;
using System.Collections.Generic;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class Login
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User User { get; set; }
    }
}
