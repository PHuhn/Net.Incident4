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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.Controllers.Admin;
using NSG.NetIncident4.Core.Application.Commands.ApplicationRoles;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.Admin
{
    [TestFixture]
    public class RolesAdminController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        public RolesAdminController_UnitTests()
        {
        }
        //
        [SetUp]
        public void Setup()
        {
            // Fixture_ControllerTestSetup();
        }
        //
        [Test]
        public void Index_AuthorizeAttribute_Test()
        {
            // given
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            // when
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on RolesAdminController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "AdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            ApplicationRoleListQueryHandler.ViewModel results = new ApplicationRoleListQueryHandler.ViewModel()
            {
                ApplicationRolesList = new List<ApplicationRoleListQuery>() {
                    new ApplicationRoleListQuery { Id = "adm", Name = "Admin" },
                    new ApplicationRoleListQuery { Id = "pub", Name = "Public" },
                    new ApplicationRoleListQuery { Id = "usr", Name = "User" }
                }
            };
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("Role list was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<ApplicationRoleListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            ApplicationRoleUserDetailQuery mediatorReturn = new ApplicationRoleUserDetailQuery()
                { Id = "adm", Name = "Admin", UserList = new List<UserListQuery>(){ new UserListQuery() { UserName = "TestUser" } } };
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleUserDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role details was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Details("adm");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationRoleUserDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.Id, model.Id);
        }
        //
        [Test]
        public async Task Create_Test()
        {
            // given
            ApplicationRoleCreateCommand mediatorParam = new ApplicationRoleCreateCommand()
                { Id = "xxx", Name = "Xxxxx" };
            // return values
            ApplicationRole mediatorReturn = new ApplicationRole()
                { Id = "xxx", Name = "Xxxxx" };
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role create was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Create(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(RolesAdminController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            ApplicationRoleDetailQuery mediatorReturn = new ApplicationRoleDetailQuery()
            { Id = "adm", Name = "Admin" };
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role edit was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Edit("adm");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationRoleUpdateCommand;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.Id, model.Id);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            ApplicationRoleUpdateCommand mediatorParam = new ApplicationRoleUpdateCommand()
            { Id = "adm", Name = "Admin" };
            // return values
            int mediatorReturn = 1;
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role edit was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(RolesAdminController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            ApplicationRoleUserDetailQuery mediatorReturn = new ApplicationRoleUserDetailQuery()
                { Id = "adm", Name = "Admin" };
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleUserDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role delete was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.Delete("adm");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as ApplicationRoleUserDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.Id, model.Id);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            ApplicationRoleDeleteCommand mediatorParam = new ApplicationRoleDeleteCommand()
                { Id = "adm" };
            // return values
            int mediatorReturn = 1;
            Mock<ILogger<RolesAdminController>> mockLogger = new Mock<ILogger<RolesAdminController>>();
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ApplicationRoleDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Role edit was not sent.");
            RolesAdminController sut = new RolesAdminController(mockLogger.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/RolesAdmin/", controllerHeaders);
            // when
            var actual = await sut.DeleteConfirmed("adm");
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(RolesAdminController.Alerts.Count, 0);
        }
        //
    }
}