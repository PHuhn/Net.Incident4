﻿// ===========================================================================
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.UI.Services;
//
using NSG.NetIncident4.Core.UI.ApiModels;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.ViewHelpers;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Notification;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        //
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        //
        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSender emailSender)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        //
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] ApiModels.LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.EmailConfirmed == true)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    //
                    if( userRoles == null )
                    {
                        ModelState.AddModelError("", $"{model.Username} requires a user role.");
                        return Unauthorized();
                    }
                    if (userRoles.Count == 0)
                    {
                        ModelState.AddModelError("", $"{model.Username} requires a user role.");
                        return Unauthorized();
                    }
                    //
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    //
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    //
                    AuthSettings authSettings =
                        _configuration.GetSection("AuthSettings").Get<AuthSettings>();
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtSecret));
                    //
                    var token = new JwtSecurityToken(
                        issuer: authSettings.JwtIssuer, // _configuration["JWT:ValidIssuer"],
                        audience: authSettings.JwtAudience, // _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(authSettings.JwtExpirationHours),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    //
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    ModelState.AddModelError("", "You need to confirm your email.");
                    return UnauthorizedNotConfirmed();
                }
            }
            return NotFound();
        }
        //
        /// <summary>
        /// Creates an <see cref="UnauthorizedObjectResult"/> that produces a <see cref="StatusCodes.Status401Unauthorized"/> response
        /// with a custom title and detail.
        /// </summary>
        /// <returns>The created <see cref="UnauthorizedObjectResult"/> for the response.</returns>
        [NonAction]
        public UnauthorizedObjectResult UnauthorizedNotConfirmed()
        {
            Object _error = new { status = StatusCodes.Status401Unauthorized, title = "Unauthorized: email confirmation required", detail = "Unauthorized - user not confirmed", type = "https://tools.ietf.org/html/rfc7235#section-3.1" };
            UnauthorizedObjectResult _objectResults = new UnauthorizedObjectResult(_error);
            //
            return _objectResults;
        }
        //
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] ApiModels.RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FullName,
                UserNicName = model.UserNicName,
                CompanyId = model.CompanyId,
                CreateDate = DateTime.Now,
            };
            //
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string error = "Unknown error";
                if( result.Errors != null)
                {
                    error = (result.Errors.FirstOrDefault()).Description;
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"User creation failed! {error}" });
            }
            IEmailConfirmation _confirmation = new EmailConfirmation(this, userManager, _emailSender);
            await _confirmation.EmailConfirmationAsync(user);

            return Ok(new Response { Status = "Success", Message = "Account created, must confirm your email!" });
        }

        public static implicit operator Controller(AuthenticateController v)
        {
            throw new NotImplementedException();
        }
    }
}
// ===========================================================================
