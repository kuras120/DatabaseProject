﻿using System;
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
        public ActionResult Create([Bind(Include = "Id,DirectoryId,Name,Path,Size,UploadTime")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(file);
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
        [HttpPost]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFile(HttpPostedFileBase file, int fileFolderID, int accessType, int permission)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Home");

            if(file == null || file.ContentLength == 0)
            {
                TempData["FileUploadErrorOccured"] = true;
                TempData["FileUploadErrorMessage"] = "Nie udało się przekazać pliku";
                return RedirectToAction("Index", "Users");
            }

            var fileName = System.IO.Path.GetFileName(file.FileName);
            var path = System.IO.Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);

            File addedFile = new Models.File();
            addedFile.DirectoryId = fileFolderID;
            addedFile.Name = fileName;
            addedFile.Path = path;
            addedFile.Size = 17; // determine that later
            addedFile.UploadTime = DateTime.Now;

            db.Files.Add(addedFile);

            FileAccess addedFileAccess = new FileAccess();
            addedFileAccess.AccessType = accessType;
            addedFileAccess.FileId = addedFile.Id;
            addedFileAccess.Permissions = permission;
            addedFileAccess.UserId = (int)Session["UserID"];

            db.FileAccesses.Add(addedFileAccess);
            db.SaveChanges();

            TempData["targetDirId"] = fileFolderID;

            return RedirectToAction("goToDirectory", "Directories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeName(int fileId, string newName)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Home");

            var file = db.Files.Where(f => f.Id == fileId).FirstOrDefault();
            file.Name = newName;
            TempData["targetDirId"] = file.DirectoryId;

            db.SaveChanges();
            return RedirectToAction("goToDirectory", "Directories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int fileId)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Home");

            // Here should be check if that user can delete this file

            var file = db.Files.Where(f => f.Id == fileId).FirstOrDefault();
            var fileAccess = db.FileAccesses.Where(fa => fa.FileId == fileId);

            TempData["targetDirId"] = file.DirectoryId;

            db.Files.Remove(file);
            db.FileAccesses.RemoveRange(fileAccess);

            db.SaveChanges();

            return RedirectToAction("goToDirectory", "Directories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult Download(int fileId)
        {
            var file = db.Files.Where(f => f.Id == fileId).FirstOrDefault();
            var fileStream = new FileStreamResult(new System.IO.FileStream(file.Path, System.IO.FileMode.Open), "binary");
            fileStream.FileDownloadName = file.Name;
            return fileStream;
        }
    }
}
