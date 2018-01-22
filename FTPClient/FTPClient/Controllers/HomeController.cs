using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTPClient.DAL;
using FTPClient.Models;

namespace FTPClient.Controllers
{
    public class HomeController : Controller
    {

        DataModel context = new DataModel();

        public ActionResult Index()
        {
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

        public ActionResult Login()
        {

            return View("Index");
        }

        public ActionResult Signup()
        {

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "loginInput,passwordInput")] User user)
        {
            var findUser = context.Users.Where(x => x.Login == user.Login).FirstOrDefault();
            if (findUser == null)
            {
                if (ModelState.IsValid)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ViewBag.SignupError = true;
                return View(ViewBag);
            }
            ViewBag.SignupError = false;
            return View(ViewBag);
        }
    }
}