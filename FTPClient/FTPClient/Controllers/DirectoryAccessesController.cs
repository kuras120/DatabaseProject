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
    public class DirectoryAccessesController : Controller
    {
        private DataModel db = new DataModel();

        // GET: DirectoryAccesses
        public ActionResult Index()
        {
            var directoryAccesses = db.DirectoryAccesses.Include(d => d.Directory).Include(d => d.User);
            return View(directoryAccesses.ToList());
        }

        // GET: DirectoryAccesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DirectoryAccess directoryAccess = db.DirectoryAccesses.Find(id);
            if (directoryAccess == null)
            {
                return HttpNotFound();
            }
            return View(directoryAccess);
        }

        // GET: DirectoryAccesses/Create
        public ActionResult Create()
        {
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login");
            return View();
        }

        // POST: DirectoryAccesses/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,DirectoryId,AccessType,Permissions")] DirectoryAccess directoryAccess)
        {
            if (ModelState.IsValid)
            {
                db.DirectoryAccesses.Add(directoryAccess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", directoryAccess.DirectoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", directoryAccess.UserId);
            return View(directoryAccess);
        }

        // GET: DirectoryAccesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DirectoryAccess directoryAccess = db.DirectoryAccesses.Find(id);
            if (directoryAccess == null)
            {
                return HttpNotFound();
            }
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", directoryAccess.DirectoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", directoryAccess.UserId);
            return View(directoryAccess);
        }

        // POST: DirectoryAccesses/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,DirectoryId,AccessType,Permissions")] DirectoryAccess directoryAccess)
        {
            if (ModelState.IsValid)
            {
                db.Entry(directoryAccess).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DirectoryId = new SelectList(db.Directories, "Id", "Name", directoryAccess.DirectoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Login", directoryAccess.UserId);
            return View(directoryAccess);
        }

        // GET: DirectoryAccesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DirectoryAccess directoryAccess = db.DirectoryAccesses.Find(id);
            if (directoryAccess == null)
            {
                return HttpNotFound();
            }
            return View(directoryAccess);
        }

        // POST: DirectoryAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DirectoryAccess directoryAccess = db.DirectoryAccesses.Find(id);
            db.DirectoryAccesses.Remove(directoryAccess);
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
