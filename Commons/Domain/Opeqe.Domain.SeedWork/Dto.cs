using MediatR;
using System;
using System.Collections.Generic;

namespace Opeqe.Domain.SeedWork
{//
    public abstract class DTO
    {
        private int? _requestedHashCode;
        private string _Id;
        public virtual string Id
        {
            get => _Id;
            set => _Id = value;
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
            return Id == string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DTO))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            DTO item = (DTO)obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }
            else
            {
                return item.Id == Id;
            }
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
                }

                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }
        public static bool operator ==(DTO left, DTO right)
        {
            if (Object.Equals(left, null))
            {
                return (Object.Equals(right, null)) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(DTO left, DTO right)
        {
            return !(left == right);
        }
    }
}
