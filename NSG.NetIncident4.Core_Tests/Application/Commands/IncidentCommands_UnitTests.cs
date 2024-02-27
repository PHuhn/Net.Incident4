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
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class IncidentCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        static Mock<IApplication> _applicationMock = null;
        static Mock<INotificationService> _notificationMock = null;
        UserServerData _user = null;
        //
        public IncidentCommands_UnitTests()
        {
            Console.WriteLine("IncidentCommands_UnitTests ...");
        }
        //
        [SetUp]
        public void SetupAsync()
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
        }
        //
        public void IncidentReport( ApplicationDbContext db_context )
        {
            Console.WriteLine("IncidentReport");
            //
            foreach (Incident _in in db_context.Incidents.Include(_i => _i.IncidentIncidentNotes))
            {
                Console.WriteLine(_in.IncidentId.ToString() + " " + _in.IPAddress);
            }
        }
        //
        [Test]
        public void IncidentDeleteCommand_Test()
        {
            // given
            Console.WriteLine("IncidentDeleteCommand_Test ...");
            long _incidentId = 6;
            long _incidentNoteId = 7;
            Incident _create = new Incident()
            {
                IncidentId = _incidentId,
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
            var _mockDbSet = new List<Incident>() { _create }.BuildMock().BuildMockDbSet();
            IncidentNote _note = new IncidentNote()
            {
                IncidentNoteId = _incidentNoteId,
                NoteTypeId = 3,
                Note = "Note"
            };
            var _mockDbSetNote = new List<IncidentNote>() { _note }.BuildMock().BuildMockDbSet();
            IncidentIncidentNote _incIncNote = new IncidentIncidentNote()
            {
                IncidentId = _incidentId,
                Incident = _create,
                IncidentNoteId = _incidentNoteId,
                IncidentNote = _note
            };
            var _mockDbSetIncNote = new List<IncidentIncidentNote>() { _incIncNote }.BuildMock().BuildMockDbSet();
            NetworkLog _networkLog = new NetworkLog()
            {
                NetworkLogId = 65,
                ServerId = 1,
                // Nullable Reference Types
                Incident = _create,
                IPAddress = "192.168.200.21",
                NetworkLogDate = DateTime.Now,
                Log = "Bad, bad thing are happening",
                IncidentTypeId = 3
            };
            var _mockDbSetLogs = new List<NetworkLog>() { _networkLog }.BuildMock().BuildMockDbSet();
            // _mockDbSet.Setup(x => x.FindAsync(_incidentId, _cancelToken)).ReturnsAsync(_create);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Incidents).Returns(_mockDbSet.Object);
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSetNote.Object);
            _contextMock.Setup(x => x.IncidentIncidentNotes).Returns(_mockDbSetIncNote.Object);
            _contextMock.Setup(x => x.NetworkLogs).Returns(_mockDbSetLogs.Object);
            _mediatorMock = new Mock<IMediator>();
            _applicationMock.Setup(x => x.IsCompanyAdminRole()).Returns(true);
            // when
            IncidentDeleteCommandHandler _handler = new IncidentDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object, _applicationMock.Object);
            IncidentDeleteCommand _delete = new IncidentDeleteCommand()
            {
                IncidentId = _incidentId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            // then
            int _count = _deleteResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public void IncidentListQuery_Test()
        {
            // given
            Console.WriteLine("IncidentListQuery_Test ...");
            var _mockDbSet = NSG_Helpers.incidentsFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Incidents).Returns(_mockDbSet.Object);
            _applicationMock.Setup(x => x.IsAuthenticated()).Returns(true);
            string _jsonString = "{'first':0,'rows':3,'sortOrder':1,'filters':{'ServerId':{'value':1,'matchMode':'equals'},'Mailed':{'value':'false','matchMode':'equals'},'Closed':{'value':'false','matchMode':'equals'},'Special':{'value':'false','matchMode':'equals'}},'globalFilter':null}";
            IncidentReport(_contextMock.Object);
            // when
            IncidentListQueryHandler _handler = new IncidentListQueryHandler(_contextMock.Object, _applicationMock.Object);
            IncidentListQueryHandler.ListQuery _listQuery =
                new IncidentListQueryHandler.ListQuery() { JsonString = _jsonString };
            Task<IncidentListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            // then
            IList<IncidentListQuery> _list = _viewModelResults.Result.results;
            Assert.That(_list.Count, Is.EqualTo(3));
        }
        //
    }
}
