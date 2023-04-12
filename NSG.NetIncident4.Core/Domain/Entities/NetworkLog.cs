﻿//
// ---------------------------------------------------------------------------
// NetworkLog.
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Entity class of a network log incident.
    /// </summary>
    [Table("NetworkLog")]
    public partial class NetworkLog
    {
        //
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "NetworkLogId is required.")]
        public long NetworkLogId { get; set; }
        [Required(ErrorMessage = "ServerId is required.")]
        public int ServerId { get; set; }
        public long? IncidentId { get; set; }
        [Required(ErrorMessage = "IPAddress is required."), MaxLength(50, ErrorMessage = "'IPAddress' must be 50 or less characters.")]
        public string IPAddress { get; set; }
        [Required(ErrorMessage = "NetworkLogDate is required.")]
        public DateTime NetworkLogDate { get; set; }
        [Required(ErrorMessage = "Log is required."), MaxLength(1073741823, ErrorMessage = "'Log' must be 1073741823 or less characters.")]
        public string Log { get; set; }
        [Required(ErrorMessage = "IncidentTypeId is required.")]
        public int IncidentTypeId { get; set; }
        //
        public virtual IncidentType IncidentType { get; set; }
        public virtual Server Server { get; set; }
        // Incident is an optional nullable relationship
        public virtual Incident Incident { get; set; }
        //
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public NetworkLog()
        {
            NetworkLogId = 0;
            ServerId = 0;
            IPAddress = "";
            NetworkLogDate = DateTime.Now;
            Log = "";
            IncidentTypeId = 0;
            //
            IncidentType = new IncidentType();
            Server = new Server();
            Incident = new Incident();
        }
    }
}
// ---------------------------------------------------------------------------
