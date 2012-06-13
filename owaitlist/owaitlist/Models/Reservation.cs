using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace owaitlist.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public String User { get; set; }

        [Required(ErrorMessage="You need to add a phone number to be added to the waitlist")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage="You need to set the number of guests (including yourself)")]
        [Range(1, int.MaxValue, ErrorMessage = "There needs to be at least one guest")]
        public int Guests { get; set; }

        public int RestaurantId { get; set; }
    }
}