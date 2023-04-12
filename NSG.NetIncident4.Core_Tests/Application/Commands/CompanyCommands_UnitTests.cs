using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Companies;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Application.Infrastructure;
using NSG.NetIncident4.Core.Persistence;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class CompanyCommands_UnitTests
    {
        //
        // static Mock<IMediator> _mockGetCompaniesMediator = null;
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        //
        //
        public CompanyCommands_UnitTests()
        {
            Console.WriteLine("CompanyCommands_UnitTests");
        }
        //
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup");
            //
        }
        //
        [Test]
        public void CompanyCreateCommand_Test()
        {
            // given
            Console.WriteLine("CompanyCreateCommand_Test ...");
            var _mockDbSet = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<Company>();
            //
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Companies).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            CompanyCreateCommand _create = new CompanyCreateCommand()
            {
                //                  123456789012
                CompanyShortName = "CompanyShort",
                CompanyName = "CompanyName",
                Address = "Address",
                City = "City",
                State = "Stat",
                PostalCode = "55555-4444",
                Country = "USA",
                PhoneNumber = "PhoneNumber",
                Notes = "Notes",
            };
            // when
            CompanyCreateCommandHandler _handler = new CompanyCreateCommandHandler(_contextMock.Object);
            Task<Company> _createResults = _handler.Handle(_create, CancellationToken.None);
            // then
            Company _entity = _createResults.Result;
            Assert.AreEqual(_create.CompanyShortName, _entity.CompanyShortName);
        }
        //
        [Test]
        public void CompanyCreateCommand_ToLong_Test()
        {
            // given
            Console.WriteLine("CompanyCreateCommand_ToLong_Test ...");
            _contextMock = MockHelpers.GetDbContextMock();
            CompanyCreateCommandHandler _handler = new CompanyCreateCommandHandler(_contextMock.Object);
            CompanyCreateCommand _create = new CompanyCreateCommand()
            {
                CompanyShortName = "CompanyShort-tolong",
                CompanyName = "CompanyName",
                Address = "Address",
                City = "City",
                State = "Stat",
                PostalCode = "PostalCode",
                Country = "Country",
                PhoneNumber = "PhoneNumber",
                Notes = "Notes",
            };
            try
            {
                Task<Company> _createResults = _handler.Handle(_create, CancellationToken.None);
            }
            catch (CreateCommandValidationException _ex1)
            {
                Console.WriteLine(_ex1.Message);
            }
            catch (Exception _ex2)
            {
                Console.WriteLine(_ex2.Message);
                Assert.Fail();
            }
        }
        //
        [Test]
        public void CompanyUpdateCommand_Test()
        {
            // given
            Console.WriteLine("CompanyUpdateCommand_Test ...");
            _mediatorMock = new Mock<IMediator>();
            int _companyId = 2;
            Company? _return = NSG_Helpers.companiesFakeData.Find(e => e.CompanyId == _companyId);
            if( _return != null)
            {
                var _mockDbSet = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
                _mockDbSet.Setup(x => x.FindAsync(_companyId, _cancelToken)).ReturnsAsync(_return);
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.Companies).Returns(_mockDbSet.Object);
                var _saveResult = _contextMock
                    .Setup(r => r.SaveChangesAsync(_cancelToken))
                    .Returns(Task.FromResult(1));
                var _companiesViewModel = new GetUserCompanyListQueryHandler.ViewModel()
                {
                    CompanyList = new List<int>() { 1, _companyId }
                };
                Console.WriteLine(_companiesViewModel);
                _mediatorMock.Setup(x => x.Send(
                    It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
                    .Returns(Task.FromResult(_companiesViewModel));
                CompanyUpdateCommand _update = new CompanyUpdateCommand()
                {
                    CompanyId = _companyId,
                    CompanyShortName = "CompanyShort",
                    CompanyName = "CompanyName",
                    Address = "Address",
                    City = "City",
                    State = "Stat",
                    PostalCode = "PostalCode",
                    Country = "Country",
                    PhoneNumber = "PhoneNumber",
                    Notes = "Notes",
                };
                // when
                CompanyUpdateCommandHandler _handler = new CompanyUpdateCommandHandler(_contextMock.Object, _mediatorMock.Object);
                Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
                int _count = _updateResults.Result;
                Assert.AreEqual(1, _count);
            }
            else
            {
                Assert.Fail($"Company: {_companyId} not found");
            }
        }
        //
        [Test]
        public void CompanyDeleteCommand_Test()
        {
            // given
            Console.WriteLine("CompanyDeleteCommand_Test ...");
            int _companyId = 2;
            Company? _return = NSG_Helpers.companiesFakeData.Find(e => e.CompanyId == _companyId);
            if( _return != null)
            {
                var _mockDbSet = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
                // _mockDbSet.Setup(x => x.FindAsync(_companyId, _cancelToken)).ReturnsAsync(_return);
                _contextMock = MockHelpers.GetDbContextMock();
                _contextMock.Setup(x => x.Companies).Returns(_mockDbSet.Object);
                _mediatorMock = new Mock<IMediator>();
                // when
                CompanyDeleteCommandHandler _handler = new CompanyDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
                CompanyDeleteCommand _delete = new CompanyDeleteCommand()
                {
                    CompanyId = _return.CompanyId,
                };
                Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
                // then
                int _count = _deleteResults.Result;
                Assert.AreEqual(1, _count);
            }
            else
            {
                Assert.Fail($"Company: {_companyId} not found");
            }
        }
        //
        //[Test]
        //public void CompanyListQuery_Test()
        //{
        //    CompanyListQueryHandler _handler = new CompanyListQueryHandler(db_context);
        //    CompanyListQueryHandler.ListQuery _listQuery =
        //        new CompanyListQueryHandler.ListQuery();
        //    Task<CompanyListQueryHandler.ViewModel> _viewModelResults =
        //        _handler.Handle(_listQuery, CancellationToken.None);
        //    IList<CompanyServerListQuery> _list = _viewModelResults.Result.CompaniesList;
        //    Assert.IsTrue(_list.Count == 1 || _list.Count == 2);
        //}
        //
        //[Test]
        //public void CompanyDetailQuery_Test()
        //{
        //    CompanyDetailQueryHandler _handler = new CompanyDetailQueryHandler(db_context);
        //    CompanyDetailQueryHandler.DetailQuery _detailQuery =
        //        new CompanyDetailQueryHandler.DetailQuery();
        //    _detailQuery.CompanyId = 1;
        //    Task<CompanyServerDetailQuery> _detailResults =
        //        _handler.Handle(_detailQuery, CancellationToken.None);
        //    CompanyServerDetailQuery _detail = _detailResults.Result;
        //    Assert.AreEqual(1, _detail.CompanyId);
        //}
        //
    }
}
