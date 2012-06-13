using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using owaitlist.Models;

namespace owaitlist.Controllers
{
    public class ResourceController : Controller
    {
        OwlEntities db = new OwlEntities();

        public ActionResult GetPhoto(int id)
        {
            
            return View();
        }

    }
}
