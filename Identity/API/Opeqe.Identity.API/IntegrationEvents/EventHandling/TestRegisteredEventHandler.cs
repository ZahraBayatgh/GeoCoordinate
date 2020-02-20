using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Identity.API.IntegrationEvents.Events.Identity;
using System.Threading.Tasks;

namespace Opeqe.Identity.API.IntegrationEvents.EventHandling
{
    public class TestRegisteredEventHandler : IIntegrationEventHandler<RegisteredIntegrationEvent>
    {
        public async Task Handle(RegisteredIntegrationEvent @event)
        {
            //
        }
    }
}
