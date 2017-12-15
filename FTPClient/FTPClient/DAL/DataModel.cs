using System.Data.Entity.ModelConfiguration.Conventions;
using FTPClient.DAL;
using FTPClient.Migrations;
using FTPClient.Models;

namespace FTPClient.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataModel : DbContext
    {
        // Your context has been configured to use a 'DataModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FTPClient.DataModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DataModel' 
        // connection string in the application configuration file.
        public DataModel()
            : base("name=DataModel")
        {
            Database.SetInitializer(new DataInitializer());

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Group> Groups { set; get; }
        public virtual DbSet<FileAccess> FileAccesses { get; set; }
        public virtual DbSet<DirectoryAccess> DirectoryAccesses { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}