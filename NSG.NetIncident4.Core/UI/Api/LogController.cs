using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        string codeName = "LogController";
        //
        /// <summary>
        /// Explicitly pass mediator
        /// </summary>
        /// <param name="mediator"></param>
        public LogController(IMediator mediator) : base(mediator)
        {
        }
        //
        // POST api/<controller>
        /// <summary>
        /// Write a remote log.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="method"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        [HttpGet]
        public async Task<LogData> PostAsync(byte severity, string method, string message, string exception = "")
        {
            //
            LogData _entity = await Mediator.Send(
                new LogCreateCommand(severity, method, message, exception));
            return _entity;
            //
        }
        //
    }
}