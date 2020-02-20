using Autofac;
using HSP.Common.EventBus.Abstractions;
using HSP.NGT.Application.IntegrationEvents.EventHandling.OrderEventHandlers;
using System.Reflection;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public class ApplicationModule
        : Autofac.Module
    {


        public ApplicationModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(OrderStartedIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
