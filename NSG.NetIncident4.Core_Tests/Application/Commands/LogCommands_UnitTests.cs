using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class LogCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        static Mock<IApplication> _mockApplication = null;
        static string _userName;
        static string _appName;
        static DateTime _appNow;
        //
        public LogCommands_UnitTests()
        {
            Console.WriteLine("LogCommands_UnitTests ...");
            _userName = NSG_Helpers.User_Name;
            _appName = "The Application!";
            _appNow = new DateTime(2023, 3, 30, 10, 30, 45);
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
            //
            _mockApplication = new Mock<IApplication>();
            _userName = "Phil";
            //
            LogData _log1 = new LogData
            {
                Date = DateTime.Now, Application = "This application", Method = "The method",
                LogLevel = (byte)LoggingLevel.Info, Level = Enum.GetName(LoggingLevel.Info.GetType(), LoggingLevel.Info),
                UserAccount = _userName, Message = "Information Message",
                Exception = ""
            };
            LogData _log2 = new LogData
            {
                Date = DateTime.Now, Application = "This application", Method = "The method",
                LogLevel = (byte)LoggingLevel.Error, Level = Enum.GetName(LoggingLevel.Error.GetType(), LoggingLevel.Info),
                UserAccount = _userName, Message = "Information Message",
                Exception = ""
            };
        }

        public Mock<IApplication> MockApplication(string userName, string appName, DateTime appNow)
        {
            Mock<IApplication> _application = new Mock<IApplication>();
            _application.Setup(x => x.Now()).Returns(appNow);
            _application.Setup(x => x.GetApplicationName()).Returns(appName);
            _application.Setup(x => x.GetUserAccount()).Returns(userName);
            return _application;
        }

        //
        [Test]
        public async Task LogCreateCommand_Simple01_TestAsync()
        {
            // given
            string _message = "Message";
            MethodBase _method = MethodBase.GetCurrentMethod();
            string _expectedMethod = "NSG.NetIncident4.Core_Tests.Application.Commands.LogCommands_UnitTests";
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommand _create = new LogCreateCommand(
                LoggingLevel.Warning, _method, _message, null);
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogData _entity = await _handler.Handle(_create, _cancelToken);
            // then
            Console.WriteLine(_entity.LogToString());
            // Assert.That(5, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo((byte)2));
            Assert.That(_entity.Level, Is.EqualTo("Warning"));
            Assert.That(_entity.Method.Substring(0, _expectedMethod.Length), Is.EqualTo(_expectedMethod));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo(""));
        }
        //
        [Test]
        public async Task LogCreateCommand_Simple02_Test()
        {
            // given
            string _message = "Message";
            string _method = "MethodBase";
            byte _severity = 2;
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogCreateCommand _create = new LogCreateCommand(
                _severity, _method, _message, null);
            LogData _entity = await _handler.Handle(_create, _cancelToken);
            // then
            Console.WriteLine(_entity.LogToString());
            // Assert.That(3, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo(_severity));
            Assert.That(_entity.Level, Is.EqualTo("Warning"));
            Assert.That(_entity.Method, Is.EqualTo(_method));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo(""));
        }
        //
        [Test]
        public async Task LogCreateCommand_Error01_TestAsync()
        {
            string _message = "Message";
            MethodBase _method = MethodBase.GetCurrentMethod();
            Exception _exception = new Exception("Test exception");
            string _expectedMethod = "NSG.NetIncident4.Core_Tests.Application.Commands.LogCommands_UnitTests";
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogCreateCommand _create = new LogCreateCommand(
                LoggingLevel.Error, _method, _message, _exception);
            LogData _entity = await _handler.Handle(_create, CancellationToken.None);
            Console.WriteLine(_entity.LogToString());
            // Assert.AreEqual(3, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo((byte)1));
            Assert.That(_entity.Level, Is.EqualTo("Error"));
            Assert.That(_entity.Method.Substring(0, _expectedMethod.Length), Is.EqualTo(_expectedMethod));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo("System.Exception: Test exception"));
        }
        //
        [Test]
        public async Task LogCreateCommand_Error02_Test()
        {
            string _message = "Message";
            string _method = "MethodBase";
            byte _severity = 1;
            string _exception = "System.Exception: Test exception";
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogCreateCommand _create = new LogCreateCommand(
                _severity, _method, _message, _exception);
            LogData _entity = await _handler.Handle(_create, CancellationToken.None);
            Console.WriteLine(_entity.LogToString());
            // Assert.That(3, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo(_severity));
            Assert.That(_entity.Level, Is.EqualTo("Error"));
            Assert.That(_entity.Method, Is.EqualTo(_method));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo(_exception));
        }
        //
        [Test]
        public async Task LogCreateCommand_Audit_Test()
        {
            string _message = "Message";
            string _method = "MethodBase";
            byte _severity = 0;
            string _exception = "System.Exception: Test exception";
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogCreateCommand _create = new LogCreateCommand(
                _severity, _method, _message, _exception);
            LogData _entity = await _handler.Handle(_create, CancellationToken.None);
            Console.WriteLine(_entity.LogToString());
            // Assert.AreEqual(3, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo(_severity));
            Assert.That(_entity.Level, Is.EqualTo("Audit"));
            Assert.That(_entity.Method, Is.EqualTo(_method));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo(_exception));
        }
        //
        [Test]
        public async Task LogCreateCommand_IncorrectSeverity_Test()
        {
            // given
            string _message = "Message";
            string _method = "MethodBase";
            byte _severity = 9;
            string _exception = "System.Exception: Test exception";
            _mockApplication = MockApplication(_userName, _appName, _appNow);
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<LogData>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            // when
            LogCreateCommandHandler _handler = new LogCreateCommandHandler(_contextMock.Object, _mockApplication.Object);
            LogCreateCommand _create = new LogCreateCommand(
                _severity, _method, _message, _exception);
            LogData _entity = await _handler.Handle(_create, CancellationToken.None);
            // then
            Console.WriteLine(_entity.LogToString());
            // Assert.AreEqual(3, _entity.Id);
            Assert.That(_entity.LogLevel, Is.EqualTo(_severity));
            Assert.That(_entity.Level, Is.EqualTo("Level-9"));
            Assert.That(_entity.Method, Is.EqualTo(_method));
            Assert.That(_entity.Message, Is.EqualTo(_message));
            Assert.That(_entity.Exception, Is.EqualTo(_exception));
        }
        //
        [Test]
        public void LogListQuery_Test()
        {
            // given
            Console.WriteLine("LogListQuery_Test ...");
            LazyLoadEvent2 _event2 = new LazyLoadEvent2() { rows = 4, first = 0 };
            var _mockDbSet = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSet.Object);
            // when
            LogListQueryHandler _handler = new LogListQueryHandler(_contextMock.Object);
            LogListQueryHandler.ListQuery _listQuery =
                new LogListQueryHandler.ListQuery() { UserAccount = _userName, lazyLoadEvent = _event2 };
            Task<LogListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            // then
            IList<LogListQuery> _list = _viewModelResults.Result.LogsList;
            Assert.That(_list.Count, Is.EqualTo(4));
        }
        //
    }
}
