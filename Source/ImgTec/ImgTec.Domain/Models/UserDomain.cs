using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace ImgTec.Domain.Models
{
    public class UserDomain : BaseIdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<CaseDomain> Cases { get; set; }
    }

    public class BaseIdentityUser : IUser<String>
    {
        public string Id { get; private set; }
        public string UserName { get; set; }
    }
}
