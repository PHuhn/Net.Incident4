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
using MediatR;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.UI.Controllers.CompanyAdmin;
using NSG.NetIncident4.Core.Application.Commands.CompanyEmailTemplates;
using NSG.NetIncident4.Core.Application.Commands.Companies;
using NSG.NetIncident4.Core.Application.Commands.IncidentTypes;
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller.CompanyAdmin
{
    [TestFixture]
    public class CompanyEmailTemplatesController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "AdminUser";
        private CompanyEmailTemplateDetailQuery companyEmailTemplate1 = null;
        private CompanyEmailTemplateDetailQuery companyEmailTemplate2 = null;
        private CompanyEmailTemplateCreateCommand emailTemplateCreate = null;
        private EmailTemplate emailTemplate = null;
        private CompanyEmailTemplateListQuery emailTemplateListUser1 = null;
        //
        public CompanyEmailTemplatesController_UnitTests()
        {
            //
            companyEmailTemplate1 = new CompanyEmailTemplateDetailQuery()
            {
                CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
                IncidentTypeId = 4, IncidentTypeShortDesc = "PHP", IncidentTypeDesc = "PHP",
                SubjectLine = "PHP probe from ${IPAddress}", EmailBody = "Hi\\n\\nStop the intrusion from your IP address",
                TimeTemplate = "${NetworkLogDate} ${TimeZone}", ThanksTemplate = "\\nThank you,\\n${FromName}",
                LogTemplate = "\\n${Log}\\n", Template = "-",
                FromServer = true
            };
            //
            companyEmailTemplate2 = new CompanyEmailTemplateDetailQuery()
            {
                CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
                IncidentTypeId = 5, IncidentTypeShortDesc = "XSS", IncidentTypeDesc = "Cross Site Scripting",
                SubjectLine = "PHP probe from ${IPAddress}", EmailBody = "Hi\\n\\nStop the intrusion from your IP address",
                TimeTemplate = "${NetworkLogDate} ${TimeZone}", ThanksTemplate = "\\nThank you,\\n${FromName}",
                LogTemplate = "\\n${Log}\\n", Template = "-",
                FromServer = true
            };
            //
            emailTemplateCreate = new CompanyEmailTemplateCreateCommand()
            {
                CompanyId = companyEmailTemplate2.CompanyId,
                IncidentTypeId = companyEmailTemplate2.IncidentTypeId,
                SubjectLine = companyEmailTemplate2.SubjectLine,
                EmailBody = companyEmailTemplate2.EmailBody,
                TimeTemplate = companyEmailTemplate2.TimeTemplate,
                ThanksTemplate = companyEmailTemplate2.ThanksTemplate,
                LogTemplate = companyEmailTemplate2.LogTemplate,
                Template = companyEmailTemplate2.Template,
                FromServer = companyEmailTemplate2.FromServer
            };
            //
            emailTemplate = new EmailTemplate()
            {
                CompanyId = companyEmailTemplate2.CompanyId,
                IncidentTypeId = companyEmailTemplate2.IncidentTypeId,
                SubjectLine = companyEmailTemplate2.SubjectLine,
                EmailBody = companyEmailTemplate2.EmailBody,
                TimeTemplate = companyEmailTemplate2.TimeTemplate,
                ThanksTemplate = companyEmailTemplate2.ThanksTemplate,
                LogTemplate = companyEmailTemplate2.LogTemplate,
                Template = companyEmailTemplate2.Template,
                FromServer = companyEmailTemplate2.FromServer
            };
            //
            emailTemplateListUser1 = new CompanyEmailTemplateListQuery()
            {
                CompanyId = companyEmailTemplate1.CompanyId,
                IncidentTypeId = companyEmailTemplate1.IncidentTypeId,
                IncidentTypeShortDesc = companyEmailTemplate1.IncidentTypeShortDesc,
                IncidentTypeDesc = companyEmailTemplate1.IncidentTypeDesc,
                SubjectLine = companyEmailTemplate1.SubjectLine,
                EmailBody = companyEmailTemplate1.EmailBody,
                FromServer = companyEmailTemplate1.FromServer
            };
            //
        }
        public Mock<IMediator> CreateUserCompanySelectionMock()
        {
            Mock<IMediator> _mockMediator = new Mock<IMediator>();
            UserCompanySelectionListQueryHandler.ViewModel _viewModel = new UserCompanySelectionListQueryHandler.ViewModel()
            {
                CompanyList = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "NSG", Value = "1" }
                }
            };
            _mockMediator
                .Setup(m => m.Send(It.IsAny<UserCompanySelectionListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_viewModel)
                    .Verifiable("CompanyEmailTemplate list was not sent.");
            return _mockMediator;
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
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.CustomAttributes.Where(a => a.AttributeType.Name == "AuthorizeAttribute").FirstOrDefault();
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on CompanyEmailTemplatesController");
            Assert.AreEqual(attribute.NamedArguments[0].TypedValue.Value, "CompanyAdminRole");
        }
        //
        #region "Query section"
        //
        [Test]
        public async Task Index_Test()
        {
            // given
            // return values
            CompanyEmailTemplateListQueryHandler.ViewModel results = new CompanyEmailTemplateListQueryHandler.ViewModel()
            {
                EmailTemplatesList = new List<CompanyEmailTemplateListQuery>()
                {
                    emailTemplateListUser1
                }
            };
            IncidentTypeListQueryHandler.ViewModel incidentTypeListViewModel = new IncidentTypeListQueryHandler.ViewModel()
            {
                IncidentTypesList = new List<IncidentTypeListQuery>()
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = CreateUserCompanySelectionMock();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("CompanyEmailTemplate list was not sent.");
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(incidentTypeListViewModel)
                .Verifiable("EmailTemplate list was not sent.");
            // IncidentTypeListQueryHandler.ViewModel _results = await Mediator.Send(new IncidentTypeListQueryHandler.ListQuery());
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Index(1);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyEmailTemplatesIndexViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.CompanySelect.Count);
            Assert.AreEqual(1, model.CompanyEmailTemplates.Count);
        }
        //
        [Test]
        public async Task Details_Test()
        {
            // given
            // return values
            CompanyEmailTemplateDetailQuery mediatorReturn = companyEmailTemplate1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate details was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Details(mediatorReturn.CompanyId, mediatorReturn.IncidentTypeId);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyEmailTemplateDetailQuery;
            Assert.IsNotNull(model);
            // Compound key
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
            Assert.AreEqual(mediatorReturn.IncidentTypeId, model.IncidentTypeId);
        }
        //
        #endregion // Query section
        //
        #region "Create section"
        //
        [Test]
        public async Task Create_Get_Test()
        {
            // given
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = CreateUserCompanySelectionMock();
            //
            IncidentTypeSelectionListQueryHandler.ViewModel mediatoriIncidentType = new IncidentTypeSelectionListQueryHandler.ViewModel()
            {
                IncidentTypesList = new List<SelectListItem>() { new SelectListItem() { Value = "1", Text = "XXX" } }
            };
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeSelectionListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatoriIncidentType)
                .Verifiable("CompanyEmailTemplate edit was not sent.");
            //
            CompanyEmailTemplateSelectionListQueryHandler.ViewModel mediatorETSelectList = new CompanyEmailTemplateSelectionListQueryHandler.ViewModel()
            {
                EmailTemplatesList = new List<SelectListItem>() { new SelectListItem() { Value = "2", Text = "SQL" } }
            };
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateSelectionListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorETSelectList)
                .Verifiable("CompanyEmailTemplate edit was not sent.");
            //
            IncidentTypeListQueryHandler.ViewModel mediatorReturn = new IncidentTypeListQueryHandler.ViewModel()
            {
                IncidentTypesList = new List<IncidentTypeListQuery>()
            };
            mockMediator
                .Setup(m => m.Send(It.IsAny<IncidentTypeListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate edit was not sent.");
            //
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Create(1);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyEmailTemplateCreateCommand;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.CompanyId);
            Assert.AreEqual(0, model.IncidentTypeId);
        }
        //
        [Test]
        public async Task Create_Test()
        {
            // given
            CompanyEmailTemplateCreateCommand mediatorParam = emailTemplateCreate;
            // return values
            EmailTemplate mediatorReturn = emailTemplate;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("Note type create was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            CompanyEmailTemplatesController.Alerts = new List<AlertMessage>();
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Create(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(CompanyEmailTemplatesController.Alerts.Count, 0);
        }
        //
        #endregion // Create section
        //
        #region "Edit section"
        //
        [Test]
        public async Task Edit_Get_Test()
        {
            // given
            // return values
            CompanyEmailTemplateDetailQuery mediatorReturn = companyEmailTemplate1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate edit was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorReturn.CompanyId, mediatorReturn.IncidentTypeId);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyEmailTemplateUpdateCommand;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
            Assert.AreEqual(mediatorReturn.IncidentTypeId, model.IncidentTypeId);
        }
        //
        [Test]
        public async Task Edit_Save_Test()
        {
            // given
            CompanyEmailTemplateUpdateCommand mediatorParam = new CompanyEmailTemplateUpdateCommand()
            {
            };
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate edit was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            CompanyEmailTemplatesController.Alerts = new List<AlertMessage>();
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Edit(mediatorParam);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Details");
            Assert.AreEqual(CompanyEmailTemplatesController.Alerts.Count, 0);
        }
        //
        #endregion // Edit section
        //
        #region "Delete section"
        //
        [Test]
        public async Task Delete_Get_Test()
        {
            // given
            // return values
            CompanyEmailTemplateDetailQuery mediatorReturn = companyEmailTemplate2;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate delete was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.Delete(mediatorReturn.CompanyId, mediatorReturn.IncidentTypeId);
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as CompanyEmailTemplateDetailQuery;
            Assert.IsNotNull(model);
            Assert.AreEqual(mediatorReturn.CompanyId, model.CompanyId);
        }
        //
        [Test]
        public async Task DeleteConfirmed_Test()
        {
            // given
            CompanyEmailTemplateDetailQuery mediatorParam = companyEmailTemplate2;
            // return values
            int mediatorReturn = 1;
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<CompanyEmailTemplateDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorReturn)
                .Verifiable("CompanyEmailTemplate delete was not sent.");
            CompanyEmailTemplatesController sut = new CompanyEmailTemplatesController(mockMediator.Object);
            CompanyEmailTemplatesController.Alerts = new List<AlertMessage>();
            sut.ControllerContext = Fixture_ControllerContext("TestUser", "admin", "/CompanyEmailTemplates/", controllerHeaders);
            // when
            var actual = await sut.DeleteConfirmed(mediatorParam.CompanyId, mediatorParam.IncidentTypeId);
            // then
            var viewResult = actual as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(viewResult.ActionName, "Index");
            Assert.AreEqual(CompanyEmailTemplatesController.Alerts.Count, 0);
        }
        //
        #endregion // Delete section
        //
    }
}
//