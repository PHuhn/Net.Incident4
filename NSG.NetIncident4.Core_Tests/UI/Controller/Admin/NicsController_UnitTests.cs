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
using NSG.NetIncident4.Core.UI.Controllers.Admin;
using NSG.NetIncident4.Core.Application.Commands.NICs;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.Admin
{
    [TestFixture]
    public class NicsController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        public NicsController_UnitTests()
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
            NicsController sut = new NicsController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on NicsController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "AdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            NICListQueryHandler.ViewModel results = new NICListQueryHandler.ViewModel()
            {
                NICsList = new List<NICListQuery>() {
                    new NICListQuery { NIC_Id = "NIC1", NICDescription = "NIC 1", NICAbuseEmailAddress = "abuse@NIC1.net", NICRestService = "", NICWebSite = "https://NIC1.net" },
                    new NICListQuery { NIC_Id = "NIC2", NICDescription = "NIC 2", NICAbuseEmailAddress = "abuse@NIC2.net", NICRestService = "", NICWebSite = "https://NIC2.net" },
                    new NICListQuery { NIC_Id = "NIC3", NICDescription = "NIC 3", NICAbuseEmailAddress = "abuse@NIC3.net", NICRestService = "", NICWebSite = "https://NIC3.net" }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("NIC list was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<NICListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            NICDetailQuery mediatorReturn = new NICDetailQuery()
                { NIC_Id = "NIC3", NICDescription = "NIC 3", NICAbuseEmailAddress = "abuse@NIC3.net", NICRestService = "", NICWebSite = "https://NIC3.net" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC details was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Details("NIC3");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NICDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NIC_Id, model.NIC_Id);
        }
        //
        [Test]
        public async Task Create_Test()
        {
            // given
            NICCreateCommand mediatorParam = new NICCreateCommand()
                { NIC_Id = "NIC4", NICDescription = "NIC 4", NICAbuseEmailAddress = "abuse@NIC4.net", NICRestService = "", NICWebSite = "https://NIC4.net" };
            // return values
            NIC mediatorReturn = new NIC()
                { NIC_Id = "NIC4", NICDescription = "NIC 4", NICAbuseEmailAddress = "abuse@NIC4.net", NICRestService = "", NICWebSite = "https://NIC4.net" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC create was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Create(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(NicsController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            NICDetailQuery mediatorReturn = new NICDetailQuery()
                { NIC_Id = "NIC4", NICDescription = "NIC 4", NICAbuseEmailAddress = "abuse@NIC4.net", NICRestService = "", NICWebSite = "https://NIC4.net" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC edit was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Edit("NIC4");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NICDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NIC_Id, model.NIC_Id);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            NICUpdateCommand mediatorParam = new NICUpdateCommand()
                { NIC_Id = "NIC4", NICDescription = "NIC 4", NICAbuseEmailAddress = "abuse@NIC4.net", NICRestService = "", NICWebSite = "https://NIC4.net" };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC edit was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(NicsController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            NICDetailQuery mediatorReturn = new NICDetailQuery()
                { NIC_Id = "NIC4", NICDescription = "NIC 4", NICAbuseEmailAddress = "abuse@NIC4.net", NICRestService = "", NICWebSite = "https://NIC4.net" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC delete was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.Delete("NIC4");
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NICDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NIC_Id, model.NIC_Id);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            NICDeleteCommand mediatorParam = new NICDeleteCommand()
                { NIC_Id = "NIC4" };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NICDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("NIC edit was not sent.");
            NicsController sut = new NicsController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/Nics", controllerHeaders);
            // when
            var actual = await sut.DeleteConfirmed("NIC4");
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(NicsController.Alerts.Count, 0);
        }
        //
    }
}