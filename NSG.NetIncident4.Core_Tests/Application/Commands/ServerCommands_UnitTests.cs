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
using NSG.NetIncident4.Core.Application.Commands.Servers;
using NSG.Integration.Helpers;
using Microsoft.Extensions.Options;
using NSG.NetIncident4.Core.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class ServerCommands_UnitTests : UnitTestFixture
    {
        //
        static Mock<IMediator> _mockGetCompaniesMediator = null;
        static CancellationToken _cancelToken = CancellationToken.None;
        string _testName;
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
            //
            Fixture_UnitTestSetup();
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            // set up mock to get list of permissible list of companies
            GetUserCompanyListQueryHandler.ViewModel _retViewModel =
                new GetUserCompanyListQueryHandler.ViewModel() { CompanyList = new List<int>() { 1 } };
            _mockGetCompaniesMediator = new Mock<IMediator>();
            _mockGetCompaniesMediator.Setup(x => x.Send(
                It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retViewModel));
            //
        }
        //
        [Test]
        public void ServerCreateCommand_Test()
        {
            
            ServerCreateCommandHandler _handler = new ServerCreateCommandHandler(db_context, _mockGetCompaniesMediator.Object);
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
            Task<Server> _createResults = _handler.Handle(_create, CancellationToken.None);
            Server _entity = _createResults.Result;
            Assert.AreNotEqual(0, _entity.ServerId);
            Assert.AreEqual(_create.ServerShortName, _entity.ServerShortName);
        }
        [TearDown]
        public void ServerUpdateCommand_TearDown()
        {
            Dispose();
        }
        //
        [Test]
        public void ServerUpdateCommand_Test()
        {
            ServerUpdateCommandHandler _handler = new ServerUpdateCommandHandler(db_context, _mockGetCompaniesMediator.Object);
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
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void ServerDeleteCommand_Test()
        {
            // Add a row to be deleted.
            Server _create = new Server()
            {
                CompanyId = 1,
                ServerShortName = "ServerShortN",
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
                DST_End = DateTime.Now,
            };
            db_context.Servers.Add(_create);
            db_context.SaveChanges();
            // Now delete what was just created ...
            ServerDeleteCommandHandler _handler = new ServerDeleteCommandHandler(db_context, _mockGetCompaniesMediator.Object);
            ServerDeleteCommand _delete = new ServerDeleteCommand()
            {
                ServerId = _create.ServerId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            int _count = _deleteResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
    }
}
