// ===========================================================================
// Network Incident update command.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Transactions;
//
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
    //
    /// <summary>
    /// 'Incident' update command handler.
    /// </summary>
    public class NetworkIncidentUpdateCommandHandler : IRequestHandler<NetworkIncidentSaveQuery, NetworkIncidentDetailQuery>
	{
		private readonly ApplicationDbContext _context;
        protected IMediator Mediator;
        private IApplication _application;
        private INotificationService _notification;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to update the Incident entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public NetworkIncidentUpdateCommandHandler(ApplicationDbContext context, IMediator mediator, IApplication application, INotificationService notification)
        {
            _context = context;
            Mediator = mediator;
            _application = application;
            _notification = notification;
        }
        //
        /// <summary>
        /// 'Incident' command handle method, passing two interfaces.
        /// </summary>
        /// <param name="request">This update command request.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        /// <returns>Returns the row count.</returns>
        public async Task<NetworkIncidentDetailQuery> Handle(NetworkIncidentSaveQuery request, CancellationToken cancellationToken)
		{
            string codeName = "NetworkIncidentUpdateCommandHandler";
            if (_application.IsEditableRole() == false)
            {
                throw new NetworkIncidentUpdateCommandPermissionsException("user not in editable group.");
            }
            Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(request);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new NetworkIncidentUpdateCommandValidationException(_results.FluentValidationErrors());
			}
            string _params = $"Entering with, incidentId: {request.incident.IncidentId}, UserName: {request.user.UserName}";
            System.Diagnostics.Debug.WriteLine(_params);
            //
            var _entity = await _context.Incidents
                .Include(_i => _i.IncidentIncidentNotes)
                .ThenInclude(IncidentIncidentNotes => IncidentIncidentNotes.IncidentNote)
                .SingleOrDefaultAsync(_r => _r.IncidentId == request.incident.IncidentId, cancellationToken);
			if (_entity == null)
			{
				throw new NetworkIncidentUpdateCommandKeyNotFoundException(request.incident.IncidentId);
			}
            try
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Debug, MethodBase.GetCurrentMethod(), _params));
                bool _mailedBefore = _entity.Mailed;
                // Move from update command class to entity class.
                MoveRequestToEntity(request, _entity);
                // ** IncidentNotes **
                // Add and update notes
                await IncidentNotesAddUpdateAsync(request, _entity);
                // Delete notes and IncidentIncidentNotes
                IncidentNotesDelete(request, _entity);
                // ** NetworkLogs **
                // Update logs
                NetworkLogsUpdate(request, _entity);
                // Delete Logs;
                await NetworkLogsDeleteAsync(request, _entity);
                //
                await _context.SaveChangesAsync(cancellationToken);
                //
                if (_mailedBefore == false && request.incident.Mailed == true)
                {
                    await Task.Run(async () =>
                    {
                         await EMailIspReportAsync(request);
                    });
                }
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Info, MethodBase.GetCurrentMethod(),
                    $"Inside, Saved id: {request.incident.IncidentId}"));
            }
            catch (Exception _ex)
            {
                _context.RollBackChanges();
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Warning, MethodBase.GetCurrentMethod(),
                    _ex.GetBaseException().Message, _ex));
                System.Diagnostics.Debug.WriteLine(_ex.ToString());
                throw ( new Exception($"{codeName}: update failed", _ex) );
            }
            //
            return await Mediator.Send(new NetworkIncidentDetailQueryHandler.DetailQuery() { IncidentId = _entity.IncidentId });
        }
        //
        #region "MoveRequestToEntity"
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        void MoveRequestToEntity(NetworkIncidentSaveQuery request, Incident entity)
        {
            if( entity.ServerId != request.incident.ServerId)
            {
                entity.ServerId = request.incident.ServerId;
            }
            if(entity.IPAddress != request.incident.IPAddress)
            {
                entity.IPAddress = request.incident.IPAddress;
            }
            if (entity.NIC_Id != request.incident.NIC)
            {
                entity.NIC_Id = request.incident.NIC;
            }
            if (entity.NetworkName != request.incident.NetworkName)
            {
                entity.NetworkName = request.incident.NetworkName;
            }
            if (entity.AbuseEmailAddress != request.incident.AbuseEmailAddress)
            {
                entity.AbuseEmailAddress = request.incident.AbuseEmailAddress;
            }
            if (entity.ISPTicketNumber != request.incident.ISPTicketNumber)
            {
                entity.ISPTicketNumber = request.incident.ISPTicketNumber;
            }
            if (entity.Mailed != request.incident.Mailed)
            {
                entity.Mailed = request.incident.Mailed;
            }
            if (entity.Closed != request.incident.Closed)
            {
                entity.Closed = request.incident.Closed;
            }
            if (entity.Special != request.incident.Special)
            {
                entity.Special = request.incident.Special;
            }
            if (entity.Notes != request.incident.Notes)
            {
                entity.Notes = request.incident.Notes;
            }
        }
        //
        #endregion // MoveRequestToEntity
        //
        // IncidentNotesAddUpdateAsync
        // IncidentNotesDelete
        //
        #region "IncidentNotes processing"
        //
        /// <summary>
        /// Add or update incident notes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        async Task IncidentNotesAddUpdateAsync(NetworkIncidentSaveQuery request, Incident entity)
        {
            //var _incidentIncidentNotes = new List<IncidentIncidentNote>();
            //var _incidentNotes = new List<IncidentNote>();
            foreach (IncidentNoteData _ind in request.incidentNotes.Where(_r => _r.IsChanged == true))
            {
                if (_ind.IncidentNoteId > 0)
                {
                    // Update notes
                    IncidentNote _incidentNote = await _context.IncidentNotes
                        .FirstOrDefaultAsync(_in => _in.IncidentNoteId == _ind.IncidentNoteId);
                    if (_incidentNote.NoteTypeId != _ind.NoteTypeId)
                    {
                        _incidentNote.NoteTypeId = _ind.NoteTypeId;
                    }
                    if (_incidentNote.Note != _ind.Note)
                    {
                        _incidentNote.Note = _ind.Note;
                    }
                }
                if (_ind.IncidentNoteId < 0)
                {
                    // Add notes and IncidentIncidentNotes
                    var _incidentNote = new IncidentNote()
                    {
                        NoteTypeId = _ind.NoteTypeId,
                        Note = _ind.Note,
                        CreatedDate = _ind.CreatedDate
                    };
                    _context.IncidentNotes.Add(_incidentNote);
                    var _incidentIncidentNote = new IncidentIncidentNote()
                    {
                        Incident = entity,
                        IncidentNote = _incidentNote
                    };
                    _context.IncidentIncidentNotes.Add(_incidentIncidentNote);
                }
            }
            //
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        void IncidentNotesDelete(NetworkIncidentSaveQuery request, Incident entity)
        {
            foreach (IncidentNoteData _ind in request.deletedNotes)
            {
                if (_ind.IncidentNoteId > 0)
                {
                    IncidentIncidentNote _inn = entity.IncidentIncidentNotes.FirstOrDefault(_einn => _einn.IncidentNoteId == _ind.IncidentNoteId);
                    if( _inn != null)
                    {
                        IncidentNote _incidentNoteDelete = _inn.IncidentNote;
                        _context.IncidentIncidentNotes.Remove(_inn);
                        _context.IncidentNotes.Remove(_incidentNoteDelete);
                    }
                }
            }
            //
        }
        //
        #endregion // IncidentNotes processing
        //
        // NetworkLogsUpdate
        // NetworkLogsDeleteAsync
        //
        #region "NetworkLogs processing"
        //
        /// <summary>
        /// Update network logs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        void NetworkLogsUpdate(NetworkIncidentSaveQuery request, Incident entity)
        {
            foreach (NetworkLogData _nld in request.networkLogs.Where(_l => _l.IsChanged == true))
            {
                NetworkLog _networkLog = entity.NetworkLogs.FirstOrDefault(_r => _r.NetworkLogId == _nld.NetworkLogId);
                if (_networkLog != null)
                {
                    if(_networkLog.IncidentId != _nld.IncidentId)
                    {
                        _networkLog.IncidentId = (_nld.IncidentId == 0 ? null : _nld.IncidentId);
                    }
                }
            }
        }
        //
        /// <summary>
        /// Delete network log recoreds
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        async Task<string> NetworkLogsDeleteAsync(NetworkIncidentSaveQuery request, Incident entity)
        {
            string _ret = "";
            // Delete Logs
            foreach (NetworkLogData _nld in request.deletedLogs)
            {
                if (_nld.Selected == false)
                {
                    NetworkLog _networkLog = await _context.NetworkLogs
                        .FirstOrDefaultAsync(_in => _in.NetworkLogId == _nld.NetworkLogId);
                    if(_networkLog != null)
                    {
                        _context.NetworkLogs.Remove(_networkLog);
                    }
                    else
                    {
                        _ret = (_ret == "" ? "" : ", ") + $"{_nld.IPAddress} not found.";
                    }
                }
                else
                {
                    _ret = (_ret == "" ? "" : ", ") + $"{_nld.IPAddress} is selected, please unselect and save.";
                }
            }
            return _ret;
        }
        //
        #endregion // NetworkLogs processing
        //
        //
        /// <summary>
        /// EMail the last ISP Report
        /// </summary>
        /// <param name="data"></param>
        private async Task EMailIspReportAsync(NetworkIncidentSaveQuery data)
        {
            string codeName = "EMailIspReportAsync";
            Incident _incident = await _context.Incidents
                .Include(_i => _i.IncidentIncidentNotes)
                .FirstOrDefaultAsync(i => i.IncidentId == data.incident.IncidentId);
            var _note = _incident.IncidentIncidentNotes
                .Where(_iin => _iin.IncidentNote.NoteType.NoteTypeClientScript == "email")
                .Select(_iin => _iin.IncidentNote).FirstOrDefault();
            if (_note != null)
            {
                try
                {
                    // translate the message from json string of sendgrid type
                    SendGridMessage _sgm = JsonConvert.DeserializeObject<SendGridMessage>(_note.Note);
                    await _notification.SendEmailAsync(MimeKit.SendGridExtensions.NewMimeMessage(_sgm));
                }
                catch (Exception _ex)
                {
                    await Mediator.Send(new LogCreateCommand(
                        LoggingLevel.Warning, MethodBase.GetCurrentMethod(),
                        _ex.GetBaseException().Message, _ex));
                    System.Diagnostics.Debug.WriteLine(_ex.ToString());
                    throw (new Exception($"{codeName}: email failed, see logs.", _ex));
                }
            }
        }
		//
	}
	//
	/// <summary>
	/// Custom NetworkIncidentSaveQuery record not found exception.
	/// </summary>
	public class NetworkIncidentUpdateCommandKeyNotFoundException : KeyNotFoundException
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentSaveQuery record not found exception.
		/// </summary>
		/// <param name="id">The key for the record.</param>
		public NetworkIncidentUpdateCommandKeyNotFoundException(long incidentId)
			: base($"NetworkIncidentSaveQuery key not found exception: id: {incidentId}")
		{
		}
	}
	//
	/// <summary>
	/// Custom NetworkIncidentSaveQuery validation exception.
	/// </summary>
	public class NetworkIncidentUpdateCommandValidationException : Exception
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentSaveQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public NetworkIncidentUpdateCommandValidationException(string errorMessage)
			: base($"NetworkIncidentSaveQuery validation exception: errors: {errorMessage}")
		{
		}
	}
    //
    /// <summary>
    /// Custom NetworkIncidentSaveQuery permissions exception.
    /// </summary>
    public class NetworkIncidentUpdateCommandPermissionsException : Exception
    {
        //
        /// <summary>
        /// Implementation of NetworkIncidentSaveQuery permissions exception.
        /// </summary>
        /// <param name="errorMessage">The permissions error messages.</param>
        public NetworkIncidentUpdateCommandPermissionsException(string errorMessage)
            : base($"NetworkIncidentSaveQuery permissions exception: {errorMessage}")
        {
        }
    }
    //
}
// ===========================================================================
