// ===========================================================================
using System;
using System.ComponentModel.DataAnnotations;
//
namespace NSG.NetIncident4.Core.UI.ApiModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "'User Name' is required")]
        public string Username { get; set; } = String.Empty;

        [Required(ErrorMessage = "'First Name' is required")]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "'Last Name' is required")]
        public string LastName { get; set; } = String.Empty;

        [Required(ErrorMessage = "'Full Name' is required")]
        public string FullName { get; set; } = String.Empty;

        [Required(ErrorMessage = "'Nic Name' is required")]
        public string UserNicName { get; set; } = String.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "'Company id' is required")]
        public int CompanyId { get; set; } = 0;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = String.Empty;
    }
}
// ===========================================================================
