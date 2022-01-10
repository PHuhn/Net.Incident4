﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
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
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            string _user = Base_GetUserAccount();
            ViewBag.UserAccount = _user;
            LogListQueryHandler.ListQuery _parm = new LogListQueryHandler.ListQuery() { UserAccount = _user };
            Console.WriteLine($"{codeName}: User: {_parm.UserAccount}");
            LogListQueryHandler.ViewModel _results = await Mediator.Send(_parm);
            return View("Index", _results.LogsList);
        }
        //
    }
}