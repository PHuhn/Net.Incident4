// ===========================================================================
// File: ViewHelpers.cs
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
namespace NSG.NetIncident4.Core.UI.ViewHelpers
{
    public interface IViewHelpers
    {
        //
        Task EmailConfirmationAsync(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user);
        //
    }
    //
    /// <summary>
    /// class of helpers for views
    /// </summary>
    public class ViewHelpers : IViewHelpers
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
        public ViewHelpers(object context)
        {
            _context = context;
            _page = _context as PageModel;
            _api = _context as ControllerBase;
            _controller = _context as Controller;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="user">an application user</param>
        /// <returns></returns>
        public async Task EmailConfirmationAsync(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user)
        {
            var userId = await userManager.GetUserIdAsync(user);
            var email = await userManager.GetEmailAsync(user);
            var userName = user.UserName;
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = "";
            if (_page != null)
            {
                // Identity page
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
                }
            }
            await emailSender.SendEmailAsync( email, "Confirm your email",
                $"Please confirm your account: {userName} by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            return;
        }
    }
}
// ===========================================================================
