﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSG.NetIncident4.Core.Infrastructure.Authentication
{
    public class AuthSettings
    {
        //
        /// <summary>
        /// Private jwt secret, read from appsettings.json file
        /// </summary>
        public string JwtSecret { get; set; } = String.Empty;
        public string JwtIssuer { get; set; } = String.Empty;
        public string JwtAudience { get; set; } = String.Empty;
        public int JwtExpirationHours { get; set; } = 1;
        public bool JwtRequireHttps { get; set; } = true;
        //
        /// <summary>
        /// cookie settings for MVC app.
        /// </summary>
        public bool CookieSlidingExpiration { get; set; }
        public int CookieExpirationHours { get; set; }
        //
        /// <summary>
        /// allow the augular SPA access to web API.
        /// </summary>
        public string CorsAllowOrigins { get; set; } = String.Empty;
        //
    }
}
