//
// ---------------------------------------------------------------------------
// Incident detail query.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//
using MediatR;
using FluentValidation;
using FluentValidation.Results;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
	//
	/// <summary>
	/// 'Incident' detail query handler.
	/// </summary>
	public class NetworkIncidentCreateQueryHandler : IRequestHandler<NetworkIncidentCreateQueryHandler.DetailQuery, NetworkIncidentDetailQuery>
	{
		private readonly ApplicationDbContext _context;
		protected IMediator Mediator;
		//
		/// <summary>
		///  The constructor for the inner handler class, to detail the Incident entity.
		/// </summary>
		/// <param name="context">The database interface context.</param>
		public NetworkIncidentCreateQueryHandler(ApplicationDbContext context, IMediator mediator)
		{
			_context = context;
			Mediator = mediator;
		}
		//
		/// <summary>
		/// 'Incident' query handle method, passing two interfaces.
		/// </summary>
		/// <param name="request">This detail query request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>Returns the row count.</returns>
		public async Task<NetworkIncidentDetailQuery> Handle(DetailQuery request, CancellationToken cancellationToken)
		{
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(request);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new NetworkIncidentCreateQueryValidationException(_results.FluentValidationErrors());
			}
			Server _server = await Extensions.GetServerByKey(_context, request.ServerId);
			if (_server == null)
			{
				throw new NetworkIncidentCreateQueryKeyNotFoundException(request.ServerId);
			}
            // Return the detail query model.
            return await CreateEmpty(_server, request.UserName);
		}
        //
        /// <summary>
        /// Create an empty unit-of-work NetworkIncidentDetailQuery,
        /// with EmailTemplates, NICs, IncidentTypes, NoteTypes populated.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        async Task<NetworkIncidentDetailQuery> CreateEmpty(Server server, string userName)
        {
            NetworkIncidentDetailQuery _detail = new NetworkIncidentDetailQuery();
			//
			_detail.incident = NetworkIncidentData.GetNetworkIncidentDataEmpry();
			_detail.incident.ServerId = server.ServerId;
            //
            _detail.message = "";
            //
            _detail.networkLogs = new List<NetworkLogData>();
            _detail.deletedLogs = new List<NetworkLogData>();
            //
            _detail.incidentNotes = new List<IncidentNoteData>();
            _detail.deletedNotes = new List<IncidentNoteData>();
            //
            _detail.typeEmailTemplates = Extensions.GetCompanyIncidentType(_context, server.CompanyId);
            //
            _detail.NICs = Extensions.GetNICs(_context);
            //
            _detail.incidentTypes = Extensions.GetIncidentTypes(_context);
            //
            _detail.noteTypes = Extensions.GetNoteTypes(_context);
			//
			_detail.user = await Mediator.Send(new ApplicationUserServerDetailQueryHandler.DetailQuery()
				{ UserName = userName, ServerShortName = server.ServerShortName });
			// _detail.user.Server = ;
			// Return the detail query model.
			return _detail;
        }
		//
		/// <summary>
		/// Get Incident detail query class (the primary key).
		/// </summary>
		public class DetailQuery : IRequest<NetworkIncidentDetailQuery>
		{
			public int ServerId { get; set; }
			//
			public string UserName { get; set; }
		}
		//
		/// <summary>
		/// FluentValidation of the 'NetworkIncidentCreateQuery' class.
		/// </summary>
		public class Validator : AbstractValidator<DetailQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'NetworkIncidentCreateQuery' validator.
			/// </summary>
			public Validator()
			{
				//
				RuleFor(x => x.ServerId).NotNull();
				//
				RuleFor(x => x.UserName).NotNull().NotEmpty();
				//
			}
			//
		}
		//
	}
	//
	/// <summary>
	/// Custom NetworkIncidentCreateQuery record not found exception.
	/// </summary>
	public class NetworkIncidentCreateQueryKeyNotFoundException : KeyNotFoundException
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentCreateQuery record not found exception.
		/// </summary>
		/// <param name="id">The key for the record.</param>
		public NetworkIncidentCreateQueryKeyNotFoundException(long incidentId)
			: base($"NetworkIncidentCreateQuery key not found exception: Id: {incidentId}")
		{
		}
	}
	//
	/// <summary>
	/// Custom NetworkIncidentCreateQuery validation exception.
	/// </summary>
	public class NetworkIncidentCreateQueryValidationException : Exception
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentCreateQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public NetworkIncidentCreateQueryValidationException(string errorMessage)
			: base($"NetworkIncidentCreateQuery validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
