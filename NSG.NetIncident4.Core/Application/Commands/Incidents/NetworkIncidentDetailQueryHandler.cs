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
using NSG.NetIncident4.Core.Infrastructure.Common;
using System.Linq;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
    //
    /// <summary>
    /// 'Incident' detail query handler.
    /// </summary>
    public class NetworkIncidentDetailQueryHandler : IRequestHandler<NetworkIncidentDetailQueryHandler.DetailQuery, NetworkIncidentDetailQuery>
	{
		private readonly ApplicationDbContext _context;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to detail the Incident entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public NetworkIncidentDetailQueryHandler(ApplicationDbContext context)
		{
			_context = context;
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
				throw new NetworkIncidentDetailQueryValidationException(_results.FluentValidationErrors());
			}
			var _entity = await GetEntityByKey(request.IncidentId);
			if (_entity == null)
			{
				throw new NetworkIncidentDetailQueryKeyNotFoundException(request.IncidentId);
			}
			//
			// Return the detail query model.
			return await CreateDetail(_entity);
		}
		//
		/// <summary>
		/// Get an entity record via the primary key.
		/// </summary>
		/// <param name="incidentId">long key</param>
		/// <returns>Returns a Incident entity record.</returns>
		private Task<Incident> GetEntityByKey(long incidentId)
		{
			return _context.Incidents
                .Include( _i => _i.Server )
                .Include( _i => _i.IncidentIncidentNotes )
                .ThenInclude(IncidentIncidentNotes => IncidentIncidentNotes.IncidentNote)
                .ThenInclude(IncidentNotes => IncidentNotes.NoteType)
                .SingleOrDefaultAsync(r => r.IncidentId == incidentId);
                // .Include("Incidents.IncidentIncidentNotes.IncidentNotes")
        }
        //
        async Task<NetworkIncidentDetailQuery> CreateDetail(Incident incident)
        {
            NetworkIncidentDetailQuery _detail = new NetworkIncidentDetailQuery();
            //
            _detail.incident = incident.ToNetworkIncidentData();
            //
            _detail.message = "";
            //
            _detail.networkLogs = await Extensions.GetNetworkLogData(_context, incident.IncidentId, incident.ServerId, incident.Mailed);
            _detail.deletedLogs = new List<NetworkLogData>();
            //
            _detail.incidentNotes = new List<IncidentNoteData>();
            foreach (IncidentIncidentNote _iin in incident.IncidentIncidentNotes)
            {
                _detail.incidentNotes.Add( _iin.IncidentNote.ToIncidentNoteData() );
            }
            _detail.deletedNotes= new List<IncidentNoteData>();
            //
            _detail.typeEmailTemplates = Extensions.GetCompanyIncidentType(_context, incident.Server.CompanyId);
            //
            _detail.NICs = Extensions.GetNICs(_context);
            //
            _detail.incidentTypes = Extensions.GetIncidentTypes(_context);
            //
            _detail.noteTypes = Extensions.GetNoteTypes(_context);
            //
            // Return the detail query model.
            return _detail;
        }
        //
        /// <summary>
        /// Get Incident detail query class (the primary key).
        /// </summary>
        public class DetailQuery : IRequest<NetworkIncidentDetailQuery>
		{
			public long IncidentId { get; set; }
		}
        //
        /// <summary>
        /// FluentValidation of the 'NetworkIncidentDetailQuery' class.
        /// </summary>
        public class Validator : AbstractValidator<DetailQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'NetworkIncidentDetailQuery' validator.
			/// </summary>
			public Validator()
			{
				//
				RuleFor(x => x.IncidentId).NotNull();
				//
			}
			//
		}
		//
	}
	//
	/// <summary>
	/// Custom NetworkIncidentDetailQuery record not found exception.
	/// </summary>
	public class NetworkIncidentDetailQueryKeyNotFoundException : KeyNotFoundException
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentDetailQuery record not found exception.
		/// </summary>
		/// <param name="id">The key for the record.</param>
		public NetworkIncidentDetailQueryKeyNotFoundException(long incidentId)
			: base($"NetworkIncidentDetailQuery key not found exception: Id: {incidentId}")
		{
		}
	}
	//
	/// <summary>
	/// Custom NetworkIncidentDetailQuery validation exception.
	/// </summary>
	public class NetworkIncidentDetailQueryValidationException : Exception
	{
		//
		/// <summary>
		/// Implementation of NetworkIncidentDetailQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public NetworkIncidentDetailQueryValidationException(string errorMessage)
			: base($"NetworkIncidentDetailQuery validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
