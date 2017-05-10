using Autofac;
using Autofac.Integration.WebApi;
using CareMobile.API.Configuration;
using CareMobile.Azure.DocumentDB;
using CareMobile.Azure.EmotionAPI;
using CareMobile.Azure.Storage;
using System.Reflection;
using System.Web.Http;

namespace CareMobile.API.Host
{
    public class AutofacBootsrapper
    {
        public static void Run()
        {
            var config = GlobalConfiguration.Configuration;

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
        }
    }
}