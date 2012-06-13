using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace owaitlist.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Location { get; set; }

        [Display(Name = "Wait time")]
        public TimeSpan WaitTime { get; set; }

        [Display(Name="auto-increment the wait time?")]
        public bool AutoIncrement { get; set; }

        [Display(Name = "specifiy the increment")]
        public TimeSpan Increment { get; set; }

        public String User { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public Restaurant()
        {
            WaitTime = new TimeSpan(0, 0, 0);
            Increment = new TimeSpan(0, 0, 0);
        }

        public String BannerImageUri { get; set; }
    }
}