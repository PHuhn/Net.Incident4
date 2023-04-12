//
using System;
using System.ComponentModel.DataAnnotations;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    public class AccountForgotPasswordConfirmationViewModel
    {
        //
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
        //
    }
    //
}
//

