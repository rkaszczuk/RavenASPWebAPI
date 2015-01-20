using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Raven.Client;
using Raven.Client.Embedded;
using RavenASPWebApi.Identity;
using RavenASPWebApi.Models;
using RavenASPWebApi.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace RavenASPWebApi.App_Start
{
    public static class Bootstrapper
    {
        public static IContainer Container { get; set; }
        public static void Run(HttpConfiguration config)
        {
            SetAutofacContainer(config);
        }

        private static void SetAutofacContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());



            builder.Register(c => new EmbeddableDocumentStore { ConnectionStringName = "RavenDB" }.Initialize()).
                As<IDocumentStore>().SingleInstance();


            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .OnRelease(c =>
                {
                    c.Dispose();
                });


            builder.Register(c => new UserManager<IdentityUser>(new UserStore<IdentityUser>(c.Resolve<IDocumentSession>())));
            builder.Register(c => new SimpleAuthorizationServerProvider(c.Resolve<UserManager<IdentityUser>>()));

            Container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
           
        }
    }
}