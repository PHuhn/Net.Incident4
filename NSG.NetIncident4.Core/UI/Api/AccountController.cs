﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//using NSG.NetIncident4.Core.UI.Identity.Account;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class AccountController : BaseApiController
    //{
    //    private readonly SignInManager<ApplicationUser> _signInManager;
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly ApplicationDbContext _context;
    //    private readonly IEmailSender _emailSender;
    //    private readonly ILogger<RegisterModel> _logger;
    //    private readonly AuthSettings _authSettings;

    //    public AccountController(
    //        UserManager<ApplicationUser> userManager,
    //        SignInManager<ApplicationUser> signInManager,
    //        ApplicationDbContext context,
    //        IEmailSender emailSender,
    //        ILogger<RegisterModel> logger,
    //        IOptions<AuthSettings> authSettings
    //        )
    //    {
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //        _context = context;
    //        _emailSender = emailSender;
    //        _logger = logger;
    //        _authSettings = authSettings.Value;
    //    }
    //    //
    //    public string Get()
    //    {
    //        return "Hello world";
    //    }

    //    [AllowAnonymous]
    //    [HttpGet("~/api/account/test")]
    //    public IActionResult Test()
    //    {
    //        //
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            return new JsonResult(new
    //            {
    //                Subject = User.Identity.AuthenticationType,
    //                Name = User.Identity.Name
    //            });
    //        }
    //        else
    //        {
    //            return new JsonResult(new { Message = "User identity is NOT authenticated." });
    //        }
    //        //
    //    }

    //    // HttpPost("authenticate")]
    //    [AllowAnonymous]
    //    [HttpPost("~/api/account/token"), Produces("application/json")]
    //    public async Task<object> Login(string userName, string password)
    //    {
    //        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

    //        if (result.Succeeded)
    //        {
    //            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == userName);
    //            return GenerateJwtToken(appUser.UserName, appUser);
    //        }

    //        throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
    //    }

    //    [AllowAnonymous]
    //    [HttpPost]
    //    public async Task Register([FromBody] NSG.NetIncident4.Core.UI.Identity.Account.RegisterModel.InputModel model)
    //    {
    //        RegisterModel _registerModel = new RegisterModel(
    //            _userManager, _signInManager, _context, _logger, _emailSender);
    //        bool _result = await _registerModel.CreateUserAsync(model);
    //        if (_result)
    //        {
    //            // even though they use UserName, require email to be verified
    //            return;
    //        }

    //        throw new ApplicationException("UNKNOWN_ERROR");
    //    }
    //    //
    //    private object GenerateJwtToken(string userName, ApplicationUser user)
    //    {
    //        var claims = new List<Claim>
    //        {
    //            new Claim(JwtRegisteredClaimNames.Sub, userName),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(ClaimTypes.NameIdentifier, user.Id)
    //        };
    //        //
    //        var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtSecret));
    //        var _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
    //        var _notBefore = DateTime.UtcNow;
    //        var _expires = _notBefore.AddMinutes(Convert.ToDouble(_authSettings.JwtExpireMinutes));
    //        //
    //        var token = new JwtSecurityToken(
    //            _authSettings.JwtIssuer,
    //            _authSettings.JwtAudience,
    //            claims,
    //            notBefore: _notBefore,
    //            expires: _expires,
    //            signingCredentials: _creds
    //        );
    //        //
    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //    //
    //}
}