using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class Directory:IIdentity
    {
        public int Id { get; set; }
        [ForeignKey("ParentDirectory")]
        public int? ParentDirectoryId { get; set; }
        public virtual Directory ParentDirectory { get; set; }
        public string Name { get; set; }
    }
}