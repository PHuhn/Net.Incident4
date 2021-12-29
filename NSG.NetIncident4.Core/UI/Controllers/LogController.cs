using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    [Authorize]
    public class LogController : BaseController
    {
        string codeName = "LogController";
        // 
        /// <summary>
        /// GET: Log
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            string _user = Base_GetUserAccount();
            ViewBag.UserAccount = _user;
            LogListQueryHandler.ListQuery _parm = new LogListQueryHandler.ListQuery() { UserAccount = _user };
            Console.WriteLine($"{codeName}: User: {_parm.UserAccount} ");
            Console.WriteLine(Mediator);
            LogListQueryHandler.ViewModel _results = await Mediator.Send(_parm);
            return View(_results.LogsList);
        }
    }
}