﻿// ===========================================================================
// Log an audit of the transaction.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// 
    /// </summary>
    public class Audit
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }
        [Required]
        public DateTime ChangeDate { get; set; }
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(256)]
        public string Program { get; set; }
        [Required]
        [MaxLength(256)]
        public string TableName { get; set; }
        [Required]
        [MaxLength(1)]
        public string UpdateType { get; set; }
        [Required]
        [MaxLength(512)]
        public string Keys { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Before { get; set; }
        [Required]
        [MaxLength(4000)]
        public string After { get; set; }
    }
}
// ===========================================================================