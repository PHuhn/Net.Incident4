using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class AccountRegisterViewModel
    {
        //
        [Required]
        [MaxLength(256)]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = String.Empty;
        //
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;
        //
        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = String.Empty;
        //
        [Required]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = String.Empty;
        //
        [Required]
        [MaxLength(16)]
        [Display(Name = "Nic Name")]
        public string UserNicName { get; set; } = String.Empty;
        //
        [Required]
        [StringLength(12)]
        [Display(Name = "Server Short Name")]
        public string ServerShortName { get; set; } = String.Empty;
        //
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = String.Empty;
        //
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = String.Empty;
        //
    }
}
