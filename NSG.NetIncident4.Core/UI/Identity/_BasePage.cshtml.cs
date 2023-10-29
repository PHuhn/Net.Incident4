using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSG.NetIncident4.Core.UI.ViewModels;

namespace NSG.NetIncident4.Core.UI.Identity
{
    public class _BasePageModel : PageModel
    {
        public _BasePageModel()
        {
        }
        //
        [BindProperty]
        public static List<AlertMessage> Alerts { get; set; }
            = new List<AlertMessage>();
        //
        #region "alert methods"
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
        private void AddAlertMessage(AlertLevel alertLevel, string message)
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
            if (id == String.Empty)
            {
                id = (Alerts.Count + 1).ToString("d3");
            }
            Alerts.Add(new AlertMessage(id, alertLevel.ToString().ToLower(), message));
        }
        //
        #endregion // alert methods
        //
    }
}
