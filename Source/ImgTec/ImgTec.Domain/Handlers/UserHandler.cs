using System;
using ImgTec.Data.Entities;
using ImgTec.Domain.Integrations.Identity;

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
