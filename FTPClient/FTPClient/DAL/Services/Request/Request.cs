using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            var tempEntity = context.Set<T>().Find(entity.Id);
            context.Entry(tempEntity).CurrentValues.SetValues(entity);
            context.SaveChanges();
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