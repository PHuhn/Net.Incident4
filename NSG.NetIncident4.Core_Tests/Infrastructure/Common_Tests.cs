// ===========================================================================
// File: Common_Tests.cs
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
//
using Moq;
//
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class Common_Tests
    {
        //
        public IConfiguration Configuration { get; set; }
        public IOptions<ApplicationSettings> applicationSettings = null;
        public ApplicationImplementation application = null;
        //
        public IHttpContextAccessor httpContextAccesor = null;
        public Mock<HttpContext> httpContext = null;
        public string userName = "TestUser";
        public IPrincipal currentUser = null;
        //
        [SetUp]
        public void Setup()
        {
            // given
            string _appSettings = "appsettings.json";
            if (_appSettings != "")
                if (!File.Exists(_appSettings))
                    throw new FileNotFoundException($"Settings file: {_appSettings} not found.");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
                .Build();
            applicationSettings =
                Options.Create<ApplicationSettings>(Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>());
            // currentUser = new Mock<IHttpContextAccessor>().Object;
            currentUser = MockHelpers.TestPrincipal(userName, Constants.adminRole);
            httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(m => m.User).Returns((ClaimsPrincipal)currentUser);
            httpContextAccesor = new HttpContextAccessor();
            httpContextAccesor.HttpContext = httpContext.Object;
            application = new ApplicationImplementation(httpContextAccesor, applicationSettings);
        }
        //
        [Test]
        public void Application_Test_Test()
        {
            HttpContext t = httpContext.Object;
            Console.WriteLine(t);
            System.Diagnostics.Debug.WriteLine(t);
            Console.WriteLine(t.User);
            System.Diagnostics.Debug.WriteLine(t.User);
            Assert.That(t.User.Identity.Name, Is.EqualTo(userName));
        }
        //
        [Test]
        public void Application_GetApplicationName_Test()
        {
            // when
            string actual = application.GetApplicationName();
            // then
            Assert.That(actual, Is.EqualTo("Net-Incident Identity"));
        }
        //
        [Test]
        public void Application_GetApplicationPhoneNumber_Test()
        {
            // when
            string actual = application.GetApplicationPhoneNumber();
            // then
            Assert.That(actual, Is.EqualTo("(734) 555-1212"));
        }
        //
        [Test]
        public void Application_GetUserAccount_Test()
        {
            // when
            string actual = application.GetUserAccount();
            // then
            Assert.That(actual, Is.EqualTo(userName));
        }
        //
        [Test]
        public void Application_IsAuthenticated_Test()
        {
            // when
            bool actual = application.IsAuthenticated();
            // then
            Assert.That(actual, Is.EqualTo(true));
        }
        //
        [Test]
        public void Application_IsAdminRole_Test()
        {
            // when
            bool actual = application.IsAdminRole();
            // then
            Assert.That(actual, Is.EqualTo(true));
        }
        //
        [Test]
        public void Application_IsCompanyAdminRole_Test()
        {
            // when
            bool actual = application.IsCompanyAdminRole();
            // then
            Assert.That(actual, Is.EqualTo(true));
        }
        //
        [Test]
        public void Application_IsEditableRole_Test()
        {
            // when
            bool actual = application.IsEditableRole();
            // then
            Assert.That(actual, Is.EqualTo(true));
        }
        //
    }
}
// ===========================================================================
