using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FTPClient.DAL;
using FTPClient.Models;

namespace FTPClient.Controllers
{
    public class UsersController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Users
        public ActionResult Index()
        {
            ViewBag.userDirectories = db.Directories.ToList();
            ViewBag.userFiles = db.Files.ToList();
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Users/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Login,Password,SignUpDate,LastPasswordChange")] User user)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Users.Add(user);

                        Directory directory = new Directory() { ParentDirectoryId = 1, Name = user.Login + "'sFolder" };
                        db.Directories.Add(directory);

                        DirectoryAccess access =
                            new DirectoryAccess() { AccessType = 7, Directory = directory, Permissions = 7, User = user };
                        db.DirectoryAccesses.Add(access);

                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Login,Password,SignUpDate,LastPasswordChange")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);

            List<DirectoryAccess> directories = db.DirectoryAccesses.Where(e => e.User.Id == user.Id).ToList();
            foreach (var dir in directories)
            {
                db.Directories.RemoveRange(db.Directories.Where(e => e.Id == dir.DirectoryId));
            }

            db.DirectoryAccesses.RemoveRange(db.DirectoryAccesses.Where(e => e.User.Id == user.Id));

            List<FileAccess> fileAccesses = new List<FileAccess>();

            fileAccesses = db.FileAccesses.Where(e => e.User.Id == user.Id).ToList();
            foreach (var file in fileAccesses)
            {
                db.Files.RemoveRange(db.Files.Where(e => e.Id == file.FileId));
            }

            db.FileAccesses.RemoveRange(db.FileAccesses.Where(e => e.User.Id == user.Id));

            db.Users.Remove(user);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult UserDashboard()
        {
            if (Session["UserID"] == null)
                RedirectToAction("Index", "Home");

            ViewBag.userDirectories = db.Directories.ToList();
            ViewBag.userFiles = db.Files.ToList();
            return View("Index");
        }
    }
}
