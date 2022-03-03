// ===========================================================================
// File: NetworkIncidentCreateCommand.cs
// Incident create command.
using System;
using System.Threading;
using System.Threading.Tasks;
//
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Common;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
	//
	/// <summary>
	/// 'Incident' create command, handler and handle.
	/// </summary>
	public class NetworkIncidentCreateCommand : IRequest<NetworkIncidentDetailQuery>
	{
        //
        public NetworkIncidentSaveQuery SaveQuery { get; set; }
        //
    }
    //
    /// <summary>
    /// 'Incident' create command handler.
    /// </summary>
    public class NetworkIncidentCreateCommandHandler : IRequestHandler<NetworkIncidentCreateCommand, NetworkIncidentDetailQuery>
	{
		private readonly ApplicationDbContext _context;
        protected IMediator Mediator;
        private IApplication _application;
        //
        //
        /// <summary>
        ///  The constructor for the inner handler class, to create the Incident entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public NetworkIncidentCreateCommandHandler(ApplicationDbContext context, IMediator mediator, IApplication application)
        {
            _context = context;
            Mediator = mediator;
            _application = application;
        }
        //
        /// <summary>
        /// 'Incident' command handle method, passing two interfaces.
        /// </summary>
        /// <param name="request">This create command request.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        /// <returns>The Incident entity class.</returns>
        public async Task<NetworkIncidentDetailQuery> Handle(NetworkIncidentCreateCommand request, CancellationToken cancellationToken)
		{
            string codeName = "NetworkIncidentCreateCommand";
            if (_application.IsEditableRole() == false)
            {
                throw new NetworkIncidentCreateCommandPermissionsException("user not in editable group.");
            }
            Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(request.SaveQuery);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new NetworkIncidentCreateCommandValidationException(_results.FluentValidationErrors());
			}
            string _userNameIP = $"Entering with, User: {request.SaveQuery.user.UserName}, IP: {request.SaveQuery.incident.IPAddress}";
            System.Diagnostics.Debug.WriteLine(_userNameIP);
            // Move from create command class to entity class.
            Incident _entity = CreateIncidentFromRequest(request.SaveQuery);
            _context.Incidents.Add(_entity);
            //
            try
            {
                // Add the IncidentNotes and link IncidentIncidentNotes
                AddIncidentNotes(request.SaveQuery, _entity);
                await NetworkLogsUpdateAsync(request.SaveQuery, _entity);
                await NetworkLogsDeleteAsync(request.SaveQuery, _entity);
                //
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Warning, MethodBase.GetCurrentMethod(),
                    _ex.GetBaseException().Message, _ex));
                System.Diagnostics.Debug.WriteLine(_ex.ToString());
                // $"NetworkIncidentCreateCommand validation exception: errors: {errorMessage}"
                throw (new Exception($"{codeName} SaveChanges (Add/Log Update/Log Delete)", _ex));
            }
            return await Mediator.Send(new NetworkIncidentDetailQueryHandler.DetailQuery() { IncidentId = _entity.IncidentId });
		}
        //
        /// <summary>
        /// New Incident with request data transferred into it.
        /// </summary>
        /// <param name="request">requested NetworkIncidentCreateCommand</param>
        /// <returns>new Incident with request data</returns>
        Incident CreateIncidentFromRequest(NetworkIncidentSaveQuery request)
        {
            return new Incident
            {
                ServerId = request.incident.ServerId,
                IPAddress = request.incident.IPAddress,
                NIC_Id = request.incident.NIC,
                NetworkName = request.incident.NetworkName,
                AbuseEmailAddress = request.incident.AbuseEmailAddress,
                ISPTicketNumber = request.incident.ISPTicketNumber,
                // cannot mail or close incident on creation
                Mailed = false, // request.Mailed (need IncidentId),
                Closed = false, // request.Closed,
                Special = request.incident.Special,
                Notes = request.incident.Notes,
                CreatedDate = DateTime.Now,
            };
        }
        //
        /// <summary>
        /// Add IncidentNotes to the Incident.
        /// </summary>
        /// <param name="request">requested NetworkIncidentCreateCommand</param>
        /// <param name="entity">the new Incident with request data</param>
        void AddIncidentNotes(NetworkIncidentSaveQuery request, Incident entity)
        {
            //var _incidentIncidentNotes = new List<IncidentIncidentNote>();
            //var _incidentNotes = new List<IncidentNote>();
            foreach (IncidentNoteData _ind in request.incidentNotes)
            {
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
        #region "NetworkLogs processing"
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        async Task NetworkLogsUpdateAsync(NetworkIncidentSaveQuery request, Incident entity)
        {
            // List<NetworkLogData> networkLogs;
            // var _networkLogs = new List<NetworkLog>();
            foreach (NetworkLogData _nld in request.networkLogs.Where(_l => _l.Selected == true))
            {
                NetworkLog _networkLog = await _context.NetworkLogs.FirstOrDefaultAsync(_nl => _nl.NetworkLogId == _nld.NetworkLogId);
                if(_networkLog != null)
                    _networkLog.Incident = entity;
            }
            //
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        async Task NetworkLogsDeleteAsync(NetworkIncidentSaveQuery request, Incident entity)
        {
            // List<NetworkLogData> deletedLogs;
            foreach (NetworkLogData _nld in request.deletedLogs)
            {
                NetworkLog _networkLog = await _context.NetworkLogs.FirstOrDefaultAsync(_nl => _nl.NetworkLogId == _nld.NetworkLogId);
                if(_networkLog != null)
                    _context.NetworkLogs.Remove(_networkLog);
            }
            //
        }
        //
        #endregion // NetworkLogs processing
        //
	}
	//
	/// <summary>
	/// Custom NetworkIncidentCreateCommand validation exception.
	/// </summary>
	public class NetworkIncidentCreateCommandValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentCreateCommand validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public NetworkIncidentCreateCommandValidationException(string errorMessage)
			: base($"NetworkIncidentCreateCommand validation exception: errors: {errorMessage}")
		{
		}
	}
    //
    /// <summary>
    /// Custom NetworkIncidentCreateCommand permissions exception.
    /// </summary>
    public class NetworkIncidentCreateCommandPermissionsException : Exception
    {
        //
        /// <summary>
        /// Implementation of NetworkIncidentCreateCommand permissions exception.
        /// </summary>
        /// <param name="errorMessage">The permissions error messages.</param>
        public NetworkIncidentCreateCommandPermissionsException(string errorMessage)
            : base($"NetworkIncidentCreateCommand permissions exception: {errorMessage}")
        {
        }
    }
    //
}
// ===========================================================================
