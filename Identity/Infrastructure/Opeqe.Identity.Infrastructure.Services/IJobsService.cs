namespace Opeqe.Identity.Infrastructure.Services
{
    public interface IJobsService
    {
        void AddOrUpdateRecuringJob(string recurringJobId, string methodName, bool remove, string cronExpression, object parameters);
    }
}