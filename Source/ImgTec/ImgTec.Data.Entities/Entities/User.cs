using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        public ICollection<Case> Cases { get; set; }
    }
}
