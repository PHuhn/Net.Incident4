﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MediatR;
//
using NSG.NetIncident4.Core.Application.Commands.NICs;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.UI.Controllers;
//
// in base controller MediatR;
//
namespace NSG.NetIncident4.Core.UI.Controllers.Admin
{
    [Authorize]
    [Authorize(Policy = "AdminRole")]
    public class NicsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public NicsController(ApplicationDbContext context, IMediator mediator) : base(mediator)
        {
            _context = context;
        }

        // GET: Nics
        public async Task<IActionResult> Index()
        {
            NICListQueryHandler.ViewModel _results = await Mediator.Send(new NICListQueryHandler.ListQuery());
            return View(_results.NICsList);
        }

        // GET: Nics/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NICDetailQuery _results =
                await Mediator.Send(new NICDetailQueryHandler.DetailQuery() { NIC_Id = id });
            return View(_results);
        }
        //
        // Create()
        // Create([FromForm] NICCreateCommand model)
        //
        #region "Create section"
        //
        // GET: Nics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NICCreateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NIC _nic = await Mediator.Send(model);
                    return RedirectToAction("Edit", new { id = _nic.NIC_Id });
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
        #endregion // Create section
        //
        // -------------------------------------------------------------------
        // Edit(string id)
        // Edit([FromBody]NICUpdateCommand model)
        //
        #region "Edit section"
        //
        // GET: Nics/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NICDetailQuery _results =
                await Mediator.Send(new NICDetailQueryHandler.DetailQuery() { NIC_Id = id });
            return View(_results);
        }
        //
        // POST: Nics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] NICUpdateCommand model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ret = await Mediator.Send(model);
                }
                else
                    Base_AddErrors(ModelState);
                return RedirectToAction("Edit", new { id = model.NIC_Id });
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
        // GET: Nics/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NICDetailQuery _results =
                await Mediator.Send(new NICDetailQueryHandler.DetailQuery() { NIC_Id = id });
            return View(_results);
        }

        // POST: Nics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                int _count = await Mediator.Send(new NICDeleteCommand() { NIC_Id = id });
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
        private bool NICExists(string id)
        {
            return _context.NICs.Any(e => e.NIC_Id == id);
        }
    }
}
