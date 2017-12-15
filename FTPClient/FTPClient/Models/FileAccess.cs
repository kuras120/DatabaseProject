using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class FileAccess
    {
        [Key, ForeignKey("User")]
        [Column(Order = 0)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Key, ForeignKey("File")]
        [Column(Order = 1)]
        public int FileId { get; set; }
        public virtual File File { get; set; }
        public int AccessType { get; set; }
        public int Permissions { get; set; }
    }
}