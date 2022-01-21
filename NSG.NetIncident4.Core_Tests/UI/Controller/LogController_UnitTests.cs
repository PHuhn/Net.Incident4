using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MediatR;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.Application.Commands.Logs;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller
{
    [TestFixture]
    public class LogController_UnitTests : UnitTestFixture
    {
        //
        public string userName = "TestUser";
        //
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public async Task Index_Test()
        {
            // given
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
            LogController sut = new LogController(mockMediator.Object);
            sut.ControllerContext = Fixture_ControllerContext(userName, "admin");
            // when
            var actual = await sut.Index();
            // then
            var viewResult = actual as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.ViewData.Model as List<LogListQuery>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }
        //
        [Test]
        public void Index_AuthorizeAttribute_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            // when
            LogController sut = new LogController(mockMediator.Object);
            // then
            Type type = sut.GetType();
            var attribute = type.GetCustomAttribute(typeof(AuthorizeAttribute), true);
            Assert.IsNotNull(attribute, "No AuthorizeAttribute found on LogController");
        }
        //
    }
}