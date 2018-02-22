using DCTechnologySolutions.OJDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{


    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.OnHome = "Y";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Equity()
        {
            ViewBag.Message = "Your Equity page.";

            return View();
        }

        public ActionResult CAB()
        {
            ViewBag.Message = "Your CAB page.";

            return View();
        }
        
        public ActionResult DevResources()
        {
            ViewBag.Message = "Your DevResources page.";

            return View();
        }

        public ActionResult OnlineProfile()
        {
            ViewBag.Message = "Online Profile page.";

            return View();
        }

        public ActionResult AppDev()
        {
            ViewBag.Message = "Application Development page.";

            return View();
        }

        public ActionResult Leadership()
        {
            ViewBag.Message = "Leadership page.";

            return View();
        }

        public ActionResult Consulting()
        {
            ViewBag.Message = "YConsulting page.";

            return View();
        }

        public ActionResult Conversions()
        {
            ViewBag.Message = "Conversions page.";

            return View();
        }

        public ActionResult Kinect()
        {
            ViewBag.Message = "Kinect page.";

            return View();
        }

        public ActionResult ThankYou()
        {
            ViewBag.Message = "Your Thank You page.";

            return View();
        }

        public ActionResult StylesEdit(int StyleId)
        {
            OJewelryDB db = new OJewelryDB();
            Style style = db.Styles.Find(StyleId);
            return View(style);
        }
    }
}
