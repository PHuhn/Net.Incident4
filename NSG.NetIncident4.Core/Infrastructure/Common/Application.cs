//
using System;
using System.Security.Claims;
using System.Security.Principal;
//
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
//
namespace NSG.NetIncident4.Core.Infrastructure.Common
{
    public class ApplicationImplementation : IApplication
    {
        //
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationSettings _applicationSettings;
        //
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-2.2
        /// <summary>
        /// In startup:
        ///     services.AddHttpContextAccessor();
        ///     services.AddTransient<IUserRepository, UserRepository>();
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ApplicationImplementation(IHttpContextAccessor httpContextAccessor, IOptions<ApplicationSettings> applicationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationSettings = applicationSettings.Value;
        }
        //
        /// <summary>
        /// Return a string of the Application Name.
        /// </summary>
        /// <returns>string of Application Name</returns>
        public string GetApplicationName()
        {
            return _applicationSettings.Name;
        }
        //
        /// <summary>
        /// Return a string of the Application Name.
        /// </summary>
        /// <returns>string of Application phone #</returns>
        public string GetApplicationPhoneNumber()
        {
            return _applicationSettings.Phone;
        }
        //
        /// <summary>
        /// Return a date-time of the current date/time.
        /// </summary>
        /// <returns>DateTime of the current date/time.</returns>
        public DateTime Now()
        {
            return DateTime.Now;
        }
        //
        /// <summary>
        /// Get the current user's ClaimsPrincipal via HttpContext.
        /// </summary>
        /// <returns>ClaimsPrincipal of the current user.</returns>
        public ClaimsPrincipal GetUserClaimsPrincipal()
        {
            ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal();
            if (_httpContextAccessor.HttpContext != null)
                if (_httpContextAccessor.HttpContext.User.Identity != null)
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                _claimsPrincipal = _httpContextAccessor.HttpContext.User;
            return _claimsPrincipal;
        }
        //
        /// <summary>
        /// Get the current user's user name identity via HttpContext.
        /// </summary>
        /// <returns>String of the current user.</returns>
        public string GetUserAccount()
        {
            var _currentUserName = Constants.NotAuthenticated;
            ClaimsPrincipal _claimsPrincipal = GetUserClaimsPrincipal();
            if (_claimsPrincipal.Identity != null)
                if (_claimsPrincipal.Identity.IsAuthenticated)
                    if (_claimsPrincipal.Identity.Name != null)
                        _currentUserName = _claimsPrincipal.Identity.Name;
            return _currentUserName;
        }
        //
        /// <summary>
        /// Get the current user's authentication status.
        /// </summary>
        /// <returns>boolean value if user is authenticated.</returns>
        public bool IsAuthenticated()
        {
            bool _isAuthenticated = false;
            ClaimsPrincipal _claimsPrincipal = GetUserClaimsPrincipal();
            if (_claimsPrincipal.Identity != null)
                _isAuthenticated = _claimsPrincipal.Identity.IsAuthenticated;
            return _isAuthenticated;
        }
        //
        // For role variables see Constants.cs
        //
        /// <summary>
        /// Is the user in Admin role.
        /// </summary>
        /// <returns>boolean value if user is in role.</returns>
        public bool IsAdminRole()
        {
            bool _admin = false;
            ClaimsPrincipal _claimsPrincipal = GetUserClaimsPrincipal();
            if (_claimsPrincipal.Identity != null)
                if (_claimsPrincipal.Identity.IsAuthenticated)
                    _admin = _claimsPrincipal.IsInRole(Constants.adminRole);
            return _admin;
        }
        //
        /// <summary>
        /// Is the user in either Admin or CompanyAdmin roles.
        /// </summary>
        /// <returns>boolean value if user is in role.</returns>
        public bool IsCompanyAdminRole()
        {
            bool _company = false;
            ClaimsPrincipal _claimsPrincipal = GetUserClaimsPrincipal();
            if (_claimsPrincipal.Identity != null)
            {
                if (_claimsPrincipal.Identity.IsAuthenticated)
                {
                    _company = IsAdminRole();
                    if (!_company)
                        _company = _claimsPrincipal.IsInRole(Constants.companyadminRole);
                }
            }
            return _company;
        }
        //
        /// <summary>
        /// Is the user in either Admin or CompanyAdmin roles.
        /// </summary>
        /// <returns>boolean value if user is in role.</returns>
        public bool IsEditableRole()
        {
            bool _editable = false;
            ClaimsPrincipal _claimsPrincipal = GetUserClaimsPrincipal();
            if (_claimsPrincipal.Identity != null)
            {
                if (_claimsPrincipal.Identity.IsAuthenticated)
                {
                    _editable = IsAdminRole();
                    if (!_editable)
                        _editable = _claimsPrincipal.IsInRole(Constants.companyadminRole);
                    if (!_editable)
                        _editable = _claimsPrincipal.IsInRole(Constants.userRole);
                }
            }
            return _editable;
        }
        //
    }
}
//
