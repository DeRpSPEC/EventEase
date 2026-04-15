using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Data;

namespace EventEase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EventEaseDbContext _context;

        public IndexModel(EventEaseDbContext context)
        {
            _context = context;
        }

        public int VenueCount { get; set; }
        public int EventCount { get; set; } = 0;
        public int BookingCount { get; set; }
        public List<Event> Events { get; set; } = new();

        public async Task OnGetAsync()
        {
            VenueCount = await _context.Venues.CountAsync();
            EventCount = await _context.Events.CountAsync();
            BookingCount = await _context.Bookings.CountAsync();

            Events = await _context.Events
                .Include(e => e.Venue)
                .Where(e => e.StartDate >= System.DateTime.Now)
                .OrderBy(e => e.StartDate)
                .Take(4)
                .ToListAsync();
        }
    }
}
