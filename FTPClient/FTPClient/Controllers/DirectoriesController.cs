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
    public class DirectoriesController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Directories
        public ActionResult Index()
        {
            var directories = db.Directories.Include(d => d.ParentDirectory);
            return View(directories.ToList());
        }

        // GET: Directories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = db.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }
            return View(directory);
        }

        // GET: Directories/Create
        public ActionResult Create()
        {
            ViewBag.ParentDirectoryId = new SelectList(db.Directories, "Id", "Name");
            return View();
        }

        // POST: Directories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentDirectoryId,Name")] Directory directory)
        {
            if (ModelState.IsValid)
            {
                db.Directories.Add(directory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentDirectoryId = new SelectList(db.Directories, "Id", "Name", directory.ParentDirectoryId);
            return View(directory);
        }

        // GET: Directories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = db.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentDirectoryId = new SelectList(db.Directories, "Id", "Name", directory.ParentDirectoryId);
            return View(directory);
        }

        // POST: Directories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentDirectoryId,Name")] Directory directory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(directory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentDirectoryId = new SelectList(db.Directories, "Id", "Name", directory.ParentDirectoryId);
            return View(directory);
        }

        // GET: Directories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = db.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }
            return View(directory);
        }

        // POST: Directories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Directory directory = db.Directories.Find(id);
            db.Directories.Remove(directory);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDirectory(string directoryName, int upperDirId, int accessType, int permission)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Directory dir = new Directory();
            dir.Name = directoryName;
            dir.ParentDirectoryId = upperDirId;
            db.Directories.Add(dir);
            DirectoryAccess dirAccess = new DirectoryAccess();
            dirAccess.AccessType = accessType;
            dirAccess.Permissions = permission;
            dirAccess.UserId = (int)Session["UserID"];
            dirAccess.DirectoryId = dir.Id;
            db.DirectoryAccesses.Add(dirAccess);

            db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }
    }
}
