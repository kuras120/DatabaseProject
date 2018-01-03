using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FTPClient.DAL;
using FTPClient.DAL.Services.Request;
using FTPClient.Migrations;
using FTPClient.Models;

namespace FTPClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*
            Database.SetInitializer(new DataInitializer());
            DataModel db = new DataModel();
            db.Database.Initialize(true);
            */

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            DataModel context = new DataModel();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            User user = context.Users.Where(e => e.Login == "FEZHSH5FYW").FirstOrDefault();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            System.IO.File.WriteAllText("F:\\UCZELNIA\\sprawka\\Users.txt", elapsedMs.ToString());

            watch = System.Diagnostics.Stopwatch.StartNew();
            Directory dir = context.Directories.Where(e => e.Name == "OUUA73UFXZ'sFolder").FirstOrDefault();

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;

            System.IO.File.WriteAllText("F:\\UCZELNIA\\sprawka\\Directories.txt", elapsedMs.ToString());

            watch = System.Diagnostics.Stopwatch.StartNew();
            File file = context.Files.Where(e => e.Directory.Id == 193).FirstOrDefault();

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;

            System.IO.File.WriteAllText("F:\\UCZELNIA\\sprawka\\Files.txt", elapsedMs.ToString());
            
        }
    }
}
