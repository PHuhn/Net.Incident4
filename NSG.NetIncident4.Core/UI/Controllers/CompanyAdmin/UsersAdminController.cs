﻿//
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
//
using MediatR;
//
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using NSG.NetIncident4.Core.UI.ViewHelpers;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core.UI.Controllers.CompanyAdmin
{
    [Authorize(Policy = "CompanyAdminRole")]
    [Authorize(AuthenticationSchemes = SharedConstants.IdentityApplicationScheme)]
    public class UsersAdminController : BaseController
    {
        //
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        //
        public UsersAdminController(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IMediator mediator) : base(mediator)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        //
        // -------------------------------------------------------------------
        // Index()
        // Details(int? id)
        //
        #region "Query section"
        //
        /// <summary>
        /// GET: /CompanyUsers/
        /// or
        /// GET: /CompanyUsers/Index
		/// <param name="event2"></param>
		/// <returns>Pagination of LogListQuery</returns>
		public async Task<ActionResult<Pagination<ApplicationUserListQuery>>> Index(LazyLoadEvent2 event2)
        {
            if (event2.rows == 0) { event2.rows = 10; }
            ApplicationUserListQueryHandler.ListQuery _parm = new ApplicationUserListQueryHandler.ListQuery() { lazyLoadEvent = event2 };
            ApplicationUserListQueryHandler.ViewModel _results = await Mediator.Send(_parm);
            Pagination<ApplicationUserListQuery> pagination = new Pagination<ApplicationUserListQuery>(
                _results.ApplicationUsersList as List<ApplicationUserListQuery>,
                event2,
                _results.TotalRecords
            )
            { action = "Index" };
            //
            return View(pagination);
        }
        //
        /// <summary>
        /// GET: /CompanyUsers/Details/5
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty( id ))
            {
                return NotFound();
            }
            ApplicationUserDetailQuery _results =
                await Mediator.Send(new ApplicationUserDetailQueryHandler.DetailQuery() { UserName = id });
            return View(_results);
        }
        //
        #endregion // Query section
        //
        // -------------------------------------------------------------------
        // Edit(string id)
        // Edit([FromBody]ApplicationUserUpdateCommand model)
        //
        #region "Edit section"
        //
        // GET: NoteTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if ( string.IsNullOrEmpty(id) )
            {
                return NotFound();
            }
            ApplicationUserUpdateQuery _results =
                await Mediator.Send(new ApplicationUserUpdateQueryHandler.EditQuery() { UserName = id });
            return View(_results);
        }

        // POST: NoteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm]ApplicationUserUpdateCommand model  )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await Mediator.Send(model);
                    if(user != null)
                    {
                        if(user.EmailConfirmed == false)
                        {
                            IEmailConfirmation _confirmation = new EmailConfirmation(this, _userManager, _emailSender);
                            await _confirmation.EmailConfirmationAsync(user);
                        }
                        return RedirectToAction("Details", new { id = user.UserName });
                    }
                    Error($"Return entity of {model.UserName} was empty.");
                    return RedirectToAction("Edit", new { id = model.UserName });
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
                return RedirectToAction("Edit", new { id = model.UserName });
            }
            return View();
        }
        //
        #endregion // Edit section
        //
        // -------------------------------------------------------------------
        // Delete(string id)
        // DeleteConfirmed(string id)
        //
        #region "Delete section"
        //
        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUserDetailQuery _results =
                await Mediator.Send(new ApplicationUserDetailQueryHandler.DetailQuery() { UserName = id });
            return View(_results);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                int _count = await Mediator.Send(new ApplicationUserDeleteCommand() { UserName = id });
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
        #endregion // Delete section
        //
    }
}
//
