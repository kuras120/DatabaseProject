using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.DAL.Services
{
    interface IRequest<T>
    {
        void AddEntity(T entity);
        void EditEntity(T entity);
        void DeleteEntity(T entity);
        void DeleteAll();
    }
}
