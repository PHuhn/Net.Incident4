//
// ---------------------------------------------------------------------------
//
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
//
using NSG.NetIncident4.Core.Infrastructure.Services;
using Org.BouncyCastle.Security;
//
namespace NSG.NetIncident4.Core.UI.Api
{
    //
    // -----------------------------------------------------------------------
    //
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "AnyUserRole")]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : BaseApiController
    {
        //
        /// <summary>
        /// This class maps to the appsettings.json 'ServicesSettings' JSON class.
        /// </summary>
        protected readonly ServicesSettings _servicesSettings;
        protected readonly ILogger<ServicesController> _logger;
        //
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="servicesSettings">
        /// Class that maps to the appsettings.json, the values needed for the services.
        /// </param>
        public ServicesController(IMediator mediator, ILogger<ServicesController> logger, IOptions<ServicesSettings> servicesSettings): base(mediator)
        {
            _logger = logger;
            _servicesSettings = servicesSettings.Value;
        }
        //
        // [ActionName("ping")]
        /// <summary>
        /// GET api/services/ping/192.169.3.2
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Get(string service, string ipaddress)
        {
            _logger?.LogDebug("{0} - {1}", service, ipaddress);
            switch (service.ToLower())
            {
                case "ping":
                    return Ping(ipaddress);
                case "whois":
                    return WhoIs(ipaddress);
                default:
                    throw( new InvalidParameterException ( "service of: " + service ));
            }
        }
        //
        // [ActionName("ping")]
        /// <summary>
        /// GET api/services/ping/192.169.3.2
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("Ping/{ip}")]
        public ActionResult<string> Ping(string ip)
        {
            _logger?.LogDebug("'{0}' has been invoked with {1}.", nameof(Ping), ip);
            return IpAddressCommand(ip,
                _servicesSettings.PingDir, _servicesSettings.PingCmd, _servicesSettings.PingTimeOut);
        }
        //
        // [ActionName("whois")]
        /// <summary>
        /// GET api/services/whois/192.169.3.2
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("WhoIs/{ip}")]
        public ActionResult<string> WhoIs(string ip)
        {
            _logger?.LogDebug("'{0}' has been invoked with {1}.", nameof(WhoIs), ip);
            string _whois = IpAddressCommand(ip,
                _servicesSettings.WhoisDir, _servicesSettings.WhoisCmd, _servicesSettings.WhoisTimeOut);
            string _link = WhoIsLink(_whois);
            if (_link != "")
                return IpAddressCommand(_link,
                    _servicesSettings.WhoisDir, _servicesSettings.WhoisCmd, _servicesSettings.WhoisTimeOut);
            return _whois;
        }
        //
        // -----------------------------------------------------------------------
        //
        [NonAction]
        private string IpAddressCommand(string ip, string dir, string command, int timeOut)
        {
            string _return = "";
            if (ip != "")
            {
                ip = ip.Trim();
                if (ip.Substring(0, 1).CompareTo("9") < 1 || ip.Substring(0, 3).ToLower() == "net")
                {
                    string _cmdText = String.Format(command, ip);
                    _return = NSG.Library.Helpers.OS.CallOperatingSystemCmd(_cmdText, dir, timeOut);
                }
            }
            return _return;
        }
        //
        /// <summary>
        /// link code
        /// </summary>
        /// <param name="whoisData"></param>
        /// <returns>string of ip address or empty</returns>
        [NonAction]
        protected string WhoIsLink(string whoisData)
        {
            string _link = "";
            bool _processFlag = true;
            string _nic = "";
            foreach (string _line in whoisData.Split(new[] { '\r', '\n' }))
            {
                if (_processFlag && _line.Length > 3 && (_line.Substring(0, 1) != "#" && _line.Substring(0, 1) != "%"))
                {
                    if (_line.Substring(0, 1) == "[")
                    {
                        // whois. and jwhois.
                        int _pos = _line.IndexOf("whois.");
                        if (_pos == -1)
                        {
                            // [vault.krypt.com] length is the same!
                            _pos = _line.IndexOf("vault.");
                        }
                        if (_pos > -1)
                        {
                            string[] parts = _line.Substring(_pos).Split('.');
                            _nic = _line.Substring(_pos + 6, _line.IndexOf("]") - (_pos + 6));
                        }
                    }
                    // System.Diagnostics.Debug.WriteLine(_line);
                    if (_line.Substring(0, 8).ToLower() == "netname:")
                        return "";
                    else
                    {
                        string[] _a1 = _line.Split('(');
                        if (_a1.Length > 1)
                        {
                            string[] _a2 = _a1[1].Split(')');
                            if (_a2.Length > 1)
                            {
                                if (_a2[0].Substring(0, 3) == "NET")
                                {
                                    _link = _a2[0];
                                    System.Diagnostics.Debug.WriteLine(_nic + " : " + _link);
                                    return _link;
                                }
                            }
                        }
                    }
                }
            }
            return _link;
        }
        //
    }
}