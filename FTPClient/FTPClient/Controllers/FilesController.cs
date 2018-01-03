using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FTPClient.DAL;
using FTPClient.DAL.Services.Request;
using FTPClient.Models;

namespace FTPClient.Controllers
{
    public class FilesController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Files
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.Directory);
            return View(files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name");
            return View();
        }

        // POST: Files/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DirectoryId,Name,Path,Size")] File file)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        file.UploadTime = DateTime.Now;

                        db.Files.Add(file);

                        List<DirectoryAccess> accesses = db.DirectoryAccesses.Where(e => e.DirectoryId == file.DirectoryId).ToList();

                        foreach (var access in accesses)
                        {
                            if (access.User.Login == "admin")
                            {
                                FileAccess ac =
                                    new FileAccess() {AccessType = 7, File = file, Permissions = 7, User = access.User};
                                db.FileAccesses.Add(ac);
                            }
                            else
                            {
                                FileAccess ac1 =
                                    new FileAccess() {AccessType = 3, File = file, Permissions = 3, User = access.User};
                                db.FileAccesses.Add(ac1);
                            }
                        }
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", file.DirectoryId);
            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", file.DirectoryId);
            return View(file);
        }

        // POST: Files/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DirectoryId,Name,Path,Size,UploadTime")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", file.DirectoryId);
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
    }
}
