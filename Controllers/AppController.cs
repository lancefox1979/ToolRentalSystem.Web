﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            //List<Tool> list = await _context.Tool.ToListAsync();

            List<Tool> list = await _context.Tool
                //.Where(t => t.ToolStatus.Equals("active"))
                .AsNoTracking()
                .ToListAsync();
            
            return View(list);
        }

        public async Task<IActionResult> ToolDetails(int? toolId)
        {
            if (toolId == null)
            {
                return NotFound();
            }
            
            var tool = await _context.Tool
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ToolId == toolId);

            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }
        
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> EditTool(int? toolId)
        {
            if (toolId == null)
            {
                return NotFound();
            }
            
            Tool tool = await GetTool(toolId);

            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("EditTool")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditToolPost(int? toolId)
        {
            if (toolId == null)
            {
                return NotFound();
            }
            
            Tool toolToUpdate = await GetTool(toolId);

            if (await TryUpdateModelAsync<Tool>(
                toolToUpdate,
                "",
                t => t.ToolBrand, t => t.TradeName, t => t.ToolCondition))
            {
                try
                {
                    _context.Entry(toolToUpdate).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    // ex.ToString();
                    ModelState.AddModelError("", "Cannot update Tool: " + ex.ToString());
                }

                return RedirectToAction(nameof(EditTool), new { toolId = toolToUpdate.ToolId });
            }

            return View(toolToUpdate);
        }
        
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddTool()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("AddTool")]
        //[ValidateAntiForgeryToken]
        public IActionResult AddToolPost(Tool newTool)
        {
            _context.Tool.Add(newTool);
            _context.SaveChanges();
            ViewBag.Message = "New tool successfully added to the inventory!";
            return View(newTool);
        }


                public async Task<IActionResult> DeleteTool(int? toolID)
        {
            if (toolID == null)
            {
                return NotFound();
            }

            Tool tool = await GetTool(toolID);
            
            
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        [HttpPost, ActionName("DeleteTool")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteToollPost(int? toolID)
        {
            if (toolID == null)
            {
                return NotFound();
            }

            Tool toolToDelete = await GetTool(toolID);
            
            if (await TryUpdateModelAsync<Tool>(
                toolToDelete,
                "",
                t => t.ToolStatus))
            {
                try
                {
                    _context.Entry(toolToDelete).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    // ex.ToString();
                    ModelState.AddModelError("", "Cannot update Tool: " + ex.ToString());
                }

                return RedirectToAction(nameof(DeleteTool), new { toolId = toolToDelete.ToolId });
            }
            return View(toolToDelete);
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

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Rentals()
        {
            //List<Rental> list = await _context.Rental.ToListAsync();

            List<Rental> list = await _context.Rental
                .Where(t => t.RentalStatus.Equals("rented"))
                .AsNoTracking()
                .ToListAsync();
            
            return View(list);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> ReturnTool(int? rentalId)
        {
            if (rentalId == null)
            {
                return NotFound();
            }
            
            Rental rental = await GetRental(rentalId);

            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("ReturnTool")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnToolPost(int? rentalId)
        {
            if (rentalId == null)
            {
                return NotFound();
            }
            
            Rental rentalToUpdate = await GetRental(rentalId);

            if (await TryUpdateModelAsync<Rental>(
                rentalToUpdate,
                "",
                t => t.RentalStatus))
            {
                try
                {
                    _context.Entry(rentalToUpdate).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    // ex.ToString();
                    ModelState.AddModelError("", "Cannot update Rental: " + ex.ToString());
                }

                return RedirectToAction(nameof(ReturnTool), new { rentalId = rentalToUpdate.RentalId });
            }

            return View(rentalToUpdate);
        }

        // Private helper method to return a Rental.
        private async Task<Rental> GetRental(int? rentalId)
        {
            var rental = await _context.Rental
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.RentalId == rentalId);
            
            return rental;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
