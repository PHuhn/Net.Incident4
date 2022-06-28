//
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.Integration.Helpers
{
    public class UnitTestFixture
    {
        //
        public SqliteConnection sqliteConnection;
        public ApplicationDbContext db_context;
        public UserManager<ApplicationUser> userManager;
        public RoleManager<ApplicationRole> roleManager;
        public IConfiguration configuration = null;
        public Dictionary<string, string> controllerHeaders =
            new Dictionary<string, string>()
            {
                ["cookie"] = "ai_user=Zn3LO|2021-11-19T15:30:13.062Z; .AspNetCore.Antiforgery.TgedqpnEzL8=CfDJ8MoWY2n3geRBvkgVUWBREdrAbSv3olymADsefAXujoG9VNEcZw3EiwDGCXW4wXuNsrXgp3p7ZTSQAlQvBV5m31kilXUR8tla-lte-Mo9j3HZFbLoXrP9DEhmmJr6wUTqcJd-4uKk5ehaN2u-Za-Jeac; ai_session=xBril|1645283096306.1|1645285831298.5",
                ["user-agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36 Edg/98.0.1108.50"
            };
        public Dictionary<string, string> apiHeaders =
            new Dictionary<string, string>()
            {
                ["Authorization"] = "Bearer 12345678901234567890123456789012345678901234567890",
                ["user-agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36 Edg/98.0.1108.50"
            };
        public Dictionary<string, string> emptyHeaders = new Dictionary<string, string>();
        //
        private TestServer _server;
        private HttpClient _client;
        //
        public TestServer server { get { return _server; } }
        public HttpClient client { get { return _client; } }
        //
        public UnitTestFixture()
        {
            SetupConfiguration();
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
            string projectPath = @"C:\Dat\Nsg\L\Web\22\Net.Incident4\NSG.NetIncident4.Core";
            IWebHostBuilder _builder = null;
            _server = null;
            _client = null;
            _builder = new WebHostBuilder()
                .UseContentRoot(projectPath)
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(projectPath);
                    config.AddJsonFile("appsettings.json");
                    config.AddUserSecrets<Program>(true);
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
            userManager = _server.Services.GetService<UserManager<ApplicationUser>>();
            roleManager = _server.Services.GetService<RoleManager<ApplicationRole>>();
            db_context = _server.Services.GetService<ApplicationDbContext>();
            if (userManager == null || roleManager == null)
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
                // .AddEnvironmentVariables()
                .AddJsonFile(appSettings, optional: true, reloadOnChange: false)
                .AddUserSecrets<Program>(true)
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
        ///  sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/UserAdmin/", controllerHeaders);
        /// </example>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <returns>
        /// ControllerContext with a mock HttpContext and an assigned IPrincipal User
        /// </returns>
        public ControllerContext Fixture_ControllerContext(string userName, string role, string path, Dictionary<string, string> headers)
        {
            //
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = Fixture_HttpContext(userName, role, path, headers);
            return controllerContext;
        }
        //
        /// <summary>
        /// Create a fake http context
        /// <example>
        ///  sut.HttpContext = Fixture_HttpContext("TestUser", "admin", "/UserAdmin/", controllerHeaders);
        /// </example>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <returns>
        /// HttpContext with a mock HttpContext and an assigned IPrincipal User
        /// </returns>
        public HttpContext Fixture_HttpContext(string userName, string role, string path, Dictionary<string, string> headers)
        {
            //
            Mock<HttpContext> httpContext = new Mock<HttpContext>();
            //
            // https://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
            if (userName != "")
            {
                IPrincipal currentUser = Fixture_TestPrincipal(userName, role);
                httpContext.SetupGet(m => m.User).Returns((ClaimsPrincipal)currentUser);
            }
            httpContext.SetupGet(m => m.Request).Returns(
                (HttpRequest)Fixture_CreateHttpRequest(path, headers));
            //
            return httpContext.Object;
        }
        //
        /// <summary>
        /// Create a fake HttpRequest
        /// </summary>
        /// <param name="path">path/route</param>
        /// <returns></returns>
        public HttpRequest Fixture_CreateHttpRequest(string path, Dictionary<string, string> headers)
        {
            // 
            // cookie: ai_user=Zn3LO|2021-11-19T15:30:13.062Z; .AspNetCore.Antiforgery.TgedqpnEzL8=CfDJ8MoWY2n3geRBvkgVUWBREdrAbSv3olymADsefAXujoG9VNEcZw3EiwDGCXW4wXuNsrXgp3p7ZTSQAlQvBV5m31kilXUR8tla-lte-Mo9j3HZFbLoXrP9DEhmmJr6wUTqcJd-4uKk5ehaN2u-Za-Jeac; ai_session=xBril|1645283096306.1|1645285831298.5
            // user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36 Edg/98.0.1108.50
            // Headers = headers, // Dictionary<string, string>
            //
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            httpRequest.SetupGet(m => m.Path).Returns((PathString)new PathString(path));
            httpRequest.SetupGet(m => m.PathBase).Returns(new PathString(path));
            httpRequest.SetupGet(m => m.Host).Returns((HostString)new HostString("localhost", 44378));
            httpRequest.SetupGet(m => m.Scheme).Returns("https");
            httpRequest.SetupGet(m => m.QueryString).Returns(new QueryString());
            HttpRequest hr = httpRequest.Object;
            return hr;
        }
        //             _context.Url = (new Mock<IUrlHelper>()).Object;
        public IUrlHelper Fixture_CreateUrlHelper(string fullPath)
        {
            string action = "ConfirmEmail";
            string controller = "Account";
            object values = null;
            string protocol = "https";
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            // urlHelper.Setup(m => m.Page).Returns(fullPath);
            urlHelper.Setup(m => m.Action(action,controller,values,protocol)).Returns(fullPath);
            IUrlHelper url = urlHelper.Object;
            return url;
        }

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
