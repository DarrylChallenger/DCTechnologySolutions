﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCTechnologySolutions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            HomeController home = new HomeController();
            ActionResult actionResult = home.Index();
            Assert.Fail();
        }
    }
}