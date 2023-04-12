using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.ApplicationRoles;
using System.Text;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    // NSG.Integration.Helpers.UnitTestFixture
    [TestFixture]
    public class ApplicationRoleCommands_UnitTests
    {
        //
        Mock<IMediator> _mockMediator = null;
        Mock<IRoleStore<ApplicationRole>> _roleStoreMock = null;
        //
        static CancellationToken _cancelToken = CancellationToken.None; 
        string _testName;
        //
        public ApplicationRoleCommands_UnitTests()
        {
            _testName = "ApplicationRoleCommands_UnitTests";
            Console.WriteLine($"{_testName} ...");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
            //
            _mockMediator = new Mock<IMediator>();
            //
        }
        //
        // You will need to check that the indexes work with you test data.
        //
        [Test]
        public void ApplicationRoleCreateCommand_Test()
        {
            _testName = "ApplicationRoleCreateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            // var _duplicate = await _roleManager.FindByIdAsync(request.Id);
            ApplicationRole? _role = null;
            var _findResult = _roleStoreMock
                .Setup(r => r.FindByIdAsync(It.IsAny<string>(), _cancelToken))
                .Returns(Task.FromResult<ApplicationRole>(_role));
            // var _roleresult = await _roleManager.CreateAsync(_entity);
            Mock<RoleManager<ApplicationRole>> _roleManagerMock =
                MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            var _createResult = _roleManagerMock
                .Setup(r => r.CreateAsync(It.IsAny<ApplicationRole>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            ApplicationRoleCreateCommandHandler _handler = new ApplicationRoleCreateCommandHandler(_roleManagerMock.Object, _mockMediator.Object);
            ApplicationRoleCreateCommand _create = new ApplicationRoleCreateCommand()
            {
                Id = "Id",
                Name = "Name",
            };
            Task<ApplicationRole> _createResults = _handler.Handle(_create, CancellationToken.None);
            ApplicationRole _entity = _createResults.Result;
            Assert.AreEqual("Id", _entity.Id);
        }
        //
        [Test]
        public void ApplicationRoleUpdateCommand_Test()
        {
            _testName = "ApplicationRoleUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            ApplicationRole _entity = new ApplicationRole()
            {
                Id = "pub",
                Name = "Name"
            };
            // await _roleManager.UpdateAsync(_entity);
            var _updateResult = _roleStoreMock
                .Setup(r => r.UpdateAsync(It.IsAny<ApplicationRole>(), _cancelToken))
                .Returns(Task.FromResult(IdentityResult.Success));
            Mock<RoleManager<ApplicationRole>> _roleManagerMock =
                MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            // Setup the Roles property to return a list of roles
            var _roles = new List<ApplicationRole>() { _entity, new ApplicationRole { Id = "Test", Name = "User" } }
                    .AsQueryable().BuildMock();
            _roleManagerMock.Setup(x => x.Roles).Returns(_roles);
            ApplicationRoleUpdateCommandHandler _handler = new ApplicationRoleUpdateCommandHandler(_roleManagerMock.Object, _mockMediator.Object);
            ApplicationRoleUpdateCommand _update = new ApplicationRoleUpdateCommand()
            {
                Id = _entity.Id,
                Name = _entity.Name
            };
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            int _count = _updateResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void ApplicationRoleDeleteCommand_Test()
        {
            _testName = "ApplicationRoleDeleteCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            ApplicationRole _entity = new ApplicationRole()
            {
                Id = "tst",
                Name = "Name"
            };
            // await _roleManager.UpdateAsync(_entity);
            var _updateResult = _roleStoreMock
                .Setup(r => r.DeleteAsync(It.IsAny<ApplicationRole>(), _cancelToken))
                .Returns(Task.FromResult(IdentityResult.Success));
            Mock<RoleManager<ApplicationRole>> _roleManagerMock =
                MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            // Setup the Roles property to return a list of roles
            var _roles = new List<ApplicationRole>() { _entity, new ApplicationRole { Id = "Test", Name = "User" } }
                    .AsQueryable().BuildMock();
            _roleManagerMock.Setup(x => x.Roles).Returns(_roles);
            // Now delete what the entity ...
            ApplicationRoleDeleteCommandHandler _handler = new ApplicationRoleDeleteCommandHandler(_roleManagerMock.Object, _mockMediator.Object);
            ApplicationRoleDeleteCommand _delete = new ApplicationRoleDeleteCommand()
            {
                Id = _entity.Id,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            int? _count = _deleteResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void ApplicationRoleDeleteCommand_ActiveUsersException_Test()
        {
            _testName = "ApplicationRoleDeleteCommand_ActiveUsersException_Test";
            Console.WriteLine($"{_testName} ...");
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            ApplicationRole _entity = new ApplicationRole()
            {
                Id = "tst",
                Name = "Name",
                UserRoles = new List<ApplicationUserRole>() { new ApplicationUserRole() }
            };
            // await _roleManager.UpdateAsync(_entity);
            var _updateResult = _roleStoreMock
                .Setup(r => r.DeleteAsync(It.IsAny<ApplicationRole>(), _cancelToken))
                .Returns(Task.FromResult(IdentityResult.Success));
            Mock<RoleManager<ApplicationRole>> _roleManagerMock =
                MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            // Setup the Roles property to return a list of roles
            var _roles = new List<ApplicationRole>() { _entity, new ApplicationRole { Id = "Test", Name = "User" } }
                    .AsQueryable().BuildMock();
            _roleManagerMock.Setup(x => x.Roles).Returns(_roles);
            // Now delete what the entity ...
            ApplicationRoleDeleteCommandHandler _handler = new ApplicationRoleDeleteCommandHandler(_roleManagerMock.Object, _mockMediator.Object);
            ApplicationRoleDeleteCommand _delete = new ApplicationRoleDeleteCommand()
            {
                Id = _entity.Id,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            Console.WriteLine(_deleteResults.Status);
            Assert.IsNotNull(_deleteResults.Exception);
            Console.WriteLine(_deleteResults.Exception.Message);
            Console.WriteLine(_deleteResults.Exception.InnerException);
            Assert.IsTrue(_deleteResults.Exception.InnerException is ApplicationRoleDeleteCommandActiveUsersException);
        }
        //
        [Test]
        public async Task ApplicationRoleDetailQuery_Test()
        {
            _testName = "ApplicationRoleDetailQuery_Test";
            Console.WriteLine($"{_testName} ...");
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            // var _entity = await _roleManager.FindByIdAsync(request.Id);
            ApplicationRole? _entity = new ApplicationRole() { Id = "pub", Name = "Public" };
            Mock<RoleManager<ApplicationRole>> _roleManagerMock = MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            var _findResult = _roleManagerMock
                .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationRole>(_entity));
            ApplicationRoleDetailQueryHandler _handler =
                new ApplicationRoleDetailQueryHandler(_roleManagerMock.Object);
            ApplicationRoleDetailQueryHandler.DetailQuery _detailQuery =
                new ApplicationRoleDetailQueryHandler.DetailQuery();
            _detailQuery.Id = "pub";
            ApplicationRoleDetailQuery _detail =
                await _handler.Handle(_detailQuery, CancellationToken.None);
            Assert.AreEqual(_detailQuery.Id, _detail.Id);
        }
        //
        [Test]
        public void ApplicationRoleListQuery_Test()
        {
            _testName = "ApplicationRoleListQuery_Test";
            Console.WriteLine($"{_testName} ...");
            var _roles = new List<ApplicationRole>() {
                    new ApplicationRole() { Id = "pub", Name = "Public" },
                    new ApplicationRole() { Id = "usr", Name = "User" },
                    new ApplicationRole() { Id = "adm", Name = "Admin" },
                    new ApplicationRole() { Id = "cadm", Name = "CompanyAdmin" }
            }.AsQueryable().BuildMock();
            _roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
            Mock<RoleManager<ApplicationRole>> _roleManagerMock = MockHelpers.GetMockRoleManager(_roleStoreMock.Object);
            // Setup the Roles property to return a list of roles
            _roleManagerMock.Setup(x => x.Roles).Returns(_roles);
            ApplicationRoleListQueryHandler _handler = new ApplicationRoleListQueryHandler(_roleManagerMock.Object);
            ApplicationRoleListQueryHandler.ListQuery _listQuery =
                new ApplicationRoleListQueryHandler.ListQuery();
            Task<ApplicationRoleListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            IList<ApplicationRoleListQuery> _list = _viewModelResults.Result.ApplicationRolesList;
            Assert.AreEqual(4, _list.Count);
        }
        //
    }
    //
}
