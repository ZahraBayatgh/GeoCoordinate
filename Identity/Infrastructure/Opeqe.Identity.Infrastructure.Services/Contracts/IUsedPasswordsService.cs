using Opeqe.Identity.Infrastructure.Entities;
using System;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services.Contracts
{
    public interface IUsedPasswordsService
    {
        Task<bool> IsPreviouslyUsedPasswordAsync(User user, string newPassword);
        Task AddToUsedPasswordsListAsync(User user);
        Task<bool> IsLastUserPasswordTooOldAsync(int userId);
        Task<DateTime?> GetLastUserPasswordChangeDateAsync(int userId);
    }
}