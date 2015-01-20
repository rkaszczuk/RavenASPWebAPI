using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace RavenASPWebApi.Models
{
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual List<string> Roles { get; private set; }
        public int AccessFailedCount { get; set; }

        public IdentityUser()
        {
            AccessFailedCount = 0;
            Roles = new List<string>();
        }

        public void IncrementAccessFailedCount()
        {
            AccessFailedCount++;
        }
        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
        }
    }
}