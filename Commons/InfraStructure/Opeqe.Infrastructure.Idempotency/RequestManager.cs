using Opeqe.Domain.Entities;
using Opeqe.Infrastructure.Data.Context;
using System;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly IUnitOfWork _context;

        public RequestManager(IUnitOfWork context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.Set<ClientRequest>().
                FindAsync(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new Exception($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Set<ClientRequest>().Add(request);

            await _context.SaveChangesAsync();
        }
    }
}
