using FTPClient.DAL;
using FTPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FTPClient.Utilities;
using Microsoft.Ajax.Utilities;

namespace FTPClient.Controllers
{
    public class HomeController : Controller
    {
        DataModel context = new DataModel();

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
                ViewBag.SignupErrorMessage = TempData["sigupErrorMessage"];
                TempData.Remove("signupErrorOccured");
                TempData.Remove("sigupErrorMessage");
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
                var probablyGoodUser = context.Users.Where(a => a.Login.Equals(loginUser.Login)).FirstOrDefault();
                if (probablyGoodUser != null)
                {
                    System.Diagnostics.Debug.WriteLine("Prawdopodobny login");
                    System.Diagnostics.Debug.WriteLine(loginUser.Login);
                    System.Diagnostics.Debug.WriteLine("Prawdopodobne haslo");
                    System.Diagnostics.Debug.WriteLine(loginUser.Password);

                    System.Diagnostics.Debug.WriteLine("login");
                    System.Diagnostics.Debug.WriteLine(probablyGoodUser.Login);
                    System.Diagnostics.Debug.WriteLine("haslo");
                    System.Diagnostics.Debug.WriteLine(probablyGoodUser.Password);

                    HashAlgorithm algorithm = SHA256.Create();
                    String salt = loginUser.Login + probablyGoodUser.SignUpDate;
                    System.Diagnostics.Debug.WriteLine("Sol przy logowaniu");
                    System.Diagnostics.Debug.WriteLine(salt);
                    Hash hash = new Hash(algorithm, salt, loginUser.Password);
                    var hashedPassword = hash.String();
                    algorithm.Clear();

                    System.Diagnostics.Debug.WriteLine("zahaszowane");
                    System.Diagnostics.Debug.WriteLine(hashedPassword);
                    System.Diagnostics.Debug.WriteLine("stare haslo");
                    System.Diagnostics.Debug.WriteLine(probablyGoodUser.Password);

                    if (probablyGoodUser.Password == hashedPassword)
                    {
                        Session["UserID"] = probablyGoodUser.Id;
                        Session["UserLogin"] = probablyGoodUser.Login;

                        return RedirectToAction("UserDashBoard");
                    }
                }

            }
            // If true, error message will be displayed
            TempData["loginErrorOccured"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult LoginFromRegister(User loginUser)
        {
            if (ModelState.IsValid)
            {
                var probablyGoodUser = context.Users.Where(a => a.Login.Equals(loginUser.Login)).FirstOrDefault();
                if (probablyGoodUser != null)
                {
                    if (probablyGoodUser.Login == loginUser.Login && probablyGoodUser.Password == loginUser.Password)
                    {

                        Session["UserID"] = probablyGoodUser.Id;
                        Session["UserLogin"] = probablyGoodUser.Login;

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
                var obj = context.Users.Where(a => a.Login.Equals(newUser.Login));

                if (obj.Count() != 0)
                {
                    TempData["signupErrorOccured"] = true;
                    TempData["sigupErrorMessage"] = "Login jest już zajęty";
                    return RedirectToAction("Index");

                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        newUser.SignUpDate = DateTime.Now;
                        newUser.LastPasswordChange = DateTime.Now;
                        
                        HashAlgorithm algorithm = SHA256.Create();
                        String salt = newUser.Login + newUser.SignUpDate;
                        System.Diagnostics.Debug.WriteLine("Sol przy rejestracji");
                        System.Diagnostics.Debug.WriteLine(salt);
                        Hash hash = new Hash(algorithm, salt, newUser.Password);
                        newUser.Password = hash.String();
                        algorithm.Clear();

                        context.Users.Add(newUser);

                        Directory newUserDirectory = new Directory();
                        newUserDirectory.Name = newUser.Login;

                        DirectoryAccess access = new DirectoryAccess();
                        access.Directory = newUserDirectory;
                        access.User = newUser;
                        access.AccessType = 1;
                        access.Permissions = 7;

                        context.Directories.Add(newUserDirectory);
                        context.DirectoryAccesses.Add(access);
                        
                        context.SaveChanges();
                        transaction.Commit();

                        return LoginFromRegister(newUser);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        TempData["signupErrorOccured"] = true;
                        TempData["sigupErrorMessage"] = "Serwer nie odpowiada";
                        transaction.Rollback();
                        return RedirectToAction("Index");
                    }

                }
        }
    }
}