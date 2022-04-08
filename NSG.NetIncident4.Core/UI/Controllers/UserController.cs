// ===========================================================================
// File: UserController.cs
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;
//
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    [Authorize(AuthenticationSchemes = SharedConstants.IdentityApplicationScheme)]
    public class UserController : BaseController
    {
        string codeName = "UserController";
        //
        private UserManager<ApplicationUser> _userManager;
        //
        /// <summary>
        /// Explicitly pass userManager and  mediator
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(UserManager<ApplicationUser> userManager, IMediator mediator) : base(mediator)
        {
            _userManager = userManager;
        }
        // 
        /// <summary>
        /// GET: UserLogs
        /// </summary>
        /// <param name="event2"></param>
        /// <returns>Pagination of LogListQuery</returns>
        public async Task<ActionResult<Pagination<LogListQuery>>> UserLogs(LazyLoadEvent2 event2)
        {
            if (event2.rows == 0) { event2.rows = 5; }
            string _user = Base_GetUserAccount();
            ViewBag.UserAccount = _user;
            LogListQueryHandler.ListQuery _parm = new LogListQueryHandler.ListQuery() { UserAccount = _user, lazyLoadEvent = event2 };
            Console.WriteLine($"{codeName}: User: {_parm.UserAccount}");
            LogListQueryHandler.ViewModel _results = await Mediator.Send(_parm);
            Pagination<LogListQuery> pagination = new Pagination<LogListQuery>(
                _results.LogsList as List<LogListQuery>,
                event2,
                _results.TotalRecords
            )
            { action = "UserLogs" };
            //
            return View(pagination);
        }
        //
        /// <summary>
        /// Get the AccuWeather weather forecast for the current
        /// user's company's zipcode
        /// </summary>
        /// <returns>list of forecasts</returns>
        public async Task<ActionResult<List<Forecast>>> AccuWeather()
        {
            // get zip code from the user's company address
            string _user = Base_GetUserAccount();
            ApplicationUser? _entity = await _userManager.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(r => r.UserName == _user);
            if(_entity == null)
            {
                Error($"User id: {_user} not found.");
                return View(new List<Forecast>());
            }
            string _zipCode = _entity.Company.PostalCode;
            //
            List<Forecast> feed = Forecast.ToAccuForecast(_zipCode);
            return View(feed);
        }
        //
        /// <summary>
        /// Retrive the desired news feed
        /// </summary>
        /// <param name="url">url of the feed</param>
        /// <param name="max">maximum number of items</param>
        /// <returns>list of news items</returns>
        public async Task<ActionResult<List<News>>> NewsFeeds(string? url, int? max)
        {
            if (url == null) return View(new List<News>());
            if (max == null) max = 10;
            //
            return View( await
                NSG.NetIncident4.Core.UI.ViewHelpers.ViewHelpers.GetNewsFeed(url, max.Value));
        }
    }
}
// ===========================================================================
