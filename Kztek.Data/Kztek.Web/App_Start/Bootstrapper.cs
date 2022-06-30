using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Kztek.Core.Fakes;
using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Service.Admin;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Kztek.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            // This is where you register all dependencies

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            // The line below tells autofac, when a controller is initialized, pass into its constructor, the implementations of the required interfaces
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //For WebApi
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //For iParking
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();
            //builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerHttpRequest();

            //For iParkingEvent
            builder.RegisterType<Kztek.Data.Event.Infrastructure.UnitOfWork>().As<Kztek.Data.Event.Infrastructure.IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<Kztek.Data.Event.Infrastructure.DatabaseFactory>().As<Kztek.Data.Event.Infrastructure.IDatabaseFactory>().InstancePerHttpRequest();
            //builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(Kztek.Data.Event.Repository.tblAlarmRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(Kztek.Service.Admin.Event.tblAlarmService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(Kztek.Data.LockerEvent.Repository.tblLockerEventRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(Kztek.Service.Admin.LockerEvent.tblLockerEventService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();

            // This tells the MVC application to use myContainer as its dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //For WebApi
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}