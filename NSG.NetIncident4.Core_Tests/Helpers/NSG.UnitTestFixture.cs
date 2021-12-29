//
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.Integration.Helpers
{
    public class UnitTestFixture : IDisposable
    {
        //
        static public SqliteConnection sqliteConnection;
        static public ApplicationDbContext db_context;
        static public UserManager<ApplicationUser> userManager;
        static public RoleManager<ApplicationRole> roleManager;
        static public IConfiguration configuration = null;
        //
        public UnitTestFixture()
        {
        }
        //
        public void Fixture_UnitTestSetup()
        {
            // Build service colection to create identity UserManager and RoleManager. 
            ServiceCollection services = new ServiceCollection();
            NSG_Helpers.AddLoggingService(services);
            // Add ASP.NET Core Identity database in memory.
            sqliteConnection = NSG_Helpers.GetSqliteMemoryConnection();
            db_context = NSG_Helpers.GetSqliteMemoryDbContext(sqliteConnection, services);
            // Get UserManager and RoleManager.
            Console.WriteLine("Fixture_UnitTestSetup: creating managers ...");
            userManager = services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
            roleManager = services.BuildServiceProvider().GetService<RoleManager<ApplicationRole>>();
            Console.WriteLine("Fixture_UnitTestSetup: created managers ...");
            if ( userManager == null || roleManager == null)
            {
                if (userManager == null)
                {
                    Console.WriteLine("userManager is null");
                }
                if (roleManager == null)
                {
                    Console.WriteLine("roleManager is null");
                }
                throw new Exception("UnitTestFixture.Fixture_UnitTestSetup: failed to create managers.");
            }
        }
        //
        /// <summary>
        /// Get category from appsettings.json
        /// </summary>
        /// <typeparam name="T">like type ServicesSettings</typeparam>
        /// <param name="settingTag">like string "ServicesSettings"</param>
        /// <returns></returns>
        public IOptions<T> GetTestConfiguration<T>(string settingTag) where T : class
        {
            return (IOptions<T>)Options.Create<T>(
                    configuration.GetSection(settingTag).Get<T>());
        }
        public void SetupConfiguration(string appSettings = "appsettings.json")
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettings, optional: true, reloadOnChange: false)
                .Build();
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateTestPrincipal(string userName, string role)
        {
            ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, $"{userName}@any.net"),
                new Claim(ClaimTypes.Role, role)
            }, "basic"));
            //
            return _user;
        }
        //
        /// <summary>
        /// Cleanup resources
        /// </summary>
        public void Dispose()
        {
            if( sqliteConnection != null )
            {
                sqliteConnection.Close();
                sqliteConnection.Dispose();
                sqliteConnection = null;
            }
            if (db_context != null)
            {
                db_context.Database.EnsureDeleted();
                db_context.Dispose();
                db_context = null;
            }
            if (userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }
            if (roleManager != null)
            {
                roleManager.Dispose();
                roleManager = null;
            }
        }
    }
}
