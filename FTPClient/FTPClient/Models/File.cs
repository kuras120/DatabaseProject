using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class File:IIdentity
    {
        public int Id { get; set; }
        [ForeignKey("Directory")]
        public int DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? UploadTime { get; set; }
    }
}