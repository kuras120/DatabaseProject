using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class Group:IIdentity
    {
        public int Id { get; set; }
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public virtual User Admin { get; set; }
        [ForeignKey("RootDirectory")]
        public int RootDirectoryId { get; set; }
        public virtual Directory RootDirectory { get; set; }
        public string Name { get; set; }
    }
}