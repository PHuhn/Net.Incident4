//
// ---------------------------------------------------------------------------
// ApplicationUsers get administration role for logged in user.
//
using System;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Application.Infrastructure
{
    //
    /// <summary>
    /// 'ApplicationUser' list query handler.
    /// </summary>
    public static class Helpers
    {
        public static string GetUserCompanyRoleQuery(ApplicationUser user)
        {
            if (user == null)
            {
                // Call the FluentValidationErrors extension method.
                throw new GetUserCompanyRoleQueryValidationException("ApplicationUser is empty.");
            }
            if (user.UserRoles == null)
            {
                // Call the FluentValidationErrors extension method.
                throw new GetUserCompanyRoleQueryValidationException("ApplicationUser UserRoles is empty.");
            }
            string _roleId = "";
            foreach (ApplicationUserRole _userRole in user.UserRoles)
            {
                if (_userRole.Role.Id.ToLower() == "adm")
                    _roleId = _userRole.Role.Id.ToLower();
                if ((_userRole.Role.Id.ToLower() == "cadm") && (_roleId == ""))
                    _roleId = _userRole.Role.Id.ToLower();
            }
            return _roleId;
        }
        //
    }
    //
    /// <summary>
    /// Custom GetUserCompanyRoleQuery validation exception.
    /// </summary>
    public class GetUserCompanyRoleQueryValidationException : Exception
    {
        //
        /// <summary>
        /// Implementation of GetUserCompanyRoleQuery validation exception.
        /// </summary>
        /// <param name="errorMessage">The validation error messages.</param>
        public GetUserCompanyRoleQueryValidationException(string errorMessage)
            : base($"GetUserCompanyRoleQuery validation exception: errors: {errorMessage}")
        {
        }
    }
}
// ---------------------------------------------------------------------------
