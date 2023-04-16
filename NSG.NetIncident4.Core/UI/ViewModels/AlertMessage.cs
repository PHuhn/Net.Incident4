// ===========================================================================
using static Azure.Core.HttpHeader;
using System.Diagnostics.Metrics;
using System.Text;

namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// The alert message with the log level of the message.
    /// </summary>
    public class AlertMessage
    {
        public string Id { get; set; } = String.Empty;
        public string Level { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public AlertMessage(string level, string message)
        {
            // this.Key = "";
            this.Id = "";
            this.Level = level;
            this.Message = message;
        }
        public AlertMessage(string id, string level, string message)
        {
            this.Id = id;
            this.Level = level;
            this.Message = message;
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Id: {0}, ", Id);
            _return.AppendFormat("Level: {0}, ", Level);
            _return.AppendFormat("Message: {0}]", Message);
            return _return.ToString();
        }
    }
    //
    /// <summary>
    /// The lower case string value is appended to "nsg-msg-", to get
    /// the class to display the message with the appropriate colors.
    /// </summary>
    public enum AlertLevel
    {
        Error = 0,
        Warn = 1,
        Success = 2,
        Info = 3
    }
}
// ===========================================================================
