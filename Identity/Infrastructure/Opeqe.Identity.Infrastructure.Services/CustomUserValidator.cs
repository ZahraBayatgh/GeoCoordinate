using Microsoft.AspNetCore.Identity;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Infrastructure.Toolkits.GuardToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class CustomUserValidator : UserValidator<User>
    {
        private readonly ISet<string> _emailsBanList;

        public CustomUserValidator(
            IdentityErrorDescriber errors,// How to use CustomIdentityErrorDescriber
            IdentitySettings configurationRoot
            ) : base(errors)
        {
            if (configurationRoot == null)
            {
                throw new ArgumentNullException(nameof(configurationRoot));
            }

            _emailsBanList = new HashSet<string>(configurationRoot.EmailsBanList, StringComparer.OrdinalIgnoreCase);

            if (!_emailsBanList.Any())
            {
                throw new InvalidOperationException("Please fill the emails ban list in the appsettings.json file.");
            }
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            // First use the built-in validator
            var result = await base.ValidateAsync(manager, user);
            var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            // Extending the built-in validator
            //validateEmail(user, errors);
            validateUserName(user, errors);

            return !errors.Any() ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        private void validateEmail(User user, List<IdentityError> errors)
        {
            var userEmail = user?.Email;
            if (string.IsNullOrWhiteSpace(userEmail))
            {

                errors.Add(new IdentityError
                {
                    Code = "EmailIsNotSet",
                    Description = "Please complete the email information."
                });

                return; // base.ValidateAsync() will cover this case
            }

            if (_emailsBanList.Any(email => userEmail.EndsWith(email, StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add(new IdentityError
                {
                    Code = "BadEmailDomainError",
                    Description = "Please enter a valid provider email."
                });
            }
        }

        private static void validateUserName(User user, List<IdentityError> errors)
        {
            var userName = user?.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "UserIsNotSet",
                        Description = "Please complete the credentials."
                    });
                }
                return;  // base.ValidateAsync() will cover this case
            }


            if (userName.HasConsecutiveChars())
            {
                errors.Add(new IdentityError
                {
                    Code = "BadUserNameError",
                    Description = "The username you entered is not valid."
                });
            }
        }
    }
}