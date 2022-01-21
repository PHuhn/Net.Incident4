using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
// using System.Security.Policy;
//
using Microsoft.AspNetCore.Authorization;
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
    public class ViewHelpers
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
            var page = _context as PageModel;
            if (page != null)
            {
                // Identity page
                callbackUrl = page.Url.Page("/Account/ConfirmEmail", pageHandler: null,
                    values: new { userId = userId, code = code },
                    protocol: page.Request.Scheme);
            }
            else
            {
                var api = _context as ControllerBase;
                if (api != null)
                {
                    // api
                    // public static string Action(this IUrlHelper helper, string action, string controller, object values, string protocol)
                    callbackUrl = api.Url.Action("ConfirmEmail", "Account",
                        new { userId = userId, code = code }, api.Request.Scheme);
                }
                else
                {
                    var controller = _context as Controller;
                    if (controller != null)
                    {
                        // Identity page
                        callbackUrl = controller.Url.Action("ConfirmEmail", "Account",
                            new { userId = userId, code = code }, controller.Request.Scheme);
                    }
                }
            }
            await emailSender.SendEmailAsync( email, "Confirm your email",
                $"Please confirm your account: {userName} by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            return;
        }
    }
}
//