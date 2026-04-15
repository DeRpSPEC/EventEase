using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(150)]
        [Display(Name = "Venue Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(250)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 100000, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
