// ===========================================================================
// File: EmailConfirmation_UnitTests.cs
using NUnit.Framework;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
//
using MediatR;
using MockQueryable.Moq;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.Controllers;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    /*
    ** !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    **
    ** This test requires some ability to send emails.
    ** I use fake-smtp-server, which is a nodejs application.
    ** The project does have a 'fake-smtp.bat', that invokes
    ** the globally installed node package.
    **
    ** !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    */
    [TestFixture]
    public class EmailConfirmation_UnitTests
    {
        //
        public IConfiguration Configuration { get; set;  }
        private Mock<IEmailSender> _emailSender;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<HttpRequest> _request;
        private HttpContext _httpContext;
        private Mock<IUrlHelper> _urlHelperMock;
        private HttpClient _httpClient;
        private string _callBackUrl = "http://localhost:8080";
        private ApplicationUser _user = NSG_Helpers.user2;
        //
        public EmailConfirmation_UnitTests()
        {
            Console.WriteLine("EmailConfirmation_UnitTests c-tor ...");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
            //
        }
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            _emailSender = new Mock<IEmailSender>();
            // user manager
            _userManager = MockHelpers.GetMockUserManager<ApplicationUser>();
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            _userManager.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
            _ = _userManager
                .Setup(r => r.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult("aaaa00-0000-111111-2222-333333"));
            // request
            _request = new Mock<HttpRequest>();
            _request.Setup(x => x.Scheme).Returns("localhost:8080");
            _request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            _request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Authenticate"));
            // http context
            _httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == _request.Object
            );
            // url helper
            // https://stackoverflow.com/questions/42212066/net-core-url-action-mock-how-to
            _urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            // action is not an extension method (some 'action's are an extension method)
            _ = _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(_callBackUrl);
        }
        //
        [Test()]
        public async Task Home_Page_Test()
        {
            Console.WriteLine("Home_Page_Test ...");
            // given / when
            var response = await _httpClient.GetAsync("/");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("Net-Incident"));
            Assert.IsTrue(responseString.Contains("Administration and Web API Services"));
        }
        //
        [Test()]
        public async Task About_Page_Test()
        {
            Console.WriteLine("About_Page_Test ...");
            // given / when
            var response = await _httpClient.GetAsync("/Home/About");
            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("Network Incident Backend"));
        }
        //
        [Test]
        public async Task EmailConfirmationAsync_Controller_Test()
        {
            // given
            var _controllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };
            ControllerBase _controller = new BaseController((new Mock<IMediator>()).Object)
            {
                ControllerContext = _controllerContext,
                Url = _urlHelperMock.Object,
            };
            IEmailConfirmation sut = new EmailConfirmation(_controller, _userManager.Object, _emailSender.Object);
            // when
            string _emailBody = await sut.EmailConfirmationAsync(_user);
            // then
            Assert.AreEqual(_emailBody, $"Please confirm your account: {_user.UserName} by <a href='{_callBackUrl}'>clicking here</a>.");
        }
        //
        [Test()]
        public async Task EmailConfirmationAsync_Page_Test()
        {
            // given
            Console.WriteLine("EmailConfirmation_Page_Test ...");
            var response = await _httpClient.GetAsync("/Account/ConfirmEmail?userId=TestUser&code=aaaa");
            // response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual("Unable to load user with ID 'TestUser'.", responseString);
            var _actionContext = new ActionContext(_httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor());
            var _sut = new EmailTestPageModel(_userManager.Object, _emailSender.Object, _user)
            {
                PageContext = new PageContext(_actionContext),
                Url = _urlHelperMock.Object,
            };
            var _emailBody = await _sut.OnPostAsync();
            // then
            Assert.AreEqual(_emailBody, $"Please confirm your account: author by <a href='http://localhost:8080'>clicking here</a>.");
        }
        //
    }
    //
    [Microsoft.AspNetCore.Components.Route("/EmailTestPage")]
    public class EmailTestPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationUser _user;
        public EmailTestPageModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user) : base()
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _user = user;
        }
        //
        public async Task<string> OnPostAsync()
        {
            IEmailConfirmation confirmation = new EmailConfirmation(this, _userManager, _emailSender);
            string body = await confirmation.EmailConfirmationAsync(_user);
            return body;
        }
    }
    //
}
// ===========================================================================
