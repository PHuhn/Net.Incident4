using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
//
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    [Authorize(AuthenticationSchemes = SharedConstants.IdentityApplicationScheme)]
    public class LogController : BaseController
    {
        string codeName = "LogController";
        //
        /// <summary>
        /// Explicitly pass mediator
        /// </summary>
        /// <param name="mediator"></param>
        public LogController(IMediator mediator): base(mediator)
        {
        }
        // 
        /// <summary>
        /// GET: Log
        /// </summary>
        /// <param name="event2"></param>
        /// <returns></returns>
        public async Task<ActionResult<Pagination<LogListQuery>>> Index(LazyLoadEvent2? event2)
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
    }
}