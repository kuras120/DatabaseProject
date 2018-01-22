using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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

          
        }
    }
}
