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
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.IncidentTypes;
using NSG.NetIncident4.Core.Persistence;
using MockQueryable.Moq;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class IncidentTypeCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        public IncidentTypeCommands_UnitTests()
        {
            Console.WriteLine("IncidentTypeCommands_UnitTests ...");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
        }
        //
        [Test]
        public void IncidentTypeCreateCommand_Test()
        {
            // given
            Console.WriteLine($"IncidentTypeCreateCommand_Test ...");
            var _mockDbSet = NSG_Helpers.incidentTypesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.AddAsync(It.IsAny<IncidentType>(), _cancelToken)).ReturnsAsync(() => null);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            IncidentTypeCreateCommand _create = new IncidentTypeCreateCommand()
            {
                //                       12345678
                IncidentTypeShortDesc = "Incid 9",
                IncidentTypeDesc = "IncidentTypeDesc 9",
                IncidentTypeFromServer = false,
                IncidentTypeSubjectLine = "IncidentTypeSubjectLine",
                IncidentTypeEmailTemplate = "IncidentTypeEmailTemplate",
                IncidentTypeTimeTemplate = "IncidentTypeTimeTemplate",
                IncidentTypeThanksTemplate = "IncidentTypeThanksTemplate",
                IncidentTypeLogTemplate = "IncidentTypeLogTemplate",
                IncidentTypeTemplate = "IncidentTypeTemplate",
            };
            // when
            IncidentTypeCreateCommandHandler _handler = new IncidentTypeCreateCommandHandler(_contextMock.Object);
            Task<IncidentType> _createResults = _handler.Handle(_create, _cancelToken);
            // then
            IncidentType _entity = _createResults.Result;
            Assert.AreEqual("Incid 9", _entity.IncidentTypeShortDesc);
        }
        //
        [Test]
        public void IncidentTypeUpdateCommand_Test()
        {
            IncidentTypeUpdateCommandHandler _handler = new IncidentTypeUpdateCommandHandler(_contextMock.Object);
            IncidentTypeUpdateCommand _update = new IncidentTypeUpdateCommand()
            {
                IncidentTypeId = 1,
                IncidentTypeShortDesc = "Incident",
                IncidentTypeDesc = "IncidentTypeDesc",
                IncidentTypeFromServer = false,
                IncidentTypeSubjectLine = "IncidentTypeSubjectLine",
                IncidentTypeEmailTemplate = "IncidentTypeEmailTemplate",
                IncidentTypeTimeTemplate = "IncidentTypeTimeTemplate",
                IncidentTypeThanksTemplate = "IncidentTypeThanksTemplate",
                IncidentTypeLogTemplate = "IncidentTypeLogTemplate",
                IncidentTypeTemplate = "IncidentTypeTemplate",
            };
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void IncidentTypeDeleteCommand_Test()
        {
            // given
            Console.WriteLine("IncidentTypeDeleteCommand_Test ...");
            int _incidentTypeId = 5;
            IncidentType? _return = NSG_Helpers.incidentTypesFakeData.Find(e => e.IncidentTypeId == _incidentTypeId);
            if( _return != null)
            {
                var _mockDbSet = new List<IncidentType>() { _return }.BuildMock().BuildMockDbSet();
                // _mockDbSet.Setup(x => x.FindAsync(_incidentTypeId, _cancelToken)).ReturnsAsync(_return);
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSet.Object);
                _mediatorMock = new Mock<IMediator>();
                // IMediator mediator
                Mock<IMediator> _mockMediator = new Mock<IMediator>();
                // when
                IncidentTypeDeleteCommandHandler _handler = new IncidentTypeDeleteCommandHandler(_contextMock.Object, _mockMediator.Object);
                IncidentTypeDeleteCommand _delete = new IncidentTypeDeleteCommand()
                {
                    IncidentTypeId = _incidentTypeId,
                };
                Task<int> _deleteResults = _handler.Handle(_delete, _cancelToken);
                int _count = _deleteResults.Result;
                Assert.AreEqual(1, _count);
            }
            else
            {
                Assert.Fail($"Failed to find IncidentTypeId: {_incidentTypeId}");
            }
        }
        //
        [Test]
        public async Task IncidentTypeDetailQuery_Test()
        {
            // given
            Console.WriteLine("IncidentTypeDetailQuery_Test ...");
            _mediatorMock = new Mock<IMediator>();
            int _incidentTypeId = 4;
            IncidentType? _return = NSG_Helpers.incidentTypesFakeData.Find(e => e.IncidentTypeId == _incidentTypeId);
            if( _return != null )
            {
                var _mockDbSet = new List<IncidentType>() { _return }.BuildMock().BuildMockDbSet();
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSet.Object);
                // when
                IncidentTypeDetailQueryHandler _handler = new IncidentTypeDetailQueryHandler(_contextMock.Object);
                IncidentTypeDetailQueryHandler.DetailQuery _detailQuery =
                    new IncidentTypeDetailQueryHandler.DetailQuery();
                _detailQuery.IncidentTypeId = _incidentTypeId;
                IncidentTypeDetailQuery _detail =
                    await _handler.Handle(_detailQuery, _cancelToken);
                Assert.AreEqual(_detail.IncidentTypeId, _incidentTypeId);
            }
            else
            {
                Assert.Fail($"Failed to find IncidentTypeId: {_incidentTypeId}");
            }
        }
        //
        [Test]
        public void IncidentTypeListQuery_Test()
        {
            Console.WriteLine("IncidentTypeListQuery_Test ...");
            // given
            var _mockDbSet = NSG_Helpers.incidentTypesFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSet.Object);
            // when
            IncidentTypeListQueryHandler _handler = new IncidentTypeListQueryHandler(_contextMock.Object);
            IncidentTypeListQueryHandler.ListQuery _listQuery =
                new IncidentTypeListQueryHandler.ListQuery();
            Task<IncidentTypeListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            // then
            IList<IncidentTypeListQuery> _list = _viewModelResults.Result.IncidentTypesList;
            Assert.AreEqual(8, _list.Count);
        }
        //
    }
}
