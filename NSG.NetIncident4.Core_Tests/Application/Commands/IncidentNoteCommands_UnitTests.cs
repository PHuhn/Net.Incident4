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
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core_Tests.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Application.Commands.IncidentNotes;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class IncidentNoteCommands_UnitTests : UnitTestFixture
    {
        //
        string _testName = "";
        static Mock<IMediator> _mockMediator = null;
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
            //
            Fixture_UnitTestSetup();
            //
            _mockMediator = new Mock<IMediator>();
            //
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            foreach( IncidentNote _in in db_context.IncidentNotes)
            {
                Console.WriteLine(_in.IncidentNoteId.ToString() + " ");
            }
        }
        //
        // You will need to check that the indexes work with you test data.
        //
        [Test]
        public void IncidentNoteCreateCommand_Test()
        {
            _testName = "IncidentNoteCreateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            IncidentNoteCreateCommandHandler _handler = new IncidentNoteCreateCommandHandler(db_context);
            IncidentNoteCreateCommand _create = new IncidentNoteCreateCommand()
            {
                NoteTypeId = 3,
                Note = "Note"
            };
            Task<IncidentNote> _createResults = _handler.Handle(_create, CancellationToken.None);
            IncidentNote _entity = _createResults.Result;
            Assert.AreEqual(2, _entity.IncidentNoteId);
        }
        //
        [Test]
        public void IncidentNoteUpdateCommand_Test()
        {
            _testName = "IncidentNoteUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            IncidentNoteUpdateCommandHandler _handler = new IncidentNoteUpdateCommandHandler(db_context);
            IncidentNoteUpdateCommand _update = new IncidentNoteUpdateCommand()
            {
                IncidentNoteId = 1,
                NoteTypeId = 1,
                Note = "Note",
                CreatedDate = DateTime.Now,
            };
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void IncidentNoteDeleteCommand_Test()
        {
            _testName = "IncidentNoteDeleteCommand_Test";
            Console.WriteLine($"{_testName} ...");
            // Add a row to be deleted.
            IncidentNote _create = new IncidentNote()
            {
                NoteTypeId = 1,
                Note = "Note",
                CreatedDate = DateTime.Now,
            };
            db_context.IncidentNotes.Add(_create);
            db_context.SaveChanges();
            db_context.IncidentIncidentNotes.Add(new IncidentIncidentNote()
                { IncidentId = 1, IncidentNoteId = _create.IncidentNoteId });
            db_context.SaveChanges();
            //
            // Now delete what was just created ...
            IncidentNoteDeleteCommandHandler _handler = new IncidentNoteDeleteCommandHandler(db_context, _mockMediator.Object);
            IncidentNoteDeleteCommand _delete = new IncidentNoteDeleteCommand()
            {
                IncidentNoteId = _create.IncidentNoteId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            int _count = _deleteResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public async Task IncidentNoteDetailQuery_Test()
        {
            _testName = "IncidentNoteDetailQuery_Test";
            Console.WriteLine($"{_testName} ...");
            IncidentNoteDetailQueryHandler _handler = new IncidentNoteDetailQueryHandler(db_context);
            IncidentNoteDetailQueryHandler.DetailQuery _detailQuery =
                new IncidentNoteDetailQueryHandler.DetailQuery();
            _detailQuery.IncidentNoteId = 1;
            IncidentNoteDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            Assert.AreEqual(1, _detail.IncidentNoteId);
        }
        //
        [Test]
        public void IncidentNoteListQuery_Test()
        {
            _testName = "IncidentNoteListQuery_Test";
            Console.WriteLine($"{_testName} ...");
            IncidentNoteListQueryHandler _handler = new IncidentNoteListQueryHandler(db_context);
            IncidentNoteListQueryHandler.ListQuery _listQuery =
                new IncidentNoteListQueryHandler.ListQuery();
            Task<IncidentNoteListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            IList<IncidentNoteListQuery> _list = _viewModelResults.Result.IncidentNotesList;
            Assert.AreEqual(1, _list.Count);
        }
        //
    }
}
