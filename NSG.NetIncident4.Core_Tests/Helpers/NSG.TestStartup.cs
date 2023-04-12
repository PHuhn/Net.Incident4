// ===========================================================================
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
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
            ApplicationDbContext db_context = MockHelpers.GetApplicationDbContextMock().Object;
            UserManager<ApplicationUser> userManager = MockHelpers.GetMockUserManager<ApplicationUser>().Object;
            RoleManager<ApplicationRole> roleManager = MockHelpers.GetMockRoleManager<ApplicationRole>().Object;
        }
        //
    }
}
// ===========================================================================
