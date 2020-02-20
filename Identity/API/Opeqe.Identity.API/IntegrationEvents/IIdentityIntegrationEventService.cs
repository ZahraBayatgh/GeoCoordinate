using Opeqe.Common.EventBus.Events;
using System.Threading.Tasks;

namespace Opeqe.Identity.API.IntegrationEvents
{
    public interface IIdentityIntegrationEventService
    {
        Task AddAndSaveEventAsync(IntegrationEvent evt);
        Task PublishEventsThroughEventBusAsync(IntegrationEvent evt);
    }
}
