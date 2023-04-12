//
// ---------------------------------------------------------------------------
// NIC list query.
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
//
namespace NSG.NetIncident4.Core.Application.Commands.NICs
{
	//
	/// <summary>
	/// 'NIC' list query, handler and handle.
	/// </summary>
	public class NICListQuery
	{
		[System.ComponentModel.DataAnnotations.Key]
		public string NIC_Id { get; set; } = String.Empty;
        public string NICDescription { get; set; } = String.Empty;
        public string NICAbuseEmailAddress { get; set; } = String.Empty;
        public string NICRestService { get; set; } = String.Empty;
        public string NICWebSite { get; set; } = String.Empty;
    }
	//
	/// <summary>
	/// 'NIC' list query handler.
	/// </summary>
	public class NICListQueryHandler : IRequestHandler<NICListQueryHandler.ListQuery, NICListQueryHandler.ViewModel>
	{
		private readonly ApplicationDbContext _context;
		//
		/// <summary>
		///  The constructor for the inner handler class, to list the NIC entity.
		/// </summary>
		/// <param name="context">The database interface context.</param>
		public NICListQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}
		//
		/// <summary>
		/// 'NIC' query handle method, passing two interfaces.
		/// </summary>
		/// <param name="queryRequest">This list query request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>Returns a list of NICListQuery.</returns>
		public async Task<ViewModel> Handle(ListQuery queryRequest, CancellationToken cancellationToken)
		{
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(queryRequest);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new ListQueryValidationException(_results.FluentValidationErrors());
			}
			//
			return new ViewModel()
			{
				NICsList = await _context.NICs
					.Select(cnt => cnt.ToNICListQuery()).ToListAsync()
			};
		}
		//
		/// <summary>
		/// The NIC list query class view class.
		/// </summary>
		public class ViewModel
		{
			public IList<NICListQuery> NICsList { get; set; } = new List<NICListQuery>();
		}
		//
		/// <summary>
		/// Get NIC list query class (the primary key).
		/// </summary>
		public class ListQuery : IRequest<ViewModel>
		{
		}
		//
		/// <summary>
		/// FluentValidation of the 'NICListQuery' class.
		/// </summary>
		public class Validator : AbstractValidator<ListQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'NICListQuery' validator.
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
	/// Custom NICListQuery validation exception.
	/// </summary>
	public class ListQueryValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of NICListQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public ListQueryValidationException(string errorMessage)
			: base($"NICListQuery validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------

