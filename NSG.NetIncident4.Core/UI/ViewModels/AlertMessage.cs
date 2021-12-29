// ===========================================================================
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    //
    /// <summary>
    /// The alert message with the log level of the message.
    /// </summary>
    public class AlertMessage
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public AlertMessage(string level, string message)
        {
            // this.Key = "";
            // this.Code = "";
            this.Level = level;
            this.Message = message;
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
