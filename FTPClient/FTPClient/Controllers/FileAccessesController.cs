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
    public class FileAccessesController : Controller
    {
        private DataModel db = new DataModel();

        // GET: FileAccesses
        public ActionResult Index()
        {
            var fileAccesses = db.FileAccesses.Include(f => f.File).Include(f => f.User);
            return View(fileAccesses.ToList());
        }

        // GET: FileAccesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileAccess fileAccess = db.FileAccesses.Find(id);
            if (fileAccess == null)
            {
                return HttpNotFound();
            }
            return View(fileAccess);
        }

        // GET: FileAccesses/Create
        public ActionResult Create()
        {
            ViewBag.FileId = new SelectList(db.Files, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login");
            return View();
        }

        // POST: FileAccesses/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FileId,AccessType,Permissions")] FileAccess fileAccess)
        {
            if (ModelState.IsValid)
            {
                db.FileAccesses.Add(fileAccess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FileId = new SelectList(db.Files, "Id", "Name", fileAccess.FileId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", fileAccess.UserId);
            return View(fileAccess);
        }

        // GET: FileAccesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileAccess fileAccess = db.FileAccesses.Find(id);
            if (fileAccess == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileId = new SelectList(db.Files, "Id", "Name", fileAccess.FileId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", fileAccess.UserId);
            return View(fileAccess);
        }

        // POST: FileAccesses/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FileId,AccessType,Permissions")] FileAccess fileAccess)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fileAccess).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileId = new SelectList(db.Files, "Id", "Name", fileAccess.FileId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", fileAccess.UserId);
            return View(fileAccess);
        }

        // GET: FileAccesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileAccess fileAccess = db.FileAccesses.Find(id);
            if (fileAccess == null)
            {
                return HttpNotFound();
            }
            return View(fileAccess);
        }

        // POST: FileAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileAccess fileAccess = db.FileAccesses.Find(id);
            db.FileAccesses.Remove(fileAccess);
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
