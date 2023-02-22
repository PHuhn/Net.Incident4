using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class IncidentCommands_UnitTests : UnitTestFixture
    {
        //
        private string _testName = "";
        Mock<IMediator> _mockMediator = null;
        static Mock<IApplication> _mockApplication = null;
        static Mock<INotificationService> _mockNotification = null;
        static CancellationToken _cancelToken = CancellationToken.None;
        UserServerData _user = null;
        //
        public IncidentCommands_UnitTests()
        {
            Console.WriteLine("IncidentCommands_UnitTests ...");
        }
        //
        [SetUp]
        public async Task SetupAsync()
        {
            Console.WriteLine("Setup");
            //
            Fixture_UnitTestSetup();
            _mockMediator = new Mock<IMediator>();
            _mockApplication = new Mock<IApplication>();
            _mockNotification = new Mock<INotificationService>();
            _user = new UserServerData()
            {
                UserName = "Phil",
                CompanyId = 1
            };
            //
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            foreach(Incident _in in db_context.Incidents.Include(_i => _i.IncidentIncidentNotes))
            {
                Console.WriteLine(_in.IncidentId.ToString() + " " + _in.IPAddress);
            }
            Incident _inc = await db_context.Incidents
                .Include(_i => _i.IncidentIncidentNotes)
                .Include(_i => _i.Server)
                .SingleOrDefaultAsync(r => r.IncidentId == 1);
            foreach(IncidentIncidentNote _iin in _inc.IncidentIncidentNotes)
            {
                Console.WriteLine(_iin.IncidentNote.IncidentNoteId.ToString() + " " + _iin.IncidentNote.NoteTypeId.ToString());
            }
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_Test()
        {
            _testName = "NetworkIncidentCreateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            NetworkIncidentDetailQuery _retModel =
                new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = 2 };
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(true);
            _mockMediator.Setup(x => x.Send(
                It.IsAny< NetworkIncidentDetailQueryHandler.DetailQuery> (), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(db_context, _mockMediator.Object, _mockApplication.Object);
            var _nld = new NetworkLogData() { NetworkLogId = 4, ServerId = 1, IncidentId = null, IPAddress = "54.183.209.144", IncidentTypeId = 3, Selected = true };
            NetworkIncidentSaveQuery _create = new NetworkIncidentSaveQuery()
            {
                incident = new NetworkIncidentData()
                {
                    ServerId = 1,
                    IPAddress = "11.10.10.10",
                    NIC = "ripe.net",
                    NetworkName = "NetworkName",
                    AbuseEmailAddress = "AbuseEmailAddress",
                    ISPTicketNumber = "ISPTicketNumber",
                    Mailed = false,
                    Closed = false,
                    Special = false,
                    Notes = "Notes",
                    CreatedDate = DateTime.Now,
                },
                user = _user,
                incidentNotes = new List<IncidentNoteData>(),
                deletedNotes = new List<IncidentNoteData>(),
                networkLogs = new List<NetworkLogData>() { _nld },
                deletedLogs = new List<NetworkLogData>()
            };
            Task<NetworkIncidentDetailQuery> _createResults = _handler.Handle(new NetworkIncidentCreateCommand() { SaveQuery = _create }, CancellationToken.None);
            Assert.IsNull(_createResults.Exception);
            NetworkIncidentDetailQuery _entity = _createResults.Result;
            Assert.AreEqual(2, _entity.incident.IncidentId);
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_PermissionsError_Test()
        {
            _testName = "NetworkIncidentCreateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(false);
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(db_context, _mockMediator.Object, _mockApplication.Object);
            NetworkIncidentCreateCommand _create = new NetworkIncidentCreateCommand();
            Task<NetworkIncidentDetailQuery> _createResults = _handler.Handle(_create, CancellationToken.None);
            Assert.IsNotNull(_createResults.Exception);
            Assert.IsTrue(_createResults.Exception.InnerException is NetworkIncidentCreateCommandPermissionsException);
            Console.WriteLine(_createResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_ValidationError_Test()
        {
            _testName = "NetworkIncidentCreateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(true);
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(db_context, _mockMediator.Object, _mockApplication.Object);
            NetworkIncidentCreateCommand _create = new NetworkIncidentCreateCommand()
            {
                SaveQuery = new NetworkIncidentSaveQuery()
                {
                    incident = new NetworkIncidentData(),
                    incidentNotes = new List<IncidentNoteData>(),
                    deletedNotes = new List<IncidentNoteData>(),
                    networkLogs = new List<NetworkLogData>(),
                    deletedLogs = new List<NetworkLogData>(),
                    user = new UserServerData(),
                    message = ""
                }
            };
            Task<NetworkIncidentDetailQuery> _createResults = _handler.Handle(_create, CancellationToken.None);
            Assert.IsNotNull(_createResults.Exception);
            Assert.IsTrue(_createResults.Exception.InnerException is NetworkIncidentCreateCommandValidationException);
            Console.WriteLine(_createResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_Test()
        {
            _testName = "NetworkIncidentUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            long _incidentId = 1;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _mockMediator.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(true);
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                db_context, _mockMediator.Object, _mockApplication.Object, _mockNotification.Object);
            NetworkIncidentSaveQuery _update = new NetworkIncidentSaveQuery()
            {
                incident = new NetworkIncidentData()
                {
                    IncidentId = _incidentId,
                    ServerId = 1,
                    IPAddress = "11.10.10.10",
                    NIC = "ripe.net",
                    NetworkName = "NetworkName",
                    AbuseEmailAddress = "AbuseEmailAddress",
                    ISPTicketNumber = "ISPTicketNumber",
                    Mailed = false,
                    Closed = false,
                    Special = false,
                    Notes = "Notes",
                },
                user = _user,
                incidentNotes = new List<IncidentNoteData>(),
                deletedNotes = new List<IncidentNoteData>(),
                networkLogs = new List<NetworkLogData>(),
                deletedLogs = new List<NetworkLogData>()
            };
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            Assert.IsNull(_updateResults.Exception);
            NetworkIncidentDetailQuery _entity = _updateResults.Result;
            Assert.AreEqual(_incidentId, _entity.incident.IncidentId);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_PermissionsError_Test()
        {
            // NetworkIncidentUpdateCommandPermissionsException("user not in editable group.");
            _testName = "NetworkIncidentUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(false);
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                db_context, _mockMediator.Object, _mockApplication.Object, _mockNotification.Object);
            NetworkIncidentSaveQuery _update = new NetworkIncidentSaveQuery();
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandPermissionsException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_ValidationError_Test()
        {
            _testName = "NetworkIncidentUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            long _incidentId = 1;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _mockMediator.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(true);
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                db_context, _mockMediator.Object, _mockApplication.Object, _mockNotification.Object);
            NetworkIncidentSaveQuery _update = new NetworkIncidentSaveQuery()
            {
                incident = new NetworkIncidentData()
                {
                    IncidentId = _incidentId,
                    ServerId = 0,
                    IPAddress = "11.10",
                    NIC = "",
                    NetworkName = "",
                    AbuseEmailAddress = "",
                    ISPTicketNumber = "",
                    Mailed = false,
                    Closed = false,
                    Special = false,
                    Notes = "",
                },
            };
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandValidationException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_NotFoundError_Test()
        {
            _testName = "NetworkIncidentUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            long _incidentId = 9;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _mockMediator.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _mockApplication.Setup(x => x.IsEditableRole()).Returns(true);
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                db_context, _mockMediator.Object, _mockApplication.Object, _mockNotification.Object);
            NetworkIncidentSaveQuery _update = new NetworkIncidentSaveQuery()
            {
                incident = new NetworkIncidentData()
                {
                    IncidentId = _incidentId,
                    ServerId = 1,
                    IPAddress = "11.10.10.10",
                    NIC = "ripe.net",
                    NetworkName = "NetworkName",
                    AbuseEmailAddress = "AbuseEmailAddress",
                    ISPTicketNumber = "ISPTicketNumber",
                    Mailed = false,
                    Closed = false,
                    Special = false,
                    Notes = "Notes",
                },
                user = _user,
                incidentNotes = new List<IncidentNoteData>(),
                deletedNotes = new List<IncidentNoteData>(),
                networkLogs = new List<NetworkLogData>(),
                deletedLogs = new List<NetworkLogData>()
            };
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandKeyNotFoundException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void IncidentDeleteCommand_Test()
        {
            _testName = "IncidentDeleteCommand_Test";
            Console.WriteLine($"{_testName} ...");
            // Add a row to be deleted.
            Incident _create = new Incident()
            {
                ServerId = 1,
                IPAddress = "11.10.10.10",
                NIC_Id = "ripe.net",
                NetworkName = "NetworkName",
                AbuseEmailAddress = "AbuseEmailAddress",
                ISPTicketNumber = "ISPTicketNumber",
                Mailed = false,
                Closed = false,
                Special = false,
                Notes = "Notes",
                CreatedDate = DateTime.Now,
            };
            IncidentNote _note = new IncidentNote()
            {
                NoteTypeId = 3,
                Note = "Note"
            };
            db_context.Incidents.Add(_create);
            db_context.IncidentNotes.Add(_note);
            db_context.IncidentIncidentNotes.Add(new IncidentIncidentNote()
            {
                Incident = _create,
                IncidentNote = _note
            });
            db_context.NetworkLogs.Add(new NetworkLog()
            {
                ServerId = 1,
                // Nullable Reference Types
                Incident = _create,
                IPAddress = "192.168.200.21",
                NetworkLogDate = DateTime.Now,
                Log = "Bad, bad thing are happening",
                IncidentTypeId = 3
            });
            db_context.SaveChanges();
            //
            _mockApplication.Setup(x => x.IsCompanyAdminRole()).Returns(true);
            //
            // Now delete what was just created ...
            IncidentDeleteCommandHandler _handler = new IncidentDeleteCommandHandler(db_context, _mockMediator.Object, _mockApplication.Object);
            IncidentDeleteCommand _delete = new IncidentDeleteCommand()
            {
                IncidentId = _create.IncidentId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            int _count = _deleteResults.Result;
            Assert.AreEqual(3, _count);
        }
        //
        [Test]
        public async Task NetworkIncidentDetailQuery_Test()
        {
            _testName = "NetworkIncidentDetailQuery_Test";
            Console.WriteLine($"{_testName} ...");
            NetworkIncidentDetailQueryHandler _handler = new NetworkIncidentDetailQueryHandler(db_context);
            NetworkIncidentDetailQueryHandler.DetailQuery _detailQuery =
                new NetworkIncidentDetailQueryHandler.DetailQuery() { IncidentId = 1 };
            NetworkIncidentDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            Assert.AreEqual(1, _detail.incident.IncidentId);
        }
        //
        [Test]
        public async Task NetworkIncidentCreateQuery_Test()
        {
            _testName = "NetworkIncidentCreateQuery_Test";
            Console.WriteLine($"{_testName} ...");
            _mockMediator.Setup(x => x.Send(
                It.IsAny<ApplicationUserServerDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(new ApplicationUserServerDetailQuery()));
            NetworkIncidentCreateQueryHandler _handler = new NetworkIncidentCreateQueryHandler(db_context, _mockMediator.Object);
            NetworkIncidentCreateQueryHandler.DetailQuery _detailQuery =
                new NetworkIncidentCreateQueryHandler.DetailQuery() { ServerId = 1, UserName = "Fred" };
            NetworkIncidentDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            Assert.AreEqual(1, _detail.incident.ServerId);
            Assert.AreEqual(11, _detail.NICs.Count);
            Assert.AreEqual(8, _detail.incidentTypes.Count);
            Assert.AreEqual(5, _detail.noteTypes.Count);
        }
        //
        [Test]
        public void IncidentListQuery_Test()
        {
            _testName = "IncidentListQuery_Test";
            Console.WriteLine($"{_testName} ...");
            _mockApplication.Setup(x => x.IsAuthenticated()).Returns(true);
            string _jsonString = "{'first':0,'rows':3,'sortOrder':1,'filters':{'ServerId':{'value':1,'matchMode':'equals'},'Mailed':{'value':'false','matchMode':'equals'},'Closed':{'value':'false','matchMode':'equals'},'Special':{'value':'false','matchMode':'equals'}},'globalFilter':null}";
            IncidentListQueryHandler _handler = new IncidentListQueryHandler(db_context, _mockApplication.Object);
            IncidentListQueryHandler.ListQuery _listQuery =
                new IncidentListQueryHandler.ListQuery() { JsonString = _jsonString };
            Task<IncidentListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            IList<IncidentListQuery> _list = _viewModelResults.Result.results;
            Assert.AreEqual(1, _list.Count);
        }
        //
    }
}
