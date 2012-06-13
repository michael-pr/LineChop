using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace owaitlist.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string error)
        {
            ViewData["Title"] = "There's something wrong";
            ViewData["Description"] = error;
            return View();
        }


        public ActionResult HttpError404(string error)
        {
            ViewData["Title"] = "Uh oh! Couldn't find that";
            ViewData["Description"] = "There's a good chance that the page you were looking for doesn't exist.";
            return View("Error");
        }

        public ActionResult HttpError500(string error)
        {
            ViewData["Title"] = "Yikes! The server is having some issues";
            ViewData["Description"] = "The desk gnomes running our server are rioting. Check back later when we've calmed them down!";
            return View("Error");
        }

        public ActionResult AccessDenied(string error)
        {
            ViewData["Title"] = "Access is denied.";
            ViewData["Description"] = "You are not authorized for this operation. Please contact administrator.";
            return View("Error");
        }

        public ActionResult General(string error)
        {
            ViewData["Title"] = "Sorry, an error occurred while processing your request.";
            ViewData["Description"] = error;
            return View("Error");
        }
    }
}
