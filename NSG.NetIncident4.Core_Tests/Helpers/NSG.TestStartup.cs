using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.Integration.Helpers
{
    public class TestStartup : NSG.NetIncident4.Core.Startup
    {
        //
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        //
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            //
            ApplicationDbContext db_context =
                NSG_Helpers.GetSqliteMemoryDbContext(NSG_Helpers.GetSqliteMemoryConnection(), services);
            UserManager<ApplicationUser> userManager =
                services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager =
                services.BuildServiceProvider().GetService<RoleManager<ApplicationRole>>();
            DatabaseSeeder _seeder =
                new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
        }
        //
        //public override void Configure(
        //    IApplicationBuilder app,
        //    IWebHostEnvironment env,
        //    ApplicationDbContext context,
        //    UserManager<ApplicationUser> userManager,
        //    RoleManager<ApplicationRole> roleManager)
        //{
        //    app.UseExceptionHandler(errorApp =>
        //    {
        //        System.Diagnostics.Debug.Write(errorApp.ToString());
        //    });
        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();
        //    app.UseRouting();
        //    app.UseCors("CorsAnyOrigin");
        //    app.UseAuthentication();
        //    app.UseAuthorization();
        //    app.UseCookiePolicy();
        //    app.UseSession();
        //    app.UseSwagger();
        //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSG Net-Incident4.Core v1"));
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //            name: "default",
        //            pattern: "{controller=Home}/{action=Index}/{id?}");
        //        endpoints.MapRazorPages();
        //    });
        //}
        //
    }
}
// ===========================================================================
