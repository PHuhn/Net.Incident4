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
using NSG.NetIncident4.Core.Application.Commands.NICs;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class NIC_Commands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        public NIC_Commands_UnitTests()
        {
            Console.WriteLine("NIC_Commands_UnitTests");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
        }
        //
        [Test]
        public void NICCreateCommand_Test()
        {
            // given
            var _mockDbSet = NSG_Helpers.nicsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<NIC>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            NICCreateCommandHandler _handler = new NICCreateCommandHandler(_contextMock.Object);
            NICCreateCommand _create = new NICCreateCommand()
            {
                NIC_Id = "NIC_Id",
                NICDescription = "NICDescription",
                NICAbuseEmailAddress = "NICAbuseEmailAddress",
                NICRestService = "NICRestService",
                NICWebSite = "NICWebSite",
            };
            Task<NIC> _createResults = _handler.Handle(_create, CancellationToken.None);
            NIC _entity = _createResults.Result;
            Assert.That(_entity.NIC_Id, Is.EqualTo("NIC_Id"));
        }
        //
        [Test]
        public void NICUpdateCommand_Test()
        {
            NICUpdateCommandHandler _handler = new NICUpdateCommandHandler(_contextMock.Object);
            NICUpdateCommand _update = new NICUpdateCommand()
            {
                NIC_Id = "other",
                NICDescription = "NICDescription",
                NICAbuseEmailAddress = "NICAbuseEmailAddress",
                NICRestService = "NICRestService",
                NICWebSite = "NICWebSite",
            };
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public void NICDeleteCommand_Test()
        {
            // given
            Console.WriteLine("NICDeleteCommand_Test ...");
            string _nicId = "other";
            NIC? _return = NSG_Helpers.nicsFakeData.Find(n => n.NIC_Id == _nicId);
            var _mockDbSet = new List<NIC>() { _return }.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSet.Object);
            _mediatorMock = new Mock<IMediator>();
            // when
            NICDeleteCommandHandler _handler = new NICDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
            NICDeleteCommand _delete = new NICDeleteCommand()
            {
                NIC_Id = _nicId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, _cancelToken);
            int _count = _deleteResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public async Task NICDetailQuery_Test()
        {
            // given
            Console.WriteLine("NICDetailQuery_Test ...");
            string _nicId = "unknown";
            NIC? _return = NSG_Helpers.nicsFakeData.Find(n => n.NIC_Id == _nicId);
            var _mockDbSet = new List<NIC>() { _return }.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSet.Object);
            _mediatorMock = new Mock<IMediator>();
            // when
            NICDetailQueryHandler _handler = new NICDetailQueryHandler(_contextMock.Object);
            NICDetailQueryHandler.DetailQuery _detailQuery =
                new NICDetailQueryHandler.DetailQuery();
            _detailQuery.NIC_Id = _nicId;
            NICDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            // then
            Assert.That(_detail.NIC_Id, Is.EqualTo(_nicId));
        }
        //
        [Test]
        public void NICListQuery_Test()
        {
            // given
            Console.WriteLine("NICListQuery_Test ...");
            var _mockDbSet = NSG_Helpers.nicsFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSet.Object);
            int displayRows = 5;
            LazyLoadEvent2 event2 = new LazyLoadEvent2() { first = 0, rows = displayRows };
            // when
            NICListQueryHandler _handler = new NICListQueryHandler(_contextMock.Object);
            NICListQueryHandler.ListQuery _listQuery =
                new NICListQueryHandler.ListQuery() { lazyLoadEvent = event2 };
            Task<NICListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            IList<NICListQuery> _list = _viewModelResults.Result.NICsList;
            Assert.That(_list.Count, Is.EqualTo(displayRows));
        }
        //
    }
}
