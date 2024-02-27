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
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.IncidentNotes;
using NSG.NetIncident4.Core.Persistence;
using SendGrid.Helpers.Mail;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class IncidentNoteCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        public IncidentNoteCommands_UnitTests()
        {
            Console.WriteLine("IncidentNoteCommands_UnitTests ...");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
        }
        //
        [Test]
        public void IncidentNoteCreateCommand_Test()
        {
            // given
            Console.WriteLine("IncidentNoteCreateCommand_Test ...");
            var _mockDbSet = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.DbSetAddAsync<IncidentNote>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            IncidentNoteCreateCommand _create = new IncidentNoteCreateCommand()
            {
                NoteTypeId = 1,
                Note = "Note",
            };
            // when
            IncidentNoteCreateCommandHandler _handler = new IncidentNoteCreateCommandHandler(_contextMock.Object);
            Task<IncidentNote> _createResults = _handler.Handle(_create, _cancelToken);
            IncidentNote _entity = _createResults.Result;
            Assert.That(_create.NoteTypeId, Is.EqualTo(_entity.NoteTypeId));
        }
        //
        [Test]
        public void IncidentNoteUpdateCommand_Test()
        {
            Console.WriteLine($"IncidentNoteUpdateCommand_Test ...");
            long _incidentNoteId = 4;
            IncidentNote? _return = NSG_Helpers.incidentNotesFakeData.Find(e => e.IncidentNoteId == _incidentNoteId);
            var _mockDbSet = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_incidentNoteId, _cancelToken)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            IncidentNoteUpdateCommand _update = new IncidentNoteUpdateCommand()
            {
                IncidentNoteId = 1,
                NoteTypeId = 1,
                Note = "Note",
                CreatedDate = DateTime.Now,
            };
            IncidentNoteUpdateCommandHandler _handler = new IncidentNoteUpdateCommandHandler(_contextMock.Object);
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public void IncidentNoteDeleteCommand_Test()
        {
            // given
            Console.WriteLine("IncidentNoteDeleteCommand_Test ...");
            long _incidentNoteId = 5;
            IncidentNote? _return = NSG_Helpers.incidentNotesFakeData.Find(e => e.IncidentNoteId == _incidentNoteId);
            var _mockDbSet = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_incidentNoteId, _cancelToken)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSet.Object);
            _mediatorMock = new Mock<IMediator>();
            // when
            IncidentNoteDeleteCommandHandler _handler = new IncidentNoteDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
            IncidentNoteDeleteCommand _delete = new IncidentNoteDeleteCommand()
            {
                IncidentNoteId = _return.IncidentNoteId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            // then
            int _count = _deleteResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public async Task IncidentNoteDetailQuery_Test()
        {
            Console.WriteLine("IncidentNoteDetailQuery_Test ...");
            _mediatorMock = new Mock<IMediator>();
            long _incidentNoteId = 4;
            IncidentNote? _return = NSG_Helpers.incidentNotesFakeData.Find(e => e.IncidentNoteId == _incidentNoteId);
            var _mockDbSet = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_incidentNoteId, _cancelToken)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSet.Object);
            // when
            IncidentNoteDetailQueryHandler _handler = new IncidentNoteDetailQueryHandler(_contextMock.Object);
            IncidentNoteDetailQueryHandler.DetailQuery _detailQuery =
                new IncidentNoteDetailQueryHandler.DetailQuery();
            _detailQuery.IncidentNoteId = _incidentNoteId;
            IncidentNoteDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            Assert.That(_detailQuery.IncidentNoteId, Is.EqualTo(_detail.IncidentNoteId));
        }
        //
        [Test]
        public void IncidentNoteListQuery_Test()
        {
            Console.WriteLine("IncidentNoteListQuery_Test ...");
            // given
            var _mockDbSet = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSet.Object);
            // when
            IncidentNoteListQueryHandler _handler = new IncidentNoteListQueryHandler(_contextMock.Object);
            IncidentNoteListQueryHandler.ListQuery _listQuery =
                new IncidentNoteListQueryHandler.ListQuery();
            Task<IncidentNoteListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            IList<IncidentNoteListQuery> _list = _viewModelResults.Result.IncidentNotesList;
            Assert.That(_list.Count, Is.EqualTo(6));
        }
        //
    }
}
