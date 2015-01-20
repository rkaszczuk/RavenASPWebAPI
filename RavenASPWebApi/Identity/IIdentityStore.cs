using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RavenASPWebApi.Identity
{
    public interface IIdentityStore
    {
        bool Disposed { get; }
    }
}