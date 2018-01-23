using FTPClient.DAL;
using FTPClient.Models;
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
            if(TempData.ContainsKey("loginErrorOccured"))
            {
                ViewBag.isLoginError = TempData["loginErrorOccured"];
                TempData.Remove("loginErrorOccured");
            }
            if (TempData.ContainsKey("signupErrorOccured"))
            {
                ViewBag.isSignupError = TempData["signupErrorOccured"];
                TempData.Remove("signupErrorOccured");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult Login(User loginUser)
        {
            if(ModelState.IsValid)
            {
                using (DataModel db = new DataModel())
                {
                    var obj = db.Users.Where(a => a.Login.Equals(loginUser.Login) && a.Password.Equals(loginUser.Password));
                    if(obj.ToList().Count  != 0)
                    {
                        var foundUser = obj.First();
                        Session["UserID"] = foundUser.Id;
                        Session["UserLogin"] = foundUser.Login;

                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            // If true, error message will be displayed
            TempData["loginErrorOccured"] = true;
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("UserDashboard", "Users");
            }
            else
                return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult Signup(User newUser)
        {
            using (var db = new DataModel())
            {
                var obj = db.Users.Where(a => a.Login.Equals(newUser.Login));

                if(obj.Count() != 0)
                {
                    TempData["signupErrorOccured"] = true;
                    return RedirectToAction("Index");

                }
                newUser.SignUpDate = DateTime.Now;
                newUser.LastPasswordChange = DateTime.Now;
                db.Users.Add(newUser);
                db.SaveChanges();
                return Login(newUser);
            }
        }
    }
}