using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.UI.ViewHelpers;

namespace NSG.NetIncident4.Core.UI.Identity.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender  = emailSender;
            _context = context;
            Username = "";
            CompanyName = "";
            Role = "";
            StatusMessage = "";
            Input = new InputModel();
        }

        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }
        //
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            //
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            //
            [Required]
            [MaxLength(100)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            //
            [Required]
            [MaxLength(100)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            //
            [Required]
            [MaxLength(255)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            //
            [Required]
            [MaxLength(16)]
            [Display(Name = "Nic Name")]
            public string UserNicName { get; set; }
            //
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            //
            public InputModel()
            {
                Email = "";
                FirstName = "";
                LastName = "";
                FullName = "";
                UserNicName = "";
                PhoneNumber = "";
            }
        }

    private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            Username = userName;
            CompanyName = "";
            Role = "";
            if ( user.Company != null)
            {
                CompanyName = $"{user.Company.CompanyName} ({user.CompanyId})";
            }
            else
            {
                var company = _context.Companies.FirstOrDefault(c => c.CompanyId == user.CompanyId);
                CompanyName = $"{(company != null ? company.CompanyName : "")} - ({user.CompanyId})";
            }
            if (roles.Count > 0)
            {
                Role = roles[0];
            } else
            {
                Role = "-";
            }
            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                UserNicName = user.UserNicName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            //
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if( user.Email != Input.Email)
            {
                user.EmailConfirmed = false;
            }
            if (user.PhoneNumber != Input.PhoneNumber)
            {
                user.PhoneNumberConfirmed = false;
            }
            //
            user.Email = Input.Email;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.FullName = Input.FullName;
            user.PhoneNumber = Input.PhoneNumber;
            user.UserNicName = Input.UserNicName;
            //
            var setUserResult = await _userManager.UpdateAsync(user);
            if (!setUserResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set user.";
                return RedirectToPage();
            }
            //
            if( user.EmailConfirmed == false)
            {
                var setEmailResult = await OnPostSendVerificationEmailAsync();
            } else
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        //
        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            //
            await ViewHelpers.ViewHelpers.EmailConfirmationAsync(this, _userManager, _emailSender, user);
            StatusMessage = "Verification email sent. Please check your email.";
            await _signInManager.SignOutAsync();
            return LocalRedirect("/home");
        }
        //
    }
}
