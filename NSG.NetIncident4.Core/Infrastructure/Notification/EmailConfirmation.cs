// ===========================================================================
// File: IEmailConfirmationAsync.cs
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
//
using NSG.NetIncident4.Core.Domain.Entities;
using static System.Net.WebRequestMethods;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
    public class EmailConfirmation : IEmailConfirmation
    {
        //
        object _context;
        PageModel? _page;
        ControllerBase? _api;
        Controller? _controller;
        UserManager<ApplicationUser> _userManager;
        IEmailSender _emailSender;
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">called from 'this', ie page, controller or api</param>
        /// <param name="userManager"></param>
        /// <param name="emailSender"></param>
        public EmailConfirmation(object context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            if(context == null) throw new ArgumentNullException("context");
            if (userManager == null) throw new ArgumentNullException("UserManager");
            if (emailSender == null) throw new ArgumentNullException("EmailSender");
            _context = context;
            if(_context != null)
            {
                _page = _context as PageModel;
                _api = _context as ControllerBase;
                _controller = _context as Controller;
            }
            _userManager = userManager;
            _emailSender = emailSender;
        }
        //
        /// <summary>
        /// Send out an e-mial confirmation notification.  This is sent with
        /// the initial account registration, and if the user e-mail acount is
        /// changed.  Additionally and unauthenticated razor page if the user
        /// wishes to have the notification resent.
        /// </summary>
        /// <param name="user">an application user</param>
        /// <returns></returns>
        public async Task<string> EmailConfirmationAsync(ApplicationUser user)
        {
            string callbackUrl = "";
            var userId = user.Id;
            var email = user.Email;
            var userName = user.UserName;
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            if (_page != null)
            {
                // Identity page
                //  Request.Host: localhost:9114
                //  Request.Path: /Account/ResendEmailConfirmation
                //  Request.Scheme: https
                //  Routes: page - /Account/ResendEmailConfirmation
                //  Protocol: HTTP/2
                // callbackUrl = _page.Url.Page("/Account/ConfirmEmail", null,
                callbackUrl = _page.Url.Action("ConfirmEmail", "Account",
                    new { userId = userId, code = code }, _page.Request.Scheme);
            }
            else
            {
                if (_api != null)
                {
                    callbackUrl = _api.Url.Action("ConfirmEmail", "Account",
                        new { userId = userId, code = code }, _api.Request.Scheme);
                }
                else
                {
                    if (_controller != null)
                    {
                        callbackUrl = _controller.Url.Action("ConfirmEmail", "Account",
                            new { userId = userId, code = code }, _controller.Request.Scheme);
                    }
                    else
                    {
                        throw new Exception($"EmailConfirmationAsync: invalid content (page/controller/api).");
                    }
                }
            }
            string _emailBody = $"Please confirm your account: {userName} by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            await _emailSender.SendEmailAsync(email, "Confirm your email", _emailBody);
            return _emailBody;
        }
    }
}
// ===========================================================================
