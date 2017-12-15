using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Models
{
    public interface IIdentity
    {
        [Key]
        int Id { get; set; }
    }
}
