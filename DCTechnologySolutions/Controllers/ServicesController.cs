using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult Index()
        {
            return View("AppDev");
        }

        public ActionResult OnlineProfile()
        {
            return View();
        }

        public ActionResult AppDev()
        {
            return View();
        }

        public ActionResult Leadership()
        {
            return View();
        }

        public ActionResult Consulting()
        {
            return View();
        }

        public ActionResult Kinect()
        {
            return View();
        }

    }
}