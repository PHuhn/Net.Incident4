using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
//
using Moq;
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.UI.ApiModels;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Routing;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    [TestFixture]
    public class Authenticate_UnitTests : UnitTestFixture
    {
        //
        AuthenticateController sut = null;
        IEmailSender emailSender = null;
        Mock<IUrlHelper> _urlHelper = null;
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            //
            Fixture_UnitTestSetup();
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            Fixture_ControllerTestSetup();
            var _emailSender = new Mock<IEmailSender>();
            emailSender = _emailSender.Object;
            sut = new AuthenticateController(userManager, configuration, emailSender);
            var _httpContext = new Mock<HttpContext>();
            var _httpSession = new Mock<ISession>();
            _httpContext.SetupGet(m => m.Session).Returns(_httpSession.Object);
            _httpContext.SetupGet(m => m.Request.Scheme).Returns("http"); // or https
            _urlHelper = new Mock<IUrlHelper>();
            //_urlHelper.Setup(h => h.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
            //    .Returns("http://localhost/Account/ConfirmEmail");
            // UrlHelperExtensions.Action(IUrlHelper helper, String action, String controller, Object values, String protocol)
            // ControllerContext(ActionContext _httpContext)
            // ActionContext(HttpContext httpContext, RouteData routeData, ActionDescriptor actionDescriptor)
            // public RouteData(RouteValueDictionary values)
            // System.ArgumentException : The action descriptor must be of type 'Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor'. (Parameter 'context')
            // var actionContext = new ActionContext(context.Object, new RouteData(new RouteValueDictionary()), new ActionDescriptor());
            //sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = _httpContext.Object;
            sut.Url = _urlHelper.Object;
            Console.WriteLine("MySetup finished");
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Good_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = NSG_Helpers.User_Name, Password = NSG_Helpers.Password };
            IActionResult _results = await sut.Login(_model);
            Assert.IsInstanceOf<OkObjectResult>(_results);
        }
        //
        [Test]
        public async Task AuthenticateController_Login_NotVerified_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = NSG_Helpers.User_Name2, Password = NSG_Helpers.Password2 };
            IActionResult _results = await sut.Login(_model);
            Assert.IsInstanceOf<UnauthorizedResult>(_results);
            UnauthorizedResult _unResults = _results as UnauthorizedResult;
            Assert.AreEqual(_unResults.StatusCode, 401);
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Bad_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = "NonUser", Password = "NonUserPassword" };
            IActionResult _results = await sut.Login(_model);
            Assert.IsInstanceOf<UnauthorizedResult>(_results);
        }
        //
        [Test]
        public async Task AuthenticateController_Register_Good_Test()
        {
            // given
            RegisterModel _model = new RegisterModel()
            {
                Username = "John", FirstName = "John", LastName = "Smith",
                FullName = "John Smith", UserNicName = "John", Email = "JSmith@nsg.com",
                CompanyId = 1, Password = "Password0!"
            };
            // when
            IActionResult _results = await sut.Register(_model);
            // then
            Assert.IsInstanceOf<OkObjectResult>(_results);
        }
        //
        [Test]
        public async Task AuthenticateController_Register_Bad_Test()
        {
            // given
            RegisterModel _model = new RegisterModel()
            {
                Username = "John", FirstName = "John", LastName = "Smith", FullName = "John Smith",
                UserNicName = "John", Email = "JSmith@nsg.com", CompanyId = 1, Password = "Password"
            };
            // when
            IActionResult _results = await sut.Register(_model);
            // then
            Assert.IsInstanceOf<ObjectResult>(_results);
        }
        //
        private Mock<IUrlHelper> CreateMockUrlHelper(ActionContext context = null)
        {
            context ??= GetActionContextForController("/api");

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupGet(h => h.ActionContext)
                .Returns(context);
            return urlHelper;
        }

        private static ActionContext GetActionContextForPage(string page)
        {
            return new()
            {
                ActionDescriptor = new()
                {
                    RouteValues = new Dictionary<string, string>
            {
                { "page", page },
            }
                },
                RouteData = new()
                {
                    Values =
            {
                [ "page" ] = page
            }
                }
            };
        }
        //
        private static ActionContext GetActionContextForController(string controller)
        {
            return new()
            {
                ActionDescriptor = new()
                {
                    RouteValues = new Dictionary<string, string>
                    {
                        { "controller", controller },
                    }
                },
                RouteData = new()
                {
                    Values =
                    {
                        [ "controller" ] = controller
                    }
                }
            };
        }
    }
}
