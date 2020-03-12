using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<IActionResult> Tools()
        {
            List<Tool> list = await _context.Tool.ToListAsync();

            return View(list);
        }

        public async Task<IActionResult> ToolDetails(int? toolId)
        {
            if (toolId == null)
            {
                return NotFound();
            }
            
            var tool = await _context.Tool.AsNoTracking().FirstOrDefaultAsync(t => t.ToolId == toolId);

            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }
        
        //[ActionName("EditTool")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTool(int? toolId)
        {
            if (toolId == null)
            {
                return NotFound();
            }
            
            var tool = await GetTool(toolId);

            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // Private helper method to return a Tool.
        private async Task<Tool> GetTool(int? toolId)
        {
            var tool = await _context.Tool
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ToolId == toolId);
            
            return tool;
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
