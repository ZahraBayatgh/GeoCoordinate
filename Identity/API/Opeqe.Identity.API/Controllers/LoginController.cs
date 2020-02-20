using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Identity.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Opeqe.API.Areas.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    //ورود به سیستم"
    public class LoginController : Controller
    {
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IdentitySettings _siteSettings;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IApplicationSignInManager signInManager,
                               IApplicationUserManager userManager,
                               IApplicationRoleManager roleManager,
                               IdentitySettings siteSettings,
                               ILogger<LoginController> logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _siteSettings = siteSettings ?? throw new ArgumentNullException(nameof(siteSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            User user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                if (user.IsActive)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Create the token
                        string username = model.UserName;
                        IList<string> roles = await _userManager.GetRolesAsync(user);
                        List<Claim> roleClaims = new List<Claim>();

                        foreach (string roleName in roles)
                        {
                            Role role = await _roleManager.FindByNameAsync(roleName);
                            IList<Claim> claims = await _roleManager.GetClaimsAsync(role);
                            roleClaims.AddRange(claims);
                        }

                        string userRole = roles.Count > 0 ? roles[0] : "";
                        roleClaims.AddRange(new List<Claim>
                        {

                            new Claim("UserId", user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        });

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.Tokens.Key));
                        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            _siteSettings.Tokens.Issuer,
                            _siteSettings.Tokens.Audience,
                            roleClaims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);
                    }
                    if (result.IsNotAllowed)
                    {
                        return BadRequest("Your registration is not approved, please re-register .");
                    }
                    else if (result.IsLockedOut)
                    {
                        return BadRequest("Your user is locked by the system administrator. Please contact support.");
                    }
                    else
                    {
                        return BadRequest("The password you entered for this username is incorrect.");
                    }
                }
                else
                {
                    return BadRequest("Your user has been disabled by the system administrator. Please contact support.");
                }
            }

            return BadRequest("Username not found");
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            User user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;
            await _signInManager.SignOutAsync();
            if (user != null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                _logger.LogInformation(4, $"{user.UserName} logged out.");
            }

            return Ok();
        }
        private string GetErros()
        {
            List<string> errors = new List<string>();
            foreach (KeyValuePair<string, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry> state in ModelState)
            {
                foreach (Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error in state.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            string result = string.Join(", ", errors.ToArray());

            return result;
        }
    }
}