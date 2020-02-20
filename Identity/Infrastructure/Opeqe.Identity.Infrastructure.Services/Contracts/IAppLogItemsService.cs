using Opeqe.Identity.Infrastructure.ViewModels;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services.Contracts
{
    public interface IAppLogItemsService
    {
        Task DeleteAllAsync(string logLevel = "");
        Task DeleteAsync(int logItemId);
        Task DeleteOlderThanAsync(DateTime cutoffDateUtc, string logLevel = "");
        Task<int> GetCountAsync(string logLevel = "");
        Task<PagedAppLogItemsViewModel> GetPagedAppLogItemsAsync(int pageNumber, int pageSize, SortOrder sortOrder, string logLevel = "");
    }
}