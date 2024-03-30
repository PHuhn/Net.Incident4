using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Application.Commands.CompanyServers;
using NSG.NetIncident4.Core.Application.Infrastructure;
using MockQueryable.Moq;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
using NSG.PrimeNG.LazyLoading;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
	[TestFixture]
	public class CompanyServer_UnitTests
	{
		//
		static CancellationToken _cancelToken = CancellationToken.None;
		static Mock<IMediator> _mediatorMock = null;
		static Mock<ApplicationDbContext> _contextMock = null;
		//
		public void ClassInitialize()
		{
			Console.WriteLine("CompanyServer_UnitTests");
		}
		//
		[SetUp]
		public void Setup()
		{
			Console.WriteLine("Setup");
			//
			GetUserCompanyListQueryHandler.ViewModel _retViewModel =
				new GetUserCompanyListQueryHandler.ViewModel() { CompanyList = new List<int>() { 1, 2, 3, 4 } };
			_mediatorMock = new Mock<IMediator>();
			_mediatorMock.Setup(x => x.Send(
				It.IsAny<GetUserCompanyListQueryHandler.ListQuery>(), _cancelToken))
				.Returns(Task.FromResult(_retViewModel));
			//
		}
		//
		[Test]
		public void CompanyServerDetailQuery_Test()
		{
			// given
			Console.WriteLine("IncidentTypeDetailQuery_Test ...");
			int _companyId = 1;
			Company? _return = NSG_Helpers.companiesFakeData.Find(e => e.CompanyId == _companyId);
			if (_return != null)
			{
				var _mockDbSetSrvrs = new List<Server>() { NSG_Helpers.server1 }.BuildMock().BuildMockDbSet();
				var _mockDbSet = new List<Company>() { _return }.BuildMock().BuildMockDbSet();
				_contextMock = MockHelpers.GetDbContextMock();
				_contextMock.Setup(x => x.Companies).Returns(_mockDbSet.Object);
				_contextMock.Setup(x => x.Servers).Returns(_mockDbSetSrvrs.Object);
				// when
				CompanyServerDetailQueryHandler _handler = new CompanyServerDetailQueryHandler(_contextMock.Object, _mediatorMock.Object);
				CompanyServerDetailQueryHandler.DetailQuery _detailQuery =
					new CompanyServerDetailQueryHandler.DetailQuery() { CompanyId = 1 };
				_detailQuery.CompanyId = _companyId;
				Task<CompanyServerDetailQuery> _detailResults =
					_handler.Handle(_detailQuery, _cancelToken);
				CompanyServerDetailQuery _detail = _detailResults.Result;
				Assert.That(_detail.CompanyId, Is.EqualTo(_companyId));
			}
		}
		//
		[Test]
		public void CompanyServerListQuery_Test()
		{
			// given
			Console.WriteLine("CompanyServerListQuery_Test");
			var _mockDbSet = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
			_contextMock = MockHelpers.GetDbContextMock();
			_contextMock.Setup(x => x.Companies).Returns(_mockDbSet.Object);
			LazyLoadEvent2 event2 = new LazyLoadEvent2() { first = 0, rows = 10 };
			// when
			CompanyServerListQueryHandler _handler = new CompanyServerListQueryHandler(_contextMock.Object, _mediatorMock.Object);
			CompanyServerListQueryHandler.ListQuery _listQuery =
				new CompanyServerListQueryHandler.ListQuery() { lazyLoadEvent = event2 };
			Task<CompanyServerListQueryHandler.ViewModel> _viewModelResults =
				_handler.Handle(_listQuery, _cancelToken);
			// then
			IList<CompanyServerListQuery> _list = _viewModelResults.Result.CompaniesList;
			Assert.That((_list.Count == 1 || _list.Count == 2));
		}
		//
	}
}
