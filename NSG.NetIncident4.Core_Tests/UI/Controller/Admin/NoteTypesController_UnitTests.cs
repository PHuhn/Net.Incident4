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
using NSG.NetIncident4.Core.Application.Commands.NoteTypes;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.Admin
{
    [TestFixture]
    public class NoteTypesController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        public NoteTypesController_UnitTests()
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
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on NoteTypesController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "AdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            NoteTypeListQueryHandler.ViewModel results = new NoteTypeListQueryHandler.ViewModel()
            {
                NoteTypesList = new List<NoteTypeListQuery>() {
                    new NoteTypeListQuery { NoteTypeId = 1, NoteTypeShortDesc = "nt1", NoteTypeDesc = "N T #1", NoteTypeClientScript = "" },
                    new NoteTypeListQuery { NoteTypeId = 2, NoteTypeShortDesc = "nt2", NoteTypeDesc = "N T #2", NoteTypeClientScript = "" },
                    new NoteTypeListQuery { NoteTypeId = 3, NoteTypeShortDesc = "nt3", NoteTypeDesc = "N T #3", NoteTypeClientScript = "" }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("Note type list was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<NoteTypeListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            NoteTypeDetailQuery mediatorReturn = new NoteTypeDetailQuery()
                { NoteTypeId = 1, NoteTypeShortDesc = "nt1", NoteTypeDesc = "N T #1", NoteTypeClientScript = "" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type details was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Details(1);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NoteTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NoteTypeId, model.NoteTypeId);
        }
        //
        [Test]
        public async Task Create_Test()
        {
            // given
            NoteTypeCreateCommand mediatorParam = new NoteTypeCreateCommand()
                { NoteTypeId = 4, NoteTypeShortDesc = "nt4", NoteTypeDesc = "N T #4", NoteTypeClientScript = "" };
            // return values
            NoteType mediatorReturn = new NoteType()
                { NoteTypeId = 4, NoteTypeShortDesc = "nt4", NoteTypeDesc = "N T #4", NoteTypeClientScript = "" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type create was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Create(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Edit");
            Assert.AreEqual(NoteTypesController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            NoteTypeDetailQuery mediatorReturn = new NoteTypeDetailQuery()
                { NoteTypeId = 4, NoteTypeShortDesc = "nt4", NoteTypeDesc = "N T #4", NoteTypeClientScript = "" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type edit was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Edit(4);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NoteTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NoteTypeId, model.NoteTypeId);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            NoteTypeUpdateCommand mediatorParam = new NoteTypeUpdateCommand()
                { NoteTypeId = 4, NoteTypeShortDesc = "nt4", NoteTypeDesc = "N T #4", NoteTypeClientScript = "" };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type edit was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(NoteTypesController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            NoteTypeDetailQuery mediatorReturn = new NoteTypeDetailQuery()
                { NoteTypeId = 4, NoteTypeShortDesc = "nt4", NoteTypeDesc = "N T #4", NoteTypeClientScript = "" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type delete was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.Delete(4);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as NoteTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.NoteTypeId, model.NoteTypeId);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            NoteTypeDeleteCommand mediatorParam = new NoteTypeDeleteCommand()
                { NoteTypeId = 4 };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<NoteTypeDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type edit was not sent.");
            NoteTypesController sut = new NoteTypesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/NoteTypes/", controllerHeaders);
            // when
            var actual = await sut.DeleteConfirmed(4);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(NoteTypesController.Alerts.Count, 0);
        }
        //
    }
}