using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolRentalSystem.Web.Models;
using ToolRentalSystem.Web.Models.Database;
using ToolRentalSystem.Web.ViewModels;

namespace ToolRentalSystem.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly ToolRentalSystemDBContext _context;

        private ToolsModel toolsModel;

        public AppController(ToolRentalSystemDBContext context)
        {
            _context = context;
            toolsModel = new ToolsModel();
        }

        // public async Task<IActionResult> Tool()
        // {
        //     var tools = await _context.Tool.ToArrayAsync();

        //     return View(tools);
        // }

        public IActionResult Tools()
        {
            List<ToolsModel> toolsList = new List<ToolsModel>();

            var query = _context.ToolDetail.Join(
                _context.ToolClassification,
                toolDetail => toolDetail.ToolClassificationId,
                toolClass => toolClass.ToolClassificationId,
                (toolDetail, toolClass) => new
                {
                    Type = toolClass.ToolClassification1,
                    Brand = toolDetail.ToolBrand,
                    TradeName = toolDetail.TradeName
                });
            
            foreach (var item in query)
            {
                ToolsModel model = new ToolsModel();

                model.Type = item.Type;
                model.Brand = item.Brand;
                model.TradeName = item.TradeName;

                toolsList.Add(model);
            }

            return View(toolsList);
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
