﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
//
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    [Authorize]
    [Authorize(Policy = "AnyUserRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : BaseApiController
    {
        //
        /// <summary>
        /// Incidents controller parameterless constructor
        /// All parameters are handled by IMediator from the base BaseApiController;
        /// </summary>
        /// <param name="mediator"></param>
        public IncidentsController(IMediator mediator) : base(mediator)
        {
        }
        //
        //  GetIncidents([FromQuery(Name = "lazyLoadEvent")]string lazyLoadEvent)
        //
        #region"Incident list"
        //
        // Route("api/Incidents/{data}") ]
        // LazyLoadEvent data
        /// <summary>
        /// GET: api/Incidents
        /// Example:
        /// /api/incidents?lazyLoadEvent={"first":0,"rows":3,"filters":{"ServerId":{"value":1,"matchMode":"equals"},"Mailed":{"value":false,"matchMode":"equals"}}}
        /// </summary>
        /// <param name="lazyLoadEvent"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IncidentListQueryHandler.ViewModel> GetIncidents(string lazyLoadEvent)
        {
            // cheating hack
            string _uri = System.Web.HttpUtility.UrlDecode(Request.QueryString.Value);
            if (string.IsNullOrEmpty(_uri) || _uri.Length < 3)
            {
                IncidentListQueryHandler.ViewModel _return = new IncidentListQueryHandler.ViewModel();
                _return.message = "Invalid pagination options.";
                return _return;
            }
            if (_uri.Substring(0, 1) == "?")
            {
                _uri = _uri.Substring(1);
            }
            IncidentListQueryHandler.ViewModel _incidentViewModel =
                await Mediator.Send(new IncidentListQueryHandler.ListQuery() { JsonString = _uri });
            return _incidentViewModel;
        }
        //
        #endregion // Incident list
        //
        //  DeleteIncident(long id)
        //
        #region "Incident Delete"
        //
        /// <summary>
        /// DELETE: api/Incidents/5
        /// 
        /// Delete incident and notes and drop link to network-log.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Count of deleted (can be more than 1)</returns>
        [Authorize]
        [Authorize(Policy = "CompanyAdminRole")]
        [HttpDelete("{id}")]
        public async Task<int> DeleteIncident(long id)
        {
            int _count = 0;
            try
            {
                _count = await Mediator.Send(new IncidentDeleteCommand() { IncidentId = id });
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex ));
            }
            return _count;
        }
        //
        #endregion // Delete
        //
    }
}
