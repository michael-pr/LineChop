using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using owaitlist.Models;
using System.IO;
using System.Drawing;

namespace owaitlist.Controllers 
{
    [Authorize]
    public class ManageController : Controller
    {
        OwlEntities db = new OwlEntities();

        public ActionResult Index()
        {
            var restaurant = db.Restaurants.Where(r => r.User.Equals(User.Identity.Name)).First();
            return View(restaurant);
        }

        public ActionResult Create()
        {
            var exists = db.Restaurants.Where(r => r.User.Equals(User.Identity.Name)).Count() > 0;
            if (exists)
                return Index();
            else
                return View(new Restaurant() { User = User.Identity.Name });
        }

        [HttpPost]
        public ActionResult Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return Index();
            }
            return View();
        }

        public ActionResult Settings()
        {
            var restaurant = db.Restaurants.Where(r => r.User.Equals(User.Identity.Name)).FirstOrDefault();
            return View(restaurant);
        }

        public ActionResult UpdateWaitTime(int Hours, int Minutes)
        {
            var restaurant = db.Restaurants.Where(r => r.User.Equals(User.Identity.Name)).FirstOrDefault();
            restaurant.WaitTime = new TimeSpan(Hours, Minutes, 0);
            db.SaveChanges();
            return PartialView("ViewWaitTime", restaurant.WaitTime);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(int id)
        {
            //read image into byte array
            HttpPostedFileBase file = Request.Files["file-location"];
            Int32 length = file.ContentLength;
            byte[] tempImage = new byte[length];
            file.InputStream.Read(tempImage, 0, length);

            //store image location in entity in database
            var restaurant = db.Restaurants.Find(id);
            restaurant.BannerImageUri = string.Format("/Banners/{0}.jpg", id);
            db.SaveChanges();

            //save image to the location, resize if needed
            MemoryStream stream = new MemoryStream(tempImage);
            Bitmap image = (Bitmap)Bitmap.FromStream(stream);
            if (image.Width != 1024 && image.Height != 300)
            {
                Bitmap newImage = new Bitmap(1024, 300);
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(image, 0, 0, 1024, 300);
                newImage.Save(Server.MapPath("~" + restaurant.BannerImageUri));
            }
            else
                image.Save(Server.MapPath("~" + restaurant.BannerImageUri));
            return RedirectToAction("Settings", "Manage");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var rest = db.Restaurants.Find(restaurant.Id);
                rest = restaurant;
                db.SaveChanges();
                ViewBag.Text = "Saved changes";
            }
            else
                ViewBag.Text = "Unabled to save changes";
            return PartialView("Update");
        }

        public ActionResult UpdateReservation(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var restaurant = db.Restaurants.Find(reservation.RestaurantId);
                if (reservation != null)
                {
                    var reserve = db.Reservations.Find(reservation.Id);
                    db.Reservations.Remove(reserve);
                    if (restaurant.AutoIncrement)
                        restaurant.WaitTime = restaurant.WaitTime.Subtract(new TimeSpan(restaurant.Increment.Ticks * reserve.Guests));
                    db.SaveChanges();
                    return PartialView("ViewReservation", restaurant.Reservations);
                }
            }
            return PartialView();
        }

        public ActionResult ViewReservation(int id)
        {
            var restaurant = db.Restaurants.Find(id);
            if (restaurant != null)
            {
                return PartialView(restaurant.Reservations);
            }
            else return PartialView();
        }

        public ActionResult ViewWaitTime(int id)
        {
            var restaurant = db.Restaurants.Find(id);
            if (restaurant != null)
            {
                return PartialView(restaurant.WaitTime);
            }
            else return PartialView();
        }
    }
}
