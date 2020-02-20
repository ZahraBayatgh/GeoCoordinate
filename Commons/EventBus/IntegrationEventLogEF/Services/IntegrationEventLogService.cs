using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Opeqe.Common.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Opeqe.Common.IntegrationEventLogEF.Services
{//
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);

            var assemblies = GetAssemblies().ToList()
                .Where(x => x.GetName().Name.EndsWith("IntegrationEvents.Events"))
                .Select(x => x.GetName().FullName)
                .ToList();

            _eventTypes = assemblies.SelectMany(x => Assembly.Load(x)
               .GetTypes()
               .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
               .ToList()).ToList();


        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                {
                    if (reference.Name.StartsWith("Opeqe") && !list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }
                }
            }
            while (stack.Count > 0);

        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var list = await _integrationEventLogContext.IntegrationEventLogs
                .Where(e => e.TransactionId == tid && e.State == EventStateEnum.NotPublished)
                .ToListAsync();

            return list
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)))
                .ToList();
        }

        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new IntegrationEventLogEntry(@event, transaction.TransactionId);

            //_integrationEventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
            {
                eventLogEntry.TimesSent++;
            }

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}
