using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using owaitlist.Models;

namespace owaitlist.Controllers
{
    public class HomeController : Controller
    {
        OwlEntities db = new OwlEntities();

        [OutputCache(Duration = 86400, VaryByParam = "none", Location=System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string query, string location)
        {
            ViewBag.Query = query;
            ViewBag.Location = location;
            return View();
        }

        public ActionResult Find(string query)
        {
            var restaurants = from r in db.Restaurants
                              where r.Name.ToLower().Contains(query.ToLower()) ||
                              r.Location.ToLower().Contains(query.ToLower())
                              select r;
            return PartialView(restaurants.ToList());
        }

        public ActionResult Show(int id)
        {
            var restaurant = db.Restaurants.Find(id);
            return View(restaurant);
        }

        [Authorize]
        public ActionResult Reserve(Reservation model)
        {
            if (ModelState.IsValid)
            {
                var restaurant = db.Restaurants.Where(r => r.Id == model.RestaurantId).SingleOrDefault();
                if (restaurant != null)
                {
                    db.Reservations.Add(model);
                    if (restaurant.AutoIncrement)
                        restaurant.WaitTime = restaurant.WaitTime.Add(new TimeSpan((restaurant.Increment.Ticks * model.Guests)));
                    db.SaveChanges();
                    ViewBag.Message = "Your reservation was added";
                    return PartialView();
                }
                else
                {
                    ViewBag.Message = "Error adding reservation: no restaurant set";
                    return PartialView();
                }
            }
            ViewBag.Message = "There was an error adding your reservation";
            return PartialView();
        }

        public ActionResult LocationTest()
        {
            return View();
        }
    }
}
