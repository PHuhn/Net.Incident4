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
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
using NSG.NetIncident4.Core.Application.Infrastructure;
using System.Linq;
using NSG.NetIncident4.Core.Persistence;
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    // NSG.Integration.Helpers.UnitTestFixture
    [TestFixture]
    public class ApplicationUserCommands_UnitTests
    {
        //
        Mock<IMediator> _mockGetCompaniesMediator = null;
        Mock<IUserStore<ApplicationUser>> _userStoreMock = null;
        static Company _company1 = new Company() { CompanyId = 1, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
            Address = "123 Any St.", City = "Ann Arbor", State = "MI", PostalCode = "48104", Country = "USA", PhoneNumber = "(734)662-1688", Notes = "Nothing of note." };
        static Server _server1 = new Server() { ServerId = 1, CompanyId = 1, ServerShortName = "NSG Memb", ServerName = "Members Web-site", ServerDescription = "Public facing members Web-site",
            WebSite = "Web-site address: www.mimilk.com", ServerLocation = "We are in Michigan, USA.", FromName = "Phil Huhn", FromNicName = "Phil", FromEmailAddress = "PhilHuhn@yahoo.com",
            TimeZone = "EST (UTC-5)", DST = true, TimeZone_DST = "EDT (UTC-4)", DST_Start = new DateTime(2019, 3, 10, 2, 0, 0), DST_End = new DateTime(2019, 11, 3, 2, 0, 0), Company = _company1 };
        static ApplicationUser _user1 = NSG_Helpers.user1;
        static ApplicationUser _user1withServer =
            new ApplicationUser { Email = NSG_Helpers.User_Email, UserName = NSG_Helpers.User_Name, PasswordHash = "AQAAAAEAACcQAAAAEB4oAR8WhJGi5QVXpuONJ4z69YqF/69GlCz4TtjbQVLA4ef69x0iDq5GLTzvqM2vwQ==", SecurityStamp = "VFV7PXFFMU4VZF57I3T7A6TXVF4VAY2M", ConcurrencyStamp = "24240e95-400c-434e-b498-16542c90de13", CompanyId = 1, FirstName = "Phillip", LastName = "Huhn", FullName = "Phillip Huhn", UserNicName = NSG_Helpers.User_Name, EmailConfirmed = true, Company = _company1 };
        static ApplicationUser _user2 = NSG_Helpers.user2;
        static List<int> _companyList = null;
        static CancellationToken _cancelToken = CancellationToken.None;
        string _sutName = "ApplicationUserCommands";
        string _testName;
        //
        public ApplicationUserCommands_UnitTests()
        {
            Console.WriteLine("ApplicationUserCommands_UnitTests");

            _user1.UserServers = new List<ApplicationUserServer>() {
                new ApplicationUserServer()
                { Id = _user1.Id, User = _user1,
                  ServerId = _server1.ServerId, Server = _server1 }
            };
            _user1.Company = NSG_Helpers.company1;
            _user1withServer.UserServers = new List<ApplicationUserServer>() {
                new ApplicationUserServer()
                { Id = _user1withServer.Id, User = _user1withServer,
                  ServerId = _server1.ServerId, Server = _server1 }
            };
            _user2.UserServers = new List<ApplicationUserServer>() {
                new ApplicationUserServer()
                { Id = _user2.Id, User = _user2,
                  ServerId = _server1.ServerId, Server = _server1 }
            };
            _user2.Company = NSG_Helpers.company1;
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine($"Setup for {_sutName}");
            //
            // Fixture_UnitTestSetup();
            //
            _mockGetCompaniesMediator = new Mock<IMediator>();
            //
            _companyList = new List<int>() { 1 };
            GetUserCompanyListQueryHandler.ViewModel _retViewModel =
                new GetUserCompanyListQueryHandler.ViewModel() { CompanyList = _companyList };
            _mockGetCompaniesMediator.Setup(x => x.Send(
                It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
                .Returns(Task.FromResult(_retViewModel));
            List<ApplicationUser> _usersList = new List<ApplicationUser>()
            {
                new ApplicationUser { Email = NSG_Helpers.User_Email, UserName = NSG_Helpers.User_Name, PasswordHash = "AQAAAAEAACcQAAAAEB4oAR8WhJGi5QVXpuONJ4z69YqF/69GlCz4TtjbQVLA4ef69x0iDq5GLTzvqM2vwQ==", SecurityStamp = "VFV7PXFFMU4VZF57I3T7A6TXVF4VAY2M", ConcurrencyStamp = "24240e95-400c-434e-b498-16542c90de13", CompanyId = 1, FirstName = "Phillip", LastName = "Huhn", FullName = "Phillip Huhn", UserNicName = NSG_Helpers.User_Name, EmailConfirmed = true },
                new ApplicationUser { Email = NSG_Helpers.User_Email2, UserName = NSG_Helpers.User_Name2, PasswordHash = "AQAAAAEAACcQAAAAEGG4L+8q4FXRLAhrLWuALpnyStwzaYOaVA6LjBUrHHqs3AreDKMm9DnRa3B4PM1DYg==", SecurityStamp = "LTCQ4W2NCVQRESG6ZWMC7IBMWDZNICK7", ConcurrencyStamp = "2dab2144-81e5-4b76-a09c-c3b6c37f0f3b", CompanyId = 1, FirstName = "Author", LastName = "Huhn", FullName = "Author Huhn", UserNicName = "Art" },
            };
            //
        }
        //
        [Test]
        public void ApplicationUserUpdateCommand_Test()
        {
            _testName = "ApplicationUserUpdateCommand_Test";
            Console.WriteLine($"{_testName} ...");
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            ApplicationUser _entity = _user2;
            var _updateResult = _userStoreMock
                .Setup(r => r.UpdateAsync(It.IsAny<ApplicationUser>(), _cancelToken))
                .Returns(Task.FromResult(IdentityResult.Success));
            Mock<UserManager<ApplicationUser>> _userManagerMock =
                MockHelpers.GetMockUserManager(_userStoreMock.Object);
            var _mockDbSetUsers = new List<ApplicationUser>() { _user2 }.BuildMock().BuildMockDbSet();
            _userManagerMock.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
            var _mockDbSetRoles = NSG_Helpers.rolesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetUsrRoles = NSG_Helpers.userRolesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetComps = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetSrvrs = NSG_Helpers.serversFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetUsrSrvrs = NSG_Helpers.userSrvrsFakeData.BuildMock().BuildMockDbSet();
            Mock<ApplicationDbContext> _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Users).Returns(_mockDbSetUsers.Object);
            _contextMock.Setup(x => x.Roles).Returns(_mockDbSetRoles.Object);
            _contextMock.Setup(x => x.UserRoles).Returns(_mockDbSetUsrRoles.Object);
            _contextMock.Setup(x => x.Companies).Returns(_mockDbSetComps.Object);
            _contextMock.Setup(x => x.Servers).Returns(_mockDbSetSrvrs.Object);
            _contextMock.Setup(x => x.UserServers).Returns(_mockDbSetUsrSrvrs.Object);
            //
            ApplicationUserUpdateCommandHandler _handler = new ApplicationUserUpdateCommandHandler(_contextMock.Object, _userManagerMock.Object, _mockGetCompaniesMediator.Object);
            ApplicationUser _author = _user2;
            ApplicationUserUpdateCommand _update = new ApplicationUserUpdateCommand()
            {
                Id = _author.Id,
                UserName = _author.UserName,
                Email = _author.Email,
                PhoneNumber = _author.PhoneNumber,
                CompanyId = _author.CompanyId,
                FirstName = "FirstName",
                FullName = "FullName",
                LastName = "LastName",
                UserNicName = "UserNicName",
                //
                SelectedRoles = new string[] { "usr" },
                SelectedServers = new int[] { 1 },
                //
            };
            Task<ApplicationUser> _updateResults = _handler.Handle(_update, CancellationToken.None);
            ApplicationUser user = _updateResults.Result;
            Assert.That(_author.UserName, Is.EqualTo(user.UserName));
        }
        //
        [Test]
        public void ApplicationUserDeleteCommand_Test()
        {
            // given
            Console.WriteLine("ApplicationUserDeleteCommand_Test ...");
            // Add a row to be deleted.
            ApplicationUser _create = new ApplicationUser()
            {
                Id = "Id",
                UserName = "UserName",
                NormalizedUserName = "NormalizedUserName",
                Email = "Email",
                NormalizedEmail = "NormalizedEmail",
                EmailConfirmed = false,
                PasswordHash = "PasswordHash",
                SecurityStamp = "SecurityStamp",
                ConcurrencyStamp = "ConcurrencyStamp",
                PhoneNumber = "PhoneNumber",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = DateTime.Now,
                LockoutEnabled = false,
                AccessFailedCount = 1,
                CompanyId = 1,
                CreateDate = DateTime.Now,
                FirstName = "FirstName",
                FullName = "FullName",
                LastName = "LastName",
                UserNicName = "UserNicName",
                UserRoles = new List<ApplicationUserRole>()
                {
                    new ApplicationUserRole() { UserId = "Id", RoleId = "usr", Role = NSG_Helpers.usrRole }
                },
                Company = NSG_Helpers.company1
            };
            var _users = new List<ApplicationUser>() { _create }.BuildMock().BuildMockDbSet();
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users.Object);
            var _mockDbSetUsrSrvrs = new List<ApplicationUserServer>().BuildMock().BuildMockDbSet();
            Mock<ApplicationDbContext> _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Users).Returns(_users.Object);
            _contextMock.Setup(x => x.UserServers).Returns(_mockDbSetUsrSrvrs.Object);
            _ = _userManagerMock
                .Setup(r => r.DeleteAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            // when
            ApplicationUserDeleteCommandHandler _handler = new ApplicationUserDeleteCommandHandler(_contextMock.Object, _userManagerMock.Object, _mockGetCompaniesMediator.Object);
            ApplicationUserDeleteCommand _delete = new ApplicationUserDeleteCommand()
            {
                UserName = _create.UserName,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            // then
            int _count = _deleteResults.Result;
            Assert.That(_count, Is.EqualTo(1));
        }
        //
        [Test]
        public async Task ApplicationUserDetailQuery_Test()
        {
            _testName = "ApplicationUserDetailQuery_Test";
            Console.WriteLine($"{_testName} ...");
            // given
            var _users = new List<ApplicationUser>()
                { _user1, _user2 }.AsQueryable().BuildMock();
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users);
            // when
            ApplicationUserDetailQueryHandler.DetailQuery _detailQuery =
                new ApplicationUserDetailQueryHandler.DetailQuery() { UserName = NSG_Helpers.User_Name };
            ApplicationUserDetailQueryHandler _handler = new ApplicationUserDetailQueryHandler(_userManagerMock.Object, _mockGetCompaniesMediator.Object);
            ApplicationUserDetailQuery _detail =
                await _handler.Handle(_detailQuery, _cancelToken);
            // then
            Assert.That(_detailQuery.UserName, Is.EqualTo(_detail.UserName));
        }
        //
        [Test]
        public async Task ApplicationUserServerDetailQuery_Test()
        {
            _testName = "ApplicationUserServerDetailQuery_Test";
            Console.WriteLine($"{_testName} ...");
            //includes Company, UserRoles, UserServers
            var _users = new List<ApplicationUser>()
                { _user1withServer, _user2 }.AsQueryable().BuildMock();
            var _userRoles = new List<string>() { "adm" };
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users);
            var _findResult = _userManagerMock
                .Setup(r => r.GetRolesAsync(_user1withServer))
                .Returns(Task.FromResult<IList<string>>(_userRoles));
            ApplicationUserServerDetailQueryHandler.DetailQuery _detailQuery =
                new ApplicationUserServerDetailQueryHandler.DetailQuery() { UserName = _user1withServer.UserName, ServerShortName = "NSG Memb" };
            ApplicationUserServerDetailQueryHandler _handler = new ApplicationUserServerDetailQueryHandler(_userManagerMock.Object);
            ApplicationUserServerDetailQuery _detail =
                await _handler.Handle(_detailQuery, _cancelToken);
            Assert.That(_detailQuery.UserName, Is.EqualTo(_detail.UserName));
            Assert.That(_detailQuery.ServerShortName, Is.EqualTo(_detail.ServerShortName));
            Assert.That(_detail.ServerShortNames.Length, Is.EqualTo(1));
        }
        //
        [Test]
        public async Task ApplicationUserServerDetailQuery_EmptyShortName_Test()
        {
            _testName = "ApplicationUserServerDetailQuery_EmptyShortName_Test";
            Console.WriteLine($"{_testName} ...");
            // given
            //includes Company, UserRoles, UserServers
            var _users = new List<ApplicationUser>()
                { _user1withServer, _user2 }.AsQueryable().BuildMock();
            var _userRoles = new List<string>() { "adm" };
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users);
            var _findResult = _userManagerMock
                .Setup(r => r.GetRolesAsync(_user1withServer))
                .Returns(Task.FromResult<IList<string>>(_userRoles));
            ApplicationUserServerDetailQueryHandler.DetailQuery _detailQuery =
                new ApplicationUserServerDetailQueryHandler.DetailQuery() { UserName = NSG_Helpers.User_Name, ServerShortName = "" };
            // when
            ApplicationUserServerDetailQueryHandler _handler = new ApplicationUserServerDetailQueryHandler(_userManagerMock.Object);
            ApplicationUserServerDetailQuery _detail =
                await _handler.Handle(_detailQuery, _cancelToken);
            // then
            Assert.That(_detailQuery.UserName, Is.EqualTo(_detail.UserName));
            Assert.That(_detailQuery.ServerShortName, Is.EqualTo(_detail.ServerShortName));
            Assert.That(_detail.ServerShortNames.Length, Is.EqualTo(1));
        }
        //
        [Test]
        public async Task ApplicationUserServerDetailQuery_BadShortName_Test()
        {
            _testName = "ApplicationUserServerDetailQuery_BadShortName_Test";
            Console.WriteLine($"{_testName} ...");
            // given
            //includes Company, UserRoles, UserServers
            var _users = new List<ApplicationUser>()
                { _user1withServer, _user2 }.AsQueryable().BuildMock();
            var _userRoles = new List<string>() { "adm" };
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users);
            var _findResult = _userManagerMock
                .Setup(r => r.GetRolesAsync(_user1withServer))
                .Returns(Task.FromResult<IList<string>>(_userRoles));
            ApplicationUserServerDetailQueryHandler.DetailQuery _detailQuery =
                new ApplicationUserServerDetailQueryHandler.DetailQuery() { UserName = NSG_Helpers.User_Name, ServerShortName = "xxx" };
            // when
            ApplicationUserServerDetailQueryHandler _handler = new ApplicationUserServerDetailQueryHandler(_userManagerMock.Object);
            ApplicationUserServerDetailQuery _detail =
                await _handler.Handle(_detailQuery, _cancelToken);
            // then
            Console.WriteLine($"UserName: {_detailQuery.UserName}, {_detail.UserName}");
            Console.WriteLine($"ServerShortName: {_detail.ServerShortName}");
            Assert.That(_detailQuery.UserName, Is.EqualTo(_detail.UserName));
            Assert.That(_detail.ServerShortName, Is.EqualTo(""));
            Assert.That(_detail.ServerShortNames.Length, Is.EqualTo(1));
        }
        //
        [Test]
        public void ApplicationUserListQuery_Test()
        {
            _testName = "ApplicationUserListQuery_Test";
            Console.WriteLine($"{_testName} ...");
            // given
            var _users = new List<ApplicationUser>()
                { _user1, _user2 }.AsQueryable().BuildMock();
            _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> _userManagerMock = MockHelpers.GetMockUserManager(_userStoreMock.Object);
            _userManagerMock.Setup(x => x.Users).Returns(_users);
            LazyLoadEvent2 event2 = new LazyLoadEvent2() { first = 0, rows = 10 };
            // when
            ApplicationUserListQueryHandler _handler = new ApplicationUserListQueryHandler(_userManagerMock.Object, _mockGetCompaniesMediator.Object);
            ApplicationUserListQueryHandler.ListQuery _listQuery =
                new ApplicationUserListQueryHandler.ListQuery() { lazyLoadEvent = event2 };
            Task<ApplicationUserListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            // then
            IList<ApplicationUserListQuery> _list = _viewModelResults.Result.ApplicationUsersList;
            Assert.That(_list.Count, Is.EqualTo(2));
        }
        //
    }
    //
}
