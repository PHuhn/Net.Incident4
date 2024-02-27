using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
//
using MockQueryable.Moq;
using Moq;
//
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.UI.ApiModels;
using NSG.Integration.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Domain.Entities;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using NSG.NetIncident4.Core.UI.Controllers;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Routing;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    [TestFixture]
    public class Authenticate_UnitTests : UnitTestFixture
    {
        //
        AuthenticateController sut;
        // IEmailSender emailSender = null;
        // Mock<IUrlHelper> _urlHelper = null;
        Mock<UserManager<ApplicationUser>> _userManager;
        Mock<IEmailSender> _emailSender;
        AuthSettings _authSettings;
        //
        public Authenticate_UnitTests()
        {
        }
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            // configuration defined in UnitTestFixture
            _authSettings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
            _emailSender = new Mock<IEmailSender>();
            _userManager = MockHelpers.GetMockUserManager<ApplicationUser>();
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            _userManager.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
            Console.WriteLine("Setup finished");
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Good_Test()
        {
            // given
            ApplicationUser user = NSG_Helpers.user1;
            _ = _userManager.Setup(u => u.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            _ = _userManager.Setup(u => u.CheckPasswordAsync(user, NSG_Helpers.Password)).Returns(Task.FromResult(true));
            _ = _userManager.Setup(u => u.GetRolesAsync(user)).Returns(Task.FromResult(new List<string>() {NSG_Helpers.admRole.Id} as IList<string>));
            sut = new AuthenticateController(_userManager.Object, configuration, _emailSender.Object);
            LoginModel _model = new LoginModel() { Username = user.UserName, Password = NSG_Helpers.Password };
            // when
            IActionResult _results = await sut.Login(_model);
            // then
            // Assert.IsInstanceOf<OkObjectResult>(_results);
            Assert.That(_results, Is.InstanceOf<OkObjectResult>());
        }
        //
        [Test]
        public async Task AuthenticateController_Login_NotVerified_Test()
        {
            // given
            ApplicationUser user = NSG_Helpers.user2;
            _ = _userManager.Setup(u => u.FindByNameAsync(user.UserName)).Returns(Task.FromResult(user));
            _ = _userManager.Setup(u => u.CheckPasswordAsync(user, NSG_Helpers.Password2)).Returns(Task.FromResult(true));
            LoginModel _model = new LoginModel() { Username = user.UserName, Password = NSG_Helpers.Password2 };
            sut = new AuthenticateController(_userManager.Object, configuration, _emailSender.Object);
            // then
            IActionResult _results = await sut.Login(_model);
            Assert.That(_results, Is.InstanceOf<UnauthorizedObjectResult>());
            UnauthorizedObjectResult _unResults = _results as UnauthorizedObjectResult;
            Assert.That(_unResults.StatusCode, Is.EqualTo(401));
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Bad_Test()
        {
            // given
            string _username = "NonUser";
            _ = _userManager.Setup(u => u.FindByNameAsync(_username)).Returns(Task.FromResult(null as ApplicationUser));
            sut = new AuthenticateController(_userManager.Object, configuration, _emailSender.Object);
            LoginModel _model = new LoginModel() { Username = _username, Password = "NonUserPassword" };
            // when
            IActionResult _results = await sut.Login(_model);
            Assert.That(_results, Is.InstanceOf<NotFoundResult>());
        }
        //
        [Test]
        public async Task AuthenticateController_Register_Good_Test()
        {
            // given
            string _emailAddress = "JSmith@nsg.com";
            RegisterModel _model = new RegisterModel()
            {
                Username = "John", FirstName = "John", LastName = "Smith",
                FullName = "John Smith", UserNicName = "John", Email = _emailAddress,
                CompanyId = 1, Password = "Password0!"
            };
            _ = _userManager
                .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _ = _userManager
                .Setup(r => r.GetUserIdAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult("aaaa-0000-1111-2222-3333"));
            _ = _userManager
                .Setup(r => r.GetEmailAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(_emailAddress));
            _ = _userManager
                .Setup(r => r.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult("aaaa00-0000-111111-2222-333333"));
            //
            var _request = new Mock<HttpRequest>();
            _request.Setup(x => x.Scheme).Returns("localhost:8080");
            _request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            _request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Authenticate"));
            var _httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == _request.Object
            );
            var _urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl");
            var _controllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };
            sut = new AuthenticateController(_userManager.Object, configuration, _emailSender.Object)
            {
                ControllerContext = _controllerContext,
                Url = _urlHelperMock.Object
            };
            // when
            IActionResult _results = await sut.Register(_model);
            // then
            Assert.That(_results, Is.InstanceOf<OkObjectResult>());
        }
        //
        [Test]
        public async Task AuthenticateController_Register_Bad_Test()
        {
            // given
            var _descrError = "Failed to create";
            var _identityError = new IdentityError() { Code = "Err", Description = _descrError };
            var _createResults = IdentityResult.Failed(_identityError);
            _ = _userManager
                .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_createResults));
            sut = new AuthenticateController(_userManager.Object, configuration, _emailSender.Object);
            RegisterModel _model = new RegisterModel()
            {
                Username = "John", FirstName = "John", LastName = "Smith", FullName = "John Smith",
                UserNicName = "John", Email = "JSmith@nsg.com", CompanyId = 1, Password = "Password"
            };
            // when
            IActionResult _results = await sut.Register(_model);
            // then
            Assert.That(_results, Is.InstanceOf<ObjectResult>());
            ObjectResult _objectResult = _results as ObjectResult;
            Assert.That(_objectResult.StatusCode, Is.EqualTo(500));
            Assert.That((_objectResult.Value as Response).Message, Is.EqualTo("User creation failed! " + _descrError));
        }
        //
        //private Mock<IUrlHelper> CreateMockUrlHelper(ActionContext context = null)
        //{
        //    context ??= GetActionContextForController("/api");
        //    var urlHelper = new Mock<IUrlHelper>();
        //    urlHelper.SetupGet(h => h.ActionContext).Returns(context);
        //    return urlHelper;
        //}
        //
        //private static ActionContext GetActionContextForPage(string page)
        //{
        //    return new()
        //    {
        //        ActionDescriptor = new()
        //        {
        //            RouteValues = new Dictionary<string, string>
        //            {
        //                { "page", page },
        //            }
        //        },
        //        RouteData = new()
        //        {
        //            Values =
        //            {
        //                [ "page" ] = page
        //            }
        //        }
        //    };
        //}
        //
        //private static ActionContext GetActionContextForController(string controller)
        //{
        //    return new()
        //    {
        //        ActionDescriptor = new()
        //        {
        //            RouteValues = new Dictionary<string, string>
        //            {
        //                { "controller", controller },
        //            }
        //        },
        //        RouteData = new()
        //        {
        //            Values =
        //            {
        //                [ "controller" ] = controller
        //            }
        //        }
        //    };
        //}
    }
}
// _configuration.Setup(c => c.GetSection("AuthSettings")).Returns<AuthSettings>(_authSettings);
//
//Fixture_MockSetup();
//DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
//_seeder.Seed().Wait();
// Fixture_ControllerTestSetup();
//var _httpContext = new Mock<HttpContext>();
//var _httpSession = new Mock<ISession>();
//_httpContext.SetupGet(m => m.Session).Returns(_httpSession.Object);
//_httpContext.SetupGet(m => m.Request.Scheme).Returns("http"); // or https
//_urlHelper = new Mock<IUrlHelper>();
//_urlHelper.Setup(h => h.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
//    .Returns("http://localhost/Account/ConfirmEmail");
// UrlHelperExtensions.Action(IUrlHelper helper, String action, String controller, Object values, String protocol)
// ControllerContext(ActionContext _httpContext)
// ActionContext(HttpContext httpContext, RouteData routeData, ActionDescriptor actionDescriptor)
// public RouteData(RouteValueDictionary values)
// System.ArgumentException : The action descriptor must be of type 'Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor'. (Parameter 'context')
// var actionContext = new ActionContext(context.Object, new RouteData(new RouteValueDictionary()), new ActionDescriptor());
// sut.ControllerContext = new ControllerContext();
//sut.ControllerContext.HttpContext = _httpContext.Object;
//sut.Url = _urlHelper.Object;
