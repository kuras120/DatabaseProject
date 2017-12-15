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

            for (var i = 0; i < 100; i++)
            {
                System.Diagnostics.Debug.Write("-");
            }
            System.Diagnostics.Debug.Write("\n\n");

            try
            {
                DataModel context = new DataModel();
                Request<User> request = new Request<User>(context);
                User user = new User
                {
                    Login = "Wojciech",
                    Password = "admin1",
                    SignUpDate = DateTime.Now,
                    LastPasswordChange = DateTime.Now
                };
                request.DeleteAll();

                System.Diagnostics.Debug.WriteLine("Udalo sie!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            for (var i = 0; i < 100; i++)
            {
                System.Diagnostics.Debug.Write("-");
            }
            System.Diagnostics.Debug.Write("\n\n");
        }
    }
}
