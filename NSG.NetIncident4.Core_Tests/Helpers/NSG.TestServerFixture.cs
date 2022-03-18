// ===========================================================================
using System;
using System.IO;
using System.Net.Http;
//
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.Integration.Helpers
{
    public class TestServerFixture: IDisposable
    {
        //
        private string _codeFixture = "NSG.Integration.Helpers.TestServerFixture";
        private ApplicationDbContext _db_context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private TestServer _server;
        private HttpClient _client;
        //
        public TestServer server { get { return _server; } }
        public HttpClient client { get { return _client; } }
        public ApplicationDbContext db_context { get { return _db_context; } }
        public RoleManager<ApplicationRole> roleManager { get { return _roleManager; } }
        public UserManager<ApplicationUser> userManager { get { return _userManager; } }
        //
        /// <summary>
        /// Class constructor
        /// </summary>
        public TestServerFixture()
        {
            string projectPath = @"C:\Dat\Nsg\L\Web\22\Net.Incident4\NSG.NetIncident4.Core";
            IWebHostBuilder _builder = new WebHostBuilder()
                .UseContentRoot(projectPath)
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(projectPath);
                    config.AddJsonFile("appsettings.json");
                })
                .UseStartup<TestStartup>();
            _server = new TestServer(_builder);
            if (_server == null)
            {
                throw new Exception($"{_codeFixture}: failed to create test server.");
            }
            _client = _server.CreateClient();
            if (_client == null)
            {
                throw new Exception($"{ _codeFixture }: failed to create test HTTP client.");
            }
            //
            _client.BaseAddress = new Uri("https://localhost:44378/");
            _userManager = _server.Services.GetService<UserManager<ApplicationUser>>();
            _roleManager = _server.Services.GetService<RoleManager<ApplicationRole>>();
            _db_context = _server.Services.GetService<ApplicationDbContext>();
            if (userManager == null || roleManager == null)
            {
                if (userManager == null)
                {
                    Console.WriteLine($"{_codeFixture}: userManager is null");
                }
                if (roleManager == null)
                {
                    Console.WriteLine($"{_codeFixture}: roleManager is null");
                }
                throw new Exception($"{_codeFixture}: failed to create managers.");
            }
        }
        //
        /// <summary>
        /// delete/dispose resources
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
            _userManager.Dispose();
            _roleManager.Dispose();
            _db_context.Dispose();
            _client = null;
            _server = null;
            _userManager = null;
            _roleManager = null;
            _db_context = null;
        }
        //
    }
}
// ===========================================================================
