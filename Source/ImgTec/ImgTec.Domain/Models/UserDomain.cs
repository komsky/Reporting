using System;
using System.Collections.Generic;

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
    }
}
