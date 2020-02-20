namespace Opeqe.Domain.SeedWork
{
    using MediatR;
    using System;
    using System.Collections.Generic;

    public abstract class Entity : Entity<Guid>, IEntity
    {
    }


    public abstract class Entity<T> : IEquatable<Entity<T>>, IEntity
    {
        private readonly int? _requestedHashCode;
        private T _Id;
        public virtual T Id
        {
            get => _Id;
            protected set => _Id = value;
        }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }

        public bool Equals(Entity<T> other)
        {
            return other != null &&
                   EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return EqualityComparer<Entity<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }
    }

}
