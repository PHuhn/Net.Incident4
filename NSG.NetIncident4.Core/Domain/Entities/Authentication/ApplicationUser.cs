// ===========================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
using Microsoft.AspNetCore.Identity;
//
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication
{
    //
    /// <summary>
    /// Default Identity User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(16)]
        public string UserNicName { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        //
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        //
        public virtual ICollection<ApplicationUserServer> UserServers { get; }
            = new List<ApplicationUserServer>();
        //
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
            = new List<ApplicationUserRole>();
        //
        /// <summary>
        /// No parameter constructor, assigns a guid to the Id.
        /// </summary>
        public ApplicationUser() : base() { }
        //

    }
}
// ===========================================================================

