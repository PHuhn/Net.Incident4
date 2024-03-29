﻿//
// ---------------------------------------------------------------------------
// Logs.
//
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Entity class of a log record.
    /// </summary>
    [Table("Log")]
    public partial class LogData
    {
        //
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Id is required.")]
        public long Id { get; set; }
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Application is required."), MaxLength(30, ErrorMessage = "'Application' must be 30 or less characters.")]
        public string Application { get; set; }
        [Required(ErrorMessage = "Method is required."), MaxLength(255, ErrorMessage = "'Method' must be 255 or less characters.")]
        public string Method { get; set; }
        [Required(ErrorMessage = "LogLevel is required.")]
        public byte LogLevel { get; set; }
        [Required(ErrorMessage = "Level is required."), MaxLength(8, ErrorMessage = "'Level' must be 8 or less characters.")]
        public string Level { get; set; }
        [Required(ErrorMessage = "UserAccount is required."), MaxLength(255, ErrorMessage = "'UserAccount' must be 255 or less characters.")]
        public string UserAccount { get; set; }
        [Required(ErrorMessage = "Message is required."), MaxLength(4000, ErrorMessage = "'Message' must be 4000 or less characters.")]
        public string Message { get; set; }
        [MaxLength(4000, ErrorMessage = "'Exception' must be 4000 or less characters.")]
        public string Exception { get; set; }
        //
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public LogData()
        {
            Id = 0;
            Date = DateTime.Now;
            Application = "";
            Method = "";
            LogLevel = 0;
            Level = "";
            UserAccount = "";
            Message = "";
            Exception = "";
        }
    }
}
// ---------------------------------------------------------------------------
