// ===========================================================================
using System;
using System.ComponentModel.DataAnnotations;
//
namespace NSG.NetIncident4.Core.UI.ApiModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
// ===========================================================================
