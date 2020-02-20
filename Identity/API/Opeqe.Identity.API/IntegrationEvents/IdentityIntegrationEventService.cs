using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Common.EventBus.Events;
using Opeqe.Common.IntegrationEventLogEF.Services;
using Opeqe.Common.IntegrationEventLogEF.Utilities;
using Opeqe.Infrastructure.Data.Context;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Opeqe.Identity.API.IntegrationEvents
{
    public class IdentityIntegrationEventService : IIdentityIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _customIdentityDbContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<IdentityIntegrationEventService> _logger;

        public IdentityIntegrationEventService(
            ILogger<IdentityIntegrationEventService> logger,
            IEventBus eventBus,
            IUnitOfWork customIdentityDbContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customIdentityDbContext = customIdentityDbContext ?? throw new ArgumentNullException(nameof(customIdentityDbContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_customIdentityDbContext.Database.GetDbConnection());
        }

        public async Task PublishEventsThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- IdentityIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            await ResilientTransaction.New((DbContext)_customIdentityDbContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original database operation and the IntegrationEventLog thanks to a local transaction
                await _customIdentityDbContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _customIdentityDbContext.Database.CurrentTransaction);
            });
        }
    }
}
