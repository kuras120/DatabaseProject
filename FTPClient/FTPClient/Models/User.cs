using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FTPClient.Models
{
    public class User:IIdentity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? SignUpDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? LastPasswordChange { get; set; }
    }
}