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
using MediatR;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.Controllers.CompanyAdmin;
using NSG.NetIncident4.Core.Application.Commands.Companies;
using NSG.NetIncident4.Core.Application.Commands.CompanyServers;
using NSG.NetIncident4.Core.Application.Commands.Servers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.CompanyAdmin
{
    [TestFixture]
    public class CompanyServerController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        public CompanyServerController_UnitTests()
        {
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
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on CompanyServerController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "CompanyAdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            CompanyServerListQueryHandler.ViewModel results = new CompanyServerListQueryHandler.ViewModel()
            {
                CompaniesList = new List<CompanyServerListQuery>() {
                    new CompanyServerListQuery()
                    { CompanyId = 1, CompanyName = "Northern Software Group", CompanyShortName = "NSG",
                        ServerList = new List<ServerListQuery>()
                        {
                            new ServerListQuery() { ServerId = 1, ServerShortName = "Server1" },
                            new ServerListQuery() { ServerId = 2, ServerShortName = "Server2" }
                        }
                    }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyServerListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("CompanyServer list was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<CompanyServerListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            // Daylight saving time 2022 in Michigan began at 2:00 AM on
            // Sunday, March 13 and ends at 2:00 AM on Sunday, November 6
            CompanyServerDetailQuery mediatorReturn = new CompanyServerDetailQuery()
            {
                CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern",
                Address = "1 Main St.", City = "Ann Arbor", State = "MI", PostalCode = "48104",
                Country = "USA", PhoneNumber = "734-662-5252", Notes = "",
                ServerList = new List<ServerDetailQuery>()
                {
                    new ServerDetailQuery()
                    {
                        ServerId = 1, CompanyId = 1, ServerShortName = "Server1", ServerName = "Server 1",
                        ServerDescription = "Server Description", WebSite = "", ServerLocation = "Some place",
                        FromName = "Phil", FromNicName = "Phil", FromEmailAddress = "Phil@nsg.com",
                        TimeZone = "EST", DST = true, TimeZone_DST = "-4:00", DST_Start = new DateTime(2022, 3, 16, 2, 0, 0), DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
                    },
                    new ServerDetailQuery()
                    {
                        ServerId = 2, CompanyId = 1, ServerShortName = "Server2", ServerName = "Server 2",
                        ServerDescription = "Server Description", WebSite = "", ServerLocation = "Some place",
                        FromName = "Phil", FromNicName = "Phil", FromEmailAddress = "Phil@nsg.com",
                        TimeZone = "EST", DST = true, TimeZone_DST = "-4:00", DST_Start = new DateTime(2022, 3, 16, 2, 0, 0), DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
                    }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyServerDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyServer details was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.Details(3);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyServerDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task CompanyCreate_Test()
        {
            // given
            CompanyCreateCommand mediatorParam = new CompanyCreateCommand()
            {
                CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor", State = "MI",
                PostalCode = "48104", Country = "USA", PhoneNumber = "734-662-5252", Notes = "",
            };
            // return values
            Company mediatorReturn = new Company()
            {
                CompanyId = 2, CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor", State = "MI", PostalCode = "48104",
                Country = "USA", PhoneNumber = "734-662-5252", Notes = "",
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Company create was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.CompanyCreate(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Edit");
            Assert.AreEqual(CompanyServerController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            CompanyServerDetailQuery mediatorReturn = new CompanyServerDetailQuery()
            {
                CompanyId = 2, CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor",
                State = "MI", PostalCode = "48104",
                Country = "USA", PhoneNumber = "734-662-5252", Notes = "",
                ServerList = new List<ServerDetailQuery>()
                {
                    new ServerDetailQuery()
                    {
                        ServerId = 3, CompanyId = 1, ServerShortName = "XServer1",
                        ServerName = "XServer 1", ServerDescription = "XServer Description", WebSite = "",
                        ServerLocation = "Some place", FromName = "Phil", FromNicName = "Phil",
                        FromEmailAddress = "Phil@nsg.com", TimeZone = "EST", DST = true,
                        TimeZone_DST = "-4:00",DST_Start = new DateTime(2022, 3, 16, 2, 0, 0),
                        DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
                    }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyServerDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyServer edit was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.Edit(2);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyServerDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task CompanyEdit_Save_Test()
        {
            // given
            CompanyUpdateCommand mediatorParam = new CompanyUpdateCommand()
            {
                CompanyId = 2, CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor", State = "MI",
                PostalCode = "48104", Country = "USA", PhoneNumber = "734-662-5252",
                Notes = "",
            };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyServer edit was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.CompanyEdit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Edit");
            Assert.AreEqual(CompanyServerController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task ServerEdit_Save_Test()
        {
            // given
            ServerUpdateCommand mediatorParam = new ServerUpdateCommand()
            {
                ServerId = 3, CompanyId = 1, ServerShortName = "XServer1",
                ServerName = "XServer 1", ServerDescription = "XServer Description", WebSite = "",
                ServerLocation = "Some place", FromName = "Phil", FromNicName = "Phil",
                FromEmailAddress = "Phil@nsg.com", TimeZone = "EST", DST = true,
                TimeZone_DST = "-4:00", DST_Start = new DateTime(2022, 3, 16, 2, 0, 0),
                DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
            };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ServerUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Server edit was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.ServerEdit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Edit");
            Assert.AreEqual(CompanyServerController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            CompanyServerDetailQuery mediatorReturn = new CompanyServerDetailQuery()
            {
                CompanyId = 2, CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor", State = "MI",
                PostalCode = "48104", Country = "USA", PhoneNumber = "734-662-5252",
                Notes = "",
                ServerList = new List<ServerDetailQuery>()
                {
                    new ServerDetailQuery()
                    {
                        ServerId = 3, CompanyId = 1, ServerShortName = "XServer1",
                        ServerName = "XServer 1", ServerDescription = "XServer Description", WebSite = "",
                        ServerLocation = "Some place", FromName = "Phil", FromNicName = "Phil",
                        FromEmailAddress = "Phil@nsg.com", TimeZone = "EST", DST = true,
                        TimeZone_DST = "-4:00",DST_Start = new DateTime(2022, 3, 16, 2, 0, 0),
                        DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
                    }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyServerDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyServer delete was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.Delete(4);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyServerDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task CompanyDeleteConfirmed_Test()
        {
            // given
            CompanyServerDetailQuery mediatorParam = new CompanyServerDetailQuery()
            {
                CompanyId = 2, CompanyShortName = "XXX", CompanyName = "Xxxxx Yyyyy",
                Address = "2 Main St.", City = "Ann Arbor", State = "MI",
                PostalCode = "48104", Country = "USA", PhoneNumber = "734-662-5252",
                Notes = "",
                ServerList = new List<ServerDetailQuery>()
                {
                    new ServerDetailQuery()
                    {
                        ServerId = 3, CompanyId = 1, ServerShortName = "XServer1",
                        ServerName = "XServer 1", ServerDescription = "XServer Description", WebSite = "",
                        ServerLocation = "Some place", FromName = "Phil", FromNicName = "Phil",
                        FromEmailAddress = "Phil@nsg.com", TimeZone = "EST", DST = true,
                        TimeZone_DST = "-4:00",DST_Start = new DateTime(2022, 3, 16, 2, 0, 0),
                        DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
                    }
                }
            };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyServer edit was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.CompanyDelete(2, mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(CompanyServerController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task ServerDeleteConfirmed_Test()
        {
            // given
            ServerDetailQuery mediatorParam = new ServerDetailQuery()
            {
                ServerId = 3, CompanyId = 1, ServerShortName = "XServer1",
                ServerName = "XServer 1", ServerDescription = "XServer Description", WebSite = "",
                ServerLocation = "Some place", FromName = "Phil", FromNicName = "Phil",
                FromEmailAddress = "Phil@nsg.com", TimeZone = "EST", DST = true,
                TimeZone_DST = "-4:00",DST_Start = new DateTime(2022, 3, 16, 2, 0, 0),
                DST_End = new DateTime(2022, 11, 6, 2, 0, 0)
            };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<ServerDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Server delete was not sent.");
            CompanyServerController sut = new CompanyServerController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyServer/", controllerHeaders);
            // when
            var actual = await sut.ServerDelete(3, mediatorParam.CompanyId);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Delete");
            Assert.AreEqual(CompanyServerController.Alerts.Count, 0);
        }
        //
    }
}
//