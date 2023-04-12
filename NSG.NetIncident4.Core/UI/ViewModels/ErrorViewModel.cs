using System;

namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = String.Empty;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}