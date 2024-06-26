﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//
using MediatR;
//
using NSG.NetIncident4.Core.Application.Commands.Companies;
using NSG.NetIncident4.Core.Application.Commands.Servers;
using NSG.NetIncident4.Core.Application.Commands.CompanyServers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core.UI.Controllers.CompanyAdmin
{
    [Authorize(Policy = "CompanyAdminRole")]
    [Authorize(AuthenticationSchemes = SharedConstants.IdentityApplicationScheme)]
    public class CompanyServerController : BaseController
    {
        //
        public CompanyServerController(IMediator mediator) : base(mediator)
        {
        }
        //
        /// <summary>
        /// GET: CompanyServer
        /// </summary>
        /// <param name="event2"></param>
        /// <returns>Pagination of CompanyServerListQuery</returns>
        public async Task<ActionResult<Pagination<CompanyServerListQuery>>> Index(LazyLoadEvent2 event2)
        {
            if (event2.rows == 0) { event2.rows = 5; }
            CompanyServerListQueryHandler.ListQuery _parm = new CompanyServerListQueryHandler.ListQuery() { lazyLoadEvent = event2 };
            CompanyServerListQueryHandler.ViewModel _results = await Mediator.Send(_parm);
            Pagination<CompanyServerListQuery> pagination = new Pagination<CompanyServerListQuery>(
                _results.CompaniesList as List<CompanyServerListQuery>,
                event2,
                _results.TotalRecords
            )
            { action = "Index" };
            //
            return View(pagination);
        }
        //
        // GET: CompanyServer/Details/5
        public async Task<ActionResult> Details(int id)
        {
            CompanyServerDetailQuery _results =
                await Mediator.Send(new CompanyServerDetailQueryHandler.DetailQuery() { CompanyId = id });
            return View(_results);
        }
        //
        // -------------------------------------------------------------------
        // Create()
        // CompanyCreate([FromBody]CompanyCreateCommand model)
        // ServerCreate([FromBody]ServerCreateCommand model)
        //
        #region "Create section"
        //
        /// <summary>
        /// GET: CompanyServer/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new CompanyCreateCommand());
        }
        //
        /// <summary>
        /// POST: CompanyServer/CompanyCreate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CompanyCreate([FromForm]CompanyCreateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Company _company = await Mediator.Send(model);
                    return RedirectToAction("Edit", new { id = _company.CompanyId });
                }
                else
                    Base_AddErrors(ModelState);
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Create");
        }
        //
        /// <summary>
        /// POST: CompanyServer/ServerCreate
        /// </summary>
        /// <param name="model">Of type ServerCreateCommand</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ServerCreate([FromForm]ServerCreateCommand model)
        {
            try
            {
                int _companyId = model.CompanyId;
                if (ModelState.IsValid)
                {
                    if (model.DST == true && (model.DST_Start == null || model.DST_End == null))
                    {
                        Error("DTS requires start/end dates.");
                    }
                    else
                    {
                        Server _server = await Mediator.Send(model);
                    }
                }
                else
                    Base_AddErrors(ModelState);
                return RedirectToAction("Edit", new { id = _companyId });
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Index");
        }
        //
        #endregion // Create section
        //
        // -------------------------------------------------------------------
        // Edit(int id)
        // CompanyEdit([FromBody]CompanyUpdateCommand model)
        // ServerEdit([FromBody]ServerUpdateCommand model)
        //
        #region "Edit section"
        //
        /// <summary>
        /// GET: Server/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            CompanyServerDetailQuery _results =
                await Mediator.Send(new CompanyServerDetailQueryHandler.DetailQuery() { CompanyId = id });
            return View(_results);
        }
        //
        /// <summary>
        /// POST: Server/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CompanyEdit([FromForm]CompanyUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ret = await Mediator.Send(model);
                }
                else
                    Base_AddErrors(ModelState);
                return RedirectToAction("Edit", new { id = model.CompanyId });
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return View();
        }
        //
        /// <summary>
        /// POST: CompanyServer/ServerEdit/5
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ServerEdit([FromForm]ServerUpdateCommand model)
        {
            try
            {
                int _companyId = model.CompanyId;
                if (ModelState.IsValid)
                {
                    if (model.DST && model.DST_Start != null && model.DST_End != null)
                    {
                        int ret = await Mediator.Send(model);
                    }
                    else
                        Error("DTS requires start/end dates.");
                }
                else
                    Base_AddErrors(ModelState);
                return RedirectToAction("Edit", new { id = _companyId });
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Index");
        }
        //
        #endregion // Edit
        //
        // -------------------------------------------------------------------
        // Delete(int id)
        // CompanyDelete(int id, CompanyServerDetailQuery model)
        // ServerDelete(int id, ServerDetailQuery model)
        //
        #region "Delete section"
        // GET: CompanyServer/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            CompanyServerDetailQuery _results =
                await Mediator.Send(new CompanyServerDetailQueryHandler.DetailQuery() { CompanyId = id });
            return View(_results);
        }
        //
        /// <summary>
        /// POST: Server/CompanyDelete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompanyDelete(int id, CompanyServerDetailQuery model)
        {
            try
            {
                int _count = await Mediator.Send(new CompanyDeleteCommand() { CompanyId = id });
                return RedirectToAction("Index");
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Delete", new { id = id });
        }
        //
        /// <summary>
        /// POST: Server/ServerDelete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ServerDelete(int serverId, int companyId)
        {
            try
            {
                int _count = await Mediator.Send(new ServerDeleteCommand() { ServerId = serverId });
                return RedirectToAction("Delete", new { id = companyId });
            }
            catch (Exception _ex)
            {
                await Mediator.Send(new LogCreateCommand(
                    LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                    _ex.Message, _ex));
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Delete", new { id = companyId });
        }
        //
        #endregion // Delete section
        //
    }
}