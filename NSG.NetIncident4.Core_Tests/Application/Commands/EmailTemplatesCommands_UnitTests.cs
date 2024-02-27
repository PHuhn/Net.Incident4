using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Application.Commands.CompanyEmailTemplates;
using NSG.NetIncident4.Core.Application.Infrastructure;
using NSG.NetIncident4.Core.Persistence;
using System.ComponentModel.Design;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class EmailTemplatesCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        //
        public EmailTemplatesCommands_UnitTests()
        {
            Console.WriteLine("EmailTemplatesCommands_UnitTests, constructor");
        }
        //
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("EmailTemplatesCommands_UnitTests, Setup");
            // set up mock to get list of permissible list of companies
            GetUserCompanyListQueryHandler.ViewModel _retViewModel =
                new GetUserCompanyListQueryHandler.ViewModel() { CompanyList = new List<int>() { 1 } };
            _mediatorMock = new Mock<IMediator>();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retViewModel));
            //
        }
        //
        [Test]
        public void EmailTemplateCreateCommand_Test()
        {
            // given
            Console.WriteLine("EmailTemplateCreateCommand_Test ...");
            var _mockDbSet = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<EmailTemplate>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            CompanyEmailTemplateCreateCommand _create = new CompanyEmailTemplateCreateCommand()
            {
                CompanyId = 1,
                IncidentTypeId = 1,
                SubjectLine = "SubjectLine",
                EmailBody = "EmailBody",
                TimeTemplate = "TimeTemplate",
                ThanksTemplate = "ThanksTemplate",
                LogTemplate = "LogTemplate",
                Template = "Template",
                FromServer = false,
            };
            var _logger = new Mock<ILogger<CompanyEmailTemplateCreateCommandHandler>>();
            // when
            CompanyEmailTemplateCreateCommandHandler _handler = new CompanyEmailTemplateCreateCommandHandler(_contextMock.Object, _logger.Object);
            Task<EmailTemplate> _createResults = _handler.Handle(_create, _cancelToken);
            // then
            EmailTemplate _entity = _createResults.Result;
            Assert.That(_entity.CompanyId, Is.EqualTo(1));
            Assert.That(_entity.IncidentTypeId, Is.EqualTo(1));
        }
        //
        [Test]
        public void EmailTemplateUpdateCommand_Test()
        {
            // given
            Console.WriteLine("EmailTemplateUpdateCommand_Test ...");
            _mediatorMock = new Mock<IMediator>();
            int _companyId = 1;
            int _incidentTypeId = 8;
            EmailTemplate? _return = NSG_Helpers.emailTemplatesFakeData.Find(e => e.CompanyId == _companyId && e.IncidentTypeId == _incidentTypeId);
            if (_return != null)
            {
                var _mockDbSet = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSet.Object);
                var _saveResult = _contextMock
                    .Setup(r => r.SaveChangesAsync(_cancelToken))
                    .Returns(Task.FromResult(1));
                CompanyEmailTemplateUpdateCommand _update = new CompanyEmailTemplateUpdateCommand()
                {
                    CompanyId = 1,
                    IncidentTypeId = 8,
                    SubjectLine = "SubjectLine",
                    EmailBody = "EmailBody",
                    TimeTemplate = "TimeTemplate",
                    ThanksTemplate = "ThanksTemplate",
                    LogTemplate = "LogTemplate",
                    Template = "Template",
                    FromServer = false,
                };
                // when
                CompanyEmailTemplateUpdateCommandHandler _handler = new CompanyEmailTemplateUpdateCommandHandler(_contextMock.Object);
                Task<int> _updateResults = _handler.Handle(_update, _cancelToken);
                // then
                int _count = _updateResults.Result;
                Assert.That(_count, Is.EqualTo(1));
            }
            else
            {
                Assert.Fail($"EmailTemplate: {_companyId}-{_incidentTypeId} not found");
            }
        }
        //
        [Test]
        public void EmailTemplateDeleteCommand_Test()
        {
            // given
            Console.WriteLine("EmailTemplateDeleteCommand_Test ...");
            int _companyId = 1;
            int _incidentTypeId = 8;
            EmailTemplate? _return = NSG_Helpers.emailTemplatesFakeData.Find(e => e.CompanyId == _companyId && e.IncidentTypeId == _incidentTypeId);
            if (_return != null)
            {
                var _mockDbSet = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSet.Object);
                // IMediator mediator
                _mediatorMock = new Mock<IMediator>();
                // when
                // Now delete what was just created ...
                CompanyEmailTemplateDeleteCommandHandler _handler = new CompanyEmailTemplateDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
                CompanyEmailTemplateDeleteCommand _delete = new CompanyEmailTemplateDeleteCommand()
                {
                    CompanyId = _return.CompanyId,
                    IncidentTypeId = _return.IncidentTypeId,
                };
                Task<int> _deleteResults = _handler.Handle(_delete, _cancelToken);
                int _count = _deleteResults.Result;
                Assert.That(_count, Is.EqualTo(1));
            }
            else
            {
                Assert.Fail($"EmailTemplate: {_companyId}-{_incidentTypeId} not found");
            }
        }
        //
        [Test]
        public async Task EmailTemplateDetailQuery_Test()
        {
            // given
            Console.WriteLine("EmailTemplateDetailQuery_Test ...");
            int _companyId = 1;
            int _incidentTypeId = 8;
            EmailTemplate? _return = NSG_Helpers.emailTemplatesFakeData.Find(e => e.CompanyId == _companyId && e.IncidentTypeId == _incidentTypeId);
            if (_return != null)
            {
                var _mockDbSet = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
                var _mockDbSetITypes = NSG_Helpers.incidentTypesFakeData.BuildMock().BuildMockDbSet();
                var _mockDbSetComps = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSet.Object);
                _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSetITypes.Object);
                _contextMock.Setup(x => x.Companies).Returns(_mockDbSetComps.Object);
                // when
                CompanyEmailTemplateDetailQueryHandler _handler = new CompanyEmailTemplateDetailQueryHandler(_contextMock.Object);
                CompanyEmailTemplateDetailQueryHandler.DetailQuery _detailQuery =
                    new CompanyEmailTemplateDetailQueryHandler.DetailQuery();
                _detailQuery.CompanyId = _companyId;
                _detailQuery.IncidentTypeId = _incidentTypeId;
                CompanyEmailTemplateDetailQuery _detail =
                    await _handler.Handle(_detailQuery, _cancelToken);
                // then
                Assert.That(_detailQuery.CompanyId, Is.EqualTo(_detail.CompanyId));
                Assert.That(_detailQuery.IncidentTypeId, Is.EqualTo(_detail.IncidentTypeId));
            }
            else
            {
                Assert.Fail($"EmailTemplate: {_companyId}-{_incidentTypeId} not found");
            }
        }
        //
        [Test]
        public void EmailTemplateListQuery_Test()
        {
            // given
            Console.WriteLine("EmailTemplateListQuery_Test ...");
            var _mockDbSet = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSet.Object);
            // when
            CompanyEmailTemplateListQueryHandler _handler = new CompanyEmailTemplateListQueryHandler(_contextMock.Object, _mediatorMock.Object);
            CompanyEmailTemplateListQueryHandler.ListQuery _listQuery =
                new CompanyEmailTemplateListQueryHandler.ListQuery() { CompanyId = 1 };
            Task<CompanyEmailTemplateListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, _cancelToken);
            // then
            IList<CompanyEmailTemplateListQuery> _list = _viewModelResults.Result.EmailTemplatesList;
            Assert.That(_list.Count, Is.EqualTo(1));
        }
        //
    }
}
