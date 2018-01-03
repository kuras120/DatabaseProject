using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FTPClient.Models;

namespace FTPClient.DAL.Services.Response
{
    public class Response<T>:IResponse<T> where T:class, IIdentity
    {
        DataModel context;

        public Response(DataModel context)
        {
            this.context = context;
        }

        public T GetEntity(int id)
        {
            return context.Set<T>().Find(id);
        }

        public List<T> GetEntities()
        {
            List<T> entities = new List<T>();
            entities.AddRange(context.Set<T>());

            return entities;
        }
    }
}