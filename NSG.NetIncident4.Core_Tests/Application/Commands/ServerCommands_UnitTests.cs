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
using NSG.NetIncident4.Core.Application.Commands.Servers;
using NSG.NetIncident4.Core.Application.Infrastructure;
using NSG.NetIncident4.Core.Persistence;
using MockQueryable.Moq;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class ServerCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        public ServerCommands_UnitTests()
        {
            Console.WriteLine("ServerCommands_UnitTests");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
        }
        //
        public Mock<IMediator> GetUserCompanyListMock()
        {
            GetUserCompanyListQueryHandler.ViewModel _retViewModel =
                new GetUserCompanyListQueryHandler.ViewModel() { CompanyList = new List<int>() { 1, 2 } };
            var _mediatorMock = new Mock<IMediator>();
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retViewModel));
            return _mediatorMock;
        }
        //
        [Test]
        public void ServerCreateCommand_Test()
        {
            // given
            Console.WriteLine("ServerCreateCommand_Test ...");
            var _mockDbSet = NSG_Helpers.serversFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.AddAsync(It.IsAny<Server>(), _cancelToken)).ReturnsAsync(() => null);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Servers).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            _mediatorMock = GetUserCompanyListMock();
            ServerCreateCommand _create = new ServerCreateCommand()
            {
                CompanyId = 1,
                ServerShortName = "ShortName",
                ServerName = "ServerName",
                ServerDescription = "ServerDescription",
                WebSite = "WebSite",
                ServerLocation = "ServerLocation",
                FromName = "FromName",
                FromNicName = "FromNicName",
                FromEmailAddress = "FromEmailAddress",
                TimeZone = "TimeZone",
                DST = false,
                TimeZone_DST = "TimeZone_DST",
                DST_Start = DateTime.Now,
                DST_End = DateTime.Now
            };
            // when
            ServerCreateCommandHandler _handler = new ServerCreateCommandHandler(_contextMock.Object, _mediatorMock.Object);
            Task<Server> _createResults = _handler.Handle(_create, _cancelToken);
            // then
            Server _entity = _createResults.Result;
            Assert.That(_entity.CompanyId, Is.EqualTo(_create.CompanyId));
            Assert.That(_entity.ServerShortName, Is.EqualTo(_create.ServerShortName));
        }
        //
        [Test]
        public void ServerUpdateCommand_Test()
        {
            // given
            Console.WriteLine("ServerUpdateCommand_Test ...");
            _mediatorMock = GetUserCompanyListMock();
            int _serverId = 4;
            Server? _return = NSG_Helpers.serversFakeData.Find(e => e.ServerId == _serverId);
            var _mockDbSet = NSG_Helpers.serversFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_serverId, _cancelToken)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Servers).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            ServerUpdateCommand _update = new ServerUpdateCommand()
            {
                ServerId = 1,
                CompanyId = 1,
                ServerShortName = "SrvShortName",
                ServerName = "Server Long Name",
                ServerDescription = "Server Description",
                WebSite = "WebSite",
                ServerLocation = "ServerLocation",
                FromName = "FromName",
                FromNicName = "FromNicName",
                FromEmailAddress = "FromEmailAddress",
                TimeZone = "TimeZone",
                DST = false,
                TimeZone_DST = "TimeZone_DST",
                DST_Start = new DateTime(2019, 3, 10, 2, 0, 0),
                DST_End = new DateTime(2019, 11, 3, 2, 0, 0)
            };
            // when
            ServerUpdateCommandHandler _handler = new ServerUpdateCommandHandler(_contextMock.Object, _mediatorMock.Object);
            Task<int> _updateResults = _handler.Handle(_update, _cancelToken);
            int _count = _updateResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public void ServerDeleteCommand_Test()
        {
            // given
            Console.WriteLine("ServerDeleteCommand_Test ...");
            int _serverId = 2;
            Server? _return = NSG_Helpers.serversFakeData.Find(e => e.ServerId == _serverId);
            if( _return != null)
            {
                var _mockDbSet = new List<Server>() { _return }.BuildMock().BuildMockDbSet();
                _mockDbSet.Setup(x => x.FindAsync(_serverId, _cancelToken)).ReturnsAsync(_return);
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.Servers).Returns(_mockDbSet.Object);
                _mediatorMock = GetUserCompanyListMock(); 
                // when
                ServerDeleteCommandHandler _handler = new ServerDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
                ServerDeleteCommand _delete = new ServerDeleteCommand()
                {
                    ServerId = _return.ServerId,
                };
                Task<int> _deleteResults = _handler.Handle(_delete, _cancelToken);
                // then
                int _count = _deleteResults.Result;
                Assert.That(_count, Is.EqualTo(1));
            }
            else
            {
                Assert.Fail($"Server id: {_serverId} not found!");
            }
        }
        //
    }
}
