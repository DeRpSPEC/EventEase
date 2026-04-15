using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Display(Name = "Booking Reference")]
        [StringLength(20)]
        public string BookingRef { get; set; } = string.Empty;

        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Guest name is required.")]
        [StringLength(150)]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Guest email is required.")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Guest Email")]
        public string GuestEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Confirmed";

        // Foreign keys (simple representation)
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [Display(Name = "Event")]
        public int EventId { get; set; }

        // Navigation properties
        public Venue? Venue { get; set; }
        public Event? Event { get; set; }
    }
}
