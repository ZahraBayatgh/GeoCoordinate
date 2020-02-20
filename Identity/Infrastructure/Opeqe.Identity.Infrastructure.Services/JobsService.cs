using Hangfire;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class JobsService : IJobsService
    {
        private readonly Data.Context.CustomIdentityDbContext _context;
        private readonly ILogger<JobsService> _logger;
        private readonly string _connectionString;

        public JobsService(Data.Context.CustomIdentityDbContext CustomIdentityDbContext, ILogger<JobsService> logger)
        {
            _context = CustomIdentityDbContext;
            _logger = logger;
        }

        public async Task TestJob2(object parameters)
        {
        }
        public async Task InvokeMethod(string methodName, object parameters)
        {
            _logger.LogInformation($"InvokeMethod {methodName}  Started");
            System.Reflection.MethodInfo methodInfo = typeof(JobsService).GetMethod(methodName);

            if ((methodInfo.Invoke(this, new[] { parameters })) is Task task)
            {
                await task;
            }

            _logger.LogInformation($"InvokeMethod {methodName}  Finished");
        }

        public void AddOrUpdateRecuringJob(string recurringJobId, string methodName, bool remove, string cronExpression, object parameters)
        {
            if (remove)
            {
                RecurringJob.RemoveIfExists(recurringJobId);
            }
            else
            {
                RecurringJob.AddOrUpdate<JobsService>(
                     recurringJobId,
                     jobsService => jobsService.InvokeMethod(methodName, parameters),
                     () => cronExpression
                     );
            }
        }
    }
}
