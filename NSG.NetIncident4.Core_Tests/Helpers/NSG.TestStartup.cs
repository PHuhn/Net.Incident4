using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
//
namespace NSG.Integration.Helpers
{
    public class TestStartup : NSG.NetIncident4.Core.Startup
    {
        //
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public void ErrorHandler(object sender, EventArgs e)
        {
        }
        //
        public override void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            app.UseExceptionHandler(errorApp =>
            {
                System.Diagnostics.Debug.Write(errorApp.ToString());
            });
            // Perform all the configuration in the base class
            base.Configure(app, env, context, userManager, roleManager);
            // Now seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
                _seeder.Seed().Wait();
            }
        }
        //
    }
}
