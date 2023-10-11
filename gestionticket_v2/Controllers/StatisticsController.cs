using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gestionticket_v2.Data;

public class StatisticsController : Controller
{
    private gestionticket_v2Context _context;

    public StatisticsController(gestionticket_v2Context context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<JsonResult> TotalTickets()
    {
        var totalTickets = await _context.Tickets.CountAsync();
        return Json(totalTickets);
    }

    [HttpGet]
    public async Task<JsonResult> OpenClosedTickets()
    {
        var openTickets = await _context.Tickets.CountAsync(t => t.Statut == "ouvert");
        var closedTickets = await _context.Tickets.CountAsync(t => t.Statut == "fermé");

        return Json(new { openTickets, closedTickets });
    }

    [HttpGet]
    public async Task<JsonResult> TicketsPerCategory()
    {
        var ticketsPerCategory = await _context.Tickets
            .Include(t => t.Categorie) // Assure que la relation avec Categorie est chargée
            .GroupBy(t => t.Categorie.Nom) // Groupe par le nom de la catégorie
            .Select(g => new { Categorie = g.Key, Count = g.Count() })
            .ToListAsync();

        return Json(ticketsPerCategory);
    }

    /*
    [HttpGet]
    public async Task<JsonResult> TicketsPerTeamMember()
    {
        var ticketsPerMember = await _context.Tickets
            .GroupBy(t => t.AssigneeId)
            .Select(g => new { AssigneeId = g.Key, Count = g.Count() })
            .ToListAsync();

        return Json(ticketsPerMember);
    }

    [HttpGet]
    public async Task<JsonResult> AverageResolutionTime()
    {
        var averageResolutionTime = await _context.Tickets
            .Where(t => t.Statut == "fermé")
            .AverageAsync(t => (t.DateModification - t.DateCreation).TotalHours);

        return Json(averageResolutionTime);
    }

    */

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

}
