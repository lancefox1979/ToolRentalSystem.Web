using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolRentalSystem.Web.Data;
using ToolRentalSystem.Web.Models.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ToolRentalSystem.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("ToolRentalSystemDB")));
            services.AddDbContext<ToolRentalSystemDBContext>(options => options.UseMySql(Configuration.GetConnectionString("ToolRentalSystemDB")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(config =>
            {
                config.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "App", Action = "Index" });
                config.MapRoute("EditTool", "{controller}/{action}/{detailid}", new { controller = "App", Action = "EditTool" });
                config.MapRoute("AddTool", "{controller}/{action}/{toolid}", new { controller = "App", Action = "AddTool" });
            });

            //ManageRoles(app.ApplicationServices).GetAwaiter().GetResult();
        }

        // Major parts of this method borrowed from the following:
        // https://www.c-sharpcorner.com/article/role-base-authorization-in-asp-net-core-2-1/
        private async Task ManageRoles(IServiceProvider serviceProvider)
        {
            IdentityResult identityResult;
            string tempAdminPassword = "admin1";
            string[] roleNames = { "Admin", "Manager", "Employee", "Customer" };

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                foreach (var role in roleNames)
                {
                    bool exists = await roleManager.RoleExistsAsync(role);

                    if (!exists)
                    {
                        identityResult = await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                
                IdentityUser userOrtiz = await userManager.FindByEmailAsync("ortiza@toolrentalsystem.com");
                IdentityUser userFisher = await userManager.FindByEmailAsync("fishers@toolrentalsystem.com");
                IdentityUser userGarrett = await userManager.FindByEmailAsync("garretts@toolrentalsystem.com");
                IdentityUser userBiadgilgn = await userManager.FindByEmailAsync("biadgilgnh@toolrentalsystem.com");
                IdentityUser userHarper = await userManager.FindByEmailAsync("harperd@toolrentalsystem.com");
                IdentityUser userCustomer = await userManager.FindByEmailAsync("user.customer@toolrentalsystem.com");

                if (userOrtiz == null)
                {
                    userOrtiz = new IdentityUser()
                    {
                        UserName = "ortiza@toolrentalsystem.com",
                        Email = "ortiza@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userOrtiz, tempAdminPassword);
                }

                if (userFisher == null)
                {
                    userFisher = new IdentityUser()
                    {
                        UserName = "fishers@toolrentalsystem.com",
                        Email = "fishers@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userFisher, tempAdminPassword);
                }

                if (userGarrett == null)
                {
                    userGarrett = new IdentityUser()
                    {
                        UserName = "garretts@toolrentalsystem.com",
                        Email = "garretts@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userGarrett, tempAdminPassword);
                }

                if (userBiadgilgn == null)
                {
                    userBiadgilgn = new IdentityUser()
                    {
                        UserName = "biadgilgnh@toolrentalsystem.com",
                        Email = "biadgilgnh@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userBiadgilgn, tempAdminPassword);
                }
                
                if (userHarper == null)
                {
                    userHarper = new IdentityUser()
                    {
                        UserName = "harperd@toolrentalsystem.com",
                        Email = "harperd@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userHarper, tempAdminPassword);
                }

                if (userCustomer == null)
                {
                    userCustomer = new IdentityUser()
                    {
                        UserName = "user.customer@toolrentalsystem.com",
                        Email = "user.customer@toolrentalsystem.com"
                    };

                    await userManager.CreateAsync(userCustomer, tempAdminPassword);
                }

                await userManager.AddToRoleAsync(userOrtiz, "Admin");
                await userManager.AddToRoleAsync(userFisher, "Admin");
                await userManager.AddToRoleAsync(userGarrett, "Admin");
                await userManager.AddToRoleAsync(userBiadgilgn, "Admin");
                await userManager.AddToRoleAsync(userHarper, "Admin");
                await userManager.AddToRoleAsync(userCustomer, "Customer");
            }
        }
    }
}