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
using NSG.NetIncident4.Core.Application.Commands.IncidentTypes;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.Admin
{
    [TestFixture]
    public class IncidentTypesEmailTemplateController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        public IncidentTypesEmailTemplateController_UnitTests()
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
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on IncidentTypesEmailTemplateController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "AdminRole");
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            IncidentTypeListQueryHandler.ViewModel results = new IncidentTypeListQueryHandler.ViewModel()
            {
                IncidentTypesList = new List<IncidentTypeListQuery>() {
                    new IncidentTypeListQuery { IncidentTypeId = 1, IncidentTypeShortDesc = "ShortDesc1", IncidentTypeDesc = "IncidentTypeDesc 1", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" },
                    new IncidentTypeListQuery { IncidentTypeId = 2, IncidentTypeShortDesc = "ShortDesc2", IncidentTypeDesc = "IncidentTypeDesc 2", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" },
                    new IncidentTypeListQuery { IncidentTypeId = 3, IncidentTypeShortDesc = "ShortDesc3", IncidentTypeDesc = "IncidentTypeDesc 3", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("IncidentType list was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<IncidentTypeListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            IncidentTypeDetailQuery mediatorReturn = new IncidentTypeDetailQuery()
                { IncidentTypeId = 3, IncidentTypeShortDesc = "ShortDesc3", IncidentTypeDesc = "IncidentTypeDesc 3", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType details was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Details(3);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as IncidentTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.IncidentTypeId, model.IncidentTypeId);
        }
        //
        [Test]
        public async Task Create_Test()
        {
            // given
            IncidentTypeCreateCommand mediatorParam = new IncidentTypeCreateCommand()
                { IncidentTypeId = 4, IncidentTypeShortDesc = "ShortDesc4", IncidentTypeDesc = "IncidentTypeDesc 4", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // return values
            IncidentType mediatorReturn = new IncidentType()
                { IncidentTypeId = 4, IncidentTypeShortDesc = "ShortDesc4", IncidentTypeDesc = "IncidentTypeDesc 4", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType create was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Create(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(IncidentTypesEmailTemplateController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            IncidentTypeDetailQuery mediatorReturn = new IncidentTypeDetailQuery()
                { IncidentTypeId = 4, IncidentTypeShortDesc = "ShortDesc4", IncidentTypeDesc = "IncidentTypeDesc 4", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType edit was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Edit(4);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as IncidentTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.IncidentTypeId, model.IncidentTypeId);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            IncidentTypeUpdateCommand mediatorParam = new IncidentTypeUpdateCommand()
                { IncidentTypeId = 4, IncidentTypeShortDesc = "ShortDesc4", IncidentTypeDesc = "IncidentTypeDesc 4", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType edit was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(IncidentTypesEmailTemplateController.Alerts.Count, 0);
        }
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            IncidentTypeDetailQuery mediatorReturn = new IncidentTypeDetailQuery()
                { IncidentTypeId = 4, IncidentTypeShortDesc = "ShortDesc4", IncidentTypeDesc = "IncidentTypeDesc 4", IncidentTypeFromServer = false, IncidentTypeSubjectLine = "SubjectLine", IncidentTypeEmailTemplate = "EmailTemplate", IncidentTypeTimeTemplate = "Time Template", IncidentTypeThanksTemplate = "Thanks", IncidentTypeLogTemplate = "Log data", IncidentTypeTemplate = "Template" };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType delete was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            // when
            var actual = await sut.Delete(4);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as IncidentTypeDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.IncidentTypeId, model.IncidentTypeId);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            IncidentTypeDeleteCommand mediatorParam = new IncidentTypeDeleteCommand()
                { IncidentTypeId = 4 };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("IncidentType edit was not sent.");
            IncidentTypesEmailTemplateController sut = new IncidentTypesEmailTemplateController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/IncidentTypes/", controllerHeaders);
            
            // when
            var actual = await sut.DeleteConfirmed(4);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(IncidentTypesEmailTemplateController.Alerts.Count, 0);
        }
        //
    }
}
//