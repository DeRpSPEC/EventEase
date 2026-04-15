using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;
using System.Linq;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseDbContext _context;

        public EventsController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var ev = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == id.Value);
            if (ev == null) return NotFound();
            return View(ev);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues.OrderBy(v => v.Name), "VenueId", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Description,ImageUrl,VenueId")] Event ev)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ev);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Errors"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            ViewData["VenueId"] = new SelectList(_context.Venues.OrderBy(v => v.Name), "VenueId", "Name", ev.VenueId);
            return View(ev);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var ev = await _context.Events.FindAsync(id.Value);
            if (ev == null) return NotFound();
            ViewData["VenueId"] = new SelectList(_context.Venues.OrderBy(v => v.Name), "VenueId", "Name", ev.VenueId);
            return View(ev);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,StartDate,EndDate,Description,ImageUrl,VenueId")] Event ev)
        {
            if (id != ev.EventId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Event updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _context.Events.Any(e => e.EventId == ev.EventId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["Errors"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            ViewData["VenueId"] = new SelectList(_context.Venues.OrderBy(v => v.Name), "VenueId", "Name", ev.VenueId);
            return View(ev);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ev = await _context.FindAsync<Event>(id.Value);
            if (ev == null) return NotFound();
            return View(ev);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
