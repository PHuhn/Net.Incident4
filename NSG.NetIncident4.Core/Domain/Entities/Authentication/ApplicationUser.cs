// ===========================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
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
        public ApplicationUser() : base()
        {
            Id = Guid.NewGuid().ToString();
            FirstName = "";
            LastName = "";
            FullName = "";
            UserNicName = "";
            CompanyId = 0;
            CreateDate = DateTime.Now;
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
        }
        //
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Id: {0}, ", Id);
            _return.AppendFormat("FirstName: {0}, ", FirstName);
            _return.AppendFormat("LastName: {0}, ", LastName);
            _return.AppendFormat("FullName: {0}, ", FullName);
            _return.AppendFormat("UserNicName: {0}, ", UserNicName);
            _return.AppendFormat("CompanyId: {0}, ", CompanyId.ToString());
            _return.AppendFormat("CreateDate: {0}, ", CreateDate.ToString());
            _return.AppendFormat("UserName: {0}, ", UserName);
            _return.AppendFormat("NormalizedUserName: {0}, ", NormalizedUserName);
            _return.AppendFormat("Email: {0}, ", Email);
            _return.AppendFormat("NormalizedEmail: {0}, ", NormalizedEmail);
            _return.AppendFormat("EmailConfirmed: {0}, ", EmailConfirmed.ToString());
            _return.AppendFormat("PasswordHash: {0}, ", PasswordHash);
            _return.AppendFormat("SecurityStamp: {0}, ", SecurityStamp);
            _return.AppendFormat("ConcurrencyStamp: {0}, ", ConcurrencyStamp);
            _return.AppendFormat("PhoneNumber: {0}, ", PhoneNumber);
            _return.AppendFormat("PhoneNumberConfirmed: {0}, ", PhoneNumberConfirmed.ToString());
            _return.AppendFormat("TwoFactorEnabled: {0}, ", TwoFactorEnabled.ToString());
            if (LockoutEnd.HasValue)
                _return.AppendFormat("LockoutEnd: {0}, ", LockoutEnd.ToString());
            else
                _return.AppendFormat("/LockoutEnd/, ");
            _return.AppendFormat("LockoutEnabled: {0}, ", LockoutEnabled.ToString());
            _return.AppendFormat("AccessFailedCount: {0}, ", AccessFailedCount.ToString());
            if( this.UserRoles != null )
            {
                _return.Append("[");
                foreach (var _urole in this.UserRoles)
                {
                    _return.AppendFormat("Role: {0}, ", _urole.RoleId );
                }
                _return.Append("]");
            }
            if (this.UserServers != null)
            {
                _return.Append("[");
                foreach (var _userver in this.UserServers)
                {
                    _return.AppendFormat("Server Id: {0}, ", _userver.ServerId);
                }
                _return.Append("]");
            }
            _return.Append("]");
            return _return.ToString();
        }
        //
    }
}
// ===========================================================================

