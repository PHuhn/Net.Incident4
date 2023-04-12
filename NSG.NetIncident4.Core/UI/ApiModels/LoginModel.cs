// ===========================================================================
using System;
using System.ComponentModel.DataAnnotations;
//
namespace NSG.NetIncident4.Core.UI.ApiModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = String.Empty;
    }
}
// ===========================================================================
