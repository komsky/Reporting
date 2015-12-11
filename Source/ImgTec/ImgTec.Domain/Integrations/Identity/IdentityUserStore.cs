using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImgTec.Data.DataAccess.UnitOfWork;
using ImgTec.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Domain.Integrations.Identity
{
    public class IdentityUserStore :
        IUserLoginStore<ImgTecUser>,
        IUserClaimStore<ImgTecUser>,
        IUserRoleStore<ImgTecUser>,
        IUserPasswordStore<ImgTecUser>,
        IUserSecurityStampStore<ImgTecUser>,
        IUserStore<ImgTecUser>,
        IUserEmailStore<ImgTecUser>,
        IUserLockoutStore<ImgTecUser, string>,
        IUserTwoFactorStore<ImgTecUser, string>,
        IUserPhoneNumberStore<ImgTecUser>
    {
        private readonly IDataFacade _dataFacade;

        public IdentityUserStore()
        {
            _dataFacade = new DataFacade();
        }

        public IdentityUserStore(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }

        public void Dispose()
        {
            // We use Ninject to dispose
        }

        public Task CreateAsync(User user)
        {
            _dataFacade.Users.Add(user);
            return _dataFacade.CommitAsync();
        }

        public Task UpdateAsync(User user)
        {
            var existingWebAppUser = _dataFacade.Users.GetById(user.Id);

            existingWebAppUser.FirstName = user.FirstName;
            existingWebAppUser.LastName = user.LastName;
            existingWebAppUser.Email = user.Email;
            existingWebAppUser.SecurityStamp = user.SecurityStamp;
            existingWebAppUser.UserName = user.UserName;
            existingWebAppUser.EmailConfirmed = user.EmailConfirmed;
            existingWebAppUser.AccessFailedCount = user.AccessFailedCount;
            existingWebAppUser.LockoutEnabled = user.LockoutEnabled;
            existingWebAppUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
            existingWebAppUser.PasswordHash = user.PasswordHash;
            existingWebAppUser.PhoneNumber = user.PhoneNumber;
            existingWebAppUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            existingWebAppUser.TwoFactorEnabled = user.TwoFactorEnabled;

            _dataFacade.Users.Update(existingWebAppUser);
            return _dataFacade.CommitAsync();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            User user = _dataFacade.Users.GetById(userId);
            //map this into ImgTecUser
            return Task.FromResult();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            User user = _dataFacade.Users.GetByUserName(userName);
            return Task.FromResult(user);
        }

        public Task AddLoginAsync(LendAngelUser user, UserLoginInfo login)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);

            var identityUserLogin = new Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };

            webAppUser.Logins.Add(identityUserLogin);

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(LendAngelUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);

            if (webAppUser == null)
            {
                return Task.FromResult<IList<UserLoginInfo>>(new List<UserLoginInfo>());
            }

            return Task.FromResult<IList<UserLoginInfo>>(webAppUser.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task<LendAngelUser> FindAsync(UserLoginInfo login)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers
                .GetByExternalLoginProvider(login.LoginProvider, login.ProviderKey);
            var lendAngelUser = GetUserFromWebAppUser(webAppUser);
            return Task.FromResult(lendAngelUser);
        }

        public Task<IList<Claim>> GetClaimsAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);

            if (webAppUser == null)
            {
                return Task.FromResult<IList<Claim>>(new List<Claim>());
            }

            return
                Task.FromResult<IList<Claim>>(
                    webAppUser.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task AddClaimAsync(LendAngelUser user, Claim claim)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);

            var identityUserClaim = new IdentityUserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                UserId = user.Id
            };

            webAppUser.Claims.Add(identityUserClaim);

            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(LendAngelUser user, Claim claim)
        {
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = GetUserById(user);
            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _dataFacade.WebAppUsers.Update(u);
            return _dataFacade.CommitAsync();
        }

        public Task AddToRoleAsync(LendAngelUser user, string roleName)
        {

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = GetUserById(user);
            var r = _dataFacade.WebAppRoles.GetByName(roleName);
            if (r == null) throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Add(new IdentityUserRole { RoleId = r.Id.ToString(), UserId = u.Id });
            _dataFacade.WebAppUsers.Update(u);

            return _dataFacade.CommitAsync();
        }

        public Task RemoveFromRoleAsync(LendAngelUser user, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            var u = GetUserById(user);
            var r = _dataFacade.WebAppRoles.GetByName(roleName);
            if (r == null) throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Remove(new IdentityUserRole { RoleId = r.Id.ToString(), UserId = u.Id });

            _dataFacade.WebAppUsers.Update(u);
            return _dataFacade.CommitAsync();
        }

        public Task<IList<string>> GetRolesAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);

            if (webAppUser == null)
            {
                return Task.FromResult<IList<string>>(new List<string>());
            }

            return Task.FromResult<IList<string>>(webAppUser.Roles.Select(x => x.RoleId).ToList());
        }

        public Task<bool> IsInRoleAsync(LendAngelUser user, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");
            var u = GetUserById(user);

            var r = _dataFacade.WebAppRoles.GetByName(roleName);
            if (r == null) throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            return Task.FromResult<bool>(u.Roles.Any(x => x.RoleId == r.Id.ToString()));
        }

        public Task SetPasswordHashAsync(LendAngelUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);
            return Task.FromResult(webAppUser.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(LendAngelUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetSecurityStampAsync(LendAngelUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(LendAngelUser user)
        {
            if (!string.IsNullOrEmpty(user.SecurityStamp))
            {
                return Task.FromResult(user.SecurityStamp);
            }

            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);
            return Task.FromResult(webAppUser.SecurityStamp);
        }

        public Task SetEmailAsync(LendAngelUser user, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("Email was in wrong format.", "email");
            }
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(LendAngelUser user)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                return Task.FromResult(user.Email);
            }

            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);
            return webAppUser != null ? Task.FromResult(webAppUser.Email) : Task.FromResult(string.Empty);
        }

        public Task<bool> GetEmailConfirmedAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);
            return Task.FromResult(webAppUser.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(LendAngelUser user, bool confirmed)
        {
            var u = GetUserById(user);
            u.EmailConfirmed = confirmed;
            _dataFacade.WebAppUsers.Update(u);

            return _dataFacade.CommitAsync();
        }

        public Task<LendAngelUser> FindByEmailAsync(string email)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetByEmail(email);
            var lendAngelUser = GetUserFromWebAppUser(webAppUser);
            return Task.FromResult(lendAngelUser);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(LendAngelUser user)
        {
            WebAppUser webAppUser = _dataFacade.WebAppUsers.GetById(user.Id);
            var lockoutDate = webAppUser.LockoutEndDateUtc.HasValue ? new DateTimeOffset(webAppUser.LockoutEndDateUtc.Value)
                : new DateTimeOffset();
            return Task.FromResult(lockoutDate);
        }

        public Task SetLockoutEndDateAsync(LendAngelUser user, DateTimeOffset lockoutEnd)
        {
            var u = GetUserById(user);

            u.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            _dataFacade.WebAppUsers.Update(u);

            return _dataFacade.CommitAsync();
        }

        private WebAppUser GetUserById(LendAngelUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var u = _dataFacade.WebAppUsers.GetById(user.Id);
            if (u == null) throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            return u;
        }

        public Task<int> IncrementAccessFailedCountAsync(LendAngelUser user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(LendAngelUser user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(LendAngelUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(LendAngelUser user)
        {
            return Task.Factory.StartNew(() => false);
        }

        public Task SetLockoutEnabledAsync(LendAngelUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(LendAngelUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(LendAngelUser user)
        {
            return Task.Factory.StartNew(() => false);
        }

        private LendAngelUser GetUserFromWebAppUser(WebAppUser webAppUser)
        {
            if (webAppUser == null)
            {
                return null;
            }

            var lendAngelUser = Mapper.Map<LendAngelUser>(webAppUser);

            lendAngelUser.Logins = webAppUser.Logins.Select(Mapper.Map<IdentityUserLogin>).ToList();

            if (webAppUser.Claims == null)
            {
                return lendAngelUser;
            }

            var facebookAccessToken = webAppUser.Claims.FirstOrDefault(x => x.ClaimType == "FacebookAccessToken");

            if (facebookAccessToken != null)
            {
                var facebookClient = new FacebookClient(facebookAccessToken.ClaimValue);
                dynamic publicProfile = facebookClient.Get("me");

                lendAngelUser.Id = publicProfile.Id;
                lendAngelUser.ProfileImageUrl = string.Format(
                    "https://graph.facebook.com/{0}/picture?type=large&v={1}",
                    publicProfile.id,
                    DateTime.UtcNow.Ticks);
                lendAngelUser.IsFacebookUser = true;
            }

            return lendAngelUser;
        }

        public Task<string> GetPhoneNumberAsync(LendAngelUser user)
        {
            return Task<string>.Factory.StartNew(() => _dataFacade.WebAppUsers.GetById(user.Id).PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(LendAngelUser user)
        {
            return Task<bool>.Factory.StartNew(() => _dataFacade.WebAppUsers.GetById(user.Id).PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(LendAngelUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(LendAngelUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }
    }
}
