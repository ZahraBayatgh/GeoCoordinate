using Opeqe.Common.EventBus.Events;

namespace Opeqe.Identity.API.IntegrationEvents.Events
{
    public class UserRegisteredIntegrationEvent : IntegrationEvent
    {
        public UserRegisteredIntegrationEvent(int userId, int shopOperatorId, int shopId, string entityType, int entityId, string label)
        {
            UserId = userId;
            ShopOperatorId = shopOperatorId;
            ShopId = shopId;
            EntityType = entityType;
            EntityId = entityId;
            Label = label;
        }

        public int UserId { get; private set; }
        public int ShopOperatorId { get; private set; }
        public int ShopId { get; private set; }
        public string EntityType { get; private set; }
        public int EntityId { get; private set; }
        public string Label { get; private set; }
    }
}
