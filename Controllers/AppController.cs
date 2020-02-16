using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolRentalSystem.Web.Models;
using ToolRentalSystem.Web.Models.Database;

namespace ToolRentalSystem.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly ToolRentalSystemDBContext _context;

        public AppController(ToolRentalSystemDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Tool()
        {
            var tools = await _context.Tool.ToArrayAsync();

            return View(tools);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
