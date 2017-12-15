using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FTPClient.Models;

namespace FTPClient.DAL
{
    public class DataInitializer:System.Data.Entity.DropCreateDatabaseIfModelChanges<DataModel>
    {
        protected override void Seed(FTPClient.DAL.DataModel context)
        {
            var users = new List<User>
            {
                new User {Login = "admin", Password = "admin"},
                new User {Login = "admin1", Password = "admin1"}
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
        }
    }
}