// ===========================================================================
using System;
using System.ComponentModel.DataAnnotations;
//
namespace NSG.NetIncident4.Core.UI.ApiModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "'User Name' is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "'First Name' is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "'Last Name' is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "'Full Name' is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "'Nic Name' is required")]
        public string UserNicName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "'Company id' is required")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
// ===========================================================================
