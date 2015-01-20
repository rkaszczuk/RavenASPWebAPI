using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace RavenASPWebApi.Aspects
{
    [Serializable]
    public sealed class ThrowIfDisposedAspect : MethodInterceptionAspect
    {

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            if (args.Instance is Identity.IIdentityStore)
            {
                if ((args.Instance as Identity.IIdentityStore).Disposed)
                {
                    throw new ObjectDisposedException(args.Instance.GetType().ToString());
                }
            }
            base.OnInvoke(args);
        }
    }
}