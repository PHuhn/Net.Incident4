// ===========================================================================
// File: EmailConfirmation_UnitTests.cs
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
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
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class EmailConfirmation_UnitTests : TestServerFixture
    {
        //
        public IConfiguration Configuration { get; set;  }
        Mock<IEmailSender> _emailSender = null;
        //
        public EmailConfirmation_UnitTests()
        {
            //string _appSettings = "appsettings.json";
            //if (_appSettings != "")
            //    if (!File.Exists(_appSettings))
            //        throw new FileNotFoundException($"Settings file: {_appSettings} not found.");
            //Configuration = new ConfigurationBuilder()
            //    .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
            //    .Build();
            //
        }
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            //
            // Fixture_ControllerTestSetup();
            //
            _emailSender = new Mock<IEmailSender>();
        }
        //
        [Test()]
        public async Task Home_Page_Test()
        {
            // https://stackoverflow.com/questions/30358322/integration-testing-asp-net-web-api-2-using-httpserver-httpclient-no-httpcontex
            // given / when
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // then
            Assert.AreEqual("Net-Incident Web API Services", responseString);
        }
        //
        [Test()]
        public async Task EmailConfirmation_Page_Test()
        {
            // Act
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual("Net-Incident Web API Services", responseString);
            //var formData = new Dictionary<string, string>
            //{
            //    {"Email", "author@any.net"},
            //};
            //HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "Account/ResendEmailConfirmation")
            //{
            //    Content = new FormUrlEncodedContent(formData)
            //};
            //var response = await client.SendAsync(postRequest);
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            //// Additional asserts could go here 
            //var testPage = await client.GetAsync("/Account/ResendEmailConfirmation");
            //Console.WriteLine(testPage);
            //EmailTestPageModel _testConfirmation = new EmailTestPageModel(userManager, _emailSender.Object);
            // when
            // IActionResult _results = await _testConfirmation.OnPostAsync();
            // then
            // Assert.AreEqual(_results, "https://localhost/Account/ConfirmEmail");
        }
        //
        //[Test()]
        //public async Task EmailConfirmation_Controller_Test()
        //{
        //    // given
        //    Controller _testConfirmation = new EmailTestController();
        //    _testConfirmation.ControllerContext.HttpContext = Fixture_HttpContext(NSG_Helpers.User_Name2, "User", "/Register/", controllerHeaders);
        //    _testConfirmation.Url = Fixture_CreateUrlHelper("https://localhost:44378/Account/ConfirmEmail/");
        //    IEmailConfirmation confirmation = new EmailConfirmation(_testConfirmation);
        //    var _user = await userManager.FindByNameAsync(NSG_Helpers.User_Name2);
        //    // when
        //    string callBackUrl = await confirmation.EmailConfirmationAsync(userManager, _emailSender.Object, _user);
        //    // then
        //    Assert.AreEqual("https://localhost/Account/ConfirmEmail", callBackUrl);
        //}
        ////
        //[Test()]
        //public async Task EmailConfirmation_ApiController_Test()
        //{
        //    // given
        //    ControllerBase _testConfirmation = new EmailTestApiController();
        //    _testConfirmation.ControllerContext.HttpContext = Fixture_HttpContext(NSG_Helpers.User_Name2, "User", "/Register/", controllerHeaders);
        //    IEmailConfirmation confirmation = new EmailConfirmation(_testConfirmation);
        //    var _user = await userManager.FindByNameAsync(NSG_Helpers.User_Name2);
        //    // when
        //    string callBackUrl = await confirmation.EmailConfirmationAsync(userManager, _emailSender.Object, _user);
        //    // then
        //    Assert.AreEqual("https://localhost/Account/ConfirmEmail", callBackUrl);
        //}
        //
    }
    //
    [Route("/EmailTestPage")]
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
