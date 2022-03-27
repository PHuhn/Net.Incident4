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
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
    public class EmailConfirmation : IEmailConfirmation
    {
        //
        object _context;
        PageModel _page = null;
        ControllerBase _api = null;
        Controller _controller = null;
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">called from 'this', ie page, controller or api</param>
        public EmailConfirmation(object context)
        {
            if(context == null) throw new ArgumentNullException("context");
            _context = context;
            _page = _context as PageModel;
            _api = _context as ControllerBase;
            _controller = _context as Controller;
        }
        //
        /// <summary>
        /// Send out an e-mial confirmation notification.  This is sent with
        /// the initial account registration, and if the user e-mail acount is
        /// changed.  Additionally and unauthenticated razor page if the user
        /// wishes to have the notification resent.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="user">an application user</param>
        /// <returns></returns>
        public async Task<string> EmailConfirmationAsync(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user)
        {
            string callbackUrl = "";
            if ( user != null && userManager != null && emailSender != null)
            {
                var userId = user.Id;
                var email = user.Email;
                var userName = user.UserName;
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                if (_page != null)
                {
                    // Identity page
                    var prot = _page.Request.Scheme;
                    callbackUrl = _page.Url.Page("/Account/ConfirmEmail", pageHandler: null,
                        values: new { userId = userId, code = code },
                        protocol: _page.Request.Scheme);
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
                await emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account: {userName} by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            else
            {
                throw new Exception($"EmailConfirmationAsync: invalid parameter user.");
            }
            return callbackUrl;
        }
    }
}
// ===========================================================================
