using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Identity.API.IntegrationEvents;
using Opeqe.Identity.API.IntegrationEvents.Events.Identity;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Identity.Infrastructure.ViewModels;
using Opeqe.Identity.Infrastructure.ViewModels.Emails;
using Opeqe.Infrastructure.Data.Context;
using Opeqe.Infrastructure.Identity.IdentityToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opeqe.API.Areas.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RegisterController> _logger;
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUserValidator<User> _userValidator;
        private readonly IEventBus _eventBus;
        private readonly IdentitySettings _settings;
        private readonly IUnitOfWork _context;

        public RegisterController(IEmailSender emailSender,
                                  IMemoryCache cache,
                                  ILogger<RegisterController> logger,
                                  IApplicationUserManager userManager,
                                  IPasswordValidator<User> passwordValidator,
                                  IUserValidator<User> userValidator,
                                  IEventBus eventBus,
                                  IdentitySettings settings,
                                  IUnitOfWork context)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            this._eventBus = eventBus;
            this._settings = settings;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// For [Remote] validation
        /// </summary>
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email)
        {
            IdentityResult result = await _userValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username/*, Email = email*/ });
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string password, string username)
        {
            IdentityResult result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username }, password);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }


        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery] string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userManager.ConfirmEmailAsync(user, code);

            var registeredEvent = new RegisteredIntegrationEvent(user.UserName, user.Id, customerId: null, firstName: user.FirstName, lastName: user.LastName, phone: user.UserName);

            _eventBus.Publish(registeredEvent);

            return Ok();
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]RegisterViewModel model)
        {
            User user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

                if (_settings.EnableEmailConfirmation)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    await _emailSender.SendEmailAsync(
                       email: user.Email,
                       subject: "Please confirm your email",
                      message: code);

                    return Ok();
                }
            }
            return BadRequest(error: result.DumpErrors(useHtmlNewLine: false));
        }
    }
}