using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToolRentalSystem.Web.Areas.Identity.Data;

[assembly: HostingStartup(typeof(ToolRentalSystem.Web.Areas.Identity.IdentityHostingStartup))]
namespace ToolRentalSystem.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityDataContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("ToolRentalSystemDB")));

                // services.AddDefaultIdentity<IdentityUser>()
                //     .AddEntityFrameworkStores<IdentityDataContext>();
            });
        }
    }
}