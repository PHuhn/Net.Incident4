//
// ---------------------------------------------------------------------------
// ApplicationUsers list query.
//
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Application.Infrastructure;
//
namespace NSG.NetIncident4.Core.Application.Commands.ApplicationUsers
{
	//
	/// <summary>
	/// 'ApplicationUser' list query, handler and handle.
	/// </summary>
	public class ApplicationUserListQuery
	{
		[System.ComponentModel.DataAnnotations.Key]
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
        public string FullName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyName { get; set; }
		//
		public ApplicationUserListQuery()
		{
			Id = "";
			UserName = "";
			Email = "";
			FullName = "";
			CompanyId = 0;
			CompanyShortName = "";
			CompanyName = "";
		}
	}
	//
	/// <summary>
	/// 'ApplicationUser' list query handler.
	/// </summary>
	public class ApplicationUserListQueryHandler : IRequestHandler<ApplicationUserListQueryHandler.ListQuery, ApplicationUserListQueryHandler.ViewModel>
	{
        //private readonly ApplicationDbContext _context;
        //private IApplication _application;
        private UserManager<ApplicationUser> _userManager;
        private IMediator Mediator;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to list the ApplicationUser entity.
        /// </summary>
        /// <param name="userManager">The identity interface for users.</param>
        /// <param name="mediator">MediatR dependency injector.</param>
        public ApplicationUserListQueryHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
		{
            _userManager = userManager;
            Mediator = mediator;
        }
		//
		/// <summary>
		/// 'ApplicationUser' query handle method, passing two interfaces.
		/// </summary>
		/// <param name="queryRequest">This list query request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>Returns a list of ApplicationUserListQuery.</returns>
		public async Task<ViewModel> Handle(ListQuery queryRequest, CancellationToken cancellationToken)
		{
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(queryRequest);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new ListQueryValidationException(_results.FluentValidationErrors());
			}
            GetUserCompanyListQueryHandler.ViewModel _companiesViewModel =
                await Mediator.Send(new GetUserCompanyListQueryHandler.ListQuery());
			//
			return new ViewModel()
			{
				ApplicationUsersList = await _userManager.Users
					.Include(u => u.Company)
					.Where(_usr => _companiesViewModel.CompanyList.Contains(_usr.CompanyId))
					.Select(cnt => cnt.ToApplicationUserListQuery()).ToListAsync()
			};
		}
		//
		/// <summary>
		/// The ApplicationUser list query class view class.
		/// </summary>
		public class ViewModel
		{
			public IList<ApplicationUserListQuery> ApplicationUsersList { get; set; }
			//
			public ViewModel()
			{
				ApplicationUsersList = new List<ApplicationUserListQuery>();
			}
		}
		//
		/// <summary>
		/// Get ApplicationUser list query class (the primary key).
		/// </summary>
		public class ListQuery : IRequest<ViewModel>
		{
		}
		//
		/// <summary>
		/// FluentValidation of the 'ApplicationUserListQuery' class.
		/// </summary>
		public class Validator : AbstractValidator<ListQuery>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'ApplicationUserListQuery' validator.
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
	/// Custom ApplicationUserListQuery validation exception.
	/// </summary>
	public class ListQueryValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of ApplicationUserListQuery validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public ListQueryValidationException(string errorMessage)
			: base($"ApplicationUserListQuery validation exception: errors: {errorMessage}")
		{
		}
	}
    //
    /// <summary>
    /// Custom ApplicationUserListQuery validation exception.
    /// </summary>
    public class ApplicationUserListQueryUserException : Exception
    {
        //
        /// <summary>
        /// Implementation of ApplicationUserListQuery validation exception.
        /// </summary>
        /// <param name="errorMessage">The validation error messages.</param>
        public ApplicationUserListQueryUserException(string errorMessage)
            : base($"ApplicationUserListQuery user validation exception: errors: {errorMessage}")
        {
        }
    }
    //
    /// <summary>
    /// Extension method.
    /// </summary>
    public static partial class Extensions
	{
		//
		/// <summary>
		/// Extension method that translates from ApplicationUser to ApplicationUserListQuery.
		/// </summary>
		/// <param name="entity">The ApplicationUser entity class.</param>
		/// <returns>'ApplicationUserListQuery' or ApplicationUser list query.</returns>
		public static ApplicationUserListQuery ToApplicationUserListQuery(this ApplicationUser entity)
		{
			return new ApplicationUserListQuery
			{
				Id = entity.Id,
				UserName = entity.UserName,
				Email = entity.Email,
                FullName = entity.FullName,
                CompanyId = entity.CompanyId,
                CompanyShortName = (entity.Company != null ? entity.Company.CompanyShortName: "-null-"),
                CompanyName = (entity.Company != null ? entity.Company.CompanyName : "-null-")
            };
		}
	}
}
// ---------------------------------------------------------------------------

