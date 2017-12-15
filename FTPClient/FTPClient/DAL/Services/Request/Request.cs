using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTPClient.DAL.Services;
using FTPClient.Models;

namespace FTPClient.DAL.Services.Request
{
    public class Request<T>:IRequest<T> where T:class, IIdentity
    {
        private DataModel context;

        public Request(DataModel context)
        {
            this.context = context;
        }

        public void AddEntity(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void EditEntity(T entity)
        {
            context.Set<T>().Attach(entity);
        }

        public void DeleteEntity(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void DeleteAll()
        {
            foreach (var entity in context.Set<T>())
            {
                context.Set<T>().Remove(entity);
            }
            context.SaveChanges();
        }
    }
}