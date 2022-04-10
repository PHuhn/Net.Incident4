// ===========================================================================
using System;
//
namespace NSG.NetIncident4.Core.Infrastructure.Authentication
{
    //
    /// <summary>
    /// Allow for configuration of various password, user and sign-in setting
    /// in the creation of a user.  Values read from appsettings.json file.
    /// </summary>
    public class IdentitySettings
    {
        public int PasswordMinLength { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireSpecialCharacter { get; set; }
        public bool UserRequireUniqueEmail { get; set; }
        public bool SignInRequireConfirmedAccount { get; set; }
        public bool SignInRequireConfirmedEmail { get; set; }
    }
}
// ===========================================================================
