using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Aspects;

namespace RavenASPWebApi.Aspects
{
    [Serializable]
    public class NoNullParamsAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                if (args.Arguments.GetArgument(i) == null)
                {
                    throw new ArgumentNullException(args.Method.GetParameters()[i].Name);
                }
            }
        }
    }
}