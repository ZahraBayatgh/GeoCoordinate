using Autofac;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Identity.API.IntegrationEvents.Events;
using System.Reflection;

namespace Opeqe.Identity.API.Infrastructure.AutofacModules
{
    public class ApplicationModule
        : Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(UserRegisteredIntegrationEvent).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
