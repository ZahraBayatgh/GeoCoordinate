using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Services.Contracts
{
    public interface IUsersPhotoService
    {
        string GetUsersAvatarsFolderPath();
        void SetUserDefaultPhoto(User user);
        string GetUserDefaultPhoto(string photoFileName);
        string GetUserPhotoUrl(string photoFileName);
        string GetCurrentUserPhotoUrl();
    }
}