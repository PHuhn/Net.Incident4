//
// ---------------------------------------------------------------------------
// Log list query.
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
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core.Application.Commands.Logs
{
	//
	/// <summary>
	/// 'Log' list query, handler and handle.
	/// </summary>
	public class LogListQuery
	{
		public DateTime Date { get; set; }
		public string Application { get; set; } = String.Empty;
        public string Method { get; set; } = String.Empty;
        public string Level { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public string Exception { get; set; } = String.Empty;
	}
	//
	/// <summary>
	/// 'Log' list query handler.
	/// </summary>
	public class LogListQueryHandler : IRequestHandler<LogListQueryHandler.ListQuery, LogListQueryHandler.ViewModel>
	{
		private readonly ApplicationDbContext _context;
		//
		/// <summary>
		///  The constructor for the inner handler class, to list the Log entity.
		/// </summary>
		/// <param name="context">The database interface context.</param>
		public LogListQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}
		//
		/// <summary>
		/// 'Log' query handle method, passing two interfaces.
		/// </summary>
		/// <param name="queryRequest">This list query request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>Returns a list of LogListQuery.</returns>
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
				LogsList = await _context.Logs
					.Where(lg => lg.UserAccount == queryRequest.UserAccount)
					.OrderByDescending(ob => ob.Date)
					.Select(d => d).AsQueryable()
					.LazySkipTake2<LogData>(queryRequest.lazyLoadEvent)
					.Select(cnt => cnt.ToLogListQuery()).ToListAsync(),
				TotalRecords = _context.Logs
					.Where(lg => lg.UserAccount == queryRequest.UserAccount).Count()
			};
		}
		//
		/// <summary>
		/// The Log list query class view class.
		/// </summary>
		public class ViewModel
		{
			public IList<LogListQuery> LogsList { get; set; } = new List<LogListQuery>();

			public long TotalRecords { get; set; } = 0;
		}
		//
		/// <summary>
		/// Get Log list query class (the primary key).
		/// </summary>
		public class ListQuery : IRequest<ViewModel>
		{
            public string UserAccount { get; set; } = String.Empty;
            public LazyLoadEvent2 lazyLoadEvent { get; set; }
		}
		//
		/// <summary>
		/// FluentValidation of the 'LogListQuery' class.
		/// </summary>
		public class Validator : AbstractValidator<ListQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'LogListQuery' validator.
			/// </summary>
			public Validator()
			{
				RuleFor(x => x.UserAccount).NotEmpty().MinimumLength(2).MaximumLength(256);
				RuleFor(x => x.lazyLoadEvent).NotNull();
			}
			//
		}
		//
	}
	//
	/// <summary>
	/// Custom LogListQuery validation exception.
	/// </summary>
	public class ListQueryValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of LogListQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public ListQueryValidationException(string errorMessage)
			: base($"LogListQuery validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
