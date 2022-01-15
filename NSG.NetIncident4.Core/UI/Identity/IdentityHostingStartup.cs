using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;

[assembly: HostingStartup(typeof(NSG.NetIncident4.Core.UI.Identity.IdentityHostingStartup))]
namespace NSG.NetIncident4.Core.UI.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //
                string _connetionString = context.Configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(_connetionString))
                {
                    throw (new ApplicationException("No connection string found"));
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(_connetionString));
                services.AddDefaultIdentity<ApplicationUser>(options => {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.User.RequireUniqueEmail = true;
                        options.SignIn.RequireConfirmedEmail = true;
                    })
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                //
            });
        }
    }
}
//
