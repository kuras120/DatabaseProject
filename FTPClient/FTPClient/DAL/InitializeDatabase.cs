using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using FTPClient.DAL.Services.Request;
using FTPClient.Models;

namespace FTPClient.DAL
{
    public class InitializeDatabase
    {
        private DataModel context;
        private Request<User> req;
        public InitializeDatabase()
        {
            context = new DataModel();
            //req = new Request<User>(context);
            //req.DeleteAll();

        }

        public void CreateFiles()
        {
            RandomString random1 = new RandomString();

            List<User> users = context.Users.ToList();
            int counter = 0;
            foreach (var user2 in users)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<DirectoryAccess> accDir = context.DirectoryAccesses.ToList();

                        DirectoryAccess dir = accDir[counter];

                            for (int i = 0; i < 10; i++)
                            {
                                String path = "/"+dir.Directory.Name;

                                Directory temp = dir.Directory;

                                while (temp.ParentDirectory != null)
                                {
                                    path += "/" + dir.Directory.ParentDirectory.Name;
                                    temp = temp.ParentDirectory;

                                }
                                File file = new File()
                                {
                                    Directory = dir.Directory,
                                    Name = user2.Login + "'sFile" + i,
                                    Path = Crypto.SHA256(path + user2.Login + "'sFile" + i),
                                    Size = 1024,
                                    UploadTime = random1.RandomDay()
                                };
                                context.Files.Add(file);

                                FileAccess fileAccess =
                                    new FileAccess() {AccessType = 7, File = file, Permissions = 7, User = user2};
                                context.FileAccesses.Add(fileAccess);

                            }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                counter++;
            }
        }
        public void CreateGroups()
        {

            List<User> users = context.Users.ToList();

            foreach (var user1 in users)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Group group = new Group() { Admin = user1, Name = user1.Login + "'sGroup"};
                        Directory dir = new Directory() { Name = group.Name + "Root", ParentDirectoryId = 1 };
                        context.Directories.Add(dir);

                        group.RootDirectory = dir;
                        context.Groups.Add(group);

                        DirectoryAccess ac =
                            new DirectoryAccess() {AccessType = 7, Directory = dir, Permissions = 7, User = user1};
                        context.DirectoryAccesses.Add(ac);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void CreateUsers()
        {
            User userAdmin = new User()
            {
                Login = "admin",
                Password = "admin",
                LastPasswordChange = DateTime.Now,
                SignUpDate = DateTime.Now
            };
            context.Users.Add(userAdmin);
            context.SaveChanges();

            for (int i = 0; i < 1000; i++)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        RandomString random = new RandomString();

                        User user = new User()
                        {
                            Login = random.GenerateRandom(10),
                            Password = Crypto.SHA256(random.GenerateRandom(10)),
                            LastPasswordChange = random.RandomDay(),
                            SignUpDate = random.RandomDay()
                        };
                        context.Users.Add(user);

                        Directory directory = new Directory() {ParentDirectoryId = 1, Name = user.Login + "'sFolder"};
                        context.Directories.Add(directory);

                        DirectoryAccess access =
                            new DirectoryAccess() {AccessType = 7, Directory = directory, Permissions = 7, User = user};
                        context.DirectoryAccesses.Add(access);


                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        
    }
}