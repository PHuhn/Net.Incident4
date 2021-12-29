using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        //
        // POST api/<controller>
        /// <summary>
        /// Write a remote log.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="method"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public async void PostAsync(byte severity, string method, string message, string exception = "")
        {
            //
            LogData _entity = await Mediator.Send(
                new LogCreateCommand(severity, method, message, exception));
            //
        }
    }
}