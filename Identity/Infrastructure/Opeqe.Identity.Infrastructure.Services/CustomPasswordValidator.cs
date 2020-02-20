using Microsoft.AspNetCore.Identity;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class CustomPasswordValidator : PasswordValidator<User>
    {
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly ISet<string> _passwordsBanList;

        public CustomPasswordValidator(
            IdentityErrorDescriber errors,// How to use CustomIdentityErrorDescriber
            IdentitySettings configurationRoot,
            IUsedPasswordsService usedPasswordsService) : base(errors)
        {
            _usedPasswordsService = usedPasswordsService ?? throw new ArgumentNullException(nameof(_usedPasswordsService));
            if (configurationRoot == null)
            {
                throw new ArgumentNullException(nameof(configurationRoot));
            }

            _passwordsBanList = new HashSet<string>(configurationRoot.PasswordsBanList, StringComparer.OrdinalIgnoreCase);

            if (!_passwordsBanList.Any())
            {
                throw new InvalidOperationException("Please fill the passwords ban list in the appsettings.json file.");
            }
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordIsNotSet",
                    Description = "Please fill in the password."
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            if (string.IsNullOrWhiteSpace(user?.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "UserNameIsNotSet",
                    Description = "Please fill in the username."
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            // First use the built-in validator
            var result = await base.ValidateAsync(manager, user, password);
            errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            // Extending the built-in validator
            if (password.Contains(user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password cannot contain part of username."
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            if (!isSafePasword(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordIsNotSafe",
                    Description = "The entered password is easy to guess."
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            if (await _usedPasswordsService.IsPreviouslyUsedPasswordAsync(user, password))
            {
                errors.Add(new IdentityError
                {
                    Code = "IsPreviouslyUsedPassword",
                    Description = "Please choose another password. This password is already used by you and is a duplicate."
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            return !errors.Any() ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        private static bool areAllCharsEqual(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            data = data.ToLowerInvariant();
            var firstElement = data.ElementAt(0);
            var euqalCharsLen = data.ToCharArray().Count(x => x == firstElement);
            if (euqalCharsLen == data.Length)
            {
                return true;
            }

            return false;
        }

        private bool isSafePasword(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            if (data.Length < 5)
            {
                return false;
            }

            if (_passwordsBanList.Contains(data.ToLowerInvariant()))
            {
                return false;
            }

            if (areAllCharsEqual(data))
            {
                return false;
            }

            return true;
        }
    }
}