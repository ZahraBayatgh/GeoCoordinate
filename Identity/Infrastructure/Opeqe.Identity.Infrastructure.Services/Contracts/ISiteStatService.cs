using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services.Contracts
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}