using System.Threading.Tasks;

namespace Opeqe.Common.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
