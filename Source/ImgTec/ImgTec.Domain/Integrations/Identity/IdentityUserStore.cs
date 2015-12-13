using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImgTec.Data.DataAccess.UnitOfWork;
using ImgTec.Data.Entities;
using ImgTec.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Domain.Integrations.Identity
{
    public class IdentityUserStore :
        IUserLoginStore<UserDomain>,
        IUserRoleStore<UserDomain>,
        IUserPasswordStore<UserDomain>,
        IUserStore<UserDomain>,
        IUserEmailStore<UserDomain>,
        IUserLockoutStore<UserDomain, string>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDomain> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDomain> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(UserDomain user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(UserDomain user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDomain> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(UserDomain user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(UserDomain user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(UserDomain user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(UserDomain user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(UserDomain user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(UserDomain user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<UserDomain> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(UserDomain user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(UserDomain user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(UserDomain user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
