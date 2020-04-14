using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            List<Tool> list;

            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                list = await _context.Tool
                    .AsNoTracking()
                    .ToListAsync();
            }

            else
            {
                list = await _context.Tool
                    .Where(t => t.ToolStatus.Equals("active"))
                    .AsNoTracking()
                    .ToListAsync();
            }

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

            // TODO: extract method with this functionality...?
            string userNameFull = User.Identity.Name;
            string userNameShort = userNameFull.Substring(0, userNameFull.IndexOf('@'));

            toolToUpdate.ToolLastUpdated = DateTime.Now;
            toolToUpdate.ToolUpdatedBy = userNameShort;

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
            // TODO: extract method with this functionality...?
            string userNameFull = User.Identity.Name;
            string userNameShort = userNameFull.Substring(0, userNameFull.IndexOf('@'));

            newTool.ToolLastUpdated = DateTime.Now;
            newTool.ToolUpdatedBy = userNameShort;
            
            _context.Tool.Add(newTool);
            _context.SaveChanges();
            ViewBag.Message = "New tool successfully added to the inventory!";
            ViewBag.Link = "Tools"; // location that link on confirmation screen leads to
            ViewBag.LinkMessage = "Back to Tools"; // message shown for link on confirmation screen
            return View("Confirmation");
        }
        
        // public async Task<IActionResult> DeleteTool(int? toolID)
        // {
        //     if (toolID == null)
        //     {
        //         return NotFound();
        //     }

        //     Tool tool = await GetTool(toolID);

        //     if (tool == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(tool);
        // }

        // [HttpPost, ActionName("DeleteTool")]
        // //[ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteToolPost(int? toolID)
        // {
        //     if (toolID == null)
        //     {
        //         return NotFound();
        //     }

        //     Tool toolToDelete = await GetTool(toolID);
            
        //     if (await TryUpdateModelAsync<Tool>(
        //         toolToDelete,
        //         "",
        //         t => t.ToolStatus))
        //     {
        //         try
        //         {
        //             _context.Entry(toolToDelete).State = EntityState.Modified;

        //             await _context.SaveChangesAsync();
        //         }

        //         catch (DbUpdateException ex)
        //         {
        //             // ex.ToString();
        //             ModelState.AddModelError("", "Cannot update Tool: " + ex.ToString());
        //         }

        //         return RedirectToAction(nameof(DeleteTool), new { toolId = toolToDelete.ToolId });
        //     }

        //     return View(toolToDelete);
        // }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatusPost(int? toolID, string statusResponse)
        {
            if (toolID == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tool toolToUpdate = await GetTool(toolID);

            // TODO: extract method with this functionality...?
            string userNameFull = User.Identity.Name;
            string userNameShort = userNameFull.Substring(0, userNameFull.IndexOf('@'));

            toolToUpdate.ToolLastUpdated = DateTime.Now;
            toolToUpdate.ToolUpdatedBy = userNameShort;
            
            if (await TryUpdateModelAsync<Tool>(
                toolToUpdate,
                "",
                t => t.ToolStatus))
            {
                try
                {
                    _context.Entry(toolToUpdate).State = EntityState.Modified;

                    if (statusResponse.ToUpper().Equals("DEACTIVATE"))
                    {
                        toolToUpdate.ToolStatus = "inactive";
                    }

                    else if (statusResponse.ToUpper().Equals("REACTIVATE"))
                    {
                        toolToUpdate.ToolStatus = "active";
                    }

                    else
                    {
                        return BadRequest("Invalid request: " + statusResponse);
                    }

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    // ex.ToString();
                    ModelState.AddModelError("", "Cannot update Tool: " + ex.ToString());
                }

                return RedirectToAction(nameof(EditTool), new { toolId = toolToUpdate.ToolId });
            }

            return View();
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
        public async Task<IActionResult> RentOutTool(int? rentalId = null)
        {
            if (rentalId == null) {
                // get a list of asp net users
                List<AspNetUsers> aspNetUserList = await _context.AspNetUsers
                    .AsNoTracking()
                    .ToListAsync();
                
                // fill a drop down list with the ids of asp net users in the database
                ViewBag.AspNetUserDropDownList = new SelectList(aspNetUserList, "Id", "Id");

                // get a list of users from the database
                List<User> userList = await _context.User
                    .AsNoTracking()
                    .ToListAsync();
                
                // fill a drop down list with the user ids of users in the database
                ViewBag.UserDropDownList = new SelectList(userList, "UserId", "UserId");

                // get a list of the tools that are currently rented out or reserved
                var rentals = _context.Rental
                    .Where(t => t.RentalStatus.Equals("rented") || t.RentalStatus.Equals("reserved"))
                    .Select(t => t.ToolId)
                    .ToList();

                // get a list of those tools that are active and not currently rented or reserved
                List<Tool> toolList = await _context.Tool
                    .Where(t => t.ToolStatus.Equals("active") && !(rentals.Contains(t.ToolId)))
                    .AsNoTracking()
                    .ToListAsync();
                
                // fill the drop down list with tools that can be rented
                ViewBag.ToolDropDownList = new SelectList(toolList, "ToolId", "ToolId");
            } else {
                // if rent out tool accessed through reservations page
                Rental rental = await GetRental(rentalId);

                if (rental == null)
                {
                    return NotFound();
                }

                ViewBag.Rental = rental;
            }

            return View();
        }
        
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("RentOutTool")]
        //[ValidateAntiForgeryToken]
        public IActionResult RentOutToolPost(Rental newRental)
        {
            _context.Rental.Add(newRental);
            _context.SaveChanges();

            ViewBag.Message = "Tool successfully rented out to the user!";
            ViewBag.Link = "RentOutTool";
            ViewBag.LinkMessage = "Back to Rent Out Tool";// location that link on confirmation screen leads to
            return View("Confirmation");// message shown for link on confirmation screen
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

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Reservations()
        {

            List<Rental> list = await _context.Rental
                .Where(t => t.RentalStatus.Equals("reserved"))
                .AsNoTracking()
                .ToListAsync();
            
            return View(list);
        }

        public async Task<IActionResult> ReserveTool(int? toolId)
        {
            ViewBag.ToolId = toolId;
            
             List<User> userList = await _context.User 
                .AsNoTracking()
                .ToListAsync();
            
            // fill a drop down list with the user ids of users in the database
            ViewBag.UserDropDownList = new SelectList(userList, "UserId", "UserId");

            return View();
        }

        [HttpPost, ActionName("ReserveTool")]
         public IActionResult ReserveToolPost(Rental newRental)
        {
            _context.Rental.Add(newRental);
            _context.SaveChanges();

            ViewBag.Message = "Tool successfully reserved to the user!";
            ViewBag.Link = "ReserveTool";
            ViewBag.LinkMessage = "Back to Reserve Tool";// location that link on confirmation screen leads to
            return View("Confirmation");// message shown for link on confirmation screen
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}