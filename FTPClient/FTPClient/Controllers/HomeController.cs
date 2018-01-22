using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTPClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // If true, error message will be displayed
            ViewBag.isLoginError = false;
            // If true, error message will be displayed
            ViewBag.isSignUpError = false;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}