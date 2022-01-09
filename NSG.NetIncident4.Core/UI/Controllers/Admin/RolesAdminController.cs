﻿// ===========================================================================
using System.Globalization;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MediatR;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Application.Commands.ApplicationRoles;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core.UI.Controllers.Admin
{
    [Authorize(Policy = "AdminRole")]
    public class RolesAdminController : BaseController
    {
        //
        public RolesAdminController(
            ILogger<RolesAdminController> logger,
            IMediator mediator) : base(mediator)
        {
            _logger = logger;
        }
        //
        private ILogger<RolesAdminController> _logger;
        //
        // -------------------------------------------------------------------
        // Index()
        // Details(string id)
        //
        #region "Query section"
        //
        // GET: /Roles/
        public async Task<ActionResult> Index()
        {
            try
            {
                ApplicationRoleListQueryHandler.ViewModel _results = await Mediator.Send(new ApplicationRoleListQueryHandler.ListQuery());
                return View(_results.ApplicationRolesList);
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                Base_AddErrors(_ex);
            }
            return View(new List<ApplicationRoleListQuery>());
        }
        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if ( string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            try
            {
                ApplicationRoleUserDetailQuery _results =
                    await Mediator.Send(new ApplicationRoleUserDetailQueryHandler.DetailQuery() { Id = id });
                return View(_results);
            }
            catch (DetailQueryKeyNotFoundException _notFound)
            {
                System.Diagnostics.Debug.WriteLine(_notFound.Message);
                Error($"Id: {id} not found.");
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Index");
        }
        //
        #endregion // Query section
        //
        // -------------------------------------------------------------------
        // Create()
        // Create([FromForm]ApplicationRoleCreateCommand  model)
        //
        #region "Create section"
        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromForm]ApplicationRoleCreateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationRole _entity = await Mediator.Send(model);
                }
                catch( Exception _ex)
                {
                    await Mediator.Send(new LogCreateCommand(
                        LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                        _ex.Message, _ex));
                    Base_AddErrors(_ex);
                }
                return RedirectToAction("Index");
            }
            Base_AddErrors(ModelState);
            return View();
        }
        //
        #endregion // Create section
        //
        // -------------------------------------------------------------------
        // Edit(string id)
        // Edit([FromForm]ApplicationRoleUpdateCommand model)
        //
        #region "Edit section"
        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            try
            {
                ApplicationRoleDetailQuery _results =
                    await Mediator.Send(new ApplicationRoleDetailQueryHandler.DetailQuery() { Id = id });
                ApplicationRoleUpdateCommand _model = new ApplicationRoleUpdateCommand() { Id = _results.Id, Name = _results.Name };
                return View(_model);
            }
            catch (DetailQueryKeyNotFoundException _notFound)
            {
                System.Diagnostics.Debug.WriteLine(_notFound.Message);
                Error($"Id: {id} not found.");
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm]ApplicationRoleUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ret = await Mediator.Send(model);
                }
                else
                    Base_AddErrors(ModelState);
                return RedirectToAction("Index");
                //return RedirectToAction("Edit", new { id = model.Id });
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
        #endregion // Edit section
        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            try
            {
                ApplicationRoleUserDetailQuery _results =
                    await Mediator.Send(new ApplicationRoleUserDetailQueryHandler.DetailQuery() { Id = id });
                return View(_results);
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                Base_AddErrors(_ex);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int _count = await Mediator.Send(new ApplicationRoleDeleteCommand() { Id = id });
                    return RedirectToAction("Index");
                }
                catch (Exception _ex)
                {
                    await Mediator.Send(new LogCreateCommand(
                        LoggingLevel.Error, MethodBase.GetCurrentMethod(),
                        _ex.Message, _ex));
                    Base_AddErrors(_ex);
                }
            }
            return RedirectToAction("Delete", new { id = id });
        }
    }
}
// ===========================================================================
