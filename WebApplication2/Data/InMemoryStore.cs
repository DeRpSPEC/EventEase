using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventEase.Models;

namespace EventEase.Data
{
    public class InMemoryStore
    {
        private readonly List<Event> _events = new();
        private readonly List<Venue> _venues = new();
        private readonly List<Booking> _bookings = new();
        private int _nextEventId = 1;
        private int _nextVenueId = 1;
        private int _nextBookingId = 1;

        public InMemoryStore()
        {
            // Seed with some sample data
            var v1 = new Venue { VenueId = _nextVenueId++, Name = "Main Hall", Location = "Downtown", Capacity = 500 };
            var v2 = new Venue { VenueId = _nextVenueId++, Name = "Open Arena", Location = "Riverside", Capacity = 2000 };
            _venues.Add(v1);
            _venues.Add(v2);

            var e1 = new Event { EventId = _nextEventId++, Name = "Summer Concert", StartDate = DateTime.Now.AddDays(7), EndDate = DateTime.Now.AddDays(7).AddHours(3), Venue = v2, VenueId = v2.VenueId, Description = "An outdoor music event." };
            var e2 = new Event { EventId = _nextEventId++, Name = "Tech Meetup", StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(3).AddHours(2), Venue = v1, VenueId = v1.VenueId, Description = "Local developer meetup." };
            _events.Add(e1);
            _events.Add(e2);

            var b1 = new Booking { BookingId = _nextBookingId++, BookingRef = "BKG001", BookingDate = DateTime.Now, GuestName = "Alice", GuestEmail = "alice@example.com", EventId = e1.EventId, VenueId = e1.VenueId, Status = "Confirmed" };
            _bookings.Add(b1);
        }

        // Events
        public Task<List<Event>> GetEventsAsync() => Task.FromResult(_events.ToList());
        public Task<Event?> FindEventAsync(int id) => Task.FromResult(_events.FirstOrDefault(e => e.EventId == id));
        public Task AddEventAsync(Event ev)
        {
            ev.EventId = _nextEventId++;
            _events.Add(ev);
            return Task.CompletedTask;
        }
        public Task UpdateEventAsync(Event ev)
        {
            var idx = _events.FindIndex(e => e.EventId == ev.EventId);
            if (idx >= 0) _events[idx] = ev;
            return Task.CompletedTask;
        }
        public Task RemoveEventAsync(int id)
        {
            _events.RemoveAll(e => e.EventId == id);
            return Task.CompletedTask;
        }

        // Venues
        public Task<List<Venue>> GetVenuesAsync() => Task.FromResult(_venues.ToList());
        public Task<Venue?> FindVenueAsync(int id) => Task.FromResult(_venues.FirstOrDefault(v => v.VenueId == id));
        public Task AddVenueAsync(Venue v)
        {
            v.VenueId = _nextVenueId++;
            _venues.Add(v);
            return Task.CompletedTask;
        }
        public Task UpdateVenueAsync(Venue v)
        {
            var idx = _venues.FindIndex(x => x.VenueId == v.VenueId);
            if (idx >= 0) _venues[idx] = v;
            return Task.CompletedTask;
        }
        public Task RemoveVenueAsync(int id)
        {
            _venues.RemoveAll(v => v.VenueId == id);
            return Task.CompletedTask;
        }

        // Bookings
        public Task<List<Booking>> GetBookingsAsync() => Task.FromResult(_bookings.ToList());
        public Task<Booking?> FindBookingAsync(int id) => Task.FromResult(_bookings.FirstOrDefault(b => b.BookingId == id));
        public Task AddBookingAsync(Booking b)
        {
            b.BookingId = _nextBookingId++;
            _bookings.Add(b);
            return Task.CompletedTask;
        }
        public Task UpdateBookingAsync(Booking b)
        {
            var idx = _bookings.FindIndex(x => x.BookingId == b.BookingId);
            if (idx >= 0) _bookings[idx] = b;
            return Task.CompletedTask;
        }
        public Task RemoveBookingAsync(int id)
        {
            _bookings.RemoveAll(b => b.BookingId == id);
            return Task.CompletedTask;
        }

        // Counts
        public Task<int> GetVenueCountAsync() => Task.FromResult(_venues.Count);
        public Task<int> GetEventCountAsync() => Task.FromResult(_events.Count);
        public Task<int> GetBookingCountAsync() => Task.FromResult(_bookings.Count);

        public Task<List<Event>> GetUpcomingEventsAsync(int take = 4)
        {
            var list = _events.Where(e => e.StartDate >= DateTime.Now).OrderBy(e => e.StartDate).Take(take).ToList();
            return Task.FromResult(list);
        }
    }
}
