using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgTec.Data.Entities;

namespace ImgTec.Domain.Handlers
{
    public class UserHandler :IUserHandler
    {
        private readonly IdentityUserStore _identityUserStore;
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public User GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
