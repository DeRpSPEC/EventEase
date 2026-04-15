using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [StringLength(150)]
        [Display(Name = "Event Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Foreign key (simple representation)
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        // Navigation property (optional)
        public Venue? Venue { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
