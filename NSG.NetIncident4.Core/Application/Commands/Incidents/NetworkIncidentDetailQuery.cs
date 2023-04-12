//
using System;
using System.Text;
using System.Collections.Generic;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
    //
    /// <summary>
    /// Network-Incident detail unit-of-work query.
    /// Note: web api requires public setter/getters. As of 3.0
    /// System.Text.Json.JsonSerializer does not serialize fields.
    /// </summary>
    public class NetworkIncidentDetailQuery
    {
        public NetworkIncidentData incident { get; set; }
        //
        public List<IncidentNoteData> incidentNotes { get; set; }
        public List<IncidentNoteData> deletedNotes { get; set; }
        //
        public List<NetworkLogData> networkLogs { get; set; }
        public List<NetworkLogData> deletedLogs { get; set; }
        //
        public List<IncidentTypeData> typeEmailTemplates { get; set; }
        //
        public List<SelectItem> NICs { get; set; }
        //
        public List<SelectItem> incidentTypes { get; set; }
        //
        public List<SelectItemExtra> noteTypes { get; set; }
        //
        public string message { get; set; }
        //
        public ApplicationUserServerDetailQuery user { get; set; }
        //
        public NetworkIncidentDetailQuery()
        {
            incident = new NetworkIncidentData ();
            incidentNotes = new List<IncidentNoteData>();
            deletedNotes = new List<IncidentNoteData>();
            networkLogs = new List<NetworkLogData>();
            deletedLogs = new List<NetworkLogData>();
            typeEmailTemplates = new List<IncidentTypeData>();
            NICs = new List<SelectItem>();
            incidentTypes = new List<SelectItem>();
            noteTypes = new List<SelectItemExtra>();
            message = "";
            user = new ApplicationUserServerDetailQuery();
        }
    }
    //
    // ---------------------------------------------------------------------------
    /// <summary>
    /// NetworkLog table from the database.
    /// </summary>
    public class NetworkIncidentData
    {
        #region "NetworkIncident Class Properties"
        [System.ComponentModel.DataAnnotations.Key]
        //
        /// <summary>
        /// The incident id #.  Created by DB during the log load.
        /// </summary>
        public long IncidentId { get; set; } = 0;
        //
        /// <summary>
        /// The server id # from the incident log load.
        /// </summary>
        public int ServerId { get; set; } = 0;
        //
        /// <summary>
        /// The IP Address from the incident log.
        /// </summary>
        public string IPAddress { get; set; } = String.Empty;
        //
        /// <summary>
        /// The Network Information Center for IP Address.  Most likely from WHOIS
        /// </summary>
        public string NIC { get; set; } = String.Empty;
        //
        /// <summary>
        /// The network name for the ISP.  Most likely from WHOIS
        /// </summary>
        public string NetworkName { get; set; } = String.Empty;
        //
        /// <summary>
        /// The abuse email address for the ISP.  Most likely from WHOIS
        /// </summary>
        public string AbuseEmailAddress { get; set; } = String.Empty;
        //
        /// <summary>
        /// the ISP's incident #
        /// </summary>
        public string ISPTicketNumber { get; set; } = String.Empty;
        //
        /// <summary>
        /// the incident is mailed
        /// </summary>
        public bool Mailed { get; set; } = false;
        //
        /// <summary>
        /// the incident is closed
        /// </summary>
        public bool Closed { get; set; } = false;
        //
        /// <summary>
        /// If checked then special, i.e. get back to it
        /// </summary>
        public bool Special { get; set; } = false;
        //
        /// <summary>
        /// Random notes for this incident
        /// </summary>
        public string Notes { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }
        //
        /// <summary>
        /// For pseudo column, for change tracking
        /// </summary>
        public bool IsChanged { get; set; } = false;
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("IncidentId: {0}, ", IncidentId.ToString());
            _return.AppendFormat("ServerId: {0}, ", ServerId.ToString());
            _return.AppendFormat("IPAddress: {0}, ", IPAddress);
            _return.AppendFormat("NIC: {0}, ", NIC);
            _return.AppendFormat("NetworkName: {0}, ", NetworkName);
            _return.AppendFormat("AbuseEmailAddress: {0}, ", AbuseEmailAddress);
            _return.AppendFormat("ISPTicketNumber: {0}, ", ISPTicketNumber);
            _return.AppendFormat("Mailed: {0}, ", Mailed.ToString());
            _return.AppendFormat("Closed: {0}, ", Closed.ToString());
            _return.AppendFormat("Special: {0}, ", Special.ToString());
            _return.AppendFormat("Notes: {0}, ", Notes);
            _return.AppendFormat("CreatedDate: {0}, ", CreatedDate.ToString());
            _return.AppendFormat("]");
            return _return.ToString();
        }
        public static NetworkIncidentData GetNetworkIncidentDataEmpry()
        {
            return new NetworkIncidentData()
            {
                IncidentId = 0,
                ServerId = 0,
                IPAddress = "",
                NIC = "",
                NetworkName = "",
                AbuseEmailAddress = "",
                ISPTicketNumber = "",
                Mailed = false,
                Closed = false,
                Special = false,
                Notes = "",
                IsChanged = false,
            };
        }
        //
        #endregion
    }
    //
    /// <summary>
    /// NetworkLog table from the database.
    /// </summary>
    public class NetworkLogData
    {
        #region "NetworkLogData Class Properties"
        //
        /// <summary>
        /// For column NetworkLogId
        /// </summary>
        public long NetworkLogId { get; set; } = 0;
        //
        /// <summary>
        /// For column CompanyId
        /// </summary>
        public int ServerId { get; set; }
        //
        /// <summary>
        /// For column IncidentId
        /// </summary>
        public long? IncidentId { get; set; }
        //
        /// <summary>
        /// For column IPAddress
        /// </summary>
        public string IPAddress { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column NetworkLogDate
        /// </summary>
        public DateTime NetworkLogDate { get; set; }
        //
        /// <summary>
        /// For column Logs
        /// </summary>
        public string Log { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeId
        /// </summary>
        public int IncidentTypeId { get; set; }
        //
        /// <summary>
        /// For column IncidentTypeShortDesc in IncidentType
        /// </summary>
        public string IncidentTypeShortDesc { get; set; } = String.Empty;
        //
        /// <summary>
        /// For pseudo column, for grid selection
        /// </summary>
        public bool Selected { get; set; } = false;
        //
        /// <summary>
        /// For pseudo column, for change tracking
        /// </summary>
        public bool IsChanged { get; set; } = false;
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("NetworkLogId: {0}, ", NetworkLogId.ToString());
            _return.AppendFormat("ServerId: {0}, ", ServerId);
            if (IncidentId.HasValue)
                _return.AppendFormat("IncidentId: {0}, ", IncidentId.Value);
            else
                _return.AppendFormat("IncidentId: //, ");
            _return.AppendFormat("IncidentId: {0}, ", IncidentId);
            _return.AppendFormat("IPAddress: {0}, ", IPAddress);
            _return.AppendFormat("NetworkLogDate: {0}, ", NetworkLogDate.ToString());
            _return.AppendFormat("IncidentTypeId: {0}, ", IncidentTypeId.ToString());
            _return.AppendFormat("Selected: {0}, ", Selected.ToString());
            _return.AppendFormat("IsChanged: {0}, ", IsChanged.ToString());
            _return.AppendFormat("Log: {0}]", Log);
            return _return.ToString();
        }
        //
        #endregion
    }
    //
    /// <summary>
    /// IncidentNotes table from the database.
    /// </summary>
    public class IncidentNoteData
    {
        #region "IncidentNoteData Class Properties"
        //
        /// <summary>
        /// For column IncidentNotesId
        /// </summary>
        public long IncidentNoteId { get; set; }
        //
        /// <summary>
        /// For column NotesTypeId
        /// </summary>
        public int NoteTypeId { get; set; } = 0;
        //
        /// <summary>
        /// For column NoteTypeShortDesc from NoteType
        /// </summary>
        public string NoteTypeShortDesc { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column Notes
        /// </summary>
        public string Note { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }
        //
        /// <summary>
        /// For pseudo column, for change tracking
        /// </summary>
        public bool IsChanged { get; set; } = false;
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("IncidentNotesId: {0}, ", IncidentNoteId.ToString());
            _return.AppendFormat("NoteTypeId: {0}, ", NoteTypeId.ToString());
            _return.AppendFormat("NoteTypeShortDesc: {0}, ", NoteTypeShortDesc);
            _return.AppendFormat("Note: {0}, ", Note);
            _return.AppendFormat("CreatedDate: {0}]", CreatedDate.ToString());
            return _return.ToString();
        }
        //
        #endregion
    }
    //
    /// <summary>
    /// IncidentType table from the database.
    /// </summary>
    public class IncidentTypeData
    {
        #region "IncidentTypeData Class Properties"
        //
        /// <summary>
        /// For column IncidentTypeId
        /// </summary>
        public int IncidentTypeId { get; set; } = 0;
        //
        /// <summary>
        /// For column IncidentTypeShortDesc
        /// </summary>
        public string IncidentTypeShortDesc { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeDesc
        /// </summary>
        public string IncidentTypeDesc { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeFromServer
        /// </summary>
        public bool IncidentTypeFromServer { get; set; } = false;
        //
        /// <summary>
        /// For column IncidentTypeSubjectLine
        /// </summary>
        public string IncidentTypeSubjectLine { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeEmailTemplate
        /// </summary>
        public string IncidentTypeEmailTemplate { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeTimeTemplate
        /// </summary>
        public string IncidentTypeTimeTemplate { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeThanksTemplate
        /// </summary>
        public string IncidentTypeThanksTemplate { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeLogTemplate
        /// </summary>
        public string IncidentTypeLogTemplate { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column IncidentTypeTemplate
        /// </summary>
        public string IncidentTypeTemplate { get; set; } = String.Empty;
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("IncidentTypeId: {0}, ", IncidentTypeId.ToString());
            _return.AppendFormat("IncidentTypeShortDesc: {0}, ", IncidentTypeShortDesc);
            _return.AppendFormat("IncidentTypeDesc: {0}, ", IncidentTypeDesc);
            _return.AppendFormat("IncidentTypeFromServer: {0}, ", IncidentTypeFromServer);
            _return.AppendFormat("IncidentTypeSubjectLine: {0}, ", IncidentTypeSubjectLine);
            _return.AppendFormat("IncidentTypeEmailTemplate: {0}, ", IncidentTypeEmailTemplate);
            _return.AppendFormat("IncidentTypeTimeTemplate: {0}, ", IncidentTypeTimeTemplate);
            _return.AppendFormat("IncidentTypeThanksTemplate: {0}, ", IncidentTypeThanksTemplate);
            _return.AppendFormat("IncidentTypeLogTemplate: {0}, ", IncidentTypeLogTemplate);
            _return.AppendFormat("IncidentTypeTemplate: {0}]", IncidentTypeTemplate);
            return _return.ToString();
        }
        //
        #endregion
    }
    //
    /// <summary>
    /// Interface ISelectItem.
    /// </summary>
    /// <remarks>See PrimeNG's SelectItem interface.</remarks>
    public interface ISelectItem
    {
        #region "SelectItem interface Properties"
        public object value { get; set; }
        public string label { get; set; }
        public string styleClass { get; set; }
        // methods
        public string ToString();
        #endregion // SelectItem interface Properties
    }
    //
    /// <summary>
    /// POCO of a SelectItem.
    /// </summary>
    /// <remarks>Multiple constructors</remarks>
    public class SelectItem : ISelectItem
    {
        #region "SelectItem Class Properties"
        public object value { get; set; }
        public string label { get; set; }
        public string styleClass { get; set; }
        //
        /// <summary>
        /// Create a SelectItem using parameter-less (default) constructor.
        /// </summary>
        /// <remarks></remarks>
        public SelectItem()
        {
            this.value = "";
            this.label = "";
            this.styleClass = "";
        }
        //
        /// <summary>
        /// Create a SelectItem object using 2 parameters constructor, default selected to false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <remarks></remarks>
        public SelectItem(string value, string label)
        {
            this.value = value;
            this.label = label;
            this.styleClass = "";
        }
        //
        /// <summary>
        /// Create a new object using all columns constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="selected"></param>
        /// <remarks></remarks>
        public SelectItem(string value, string label, string styleClass)
        {
            this.value = value;
            this.label = label;
            this.styleClass = styleClass;
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            return String.Format("record:[value: {0}, label: {1}, styleClass: {2}]",
                this.value, this.label, this.styleClass);
        }
        //
        #endregion // SelectItem Class Properties
    }
    //
    /// <summary>
    /// POCO of a SelectItem.
    /// </summary>
    /// <remarks>Multiple constructors</remarks>
    public class SelectItemExtra : ISelectItem
    {
        #region "SelectItem Class Properties"
        public object value { get; set; }
        public string label { get; set; }
        public string styleClass { get; set; }
        public string extra { get; set; }
        //
        /// <summary>
        /// Create a SelectItem object using 2 parameters constructor, default selected to false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <remarks></remarks>
        public SelectItemExtra(string value, string label, string extra)
        {
            this.value = value;
            this.label = label;
            this.extra = extra;
            this.styleClass = "";
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            return String.Format("record:[value: {0}, label: {1}, styleClass: {2}, extra: {3}]",
                this.value, this.label, this.styleClass, this.extra);
        }
        //
        #endregion // SelectItem Class Properties
    }
    //
}
