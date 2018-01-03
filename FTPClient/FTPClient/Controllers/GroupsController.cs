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
using FTPClient.DAL.Services.Response;
using FTPClient.Models;

namespace FTPClient.Controllers
{
    public class GroupsController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups.Include(g => g.Admin).Include(g => g.RootDirectory);
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.AdminId = new SelectList(db.Users, "Id", "Login");
            return View();
        }

        // POST: Groups/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AdminId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Directory dir = new Directory() {Name = group.Name + "Root", ParentDirectoryId = 1};
                        db.Directories.Add(dir);

                        group.RootDirectory = dir;

                        db.Groups.Add(group);

                        DirectoryAccess dirAccess =
                            new DirectoryAccess()
                            {
                                UserId = group.AdminId,
                                Directory = dir,
                                AccessType = 7,
                                Permissions = 7
                            };
                        db.DirectoryAccesses.Add(dirAccess);
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

            ViewBag.AdminId = new SelectList(db.Users, "Id", "Login", group.AdminId);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminId = new SelectList(db.Users, "Id", "Login", group.AdminId);
            ViewBag.RootDirectoryId = new SelectList(db.Directories, "Id", "Name", group.RootDirectoryId);
            return View(group);
        }

        // POST: Groups/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AdminId,RootDirectoryId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminId = new SelectList(db.Users, "Id", "Login", group.AdminId);
            ViewBag.RootDirectoryId = new SelectList(db.Directories, "Id", "Name", group.RootDirectoryId);
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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
