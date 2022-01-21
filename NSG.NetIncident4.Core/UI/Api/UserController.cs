using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
//
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        //
        public UserController(IMediator mediator) : base(mediator)
        {
        }
        //
        /// <summary>
        /// GET api/<controller>/id=5?serverShortName=nsg
        /// </summary>
        /// <param name="id">users UserName</param>
        /// <param name="serverShortName">server's short-name</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationUserServerDetailQuery> GetAsync(string id, string serverShortName)
        {
            ApplicationUserServerDetailQuery _results =
                await Mediator.Send(new ApplicationUserServerDetailQueryHandler.DetailQuery()
                    {  UserName = id, ServerShortName = serverShortName });
            return _results;
        }
        //
    }
}
//
