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
using NSG.NetIncident4.Core.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        /// Get the GovWeather weather forecast for the current
        /// user's company's zipcode
        /// </summary>
        /// <returns>
        ///  * The GovWeatherCurrentWeather class,
        ///  * Location string,
        ///  * List of GovWeather7DayForecast class.
        /// </returns>
        public async Task<ActionResult<GovWeatherCurrentAnd7DayForecast>> GovWeather()
        {
            GovWeatherCurrentAnd7DayForecast _feed = new GovWeatherCurrentAnd7DayForecast();
            try
            {
                // get zip code from the user's company address
                string _user = Base_GetUserAccount();
                ApplicationUser? _entity = await _userManager.Users
                    .Include(u => u.Company)
                    .FirstOrDefaultAsync(r => r.UserName == _user);
                if (_entity == null || _entity.Company == null)
                {
                    Error($"User id: {_user} not found.");
                    return View(_feed);
                }
                string _zipCode = _entity.Company.PostalCode;
                int _zip = 0;
                if (!string.IsNullOrEmpty(_zipCode) && _zipCode.Length > 4 && !int.TryParse(_zipCode, out _zip))
                {
                    Error($"Postal code: {_zipCode} not correct format.");
                    return View(_feed);
                }
                var _weather = new GovWeather();
                string urlString = String.Format("https://graphical.weather.gov/xml/sample_products/browser_interface/ndfdXMLclient.php?listZipCodeList={0}", _zipCode.Substring(0, 5));
                string[] latlonArrayData = await _weather.ReadDataWithUrlToArrayAsync(urlString);
                var latlon = _weather.GovLatLonData(String.Join("", latlonArrayData));
                // Current weather
                string govWeatherUrlString = String.Format("https://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&unit=0&lg=english&FcstType=dwml", latlon.lat, latlon.lon);
                string[] forecastArrayData = await _weather.ReadDataWithUrlToArrayAsync(govWeatherUrlString);
                _feed.Current = _weather.GovWeatherCurrentWeatherData(String.Join("", forecastArrayData));
                // 7 day forecast
                govWeatherUrlString = String.Format("https://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&unit=0&lg=english&FcstType=dwml", latlon.lat, latlon.lon);
                forecastArrayData = await _weather.ReadDataWithUrlToArrayAsync(govWeatherUrlString);
                var _forecast = _weather.GovWeatherForecastData(String.Join("", forecastArrayData));
                _feed.Location = _forecast.Location;
                _feed.Forecast = _forecast.Forecast;
                //
                return View(_feed);
            }
            catch (Exception _ex)
            {
                Error(_ex.Message);
            }
            return View(_feed);
        }
        //
        /// <summary>
        /// Get the AccuWeather weather forecast for the current
        /// user's company's zipcode
        /// </summary>
        /// <returns>list of forecasts</returns>
        public async Task<ActionResult<List<Forecast>>> AccuWeather()
        {
            List<Forecast> _feed = new List<Forecast>();
            try
            {
                // get zip code from the user's company address
                string _user = Base_GetUserAccount();
                ApplicationUser? _entity = await _userManager.Users
                    .Include(u => u.Company)
                    .FirstOrDefaultAsync(r => r.UserName == _user);
                if (_entity == null)
                {
                    Error($"User id: {_user} not found.");
                    return View(new List<Forecast>());
                }
                string _zipCode = _entity.Company.PostalCode;
                //
                _feed = Forecast.ToAccuForecast(_zipCode);
            }
            catch (Exception _ex)
            {
                Error(_ex.Message);
            }
            return View(_feed);
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
