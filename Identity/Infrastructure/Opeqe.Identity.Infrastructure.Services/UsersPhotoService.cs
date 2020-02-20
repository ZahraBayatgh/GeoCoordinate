using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Infrastructure.Identity.IdentityToolkit;
using System;
using System.IO;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class UsersPhotoService : IUsersPhotoService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IdentitySettings _siteSettings;

        public UsersPhotoService(
            IHttpContextAccessor contextAccessor,
            IWebHostEnvironment hostingEnvironment,
            IdentitySettings siteSettings)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(_hostingEnvironment));
            _siteSettings = siteSettings ?? throw new ArgumentNullException(nameof(_siteSettings));
        }

        public string GetUsersAvatarsFolderPath()
        {
            var usersAvatarsFolder = _siteSettings.UsersAvatarsFolder;
            var uploadsRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, usersAvatarsFolder);
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }
            return uploadsRootFolder;
        }

        public void SetUserDefaultPhoto(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.PhotoFileName))
            {
                return;
            }

            var avatarPath = Path.Combine(GetUsersAvatarsFolderPath(), user.PhotoFileName ?? string.Empty);
            if (!File.Exists(avatarPath))
            {
                user.PhotoFileName = _siteSettings.UserDefaultPhoto;
            }
        }

        public string GetUserDefaultPhoto(string photoFileName)
        {
            if (string.IsNullOrWhiteSpace(photoFileName))
            {
                return _siteSettings.UserDefaultPhoto;
            }

            var avatarPath = Path.Combine(GetUsersAvatarsFolderPath(), photoFileName ?? string.Empty);
            return !File.Exists(avatarPath) ? _siteSettings.UserDefaultPhoto : photoFileName;
        }

        public string GetUserPhotoUrl(string photoFileName)
        {
            photoFileName = GetUserDefaultPhoto(photoFileName);
            return $"~/{_siteSettings.UsersAvatarsFolder}/{photoFileName}";
        }

        public string GetCurrentUserPhotoUrl()
        {
            var photoFileName = _contextAccessor.HttpContext.User.Identity.GetUserClaimValue(ApplicationClaimsPrincipalFactory.PhotoFileName);
            return GetUserPhotoUrl(photoFileName);
        }
    }
}
