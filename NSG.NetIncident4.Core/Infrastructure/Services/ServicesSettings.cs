//
// ---------------------------------------------------------------------------
// This class maps to the appsettings.json 'ServicesSettings' JSON class.
//
using System;
//
namespace NSG.NetIncident4.Core.Infrastructure.Services
{
    //
    /// <summary>
    /// Matches the appsettings.json 'ServicesSettings' JSON class,
    /// to be injected into services.
    /// </summary>
    public class ServicesSettings
    {
        //
        /// <summary>
        /// Default folder to execute the ping command
        /// </summary>
        public string PingDir { get; set; } = String.Empty;
        //
        /// <summary>
        /// The ping command to execute, string.Format , IP address is the 0 arg
        /// </summary>
        public string PingCmd { get; set; } = String.Empty;
        //
        /// <summary>
        /// Default folder to execute the whois command
        /// </summary>
        public string WhoisDir { get; set; } = String.Empty;
        //
        /// <summary>
        /// The whois command to execute, string.Format , IP address is the 0 arg
        /// </summary>
        public string WhoisCmd { get; set; } = String.Empty;
        //
    }
    //
}
// ---------------------------------------------------------------------------
