using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserSyncApi.Models
{
    public class LoginRequest
    {
        [Required, StringLength(100)]
        public string SourceSystem { get; set; } // nvarchar(100)
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}