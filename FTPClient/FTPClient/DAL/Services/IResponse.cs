using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.DAL.Services
{
    interface IResponse<T>
    {
        T GetEntity(int id);
    }
}
