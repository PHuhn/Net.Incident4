//
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Net.Http;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.Integration.Helpers
{
    public class UnitTestFixture
    {
        //
        static public SqliteConnection sqliteConnection;
        static public ApplicationDbContext db_context;
        static public UserManager<ApplicationUser> userManager;
        static public RoleManager<ApplicationRole> roleManager;
        static public IConfiguration configuration = null;
        //
        private IWebHostBuilder _builder;
        private TestServer _server;
        private HttpClient _client;
        //
        public IWebHostBuilder builder { get { return _builder; } }
        public TestServer server { get { return _server; } }
        public HttpClient client { get { return _client; } }
        //
        public UnitTestFixture()
        {
        }
        //
        /// <summary>
        /// Setup for testing CQRS commands
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Fixture_UnitTestSetup()
        {
            db_context = null;
            userManager = null;
            roleManager = null;
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
        /// Setup for testing controllers
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Fixture_ControllerTestSetup()
        {
            _builder = null;
            _server = null;
            _client = null;
            _builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Dat\Nsg\L\Web\22\Net.Incident4\NSG.NetIncident4.Core")
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                }).UseStartup<TestStartup>();
            _server = new TestServer(_builder);
            _client = _server.CreateClient();
            Console.WriteLine("Fixture_ControllerTestSetup: created server & client ...");
            if (_server == null || _client == null)
            {
                if (_server == null)
                {
                    Console.WriteLine("server is null");
                }
                if (_client == null)
                {
                    Console.WriteLine("client is null");
                }
                throw new Exception("UnitTestFixture.Fixture_ControllerTestSetup: failed to create server & client.");
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
        public ClaimsPrincipal Fixture_TestPrincipal(string userName, string role)
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
        /// Create a fake controller context
        /// <example>
        ///  sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", mockMediator.Object);
        /// </example>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="mediator"></param>
        /// <returns>
        /// ControllerContext with a mock HttpContext and an assigned IPrincipal User
        /// </returns>
        public ControllerContext Fixture_ControllerContext(string userName, string role)
        {
            // https://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
            IPrincipal currentUser = Fixture_TestPrincipal(userName, role);
            //
            Mock<HttpContext> httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(m => m.User).Returns((ClaimsPrincipal)currentUser);
            //
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext.Object;
            return controllerContext;
        }
        //
        /// <summary>
        /// Cleanup resources
        /// </summary>
        [TearDown]
        public void Fixture_TearDown()
        {
            Console.WriteLine("Fixture_UnitTestSetup: Dispose ...");
            if ( sqliteConnection != null )
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
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
        }
    }
}
