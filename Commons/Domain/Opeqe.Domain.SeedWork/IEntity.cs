using MediatR;
using System.Collections.Generic;

namespace Opeqe.Domain.SeedWork
{
    public interface IEntity
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }

        void ClearDomainEvents();
    }
}