// ===========================================================================
// File: EmailConfirmation_UnitTests.cs
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
//
using Moq;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using Smocks;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class EmailConfirmation_UnitTests : UnitTestFixture
    {
        //
        public IConfiguration Configuration { get; set;  }
        Mock<IEmailSender> _emailSender = null;
        //
        public EmailConfirmation_UnitTests()
        {
            string _appSettings = "appsettings.json";
            if (_appSettings != "")
                if (!File.Exists(_appSettings))
                    throw new FileNotFoundException($"Settings file: {_appSettings} not found.");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
                .Build();
            //
        }
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            //
            Fixture_UnitTestSetup();
            //
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            //
            _emailSender = new Mock<IEmailSender>();
        }
        //
        [Test()]
        public async Task EmailConfirmation_Page_Test()
        {
            // given
            //Smock.Run(async context =>
            //{
                EmailTestPageModel _testConfirmation = new EmailTestPageModel(userManager, _emailSender.Object);
                _testConfirmation.PageContext = MockHelpers.CreatePageContext(NSG_Helpers.User_Name2, "User", "/Register/", "/Account");
                // Mock<IUrlHelper> mockUrlHelper = MockHelpers.MockUrlHelper(NSG_Helpers.User_Name2, "User", "/Register/");
                // public static string Page(this IUrlHelper urlHelper, string pageName, string pageHandler, object values, string protocol)
                // mockUrlHelper
                //    .Setup( m => m.Page("/Account/ConfirmEmail", null, It.IsAny<object>(), It.IsAny<string>()))
                //    .Returns("https://localhost/Account/ConfirmEmail/")
                //    .Verifiable("UrlHelper.Page problem.");
                //_testConfirmation.Url = mockUrlHelper.Object;
                // _testConfirmation.PageContext = new PageContext(_testConfirmation.Url.ActionContext);
                // context.Setup(() => UrlHelperExtensions.Page(It.IsAny<IUrlHelper>(), "/Account/ConfirmEmail", null, It.IsAny<object>(), It.IsAny<string>())).Returns("https://localhost/Account/ConfirmEmail/");
                // when
                IActionResult _results = await _testConfirmation.OnPostAsync();
                // then
                Assert.AreEqual(_results, "https://localhost/Account/ConfirmEmail");
            //});
        }
        //
        [Test()]
        public async Task EmailConfirmation_Controller_Test()
        {
            // given
            Controller _testConfirmation = new EmailTestController();
            _testConfirmation.ControllerContext.HttpContext = Fixture_HttpContext(NSG_Helpers.User_Name2, "User", "/Register/", controllerHeaders);
            _testConfirmation.Url = Fixture_CreateUrlHelper("https://localhost/Account/ConfirmEmail/");
            IEmailConfirmation confirmation = new EmailConfirmation(_testConfirmation);
            var _user = await userManager.FindByNameAsync(NSG_Helpers.User_Name2);
            // when
            string callBackUrl = await confirmation.EmailConfirmationAsync(userManager, _emailSender.Object, _user);
            // then
            Assert.AreEqual("https://localhost/Account/ConfirmEmail", callBackUrl);
        }
        //
        [Test()]
        public async Task EmailConfirmation_ApiController_Test()
        {
            // given
            ControllerBase _testConfirmation = new EmailTestApiController();
            _testConfirmation.ControllerContext.HttpContext = Fixture_HttpContext(NSG_Helpers.User_Name2, "User", "/Register/", controllerHeaders);
            IEmailConfirmation confirmation = new EmailConfirmation(_testConfirmation);
            var _user = await userManager.FindByNameAsync(NSG_Helpers.User_Name2);
            // when
            string callBackUrl = await confirmation.EmailConfirmationAsync(userManager, _emailSender.Object, _user);
            // then
            Assert.AreEqual("https://localhost/Account/ConfirmEmail", callBackUrl);
        }
        //
    }
    public class EmailTestPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public EmailTestPageModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        //
        public async Task<IActionResult> OnPostAsync()
        {
            IEmailConfirmation confirmation = new EmailConfirmation(this);
            var _user = await _userManager.FindByNameAsync(NSG_Helpers.User_Name2);
            if (_user == null)
            {
                ModelState.AddModelError(string.Empty, $"User not found: {NSG_Helpers.User_Name2}");
                return Page();
            }
            string callBackUrl = await confirmation.EmailConfirmationAsync(_userManager, _emailSender, _user);
            ModelState.AddModelError(string.Empty, "Verification email sent.");
            return Page( );
        }
    }
    //
    public class EmailTestController : Controller
    {
        public EmailTestController()
        {
        }
        //
    }
    //
    [ApiController]
    public class EmailTestApiController : ControllerBase
    {
        public EmailTestApiController()
        {
        }
    }
    //
}
// ===========================================================================
