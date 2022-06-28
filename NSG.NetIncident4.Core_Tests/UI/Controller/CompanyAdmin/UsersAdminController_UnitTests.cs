using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MediatR;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.Controllers.CompanyAdmin;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.CompanyAdmin
{
    [TestFixture]
    public class UsersAdminController_UnitTests : UnitTestFixture
    {
        //
        private readonly IEmailSender emailSender = null;
        public string userName = "AdminUser";
        private ApplicationUser adminUser = null;
        private ApplicationUserDetailQuery applicationUser1 = null;
        private ApplicationUserDetailQuery applicationUser2 = null;
        private ApplicationUserListQuery applicationListUser1 = null;
        //
        public UsersAdminController_UnitTests()
        {
            adminUser = new ApplicationUser()
            {
                Id = "111-1111-111111-11",
                UserName = "AdminUser",
                Email = "AdminUser@nsg.com",
                EmailConfirmed = true,
                PhoneNumber = "734-662-1688",
                PhoneNumberConfirmed = true,
                CompanyId = 1,
                FirstName = "Test",
                FullName = "Test User",
                LastName = "User",
                UserNicName = "Test",
                CreateDate = new DateTime(2020, 1, 2, 12, 0, 0),
            };
            applicationUser1 = new ApplicationUserDetailQuery()
            {
                Id = "111-1111-111111-11", UserName = "AdminUser", Email = "AdminUser@nsg.com",
                EmailConfirmed = true, PhoneNumber = "734-662-1688", PhoneNumberConfirmed = true,
                CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
                FirstName = "Test", FullName = "Test User", LastName = "User",
                UserNicName = "Test", CreateDate = new DateTime(2020, 1, 2, 12, 0, 0),
                RoleList = new List<string>() { "Admin" },
                ServerList = new List<ApplicationUserServerListQuery>()
                {
                    new ApplicationUserServerListQuery() { CompanyShortName = "NSG", ServerShortName = "Server1" },
                    new ApplicationUserServerListQuery() { CompanyShortName = "NSG", ServerShortName = "Server2" }
                }
            };
            //
            applicationUser2 = new ApplicationUserDetailQuery()
            {
                Id = "222-2222-222222-22", UserName = "GenUser", Email = "GenUser@nsg.com",
                EmailConfirmed = true, PhoneNumber = "734-662-1688", PhoneNumberConfirmed = true,
                CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
                FirstName = "Test", FullName = "Test User", LastName = "User",
                UserNicName = "Test", CreateDate = new DateTime(2020, 1, 2, 12, 0, 0),
                RoleList = new List<string>() { "User" },
                ServerList = new List<ApplicationUserServerListQuery>()
                {
                    new ApplicationUserServerListQuery() { CompanyShortName = "NSG", ServerShortName = "Server2" }
                }
            };
            //
            applicationListUser1 = new ApplicationUserListQuery()
            {
                Id = "111-1111-111111-11", UserName = "AdminUser", Email = "AdminUser@nsg.com",
                FullName = "Test User", CompanyId = 1, CompanyShortName = "NSG",
                CompanyName = "Northern Software Group"
            };
            //
            var _userManager = MockHelpers.MockUserManager<ApplicationUser>();
            userManager = _userManager.Object;
            var _emailSender = new Mock<IEmailSender>();
            emailSender = _emailSender.Object;
        }
        //
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void Index_AuthorizeAttribute_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            // when
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on UsersAdminController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "CompanyAdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            ApplicationUserListQueryHandler.ViewModel results = new ApplicationUserListQueryHandler.ViewModel()
            {
                ApplicationUsersList = new List<ApplicationUserListQuery>() {
                    applicationListUser1
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("ApplicationUser list was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<ApplicationUserListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            ApplicationUserDetailQuery mediatorReturn = applicationUser1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("ApplicationUser details was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.Details(mediatorReturn.UserName);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationUserDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.Id, model.Id);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            ApplicationUserUpdateQuery mediatorReturn = new ApplicationUserUpdateQuery()
            {
                Id = "222-2222-222222-22", UserName = "GenUser", Email = "GenUser@nsg.com",
                PhoneNumber = "734-662-1688", CompanyId = 1, FirstName = "Test",
                FullName = "Test User", LastName = "User", UserNicName = "Test",
                //
                RolesList = new List<SelectListItem>() { new SelectListItem() { Value = "adm", Text = "Admin"} },
                ServersList = new List<SelectListItem>() { new SelectListItem() { Value = "1", Text = "Server1" } },
                CompaniesList = new List<SelectListItem>() { new SelectListItem() { Value = "1", Text = "NSG" } }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserUpdateQueryHandler.EditQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("ApplicationUser edit was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorReturn.UserName);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationUserUpdateQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            ApplicationUserUpdateCommand mediatorParam = new ApplicationUserUpdateCommand()
            {
            };
            // return values
            ApplicationUser mediatorReturn = adminUser;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("ApplicationUser edit was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(UsersAdminController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            ApplicationUserDetailQuery mediatorReturn = applicationUser2;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("ApplicationUser delete was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.Delete(mediatorReturn.UserName);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationUserDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            ApplicationUserDetailQuery mediatorParam = applicationUser2;
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("ApplicationUser edit was not sent.");
            UsersAdminController sut = new UsersAdminController(userManager, emailSender, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("AdminUser", "admin", "/UsersAdmin/", controllerHeaders);
            // when
            var actual = await sut.DeleteConfirmed(mediatorParam.UserName);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(UsersAdminController.Alerts.Count, 0);
        }
        //
    }
}
//