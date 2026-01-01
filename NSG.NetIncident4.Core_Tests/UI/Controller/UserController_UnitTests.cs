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
//
using MockQueryable.Moq;
using MediatR;
using Moq;
//
using NSG.PrimeNG.LazyLoading;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
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
            //DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            //_seeder.Seed().Wait();
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
            Assert.That(actual, Is.Not.Null);
            var viewResult = actual.Result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            var model = viewResult.Model as Pagination<LogListQuery>;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.items.Count, Is.EqualTo(1));
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
            // "No AuthorizeAttribute found on LogController"
            Assert.That(attribute, Is.Not.Null);
        }
        //
        /// <summary>
        /// Get the GovWeather weather forecast for the current
        /// user's company's zipcode
        /// Returns the following:
        ///  * The GovWeatherCurrentWeather class,
        ///  * Location string,
        ///  * List of GovWeather7DayForecast class.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UserController_GovWeather_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<UserManager<ApplicationUser>> _userManager = MockHelpers.GetMockUserManager<ApplicationUser>();
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            _userManager.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
            UserController sut = new UserController(_userManager.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("Phil", "admin", "/Log/", controllerHeaders);
            // when
            ActionResult<GovWeatherCurrentAnd7DayForecast> result = await sut.GovWeather();
            // then
            Assert.That(result, Is.Not.Null);
            var viewResult = result.Result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            var model = viewResult.Model as GovWeatherCurrentAnd7DayForecast;
            Assert.That(model.Current.Location, Is.EqualTo("Ann Arbor, Ann Arbor Municipal Airport, MI"));
            Assert.That(model.Location, Is.EqualTo("Ann Arbor, MI"));
            Assert.That(model.Forecast.Count, Is.GreaterThan(12));
        }
        //
        /// <summary>
        /// AccuWeather is no longer free service
        /// </summary>
        /// <returns></returns>
        public async Task UserController_AccuWeather_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            Mock<UserManager<ApplicationUser>> _userManager = MockHelpers.GetMockUserManager<ApplicationUser>();
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            _userManager.Setup(m => m.Users).Returns(_mockDbSetUsers.Object);
            UserController sut = new UserController(_userManager.Object, mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext("Phil", "admin", "/Log/", controllerHeaders);
            // when
            ActionResult<List<Forecast>> result = await sut.AccuWeather();
            // then
            Assert.That(result, Is.Not.Null);
            var viewResult = result.Result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            var model = viewResult.Model as List<Forecast>;
            Assert.That(model.Count, Is.EqualTo(3));
        }
    }
}
//