// ===========================================================================
// File: IEmailConfirmationAsync.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
//
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
    public interface IEmailConfirmation
    {
        //
        Task<string> EmailConfirmationAsync(ApplicationUser user);
        //
    }
}
// ===========================================================================
