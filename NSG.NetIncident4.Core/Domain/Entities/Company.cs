﻿//
// ---------------------------------------------------------------------------
// Companies.
//
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NSG.NetIncident4.Core.Persistence;
using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations.Schema;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Entity class of a company.
    /// </summary>
    public class Company
    {
        //
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "'Company Id' is required.")]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "'Company Short Name' is required."), MaxLength(12, ErrorMessage = "'CompanyShortName' must be 12 or less characters.")]
        public string CompanyShortName { get; set; }
        [Required(ErrorMessage = "'Company Name' is required."), MaxLength(80, ErrorMessage = "'CompanyName' must be 80 or less characters.")]
        public string CompanyName { get; set; }
        [MaxLength(80, ErrorMessage = "'Address' must be 80 or less characters.")]
        public string Address { get; set; }
        [MaxLength(50, ErrorMessage = "'City' must be 50 or less characters.")]
        public string City { get; set; }
        [MaxLength(4, ErrorMessage = "'State' must be 4 or less characters.")]
        public string State { get; set; }
        [MaxLength(15, ErrorMessage = "'Postal/Zip Code' must be 15 or less characters.")]
        public string PostalCode { get; set; }
        [MaxLength(50, ErrorMessage = "'Country' must be 50 or less characters.")]
        public string Country { get; set; }
        [MaxLength(50, ErrorMessage = "'Phone #' must be 50 or less characters.")]
        public string PhoneNumber { get; set; }
        public string? Notes { get; set; }
        //
        public virtual ICollection<Server> Servers { get; } = new List<Server>();
        public virtual ICollection<ApplicationUser> Users { get; } = new List<ApplicationUser>();
        public virtual ICollection<EmailTemplate> EmailTemplates { get; } = new List<EmailTemplate>();
        //
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Company()
        {
            CompanyId = 0;
            CompanyShortName = "";
            CompanyName = "";
            Address = "";
            City = "";
            State = "";
            PostalCode = "";
            Country = "";
            PhoneNumber = "";
            Notes = "";
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("CompanyId: {0}, ", CompanyId.ToString());
            _return.AppendFormat("CompanyShortName: {0}, ", CompanyShortName);
            _return.AppendFormat("CompanyName: {0}, ", CompanyName);
            _return.AppendFormat("Address: {0}, ", Address);
            _return.AppendFormat("City: {0}, ", City);
            _return.AppendFormat("State: {0}, ", State);
            _return.AppendFormat("PostalCode: {0}, ", PostalCode);
            _return.AppendFormat("Country: {0}, ", Country);
            _return.AppendFormat("PhoneNumber: {0}, ", PhoneNumber);
            _return.AppendFormat("Notes: {0}, ", Notes);
            _return.AppendFormat("]");
            return _return.ToString();
        }
        //
    }
}
// ---------------------------------------------------------------------------
