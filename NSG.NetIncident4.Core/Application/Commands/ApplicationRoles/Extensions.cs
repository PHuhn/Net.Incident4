// ---------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
//
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Application.Commands.ApplicationRoles
{
    //
    /// <summary>
    /// Extension method.
    /// </summary>
    public static partial class Extensions
    {
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public static string ApplicationRoleToString(this ApplicationRole entity)
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Id: {0}, ", entity.Id.ToString());
            _return.AppendFormat("Name: {0}, ", entity.Name);
            _return.AppendFormat("NormalizedName: {0}, ", entity.NormalizedName);
            return _return.ToString();
            //
        }
        //
        // public static ICollection<ApplicationUserRole> UsersInRole(this ApplicationRole role, ApplicationDbContext context)
        // {
        //     return context.UserRoles.Where( ur => ur.RoleId == role.Id ).Select(ur => ur).ToList();
        // }
        //
    }
}
