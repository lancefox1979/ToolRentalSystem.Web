using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToolRentalSystem.Web.Models;
using ToolRentalSystem.Web.Models.Database;
using ToolRentalSystem.Web.ViewModels;

namespace ToolRentalSystem.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly ToolRentalSystemDBContext _context;

        private ToolModel toolsModel;

        public AppController(ToolRentalSystemDBContext context)
        {
            _context = context;
            toolsModel = new ToolModel();
        }
        
        public IActionResult Tools()
        {
            List<ToolModel> toolsList = new List<ToolModel>();

            var query = _context.ToolDetail.Join(
                _context.ToolClassification,
                toolDetail => toolDetail.ToolClassificationId,
                toolClass => toolClass.ToolClassificationId,
                (toolDetail, toolClass) => new
                {
                    DetailID = toolDetail.ToolDetailId,
                    ClassificationID = toolClass.ToolClassificationId,
                    Type = toolClass.ToolClassification1,
                    Brand = toolDetail.ToolBrand,
                    TradeName = toolDetail.TradeName
                });
            
            foreach (var item in query)
            {
                ToolModel model = new ToolModel();

                model.DetailID = item.DetailID;
                model.ClassificationID = item.ClassificationID;
                model.Type = item.Type;
                model.Brand = item.Brand;
                model.TradeName = item.TradeName;

                toolsList.Add(model);
            }

            return View(toolsList);
        }
        public async Task<IActionResult> ToolDetails(int? detailId)
        {
            ToolModel toolModel;

            if (detailId == null)
            {
                return NotFound();
            }

            toolModel = await GetToolModel(detailId);

            if (toolModel == null)
            {
                return NotFound();
            }

            return View(toolModel);
        }
        
        //[ActionName("EditTool")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTool(int? detailId)
        {
            ToolModel toolModel;
            List<string> toolTypeList = new List<string>();

            if (detailId == null)
            {
                return NotFound();
            }

            toolModel = await GetToolModel(detailId);

            if (toolModel == null)
            {
                return NotFound();
            }

            var toolTypes = _context.ToolClassification.ToList();

            foreach (ToolClassification item in toolTypes)
            {
                string type = item.ToolClassification1;

                toolTypeList.Add(type);
            }

            PopulateToolTypeDropDownList(detailId);

            return View(toolModel);
        }

        // Private helper method to return a ToolModel.
        private async Task<ToolModel> GetToolModel(int? detailId)
        {
            ToolModel toolModel = new ToolModel();

            var toolDetailQuery = await _context.ToolDetail
                .Include(td => td.ToolClassification)
                .AsNoTracking()
                .FirstOrDefaultAsync(td => td.ToolDetailId == detailId);
            
            toolModel.DetailID = toolDetailQuery.ToolDetailId;
            toolModel.ClassificationID = toolDetailQuery.ToolClassificationId;
            toolModel.Type = toolDetailQuery.ToolClassification.ToolClassification1;
            toolModel.TradeName = toolDetailQuery.TradeName;
            toolModel.Brand = toolDetailQuery.ToolBrand;

            return toolModel;
        }

        private void PopulateToolTypeDropDownList(object selectedTool = null)
        {
            var query = from t in _context.ToolClassification
                        orderby t.ToolClassification1
                        select t;
            
            ViewBag.ToolTypeDropDownList = new SelectList(query.AsNoTracking(), "ToolClassificationId", "ToolClassification1", selectedTool);
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
