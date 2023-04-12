//
// ---------------------------------------------------------------------------
// Company & Server list query.
//
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using NSG.NetIncident4.Core.Application.Infrastructure;
//
namespace NSG.NetIncident4.Core.Application.Commands.CompanyServers
{
    //
    /// <summary>
    /// 'Company' list query, handler and handle.
    /// </summary>
    public class CompanyServerListQuery
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        //
        public List<ServerListQuery> ServerList { get; set; }
        //
        public CompanyServerListQuery()
        {
            CompanyId = 0;
            CompanyName = "";
            CompanyShortName = "";
            //
            ServerList = new List<ServerListQuery>();
        }
    }
    //
    public class ServerListQuery
    {
        public int ServerId { get; set; } = 0;
        public string ServerShortName { get; set; } = String.Empty;
    }
    //
    /// <summary>
    /// 'Company' list query handler.
    /// </summary>
    public class CompanyServerListQueryHandler : IRequestHandler<CompanyServerListQueryHandler.ListQuery, CompanyServerListQueryHandler.ViewModel>
    {
        private readonly ApplicationDbContext _context;
        private IMediator Mediator;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to list the Company entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public CompanyServerListQueryHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            Mediator = mediator;
        }
        //
        /// <summary>
        /// 'Company' query handle method, passing two interfaces.
        /// </summary>
        /// <param name="queryRequest">This list query request.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        /// <returns>Returns a list of CompanyListQuery.</returns>
        public async Task<ViewModel> Handle(ListQuery queryRequest, CancellationToken cancellationToken)
        {
            Validator _validator = new Validator();
            ValidationResult _results = _validator.Validate(queryRequest);
            if (!_results.IsValid)
            {
                // Call the FluentValidationErrors extension method.
                throw new ListQueryValidationException(_results.FluentValidationErrors());
            }
            // allowable companies
            GetUserCompanyListQueryHandler.ViewModel _companiesViewModel =
                await Mediator.Send(new GetUserCompanyListQueryHandler.ListQuery());
            //
            return new ViewModel()
            {
                CompaniesList = await _context.Companies
                    .Include(cpy => cpy.Servers)
                    .Where(cmp => _companiesViewModel.CompanyList.Contains(cmp.CompanyId))
                    .Select(cmpy => cmpy.ToCompanyListQuery()).ToListAsync(cancellationToken)
            };
        }
        //
        /// <summary>
        /// The Company list query class view class.
        /// </summary>
        public class ViewModel
        {
            public IList<CompanyServerListQuery> CompaniesList { get; set; } = new List<CompanyServerListQuery>();
        }
        //
        /// <summary>
        /// Get Company list query class (the primary key).
        /// </summary>
        public class ListQuery : IRequest<ViewModel>
        {
        }
        //
        /// <summary>
        /// FluentValidation of the 'CompanyListQuery' class.
        /// </summary>
        public class Validator : AbstractValidator<ListQuery>
        {
            //
            /// <summary>
            /// Constructor that will invoke the 'CompanyListQuery' validator.
            /// </summary>
            public Validator()
            {
                //
            }
            //
        }
        //
    }
    //
    /// <summary>
    /// Custom CompanyListQuery validation exception.
    /// </summary>
    public class ListQueryValidationException : Exception
    {
        //
        /// <summary>
        /// Implementation of CompanyListQuery validation exception.
        /// </summary>
        /// <param name="errorMessage">The validation error messages.</param>
        public ListQueryValidationException(string errorMessage)
            : base($"CompanyListQuery validation exception: errors: {errorMessage}")
        {
        }
    }
    //
}
// ---------------------------------------------------------------------------
