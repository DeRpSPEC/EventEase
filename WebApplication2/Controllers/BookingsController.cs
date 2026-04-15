using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseDbContext _context;

        public BookingsController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == id.Value);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venues, "VenueId", "Name");
            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Events, "EventId", "Name");
            var booking = new Booking { BookingRef = GenerateRef(), BookingDate = System.DateTime.Now };
            return View(booking);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingRef,BookingDate,GuestName,GuestEmail,Status,VenueId,EventId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Errors"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Events, "EventId", "Name", booking.EventId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.FindAsync(id.Value);
            if (booking == null) return NotFound();
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Events, "EventId", "Name", booking.EventId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingRef,BookingDate,GuestName,GuestEmail,Status,VenueId,EventId")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Booking updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(b => b.BookingId == booking.BookingId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["Errors"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            ViewData["VenueId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            ViewData["EventId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Events, "EventId", "Name", booking.EventId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.FindAsync(id.Value);
            if (booking == null) return NotFound();
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private string GenerateRef()
        {
            return "BKG" + System.DateTime.Now.Ticks.ToString().Substring(0,6);
        }
    }
}
