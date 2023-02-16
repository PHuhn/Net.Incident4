//
// ---------------------------------------------------------------------------
// Incident list query.
//
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.Infrastructure.Common;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
	//
	/// <summary>
	/// 'Incident' list query, handler and handle.
    ///  Incident Id: 52
    ///  Server Id: 4
    ///  IP Address: 103.46.138.150
    ///  NIC: apnic.net
    ///  Network Name: ZHIYUNET
    ///  Abuse Email Address: ipas@cnnic.cn
    ///  I S P Ticket Number: 
    ///  Mailed/Closed/Special: 
    ///  Notes: 
    ///  Created Date: 2018-12-19 20:42:28
	/// </summary>
	public class IncidentListQuery
    {
		[System.ComponentModel.DataAnnotations.Key]
        //
        /// <summary>
        /// The incident id #.  Created by DB during the log load.
        /// </summary>
        public long IncidentId { get; set; }
        //
        /// <summary>
        /// The server id # from the incident log load.
        /// </summary>
        public int ServerId { get; set; }
        //
        /// <summary>
        /// The IP Address from the incident log.
        /// </summary>
        public string IPAddress { get; set; }
        //
        /// <summary>
        /// The Network Information Center for IP Address.  Most likely from WHOIS
        /// </summary>
        public string NIC { get; set; }
        //
        /// <summary>
        /// The network name for the ISP.  Most likely from WHOIS
        /// </summary>
        public string NetworkName { get; set; }
        //
        /// <summary>
        /// The abuse email address for the ISP.  Most likely from WHOIS
        /// </summary>
        public string AbuseEmailAddress { get; set; }
        //
        /// <summary>
        /// the ISP's incident #
        /// </summary>
        public string ISPTicketNumber { get; set; }
        //
        /// <summary>
        /// the incident is mailed
        /// </summary>
        public bool Mailed { get; set; }
        //
        /// <summary>
        /// the incident is closed
        /// </summary>
        public bool Closed { get; set; }
        //
        /// <summary>
        /// If checked then special, i.e. get back to it
        /// </summary>
        public bool Special { get; set; }
        //
        /// <summary>
        /// Random notes for this incident
        /// </summary>
        public string Notes { get; set; }
        //
        /// <summary>
        /// For column CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }
        //
        /// <summary>
        /// For pseudo column, for change tracking
        /// </summary>
        public bool IsChanged { get; set; }
        //
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public IncidentListQuery()
        {
            IncidentId = 0;
            ServerId = 0;
            IPAddress = "";
            NIC = "";
            NetworkName = "";
            AbuseEmailAddress = "";
            ISPTicketNumber = "";
            Mailed = false;
            Closed = false;
            Special = false;
            Notes = "";
            CreatedDate = DateTime.Now;
            IsChanged = false;
        }

    }
    //
    /// <summary>
    /// 'Incident' list query handler.
    /// </summary>
    public class IncidentListQueryHandler : IRequestHandler<IncidentListQueryHandler.ListQuery, IncidentListQueryHandler.ViewModel>
	{
		private readonly ApplicationDbContext _context;
        private IApplication _application;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to list the Incident entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public IncidentListQueryHandler(ApplicationDbContext context, IApplication application)
		{
			_context = context;
            _application = application;
        }
        //
        /// <summary>
        /// 'Incident' query handle method, passing two interfaces.
        /// </summary>
        /// <param name="queryRequest">This list query request.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        /// <returns>Returns a list of IncidentListQuery.</returns>
        public async Task<ViewModel> Handle(ListQuery queryRequest, CancellationToken cancellationToken)
		{
            if(_application.IsAuthenticated() == false)
            {
                throw new IncidentListQueryPermissionsException("user not authenticated.");
            }
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(queryRequest);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new IncidentListQueryValidationException(_results.FluentValidationErrors());
			}
            ViewModel _return = new ViewModel();
            _return.loadEvent = queryRequest.JsonString;
            LazyLoadEvent? _loadEvent = JsonConvert.DeserializeObject<LazyLoadEvent>(queryRequest.JsonString);
            if( _loadEvent != null)
            {
                IQueryable<Incident> _incidentQuery = _context.Incidents
                                        .LazyFilters<Incident>(_loadEvent);
                if (!string.IsNullOrEmpty(_loadEvent.sortField))
                {
                    _incidentQuery = _incidentQuery.LazyOrderBy<Incident>(_loadEvent);
                }
                else // Default sort order
                {
                    _incidentQuery = _incidentQuery.OrderByDescending(_r => _r.IncidentId);
                }
                // 'OrderBy' must be called before the method 'Skip'.
                _incidentQuery = _incidentQuery.LazySkipTake<Incident>(_loadEvent);
                // Execute query and convert from Incident to IncidentData (POCO) ...
                _return.results = await _incidentQuery
                    .Select(incid => incid.ToIncidentListQuery()).ToListAsync();
                _return.totalRecords = GetCountPagination(_loadEvent);
            }
            else
            {
                _return.message = $"Problem deserialization of LazyLoadEvent: {queryRequest.JsonString}";
            }
            //
            return _return;
		}
        // Return a count of filtered rows of Incident
        private long GetCountPagination(LazyLoadEvent jsonData)
        {
            IQueryable<Incident> _incidentQuery = _context.Incidents;
            _incidentQuery = _incidentQuery.LazyFilters(jsonData);
            long _incidentCount = _incidentQuery.Count();
            return _incidentCount;
        }
        //
        /// <summary>
        /// The Incident list query class view class.
        /// </summary>
        public class ViewModel
		{
            /*
            ** results is an array of generic objects
            */
            public IList<IncidentListQuery> results { get; set; }
            /*
	        ** totalRecords is the total number of records
	        */
            public long totalRecords { get; set; }
            /*
	        ** the calling lazy-load-event
	        */
            public string loadEvent { get; set; }
            /*
	        ** message if any from the service
	        */
            public string message { get; set; }
            //
            public ViewModel()
            {
                results = new List<IncidentListQuery>();
                loadEvent = "";
                totalRecords = 0;
                message = "";
            }
        }
        //
        /// <summary>
        /// Get Incident list query class (the primary key).
        /// </summary>
        public class ListQuery : IRequest<ViewModel>
		{
            public string JsonString { get; set; }
        }
        //
        /// <summary>
        /// FluentValidation of the 'IncidentListQuery' class.
        /// <example>
        /// An example of the JSON:
        /// {"first":0,"rows":3,"sortOrder":1,
        ///   "filters":{"ServerId":{"value":1,"matchMode":"eq"},
        ///     "Mailed":{"value":"false","matchMode":"eq"},
        ///     "Closed":{"value":"false","matchMode":"eq"},
        ///     "Special":{"value":"false","matchMode":"eq"}},
        ///    "globalFilter":null}
        /// </example>
        /// </summary>
        public class Validator : AbstractValidator<ListQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'IncidentListQuery' validator.
			/// </summary>
			public Validator()
			{
                RuleFor(x => x.JsonString).NotNull()
                    .Must(Extensions.IsValidLazyLoadEventString)
                    .WithMessage("Invalid JSON paging request (LazyLoadEvent).");
                //
            }
            //
        }
		//
	}
	//
	/// <summary>
	/// Custom IncidentListQuery validation exception.
	/// </summary>
	public class IncidentListQueryValidationException : Exception
	{
		//
		/// <summary>
		/// Implementation of IncidentListQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public IncidentListQueryValidationException(string errorMessage)
			: base($"IncidentListQuery validation exception: errors: {errorMessage}")
		{
		}
	}
    //
    /// <summary>
    /// Custom IncidentListQuery permissions exception.
    /// </summary>
    public class IncidentListQueryPermissionsException : Exception
    {
        //
        /// <summary>
        /// Implementation of IncidentListQuery permissions exception.
        /// </summary>
        /// <param name="errorMessage">The permissions error messages.</param>
        public IncidentListQueryPermissionsException(string errorMessage)
            : base($"IncidentListQuery permissions exception: {errorMessage}")
        {
        }
    }
    //
}
// ---------------------------------------------------------------------------
