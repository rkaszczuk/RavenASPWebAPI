using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using RavenASPWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RavenASPWebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        UserManager<IdentityUser> _userManager;

        public SimpleAuthorizationServerProvider(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var config = new HttpConfiguration();
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });           

            IdentityUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));
            identity.AddClaim(new Claim("role", user.Roles.FirstOrDefault()));

            context.Validated(identity);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
    }
}