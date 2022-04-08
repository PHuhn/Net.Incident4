using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Moq;
//
using NSG.PrimeNG.LazyLoading;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller
{
    [TestFixture]
    public class UserController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        [SetUp]
        public void Setup()
        {
            Fixture_UnitTestSetup();
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
        }
        //
        [Test]
        public async Task UserController_UserLogs_Test()
        {
            // given
            long pagerRows = 4;
            // return values
            LogListQueryHandler.ViewModel results = new LogListQueryHandler.ViewModel()
            {
                LogsList = new List<LogListQuery>() {
                    new LogListQuery { Date = DateTime.Now, Application = "Test", Method = "SomeMethod", Level = "Info", Message = "Some thing happened", Exception = "" }
                }
            };
            // setup IMediator to return the value
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<LogListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(results)
                .Verifiable("Log List was not sent.");
            UserController sut = new UserController(userManager, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext(userName, "admin", "/Log/", controllerHeaders);
            LazyLoadEvent2 event2 = new LazyLoadEvent2() { first = 0, rows = pagerRows };
            // when
            var actual = await sut.UserLogs(event2);
            // then
            Assert.IsNotNull(actual);
            var viewResult = actual.Result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as Pagination<LogListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.items.Count);
        }
        //
        [Test]
        public void UserController_AuthorizeAttribute_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            // when
            UserController sut = new UserController(userManager, mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.GetCustomAttribute(typeof(AuthorizeAttribute), true);
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on LogController");
        }
        //
        [Test]
        public async Task UserController_AccuWeather_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            UserController sut = new UserController(userManager, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("Phil", "admin", "/Log/", controllerHeaders);
            // when
            ActionResult<List<Forecast>> result = await sut.AccuWeather();
            // then
            Assert.IsNotNull(result);
            var viewResult = result.Result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as List<Forecast>;
            Assert.AreEqual(3, model.Count);
        }
    }
}
//