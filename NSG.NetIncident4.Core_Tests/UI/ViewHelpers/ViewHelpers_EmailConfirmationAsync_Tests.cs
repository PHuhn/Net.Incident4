using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
//
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
//
using Moq;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.UI.ViewModels;
using NSG.NetIncident4.Core.UI.ViewHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using NSG.NetIncident4.Core.UI.Api;
using System.Configuration;
using NSG.NetIncident4.Core.Domain.Entities;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Identity.UI.Services;
using MediatR;
using NSG.NetIncident4.Core.UI.Controllers;
//
namespace NSG.NetIncident4.Core_Tests.UI.ViewHelpers
{
    [TestFixture]
    public class ViewHelpers_EmailConfirmationAsync_Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        //
        //[Test]
        //public void EmailConfirmationAsync_Controller_Test()
        //{
        //    // given
        //    ApplicationUser _user = NSG_Helpers.user2;
        //    Mock<UserManager<ApplicationUser>> _userManager;
        //    _userManager = MockHelpers.GetMockUserManager<ApplicationUser>();
        //    var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
        //    _userManager.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
        //    _ = _userManager
        //        .Setup(r => r.GetUserIdAsync(It.IsAny<ApplicationUser>()))
        //        .Returns(Task.FromResult(_user.Id));
        //    _ = _userManager
        //        .Setup(r => r.GetEmailAsync(It.IsAny<ApplicationUser>()))
        //        .Returns(Task.FromResult(_user.Email));
        //    _ = _userManager
        //        .Setup(r => r.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
        //        .Returns(Task.FromResult("aaaa00-0000-111111-2222-333333"));
        //    //
        //    Mock<IEmailSender> _emailSender = new Mock<IEmailSender>();
        //    var _request = new Mock<HttpRequest>();
        //    _request.Setup(x => x.Scheme).Returns("localhost:8080");
        //    _request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
        //    _request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Authenticate"));
        //    var _httpContext = Mock.Of<HttpContext>(_ =>
        //        _.Request == _request.Object
        //    );
        //    var _urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
        //    _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
        //        .Returns("callbackUrl");
        //    var _controllerContext = new ControllerContext()
        //    {
        //        HttpContext = _httpContext,
        //    };
        //    ControllerBase _controller = new BaseController((new Mock<IMediator>()).Object);
        //    _controller.ControllerContext = _controllerContext;
        //    _controller.Url = _urlHelperMock.Object;
        //    // when
        //    // IActionResult _results =
        //    await NSG.NetIncident4.Core.UI.ViewHelpers.ViewHelpers.EmailConfirmationAsync(_controller, _userManager.Object, _emailSender.Object, _user);
        //    // static async Task<string> EmailConfirmationAsync(object context, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationUser user)
        //    // then
        //    Assert.IsInstanceOf<OkObjectResult>(_results);
        //}
        //
    }
}