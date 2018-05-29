using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{
    public partial class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            return View("CAB");
        }

        public ActionResult CAB()
        {
            return View();
        }
        public ActionResult OJewelry()
        {
            return View();
        }

        public ActionResult Equity()
        {
            return View();
        }
    }
}