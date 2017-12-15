using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class DirectoryAccess
    {
        [Key, ForeignKey("User")]
        [Column(Order = 0)]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        [Key, ForeignKey("Directory")]
        [Column(Order = 1)]
        public int DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }
        public int AccessType { get; set; }
        public int Permissions { get; set; }

    }
}