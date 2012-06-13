using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwilioSharp.Request;
using TwilioSharp.MVC3.Controllers;
using owaitlist.Models;

namespace owaitlist.Controllers
{
    public class SMSController : TwiMLController
    {
        OwlEntities db = new OwlEntities();

        [HttpPost]
        public ActionResult Respond(TextRequest request)
        {
            string text = request.Body;
            string response;
            if (text.ToLower().Contains("check"))
            {
                int id = int.Parse(text.Split(' ')[1]);
                var restaurant = db.Restaurants.Find(id);
                if (restaurant != null)
                    response = "Wait time for " + restaurant.Name + " is " + restaurant.WaitTime.Hours + " hours " + restaurant.WaitTime.Minutes + " minutes. Text "
                        + "Reserve " + restaurant.Id + " [# of guests] to join waitlist";
                else
                    response = "Could not find restaurant by that id";
                return TwiML(sms => sms.Sms(response));
            }
            else if (text.ToLower().Contains("reserve"))
            {
                string[] parms = text.Split(' ');
                int id = int.Parse(parms[1]);
                int guests = int.Parse(parms[2]);
                var restaurant = db.Restaurants.Find(id);
                if (restaurant != null)
                {
                    if (guests >= 1)
                    {
                        restaurant.Reservations.Add(new Reservation() { PhoneNumber = request.From, Guests = guests, RestaurantId = id });
                        if(restaurant.AutoIncrement)
                            restaurant.WaitTime = restaurant.WaitTime.Add(new TimeSpan((restaurant.Increment.Ticks * guests)));
                        db.SaveChanges();
                        response = "You have been added to the waitlist with " + guests + " guests";
                    }
                    else
                        response = "There needs to be at least 1 guest";
                }
                else
                    response = "Could not find restaurant by that id";
                return TwiML(sms => sms.Sms(response));
            }
            else
            {
                return TwiML(sms => sms.Sms("Invalid request"));
            }
        }

    }
}
