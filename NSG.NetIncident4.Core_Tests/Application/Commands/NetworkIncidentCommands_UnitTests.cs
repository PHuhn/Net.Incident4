using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class NetworkIncidentCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        static Mock<IApplication> _applicationMock = null;
        static Mock<INotificationService> _notificationMock = null;
        //
        UserServerData _user = null;
        //
        public NetworkIncidentCommands_UnitTests()
        {
            Console.WriteLine("NetworkIncidentCommands_UnitTests ...");
        }
        //
        [SetUp]
        public async Task SetupAsync()
        {
            Console.WriteLine("Setup");
            //
            // Fixture_UnitTestSetup();
            _mediatorMock = new Mock<IMediator>();
            _applicationMock = new Mock<IApplication>();
            _notificationMock = new Mock<INotificationService>();
            _user = new UserServerData()
            {
                UserName = "Phil",
                CompanyId = 1
            };
            //
            //DatabaseSeeder _seeder = new DatabaseSeeder(_contextMock.Object, userManager, roleManager);
            //_seeder.Seed().Wait();
        }
        //
        public async Task IncidentReport(ApplicationDbContext db_context)
        {
            Console.WriteLine("IncidentReport");
            //
            foreach (Incident _in in db_context.Incidents.Include(_i => _i.IncidentIncidentNotes))
            {
                Console.WriteLine(_in.IncidentId.ToString() + " " + _in.IPAddress);
            }
            Incident _inc = await db_context.Incidents
                .Include(_i => _i.IncidentIncidentNotes)
                .Include(_i => _i.Server)
                .SingleOrDefaultAsync(r => r.IncidentId == 1);
            foreach (IncidentIncidentNote _iin in _inc.IncidentIncidentNotes)
            {
                Console.WriteLine(_iin.IncidentNote.IncidentNoteId.ToString() + " " + _iin.IncidentNote.NoteTypeId.ToString());
            }
        }
        public Mock<ApplicationDbContext> NetworkIncidentContextMockSeed()
        {
            var _mockDbSet = NSG_Helpers.incidentsFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetNote = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetNTypes = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetIncNote = NSG_Helpers.incidentIncidentNotesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetLogs = NSG_Helpers.networkLogsFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetITypes = NSG_Helpers.incidentTypesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetEmails = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetSrvr = NSG_Helpers.serversFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetNICs = NSG_Helpers.nicsFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.DbSetAddAsync<Incident>();
            _mockDbSetNote.DbSetAddAsync<IncidentNote>();
            Mock<ApplicationDbContext> _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Incidents).Returns(_mockDbSet.Object);
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSetNote.Object);
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSetNTypes.Object);
            _contextMock.Setup(x => x.IncidentIncidentNotes).Returns(_mockDbSetIncNote.Object);
            _contextMock.Setup(x => x.NetworkLogs).Returns(_mockDbSetLogs.Object);
            _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSetITypes.Object);
            _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSetEmails.Object);
            _contextMock.Setup(x => x.Servers).Returns(_mockDbSetSrvr.Object);
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSetNICs.Object);
            return _contextMock;
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentCreateCommand_Test ...");
            NetworkIncidentDetailQuery _retModel =
                new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = 7 };
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(true);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny< NetworkIncidentDetailQueryHandler.DetailQuery> (), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _contextMock = NetworkIncidentContextMockSeed();
            // when
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(_contextMock.Object, _mediatorMock.Object, _applicationMock.Object);
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
            Assert.AreEqual(7, _entity.incident.IncidentId);
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_PermissionsError_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentCreateCommand_Test ...");
            _contextMock = NetworkIncidentContextMockSeed();
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(false);
            // when
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(_contextMock.Object, _mediatorMock.Object, _applicationMock.Object);
            NetworkIncidentCreateCommand _create = new NetworkIncidentCreateCommand();
            Task<NetworkIncidentDetailQuery> _createResults = _handler.Handle(_create, CancellationToken.None);
            // then
            Assert.IsNotNull(_createResults.Exception);
            Assert.IsTrue(_createResults.Exception.InnerException is NetworkIncidentCreateCommandPermissionsException);
            Console.WriteLine(_createResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentCreateCommand_ValidationError_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentCreateCommand_ValidationError_Test ...");
            _contextMock = NetworkIncidentContextMockSeed();
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(true);
            // when
            NetworkIncidentCreateCommandHandler _handler = new NetworkIncidentCreateCommandHandler(_contextMock.Object, _mediatorMock.Object, _applicationMock.Object);
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
            // then
            Assert.IsNotNull(_createResults.Exception);
            Assert.IsTrue(_createResults.Exception.InnerException is NetworkIncidentCreateCommandValidationException);
            Console.WriteLine(_createResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_Test()
        {
            // given
            Console.WriteLine($"NetworkIncidentUpdateCommand_Test ...");
            long _incidentId = 6;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _contextMock = NetworkIncidentContextMockSeed();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(true);
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
            // when
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                _contextMock.Object, _mediatorMock.Object, _applicationMock.Object, _notificationMock.Object);
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            // then
            Assert.IsNull(_updateResults.Exception);
            NetworkIncidentDetailQuery _entity = _updateResults.Result;
            Assert.AreEqual(_incidentId, _entity.incident.IncidentId);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_PermissionsError_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentUpdateCommand_PermissionsError_Test ...");
            _contextMock = NetworkIncidentContextMockSeed();
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(false);
            // when
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                _contextMock.Object, _mediatorMock.Object, _applicationMock.Object, _notificationMock.Object);
            NetworkIncidentSaveQuery _update = new NetworkIncidentSaveQuery();
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            // then
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandPermissionsException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_ValidationError_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentUpdateCommand_ValidationError_Test ...");
            long _incidentId = 1;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _contextMock = NetworkIncidentContextMockSeed();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(true);
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
            // when
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                _contextMock.Object, _mediatorMock.Object, _applicationMock.Object, _notificationMock.Object);
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            // then
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandValidationException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public void NetworkIncidentUpdateCommand_NotFoundError_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentUpdateCommand_NotFoundError_Test ...");
            long _incidentId = 9;
            NetworkIncidentDetailQuery _retModel = new NetworkIncidentDetailQuery();
            _retModel.incident = new NetworkIncidentData() { IncidentId = _incidentId };
            _contextMock = NetworkIncidentContextMockSeed();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retModel));
            _applicationMock.Setup(x => x.IsEditableRole()).Returns(true);
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
            // when
            NetworkIncidentUpdateCommandHandler _handler = new NetworkIncidentUpdateCommandHandler(
                _contextMock.Object, _mediatorMock.Object, _applicationMock.Object, _notificationMock.Object);
            Task<NetworkIncidentDetailQuery> _updateResults = _handler.Handle(_update, CancellationToken.None);
            // then
            Assert.IsNotNull(_updateResults.Exception);
            Assert.IsTrue(_updateResults.Exception.InnerException is NetworkIncidentUpdateCommandKeyNotFoundException);
            Console.WriteLine(_updateResults.Exception.InnerException.Message);
        }
        //
        [Test]
        public async Task NetworkIncidentDetailQuery_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentDetailQuery_Test ...");
            long _incidentId = 6;
            _contextMock = NetworkIncidentContextMockSeed();
            // when
            NetworkIncidentDetailQueryHandler _handler = new NetworkIncidentDetailQueryHandler(_contextMock.Object);
            NetworkIncidentDetailQueryHandler.DetailQuery _detailQuery =
                new NetworkIncidentDetailQueryHandler.DetailQuery() { IncidentId = _incidentId };
            NetworkIncidentDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            // then
            Assert.AreEqual(_detail.incident.IncidentId, _incidentId);
        }
        //
        [Test]
        public async Task NetworkIncidentCreateQuery_Test()
        {
            // given
            Console.WriteLine("NetworkIncidentCreateQuery_Test ...");
            _contextMock = NetworkIncidentContextMockSeed();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ApplicationUserServerDetailQueryHandler.DetailQuery>(), _cancelToken))
                .Returns(Task.FromResult(new ApplicationUserServerDetailQuery()));
            // when
            NetworkIncidentCreateQueryHandler _handler = new NetworkIncidentCreateQueryHandler(_contextMock.Object, _mediatorMock.Object);
            NetworkIncidentCreateQueryHandler.DetailQuery _detailQuery =
                new NetworkIncidentCreateQueryHandler.DetailQuery() { ServerId = 1, UserName = "Fred" };
            NetworkIncidentDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            // then
            Assert.AreEqual(1, _detail.incident.ServerId);
            Assert.AreEqual(11, _detail.NICs.Count);
            Assert.AreEqual(8, _detail.incidentTypes.Count);
            Assert.AreEqual(5, _detail.noteTypes.Count);
        }
        //
    }
}
