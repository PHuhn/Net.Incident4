using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Authentication;

[assembly: HostingStartup(typeof(NSG.NetIncident4.Core.UI.Identity.IdentityHostingStartup))]
namespace NSG.NetIncident4.Core.UI.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //
                string _connetionString = context.Configuration.GetConnectionString("NetIncident4");
                IdentitySettings identitySettings = context.Configuration.GetSection(
                    "IdentitySettings").Get<IdentitySettings>();
                if (string.IsNullOrEmpty(_connetionString) || identitySettings == null)
                {
                    throw (new ApplicationException("No connection string found"));
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(_connetionString));
                services.AddDefaultIdentity<ApplicationUser>(options => {
                        options.Password.RequiredLength = identitySettings.PasswordMinLength;
                        options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
                        options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
                        options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
                        options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireSpecialCharacter;
                        options.User.RequireUniqueEmail = identitySettings.UserRequireUniqueEmail;
                        options.SignIn.RequireConfirmedAccount = identitySettings.SignInRequireConfirmedAccount;
                        options.SignIn.RequireConfirmedEmail = identitySettings.SignInRequireConfirmedEmail;
                    })
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                //
            });
        }
    }
}
//
