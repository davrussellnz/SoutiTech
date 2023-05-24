using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gestionticket_v2.Data;
using gestionticket_v2.Models;
using Microsoft.AspNetCore.Identity;

namespace gestionticket_v2.Controllers
{
    public class TicketsController : Controller
    {
        private readonly gestionticket_v2Context _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TicketsController> _logger;


        public TicketsController(gestionticket_v2Context context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<TicketsController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> TechnicianTickets()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not authenticated. Redirect them to the login page.
                return RedirectToAction("Login", "Home");
            }

            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null)
            {
                // User is not logged in. Redirect them to the login page.
                return RedirectToAction("Login", "Home");
            }

            var user = await _userManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                // User does not exist in the database. Handle this case appropriately.
                // For example, you can log this event and redirect the user to the login page.
                _logger.LogWarning($"User with ID '{userIdString}' does not exist in the database.");
                return RedirectToAction("Login", "Account");
            }

            var tickets = await _context.Tickets.Where(t => t.AssigneeId == userIdString).ToListAsync();
            return View(tickets);
        }



        public async Task<IActionResult> ClientTickets()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not authenticated. Redirect them to the login page.
                return RedirectToAction("Login", "Home");
            }
            var userIdString = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userIdString);
            if (user == null)
            {
                // User does not exist in the database. Handle this case appropriately.
                _logger.LogWarning($"User with ID '{userIdString}' does not exist in the database.");
                return RedirectToAction("Login", "Home");
            }

            if (userIdString != null)
            {
                var tickets = await _context.Tickets.Where(t => t.AuteurId == userIdString).ToListAsync();
                return View(tickets);
            }
            else
            {
                // Handle the case where the user is not logged in.
                return View("Error");
            }
        }





        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var gestionticket_v2Context = _context.Tickets.Include(t => t.Assignee).Include(t => t.Auteur).Include(t => t.Categorie).Include(t => t.Priorite);
            return View(await gestionticket_v2Context.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Assignee)
                .Include(t => t.Auteur)
                .Include(t => t.Categorie)
                .Include(t => t.Priorite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.PrioriteId = new SelectList(_context.Priorite, "Id", "Nom");
            ViewBag.CategorieId = new SelectList(_context.Categorie, "Id", "Nom");
            return View();
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titre,Description,PrioriteId,CategorieId")] Ticket ticket)
        {
            ModelState.Remove("Auteur");
            ModelState.Remove("Statut");
            ModelState.Remove("Assignee");
            ModelState.Remove("AuteurId");
            ModelState.Remove("AssigneeId");
            ModelState.Remove("PiecesJointes");
            ModelState.Remove("Priorite");
            ModelState.Remove("Categorie");
            if (ModelState.IsValid)
            {
                // Set default values
                ticket.Statut = "New"; // Default status
                ticket.AuteurId = _userManager.GetUserId(User); // Current user ID
                ticket.AssigneeId = await GetLeastAssignedTechnicianId(); // Assign to technician with least tickets

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
       
            // Populate select lists
            ViewData["AssigneeId"] = new SelectList(_context.Set<MembreSupportTechnique>(), "Id", "Id", ticket.AssigneeId);
            ViewData["AuteurId"] = new SelectList(_context.Set<Client>(), "Id", "Id", ticket.AuteurId);
            ViewData["CategorieId"] = new SelectList(_context.Set<Categorie>(), "Id", "Id", ticket.CategorieId);
            ViewData["PrioriteId"] = new SelectList(_context.Set<Priorite>(), "Id", "Id", ticket.PrioriteId);

            return View(ticket);
        }

        private async Task<string> GetLeastAssignedTechnicianId()
        {
            // Get all technicians
            var usersInRole = await _userManager.GetUsersInRoleAsync("MembreSupportTechnique");


            // If there are no technicians, return null
            if (!usersInRole.Any())
            {
                return null;
            }

            // Group tickets by AssigneeId, count them, and order by the count
            var technicianTicketCounts = await _context.Tickets
                .GroupBy(t => t.AssigneeId)
                .Select(g => new { TechnicianId = g.Key, TicketCount = g.Count() })
                .ToListAsync();

            // If there are no tickets, return the ID of the first technician
            if (!technicianTicketCounts.Any())
            {
                return usersInRole.First().Id;
            }

            // Find the technician with the least tickets
            var leastTicketsTechnician = technicianTicketCounts.OrderBy(t => t.TicketCount).First();

            // If the technician with the least tickets has no tickets, return the ID of the first technician
            if (leastTicketsTechnician.TicketCount == 0)
            {
                return usersInRole.First().Id;
            }

            // Return the ID of the technician with the least tickets
            return leastTicketsTechnician.TechnicianId;
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["AssigneeId"] = new SelectList(_context.Set<MembreSupportTechnique>(), "Id", "Id", ticket.AssigneeId);
            ViewData["AuteurId"] = new SelectList(_context.Set<Client>(), "Id", "Id", ticket.AuteurId);
            ViewData["CategorieId"] = new SelectList(_context.Set<Categorie>(), "Id", "Id", ticket.CategorieId);
            ViewData["PrioriteId"] = new SelectList(_context.Set<Priorite>(), "Id", "Id", ticket.PrioriteId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,Description,PrioriteId,CategorieId,AuteurId,AssigneeId,Statut,DateCreation,DateModification")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigneeId"] = new SelectList(_context.Set<MembreSupportTechnique>(), "Id", "Id", ticket.AssigneeId);
            ViewData["AuteurId"] = new SelectList(_context.Set<Client>(), "Id", "Id", ticket.AuteurId);
            ViewData["CategorieId"] = new SelectList(_context.Set<Categorie>(), "Id", "Id", ticket.CategorieId);
            ViewData["PrioriteId"] = new SelectList(_context.Set<Priorite>(), "Id", "Id", ticket.PrioriteId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Assignee)
                .Include(t => t.Auteur)
                .Include(t => t.Categorie)
                .Include(t => t.Priorite)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'gestionticket_v2Context.Ticket'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
