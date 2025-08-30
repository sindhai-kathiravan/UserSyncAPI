using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserSyncApi.Models
{
    public class DeleteUserRequest
    {
        [Required, StringLength(100)]
        public string SourceSystem { get; set; } // nvarchar(100)
        [Required]
        public int UserId { get; set; }

        [Required]
        public List<string> TargetDatabases { get; set; }

        [Required]
        public int? ModifiedByUserId { get; set; }

    }
}