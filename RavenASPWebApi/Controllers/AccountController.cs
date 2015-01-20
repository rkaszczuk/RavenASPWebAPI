using Microsoft.AspNet.Identity;
using RavenASPWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RavenASPWebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private UserManager<IdentityUser> _userManager;
        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityUser identityUser = new IdentityUser
            {
                UserName = userModel.Username,
            };
            identityUser.Roles.Add("User");

            IdentityResult result = await _userManager.CreateAsync(identityUser, userModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
