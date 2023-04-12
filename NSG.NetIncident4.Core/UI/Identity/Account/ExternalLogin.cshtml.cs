using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Persistence;
using NSG.NetIncident4.Core.UI.ViewHelpers;

namespace NSG.NetIncident4.Core.UI.Identity.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string _codeName;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _codeName = "ExternalLogin";
            Input = new InputModel() { Email = "" };
            ProviderDisplayName = "";
            ReturnUrl = "/";
            ErrorMessage = "";
            ServerSelectList = new List<SelectListItem>();
            GetCompanies();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        //
        public List<SelectListItem> ServerSelectList { get; set; }
        //
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            [MaxLength(255)]
            public string Email { get; set; }
            //
            [Required(ErrorMessage = "'First Name' is required")]
            [Display(Name = "First Name")]
            [MaxLength(100)]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "'Last Name' is required")]
            [Display(Name = "Last Name")]
            [MaxLength(100)]
            public string LastName { get; set; }

            [Required(ErrorMessage = "'Nic Name' is required")]
            [Display(Name = "Nic Name")]
            [MaxLength(16)]
            public string UserNicName { get; set; }

            [MaxLength(30)]
            [Display(Name = "Phone #")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "'Company id' is required")]
            [Display(Name = "Company Id")]
            public int ServerId { get; set; }
            //
            public InputModel()
            {
                this.FirstName = "";
                this.LastName = "";
                this.UserNicName = "";
                this.Phone = "";
                this.ServerId = 0;
                this.Email = "";
            }
            //
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string? returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            // get companies dropdown
            if( !GetCompanies())
            {
                return RedirectToPage("./Login");
            }
            //
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null || info.Principal.Identity == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            if (ModelState.IsValid)
            {
                //
                // Get server to be added (and also defines default company)...
                //
                Server? _server = await _context.Servers.FindAsync(Input.ServerId);
                if (_server != null)
                {
                    // Construct user
                    var user = new ApplicationUser
                    {
                        UserName = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        FullName = $"{Input.FirstName} {Input.LastName}",
                        UserNicName = Input.UserNicName,
                        PhoneNumber = Input.Phone,
                        CompanyId = _server.CompanyId,
                        CreateDate = DateTime.Now,
                        Email = Input.Email
                    };
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
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                            IEmailConfirmation _confirmation = new EmailConfirmation(this, _userManager, _emailSender);
                            await _confirmation.EmailConfirmationAsync(user);

                            // If account confirmation is required, we need to show the link if we don't have a real email sender
                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                            }

                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        if( error.Description.Contains("is already taken."))
                        {
                            ModelState.AddModelError(string.Empty, "If already registered this email account, then did you check your email and confirmed the account.");
                        }
                    }
                }
                else
                    ModelState.AddModelError("", $"Server not found: {Input.ServerId}");
            }
            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
        private bool GetCompanies()
        {
            // get companies dropdown
            try
            {
                ServerSelectList = _context.Servers
                    .Include(_c => _c.Company)
                    .Select(s => new SelectListItem($"{s.Company.CompanyShortName} - {s.ServerShortName}", s.ServerId.ToString())).ToList();
            }
            catch (Exception ex)
            {
                var message = $"Servers.Select failed with message: {ex.Message}";
                ErrorMessage = message;
                _logger.LogError($"{_codeName}: {message}");
                _logger.LogError(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
