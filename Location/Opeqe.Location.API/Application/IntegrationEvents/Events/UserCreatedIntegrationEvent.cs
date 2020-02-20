namespace Location.API.Application.Behaviors.IntegrationEvents.Events
{
    using Opeqe.Common.EventBus.Events;

    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public int UserId { get; }

        public UserCreatedIntegrationEvent(int userId) =>
            UserId = userId;
    }
}
