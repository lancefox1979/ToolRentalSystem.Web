﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToolRentalSystem.Web.Models;
using ToolRentalSystem.Web.Models.Database;

namespace ToolRentalSystem.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly ToolRentalSystemDBContext _context;
        private readonly IServiceProvider _provider;

        public AppController(ToolRentalSystemDBContext context, IServiceProvider provider)
        {
            _context = context;
            _provider = provider;
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
            } else {
                list = await _context.Tool
                    .Where(t => t.ToolStatus.Equals("active"))
                    .AsNoTracking()
                    .ToListAsync();
            }

            // save a list of the tool ids for tools that are currently rented out or reserved
            ViewBag.UnavailableToolIds = await _context.Rental
                .Where(t => t.RentalStatus.Equals("rented") || t.RentalStatus.Equals("reserved"))
                .Select(t => t.ToolId)
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

                //return RedirectToAction(nameof(EditTool), new { toolId = toolToUpdate.ToolId });
            }

            ViewBag.Message = "Your updates have been saved";
            ViewBag.Link = "Tools"; 
            ViewBag.LinkMessage = "Back to Tools"; 
            return View("Confirmation");
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
                        ViewBag.Link = "Tools";
                        ViewBag.LinkMessage = "Back to Tools";
                        ViewBag.Message = "This item has been deactivated!";
                        await _context.SaveChangesAsync();
                        return View("Confirmation");
                    }

                    else if (statusResponse.ToUpper().Equals("REACTIVATE"))
                    {
                        toolToUpdate.ToolStatus = "active";
                        ViewBag.Link = "Tools";
                        ViewBag.LinkMessage = "Back to Tools";
                        ViewBag.Message = "This item has been reactivated!";
                        await _context.SaveChangesAsync();
                        return View("Confirmation");
                    }

                    else
                    {
                        return BadRequest("Invalid request: " + statusResponse);
                    }

                    //await _context.SaveChangesAsync();
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

            List<Rental> list;
            IdentityUser user;

            string userName = User.Identity.Name;
            string userId = "";

            using (IServiceScope scope = _provider.CreateScope())
            {
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                user = await userManager.FindByEmailAsync(userName);
                userId = user.Id;
            }

            if (User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Employee"))
            {
                list = await _context.Rental
                    .Where(r => r.RentalStatus.Equals("rented"))
                    .AsNoTracking()
                    .ToListAsync();
            }

            else
            {
                list = await _context.Rental
                    .Where(r => r.RentalStatus.Equals("rented") 
                        || r.RentalStatus.Equals("reserved") 
                        || r.RentalStatus.Equals("returned"))
                    .Where(r => r.AspNetUserId.Equals(userId))
                    .AsNoTracking()
                    .ToListAsync();
            }
            
            return View(list);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Users()
        {

            List<AspNetUsers> list = await _context.AspNetUsers
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

                // get a list of the tools that are currently rented out or reserved
                var rentals = await _context.Rental
                    .Where(t => t.RentalStatus.Equals("rented") || t.RentalStatus.Equals("reserved"))
                    .Select(t => t.ToolId)
                    .ToListAsync();

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
        public async Task<IActionResult> RentOutToolPost(Rental rental = null, int? rentalId = null)
        {
            if (rentalId != null) {
                if (await TryUpdateModelAsync<Rental>(
                    rental,
                    "",
                    t => t.AspNetUserId, t => t.StartDate, t => t.DueDate, t => t.RentalStatus))
                {
                    try
                    {
                        _context.Entry(rental).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                    }

                    catch (DbUpdateException ex)
                    {
                        // ex.ToString();
                        ModelState.AddModelError("", "Cannot update Tool: " + ex.ToString());
                    }
                }
            } else {
                _context.Rental.Add(rental);
                _context.SaveChanges();
            }

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

                    rentalToUpdate.RentalStatus = "returned";

                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    // ex.ToString();
                    ModelState.AddModelError("", "Cannot update Rental: " + ex.ToString());
                }

                //return RedirectToAction(nameof(ReturnTool), new { rentalId = rentalToUpdate.RentalId });
            }

            ViewBag.Message = "Tool was successfully returned!";
            ViewBag.Link = "Rentals";
            ViewBag.LinkMessage = "Back to Rentals";

            return View("Confirmation");
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

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CancelReservation(int? rentalId)
        {  
            if (rentalId == null)
            {
                return NotFound();
            }

            Rental reservation = await GetRental(rentalId);

            if (reservation == null)
            {
                return NotFound();
            }
            
            return View(reservation);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("CancelReservation")]
        public IActionResult CancelReservationPost(Rental reservation)
        {
            if (reservation == null)
            {
                return NotFound();
            }
            
            try {
                // try to delete reservation
                _context.Entry(reservation).State = EntityState.Deleted;
                _context.SaveChanges();
            } catch (DbUpdateException ex) {
                // ex.ToString();
                ModelState.AddModelError("", "Cannot delete reservation: " + ex.ToString());
            }

            ViewBag.Message = "Reservation successfully cancelled!";
            ViewBag.Link = "Reservations"; // location that link on confirmation screen leads to
            ViewBag.LinkMessage = "Back to Reservations"; // message shown for link on confirmation screen
            return View("Confirmation");
        }

        [Authorize(Roles = "Admin, Manager, Customer")]
        public async Task<IActionResult> ReserveTool(int? toolId)
        {
            ViewBag.ToolId = toolId;

            // get current user's name
            string userNameFull = User.Identity.Name;

            // get current user's id
            ViewBag.User = await _context.AspNetUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserName.Equals(userNameFull, StringComparison.InvariantCultureIgnoreCase));

            // get a list of asp net users
            List<AspNetUsers> aspNetUserList = await _context.AspNetUsers
                .AsNoTracking()
                .ToListAsync();
                
            // fill a drop down list with the ids of asp net users in the database
            ViewBag.AspNetUserDropDownList = new SelectList(aspNetUserList, "Id", "Id");

            return View();
        }

        [Authorize(Roles = "Admin, Manager, Customer")]
        [HttpPost, ActionName("ReserveTool")]
         public IActionResult ReserveToolPost(Rental newRental)
        {
            _context.Rental.Add(newRental);
            _context.SaveChanges();

            ViewBag.Message = "Tool successfully reserved to the user!";
            ViewBag.Link = "Tools"; // location that link on confirmation screen leads to
            ViewBag.LinkMessage = "Back to Tools"; // message shown for link on confirmation screen
            return View("Confirmation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}