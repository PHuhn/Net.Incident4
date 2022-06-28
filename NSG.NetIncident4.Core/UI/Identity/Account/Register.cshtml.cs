using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
//
using NSG.NetIncident4.Core.UI.ViewHelpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace NSG.NetIncident4.Core.UI.Identity.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            ReturnUrl = "/";
            Input = new InputModel();
            ExternalLogins = new List<AuthenticationScheme>();
            ServerSelectList = new List<SelectListItem>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        //
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public List<SelectListItem> ServerSelectList { get; set; }
        //
        public class InputModel
        {
            [Required(ErrorMessage = "'User Name' is required")]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "'First Name' is required")]
            [MaxLength(100)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "'Last Name' is required")]
            [MaxLength(100)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "'Nic Name' is required")]
            [MaxLength(16)]
            [Display(Name = "Nic Name")]
            public string UserNicName { get; set; }

            [MaxLength(30)]
            [Display(Name = "Phone #")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "'Server' is required")]
            [Display(Name = "Server")]
            public int ServerId { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(255)]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            //
            public InputModel()
            {
                this.UserName = "";
                this.FirstName = "";
                this.LastName = "";
                this.UserNicName = "";
                this.Phone = "";
                this.ServerId = 0;
                this.Email = "";
                this.Password = "";
                this.ConfirmPassword = "";
            }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("/Home");
                }
            }
            //
            ReturnUrl = returnUrl == null ? "/" : returnUrl;
            try
            {
                ServerSelectList = _context.Servers
                    .Include(_c => _c.Company)
                    .Select(s => new SelectListItem($"{s.Company.CompanyShortName} - {s.ServerShortName}", s.ServerId.ToString())).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Register: Servers.Select failed with message: {ex.Message}");
                _logger.LogError(ex.ToString());
                return;
            }
            try
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Register: SignInManager.GetExternalAuthenticationSchemesAsync failed with message: {ex.Message}");
                _logger.LogError(ex.ToString());
            }
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //
                // Get server to be added (and also defines default company)...
                //
                Server? _server = await _context.Servers.FindAsync(Input.ServerId);
                if (_server != null)
                {
                    // Construct user
                    var user = new ApplicationUser {
                        UserName = Input.UserName, FirstName = Input.FirstName,
                        LastName = Input.LastName, FullName = $"{Input.FirstName} {Input.LastName}",
                        UserNicName = Input.UserNicName, PhoneNumber = Input.Phone,
                        CompanyId = _server.CompanyId, CreateDate = DateTime.Now, Email = Input.Email };
                    // Attach role to the user
                    ApplicationUserRole _urole = new ApplicationUserRole()
                    {
                        RoleId = "pub"
                    };
                    user.UserRoles.Add(_urole);
                    // Attach server to the user
                    ApplicationUserServer _userver = new ApplicationUserServer()
                    {
                        ServerId = Input.ServerId
                    };
                    user.UserServers.Add(_userver);
                    // Ready to create the user...
                    _logger.LogInformation(user.ToString());
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"{Input.UserName} account created with a password.");
                        await ViewHelpers.ViewHelpers.EmailConfirmationAsync(this, _userManager, _emailSender, user);
                        //
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                    ModelState.AddModelError("", $"Server not found: {Input.ServerId}");
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
