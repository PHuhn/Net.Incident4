//
// ---------------------------------------------------------------------------
//
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
//
using MediatR;
using Microsoft.AspNetCore.Cors;
//
namespace NSG.NetIncident4.Core.UI.Api
{
	[ApiController]
	public class BaseApiController : ControllerBase
	{
		// private IMediator _mediator;
		protected IMediator Mediator; // => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
		//
		/// <summary>
		/// Base constructors, so initialize Alerts list of alert-messages.
		/// </summary>
		public BaseApiController(IMediator mediator)
		{
			Mediator = mediator;
		}
		//
		// -------------------------------------------------------------------
		//
		/// <summary>
		/// Get the current user's user name identity via controller's ClaimsPrincipal.
		/// </summary>
		/// <returns>String of the current user.</returns>
		[NonAction]
		public string Base_GetUserAccount()
		{
			var currentUserName = "";
			ClaimsPrincipal currentUser = this.User;
			if (currentUser != null && currentUser.Identity.IsAuthenticated)
				currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
			if (string.IsNullOrEmpty(currentUserName))
				currentUserName = "- Not Authenticated -";
			return currentUserName;
		}
		//
	}
}