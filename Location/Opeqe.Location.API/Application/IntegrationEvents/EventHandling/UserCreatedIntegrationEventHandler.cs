using Location.API.Application.Behaviors.IntegrationEvents.Events;
using Opeqe.Common.EventBus.Abstractions;
using System.Threading.Tasks;

namespace Location.API.Application.Behaviors.IntegrationEvents.EventHandling
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            // ToDo: 
        }
    }
}