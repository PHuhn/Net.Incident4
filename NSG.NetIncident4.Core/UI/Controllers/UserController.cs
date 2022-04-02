// ===========================================================================
using System;
using System.Text.RegularExpressions;
using System.ServiceModel.Syndication;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MediatR;
//
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    [Authorize(AuthenticationSchemes = SharedConstants.IdentityApplicationScheme)]
    public class UserController : BaseController
    {
        string codeName = "UserController";
        //
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        //
        /// <summary>
        /// Explicitly pass mediator
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(UserManager<ApplicationUser> userManager, IMediator mediator) : base(mediator)
        {
            _userManager = userManager;
        }
        // 
        /// <summary>
        /// GET: Log
        /// </summary>
        /// <param name="event2"></param>
        /// <returns></returns>
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
            );
            //
            return View(pagination);
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            Regex _regex = new Regex("\"(.*?)\"");
            SyndicationFeed _feed = NSG.NetIncident4.Core.UI.ViewHelpers.Helpers.GetSyndicationFeed("https://rss.accuweather.com/rss/liveweather_rss.asp?locCode=" + _zipCode);
            List<SyndicationItem> _posts = _feed.Items.ToList();
            List<Forecast> _forecasts = new List<Forecast>();
            if (_posts.Count() > 2)
            {
                // Convert rss to forecasts
                for(var _lp = 0; _lp < 3; _lp++)
                {
                    var _post = _posts[_lp];
                    string[] _summary = _post.Summary.Text.Split("<");
                    string[] _forecast = _summary[0].Split(":");
                    var _matches = _regex.Matches(_summary[1]);
                    string _header = "";
                    DateTime _published = _post.PublishDate.LocalDateTime;
                    switch (_lp)
                    {
                        case 0: // zeroth is current temperature
                            _header = $"{_forecast[0]} as of {_published.ToShortTimeString()}";
                            _summary[0] = _forecast[1];
                            break;
                        case 1: // first is today's forecast
                            var dt = DateTime.Now;
                            _header = $"Today's  {dt.ToString("MMM", CultureInfo.InvariantCulture)} {dt.Day} forecast";
                            break;
                        case 2: // is tomorrow's forecast
                            var dt2 = DateTime.Now.AddDays(1);
                            _header = $"Tomorrow's  {dt2.ToString("MMM", CultureInfo.InvariantCulture)} {dt2.Day} forecast";
                            break;
                    }
                    string _title = _post.Title.Text;
                    string _description = _summary[0];
                    string _image = "";
                    if (_matches.Count > 0)
                    {
                        _image = _matches[0].Groups[1].ToString();
                    }
                    _forecasts.Add(new Forecast(_header, _title, _published, _description, _image));
                }
            }
            return View(_forecasts);
        }
    }
}
// ===========================================================================
