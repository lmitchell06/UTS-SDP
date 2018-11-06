using Autofac;
using Autofac.Integration.WebApi;
using SDP.TeamAlpha.Journals.Application;
using SDP.TeamAlpha.Journals.Application.LoginService;
using SDP.TeamAlpha.Journals.Application.RegisterService;
using SDP.TeamAlpha.Journals.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using SDP.TeamAlpha.Journals.Application.EntryService;

namespace SDP.TeamAlpha.Journals.Services
{
    public class DependencyConfig
    {
        public static void RegisterTypes()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Register types here

            //Register login types
            builder.RegisterType<LoginService>().As<ILoginService>();
            builder.RegisterType<LoginResponseFactory>().As<ILoginResponseFactory>();
            builder.RegisterType<LoginValidator>().As<ILoginValidator>();

            //Register register types (lol)
            builder.RegisterType<RegisterNewUserResponseFactory>().As<IRegisterNewUserResponseFactory>();
            builder.RegisterType<RegisterService>().As<IRegisterService>();

            //Register user types
            builder.RegisterType<UserFactory>().As<IUserFactory>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserSession>().As<IUserSession>();

            //Register journal types
            builder.RegisterType<JournalService>().As<IJournalService>();
            builder.RegisterType<JournalFactory>().As<IJournalFactory>();
            builder.RegisterType<JournalRepository>().As<IJournalRepository>();

            //Register entry types
            builder.RegisterType<EntryService>().As<IEntryService>();
            builder.RegisterType<EntryFactory>().As<IEntryFactory>();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}