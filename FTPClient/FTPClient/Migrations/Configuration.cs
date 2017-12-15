using System.Collections.Generic;
using FTPClient.Models;

namespace FTPClient.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FTPClient.DAL.DataModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FTPClient.DAL.DataModel context)
        {

            /*
            var users = new List<User>
            {
                new User {Login = "admin", Password = "admin", LastPasswordChange = DateTime.Now, SignUpDate = DateTime.Now},
                new User {Login = "admin1", Password = "admin1", LastPasswordChange = DateTime.Now, SignUpDate = DateTime.Now},
                new User {Login = "Janusz", Password = "SzybkieSumatoryCSA", LastPasswordChange = DateTime.Now, SignUpDate = DateTime.Now},
                new User {Login = "Jerzy", Password = "CalkaZEDoX", LastPasswordChange = DateTime.Now, SignUpDate = DateTime.Now}
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
            */
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
