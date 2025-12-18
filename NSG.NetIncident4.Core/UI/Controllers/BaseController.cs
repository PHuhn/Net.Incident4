//
using System.Security.Claims;
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using MediatR;
using FluentValidation.Results;
//
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    //
    /// <summary>
    /// Base view controller.
    /// This requires adding the following:
    ///   partial name="_Alerts"
    /// </summary>
    public class BaseController : Controller
    {
        private IMediator _mediator;
        // protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        //
        /// <summary>
        /// Collection of messages with level.  This is displayed with _Alerts.chtml
        /// shared partial view which is added to the _layout partial.
        /// </summary>
        public static List<AlertMessage> Alerts = new List<AlertMessage>();
        //
        /// <summary>
        /// Base constructors, so initialize Alerts list of alert-messages.
        /// </summary>
        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //
        //  Error
        //  Warning
        //  Success
        //  Information
        //  AddAlertMessage
        //  AddAlertMessage
        //  Base_AddErrors(Exception except)
        //  Base_AddErrors(ValidationResult modelState)
        //  Base_AddErrors(IdentityResult modelState)
        //  Base_AddErrors(ModelStateDictionary modelState)
        //
        #region "alert message"
        //
        /// <summary>
        /// Add an error (AlertLevel) AlertMessage to list of Alerts.
        /// </summary>
        /// <param name="message">an error message</param>
        public void Error(string message)
        {
            AddAlertMessage(AlertLevel.Error, message);
        }
        //
        /// <summary>
        /// Add a warning (AlertLevel) AlertMessage to list of Alerts.
        /// </summary>
        /// <param name="message">a warning message</param>
        public void Warning(string message)
        {
            AddAlertMessage(AlertLevel.Warn, message);
        }
        //
        /// <summary>
        /// Add a success (AlertLevel) AlertMessage to list of Alerts.
        /// </summary>
        /// <param name="message">a success message</param>
        public void Success(string message)
        {
            AddAlertMessage(AlertLevel.Success, message);
        }
        //
        /// <summary>
        /// Add a info (AlertLevel) AlertMessage to list of Alerts.
        /// </summary>
        /// <param name="message">a info message</param>
        public void Information(string message)
        {
            AddAlertMessage(AlertLevel.Info, message);
        }
        //
        /// <summary>
        /// do it in one place
        /// </summary>
        /// <param name="alertLevel">level of the message</param>
        /// <param name="message">a message</param>
        private void AddAlertMessage( AlertLevel alertLevel, string message )
        {
            string _id = (Alerts.Count + 1).ToString("d3");
            Alerts.Add(new AlertMessage(_id, alertLevel.ToString().ToLower(), message));
        }
        //
        /// <summary>
        /// do it in one place
        /// </summary>
        /// <param name="id">string value, can be an identifier</param>
        /// <param name="alertLevel">level of the message</param>
        /// <param name="message">a message</param>
        private void AddAlertMessage(string id, AlertLevel alertLevel, string message)
        {
            if( id == String.Empty)
            {
                id = (Alerts.Count + 1).ToString("d3");
            }
            Alerts.Add(new AlertMessage(id, alertLevel.ToString().ToLower(), message));
        }
        //
        #endregion // alert message
        //
        /// <summary>
        /// iterate call to Error for identity-result errors.
        /// </summary>
        /// <param name="result">results from identity call</param>
        public void Base_AddErrors(Exception except)
        {
            except = except.GetBaseException();
            this.AddAlertMessage(AlertLevel.Error, except.Message);
        }
        //
        /// <summary>
        /// iterate call to Error for invalid model state errors.
        /// </summary>
        /// <param name="modelState">ModelState from view model</param>
        public void Base_AddErrors(ValidationResult modelState)
        {
            if (!modelState.IsValid)
                foreach (var _failure in modelState.Errors)
                {
                    this.AddAlertMessage(_failure.PropertyName, AlertLevel.Error, 
                        string.Format("{0}: {1}\n", _failure.PropertyName, _failure.ErrorMessage));
                }
        }
        //
        /// <summary>
        /// iterate call to Error for unsuccessful identity-result errors.
        /// </summary>
        /// <param name="modelState">IdentityResult </param>
        public void Base_AddErrors(IdentityResult modelState)
        {
            if (!modelState.Succeeded)
                foreach (var _failure in modelState.Errors)
                {
                    this.AddAlertMessage(AlertLevel.Error,
                        string.Format("{0}: {1}\n", _failure.Code, _failure.Description));
                }
        }
        //
        /// <summary>
        /// iterate call to Error for invalid model state errors.
        /// </summary>
        /// <param name="modelState">ModelState from view model</param>
        public void Base_AddErrors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                foreach (ModelError me in (modelState.Values.SelectMany(e => (e.Errors))))
                {
                    if (string.IsNullOrEmpty(me.ErrorMessage))
                    {
                        if (me.Exception != null)
                            this.AddAlertMessage(AlertLevel.Error, me.Exception.Message);
                    }
                    else
                        this.AddAlertMessage(AlertLevel.Error, me.ErrorMessage);
                }
        }
        //
        // -------------------------------------------------------------------
        //
        /// <summary>
        /// Get the current user's user name identity via controller's ClaimsPrincipal.
        /// </summary>
        /// <returns>String of the current user.</returns>
        public string Base_GetUserAccount()
        {
            var currentUserName = "";
            ClaimsPrincipal currentUser = this.User;
            if (currentUser != null && currentUser.Identity.IsAuthenticated)
                currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
            if (string.IsNullOrEmpty(currentUserName))
                currentUserName = "- Not Authenticated -";
            return currentUserName;
        }
        //
    }
    //
}
