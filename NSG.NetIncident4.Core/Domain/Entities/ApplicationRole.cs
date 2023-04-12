// ===========================================================================
// Roles.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Custom Application Role, inherits from ApplicationRole
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        //
        /// <summary>
        /// No parameter constructror, let base ApplicationRole handle it.
        /// </summary>
        public ApplicationRole() : base() { }
        //
        /// <summary>
        /// 1 parameter constructror, let base ApplicationRole handle it.
        /// </summary>
        /// <param name="name">just passed to base constructor</param>
        public ApplicationRole(string name) : base(name) { }
        //
        /// <summary>
        /// 2 parameter constructror, not passed to base constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <remarks>Please use this constructor</remarks>
        public ApplicationRole(string id, string name) : base(name)
        {
            this.Id = id;
        }
        //
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        //
    }
    //
}
// ===========================================================================
