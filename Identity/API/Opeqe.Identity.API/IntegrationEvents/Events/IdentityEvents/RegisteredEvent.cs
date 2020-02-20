namespace Opeqe.Identity.API.IntegrationEvents.Events.Identity
{
    using Opeqe.Common.EventBus.Events;

    public class RegisteredIntegrationEvent : IntegrationEvent
    {
        public RegisteredIntegrationEvent(string userName, int userId, int? customerId = null, string firstName = null, string lastName = null, string phone = null)
        {
            UserName = userName;
            UserId = userId;
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        public string UserName { get; set; }
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
