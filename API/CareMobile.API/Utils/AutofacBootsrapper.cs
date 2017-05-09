using Autofac;
using Autofac.Integration.WebApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CareMobile.API.Configuration;
using CareMobile.Azure.DocumentDB;
using CareMobile.Azure.Storage;
using CareMobile.Azure.EmotionAPI;
using Owin;
using System.Web.Http;
using System.Reflection;

namespace CareMobile.API
{
    public class AutofacBootsrapper
    {
        public static void Run(IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ConfigurationSettings>().As<IConfigurationSettings>().SingleInstance();

            builder.RegisterGeneric(typeof(DocumentDBRepository<>));
            builder.RegisterType<JobApplicationRepository>().As<IJobApplicationRepository>();
            builder.RegisterType<PositionRepository>().As<IPositionRepository>();
            builder.RegisterType<EmotionApiRepository>().As<IEmotionApiRepository>();

            builder.RegisterType<EmotionRepository>().As<IEmotionRepository>();
            builder.RegisterType<StorageRepository>().As<IStorageRepository>();
            
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container); //should be the first middleware added to IAppBuilder
        }
    }
}