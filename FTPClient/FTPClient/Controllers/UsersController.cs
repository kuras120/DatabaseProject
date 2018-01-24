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
    public struct DirData { public Directory dir; public DirectoryAccess dirAccess; };
    public struct FileData { public File file; public FileAccess fileAccess; };

    public class UsersController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
               return RedirectToAction("Index", "Home");

            if(TempData["FileUploadErrorOccured"] != null)
            {
                ViewBag.FileUploadErrorOccured = TempData["FileUploadErrorOccured"];
                ViewBag.FileUploadErrorMessage = TempData["FileUploadErrorMessage"];
            }

            List<DirData> dirList = new List<DirData>();

            int userID = (int)Session["UserID"];
            int currDirId = 0;
            if(TempData["currentDirectoryID"] == null)
            {
                var userAccesses = db.DirectoryAccesses.Where(da => da.UserId == userID && da.AccessType == 1);
                foreach(var acc in userAccesses)
                {
                    var dirToCheck = db.Directories.Where(d => d.Id == acc.DirectoryId && d.ParentDirectory == null).FirstOrDefault();

                    if (dirToCheck != null)
                    {
                        currDirId = dirToCheck.Id;
                        ViewBag.ParentFolderID = dirToCheck.ParentDirectoryId;
                        break;
                    }
                }
            }
            else
            {
                currDirId = (int)TempData["currentDirectoryID"];
                var currDir = db.Directories.Where(d => d.Id == currDirId).FirstOrDefault();
                ViewBag.ParentFolderID = currDir.ParentDirectoryId;
            }

            ViewBag.CurrentDirectoryID = currDirId;
            var dirAccesses = db.DirectoryAccesses.Where(da => da.UserId == userID && da.AccessType == 1);           
            foreach(var dirAccess in dirAccesses)
            {
                var dir = db.Directories.Where(a => a.Id == dirAccess.DirectoryId && a.ParentDirectoryId==currDirId).FirstOrDefault();

                if(dir != null)
                {
                    DirData newData = new DirData();
                    newData.dir = dir;
                    newData.dirAccess = dirAccess;
                    dirList.Add(newData);
                }
            }

            ViewBag.userDirectories = dirList;


            List<FileData> fileDataList = new List<FileData>();
            var filesFromCurrDir = db.Files.Where(f => f.DirectoryId == currDirId);
            foreach(var file in filesFromCurrDir)
            {
                var fileAccess = db.FileAccesses.Where(fa => fa.FileId == file.Id && fa.UserId == userID).First();
                FileData newFile = new FileData();
                newFile.file = file;
                newFile.fileAccess = fileAccess;
                fileDataList.Add(newFile);
            }

            ViewBag.userFiles = fileDataList;

            return View();
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
            return RedirectToAction("Index");
        }
    }
}
