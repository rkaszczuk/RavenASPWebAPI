using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RavenASPWebApi.Aspects;
using Microsoft.AspNet.Identity;
using PostSharp.Extensibility;
using Raven.Client;
using System.Web.Http;
using System.Threading.Tasks;
using RavenASPWebApi.Models;

namespace RavenASPWebApi.Identity
{
    [ThrowIfDisposedAspect(AttributeTargetElements = MulticastTargets.Method)]
    public class UserStore<TUser> : IUserStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IIdentityStore where TUser : IdentityUser
    {
        private IDocumentSession session;

        [ThrowIfDisposedAspect(AttributeExclude = true)]
        public bool Disposed { get; private set; }

        public UserStore()
        {
            Disposed = false;
        }

        public UserStore(IDocumentSession _session)
            : this()
        {
            this.session = _session;
        }
        [NoNullParamsAspect]
        public Task CreateAsync(TUser user)
        {
            this.session.Store(user);
            this.session.SaveChanges();
            return Task.FromResult(user);

        }
        [NoNullParamsAspect]
        public Task DeleteAsync(TUser user)
        {
            this.session.Delete(user);
            this.session.SaveChanges();
            return Task.FromResult(true);
        }
        [NoNullParamsAspect]
        public Task<TUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(this.session.Load<TUser>(userId));
        }
        [NoNullParamsAspect]
        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(this.session.Query<TUser>().Where(x => x.UserName == userName).SingleOrDefault());
        }
        [NoNullParamsAspect]
        public Task UpdateAsync(TUser user)
        {
            var userToEdit = this.session.Load<TUser>(user.Id);
            userToEdit = user;
            this.session.SaveChanges();
            return Task.FromResult(userToEdit);
        }

        public void Dispose()
        {
            this.Disposed = true;
        }

        [NoNullParamsAspect]
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (!user.Roles.Contains(roleName, StringComparer.InvariantCultureIgnoreCase))
            {
                user.Roles.Add(roleName);
            }
            return Task.FromResult(true);
        }
        [NoNullParamsAspect]
        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.FromResult<IList<string>>(user.Roles);
        }
        [NoNullParamsAspect]
        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            return Task.FromResult(user.Roles.Contains(roleName, StringComparer.InvariantCultureIgnoreCase));
        }
        [NoNullParamsAspect]
        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            user.Roles.RemoveAll(x => String.Equals(x, roleName, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(true);
        }
        [NoNullParamsAspect]
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }
        [NoNullParamsAspect]
        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        [NoNullParamsAspect]
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(true);
        }
        [NoNullParamsAspect]
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        [NoNullParamsAspect]
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(true);
        }



    }
}