//
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
    public class NetworkIncidentSaveQuery : IRequest<NetworkIncidentDetailQuery>
    {
        //
        public NetworkIncidentData incident { get; set; }
        //
        public List<IncidentNoteData> incidentNotes { get; set; } = new List<IncidentNoteData>();
        public List<IncidentNoteData> deletedNotes { get; set; } = new List<IncidentNoteData>();
        //
        public List<NetworkLogData> networkLogs { get; set; } = new List<NetworkLogData>();
        public List<NetworkLogData> deletedLogs { get; set; } = new List<NetworkLogData>();
        //
        public UserServerData user { get; set; }
        //
        public string message { get; set; } = String.Empty;
        //
    }
    //
    /// <summary>
    /// FluentValidation of the 'NetworkIncidentSaveQuery' class.
    /// </summary>
    public class Validator : AbstractValidator<NetworkIncidentSaveQuery>
    {
        //
        /// <summary>
        /// Constructor that will invoke the 'NetworkIncidentSaveQuery' validator.
        /// </summary>
        public Validator()
        {
            //
            RuleFor(x => x.incident.IncidentId).NotNull().GreaterThan(-1);
            RuleFor(x => x.incident.ServerId).NotNull().GreaterThan(0);
            RuleFor(x => x.incident.IPAddress).NotEmpty().MinimumLength(7).MaximumLength(50)
                .Must(Extensions.ValidateIPv4);
            RuleFor(x => x.incident.NIC).NotEmpty().MaximumLength(16);
            RuleFor(x => x.incident.NetworkName).MaximumLength(255);
            RuleFor(x => x.incident.AbuseEmailAddress).MaximumLength(255);
            RuleFor(x => x.incident.ISPTicketNumber).MaximumLength(50);
            RuleFor(x => x.incident.Mailed).NotNull();
            RuleFor(x => x.incident.Closed).NotNull();
            RuleFor(x => x.incident.Special).NotNull();
            RuleFor(x => x.incident.Notes).MaximumLength(1073741823);
            //
            RuleFor(n => n.incidentNotes).NotNull();
            RuleFor(n => n.deletedNotes).NotNull();
            RuleFor(n => n.networkLogs).NotNull();
            RuleFor(n => n.deletedLogs).NotNull();
            RuleFor(u => u.user).NotNull();
            // foreach( )
            RuleForEach(x => x.incidentNotes).SetValidator(new IncidentNoteValidator());
            //
        }
        //
    }
    //
    /// <summary>
    /// 
    /// </summary>
    public class IncidentNoteValidator : AbstractValidator<IncidentNoteData>
    {
        //
        /// <summary>
        /// Constructor that will invoke the 'IncidentNoteUpdateCommand' validator.
        /// </summary>
        public IncidentNoteValidator()
        {
            //
            RuleFor(x => x.IncidentNoteId).NotNull();
            RuleFor(x => x.NoteTypeId).NotNull().GreaterThan(0);
            RuleFor(x => x.Note).NotEmpty().MaximumLength(1073741823);
            //
        }
    }
    //
    // ---------------------------------------------------------------------------
    //
    // See NetworkIncidentDetailQuery for:
    //  IncidentNoteData
    //  NetworkLogData
    //
    /// <summary>
    /// 
    /// </summary>
    public class UserServerData
    {
        #region "UserServer Class Properties"
        //
        /// <summary>
        /// For column Id
        /// </summary>
        public string Id { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column UserName
        /// </summary>
        public string UserName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column FirstName
        /// </summary>
        public string FirstName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column LastName
        /// </summary>
        public string LastName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column FullName
        /// </summary>
        public string FullName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column UserNicName
        /// </summary>
        public string UserNicName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column Email
        /// </summary>
        public string Email { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column EmailConfirmed
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;
        //
        /// <summary>
        /// For column PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column PhoneNumberConfirmed
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; } = false;
        //
        /// <summary>
        /// For column CompanyId
        /// </summary>
        public int CompanyId { get; set; }  = 0;
        //
        /// <summary>
        /// For collection of ServerShortName
        /// </summary>
        public SelectItem[] ServerShortNames { get; set; } = new SelectItem[0];
        //
        /// <summary>
        /// For collection of ServerShortName
        /// </summary>
        public string ServerShortName { get; set; } = String.Empty;
        //
        /// <summary>
        /// The currently selected server
        /// </summary>
        public ServerData? Server { get; set; }
        //
        /// <summary>
        /// For collection of roles
        /// </summary>
        public string[] Roles { get; set; } = new string[0];
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Id: {0}, ", Id);
            _return.AppendFormat("UserName: {0}, ", UserName);
            _return.AppendFormat("Email: {0}, ", Email);
            _return.AppendFormat("FirstName: {0}, ", FirstName);
            _return.AppendFormat("LastName: {0}, ", LastName);
            _return.AppendFormat("FullName: {0}, ", FullName);
            _return.AppendFormat("UserNicName: {0}, ", UserNicName);
            _return.AppendFormat("CompanyId: {0}, ", CompanyId.ToString());
            _return.AppendFormat("EmailConfirmed: {0}, ", EmailConfirmed.ToString());
            _return.AppendFormat("PhoneNumber: {0}, ", PhoneNumber);
            _return.AppendFormat("PhoneNumberConfirmed: {0}, ", PhoneNumberConfirmed.ToString());
            _return.AppendFormat("ServerShortName: {0}, ", ServerShortName);
            return _return.ToString();
            //
        }
        //
        #endregion
    }
    //
    /// <summary>
    /// 
    /// </summary>
    public class ServerData
    {
        #region "Class Properties"
        //
        /// <summary>
        /// For column ServerId
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public int ServerId { get; set; }
        //
        /// <summary>
        /// For column CompanyId
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "'Company Id' must ba a value greater than 0")]
        public int CompanyId { get; set; }
        //
        public string CompanyName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column ServerShortName
        /// </summary>
        [Required(ErrorMessage = "'Server Short Name' is required."),
            MinLength(6, ErrorMessage = "'Server Short Name' must be 6 or up to 12 characters."),
            MaxLength(12, ErrorMessage = "'Server Short Name' must be 12 or less characters.")]
        public string ServerShortName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column ServerName
        /// </summary>
        [Required(ErrorMessage = "'Server Name' is required."), MaxLength(80, ErrorMessage = "'Server Name' must be 80 or less characters.")]
        public string ServerName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column ServerDescription
        /// </summary>
        [Required(ErrorMessage = "'Server Description' is required."), MaxLength(255, ErrorMessage = "'Server Description' must be 255 or less characters.")]
        public string ServerDescription { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column ServerLocation
        /// </summary>
        [Required(ErrorMessage = "'Device' is required."), MaxLength(255, ErrorMessage = "'Device' must be 255 or less characters.")]
        public string WebSite { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column ServerLocation
        /// </summary>
        [Required(ErrorMessage = "'Server Location' is required."), MaxLength(255, ErrorMessage = "'Server Location' must be 255 or less characters.")]
        public string ServerLocation { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column FromName
        /// </summary>
        [Required(ErrorMessage = "'From Name' is required."), MaxLength(255, ErrorMessage = "'From Name' must be 255 or less characters.")]
        public string FromName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column FromNicName
        /// </summary>
        [Required(ErrorMessage = "'From Nic Name' is required."), MaxLength(16, ErrorMessage = "'From Nic Name' must be 16 or less characters.")]
        public string FromNicName { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column FromEmailAddress
        /// </summary>
        [Required(ErrorMessage = "'From Email Address' is required."), MaxLength(255, ErrorMessage = "'From Email Address' must be 255 or less characters.")]
        public string FromEmailAddress { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column TimeZone
        /// </summary>
        [Required(ErrorMessage = "'Time-Zone' is required."), MaxLength(16, ErrorMessage = "'Time-Zone' must be 16 or less characters.")]
        public string TimeZone { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column DST
        /// </summary>
        [Required(ErrorMessage = "'DST' is required.")]
        public bool DST { get; set; } = false;
        //
        /// <summary>
        /// For column TimeZone_DST
        /// </summary>
        [MaxLength(16, ErrorMessage = "'Time-Zone DST' must be 16 or less characters.")]
        public string TimeZone_DST { get; set; } = String.Empty;
        //
        /// <summary>
        /// For column DST_Start
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? DST_Start { get; set; }
        //
        /// <summary>
        /// For column DST_End
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? DST_End { get; set; }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("ServerId: {0}, ", ServerId.ToString());
            _return.AppendFormat("CompanyId: {0}, ", CompanyId.ToString());
            _return.AppendFormat("ServerShortName: {0}, ", ServerShortName);
            _return.AppendFormat("ServerName: {0}, ", ServerName);
            _return.AppendFormat("ServerDescription: {0}, ", ServerDescription);
            _return.AppendFormat("ServerLocation: {0}, ", ServerLocation);
            _return.AppendFormat("FromName: {0}, ", FromName);
            _return.AppendFormat("FromNicName: {0}, ", FromNicName);
            _return.AppendFormat("FromEmailAddress: {0}, ", FromEmailAddress);
            _return.AppendFormat("TimeZone: {0}, ", TimeZone);
            _return.AppendFormat("DST: {0}, ", DST.ToString());
            _return.AppendFormat("TimeZone_DST: {0}, ", TimeZone_DST);
            if (DST_Start.HasValue)
                _return.AppendFormat("DST_Start: {0}, ", DST_Start.ToString());
            else
                _return.AppendFormat("/DST_Start/, ");
            if (DST_End.HasValue)
                _return.AppendFormat("DST_End: {0}, ", DST_End.ToString());
            else
                _return.AppendFormat("/DST_End/, ");
            _return.AppendFormat("]");
            return _return.ToString();
        }
        //
        #endregion
    }
    //
}
//
