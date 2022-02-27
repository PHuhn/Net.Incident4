// ===========================================================================
// File: IEmailConfirmationAsync.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
    public interface IEmailConfirmation
    {
        //
        Task<string> EmailConfirmationAsync(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user);
        //
    }
}
// ===========================================================================
