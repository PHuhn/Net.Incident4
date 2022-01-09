using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
//
using Moq;
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.UI.ApiModels;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    [TestFixture]
    public class Authenticate_UnitTests : UnitTestFixture
    {
        //
        AuthenticateController _authenticateController = null;
        //
        [SetUp]
        public void MySetup()
        {
            Console.WriteLine("Setup");
            //
            Fixture_UnitTestSetup();
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            _seeder.Seed().Wait();
            SetupConfiguration();
            _authenticateController = new AuthenticateController(userManager, configuration);
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Good_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = NSG_Helpers.User_Name, Password = NSG_Helpers.Password };
            IActionResult _results = await _authenticateController.Login(_model);
            Assert.IsInstanceOf<OkObjectResult>(_results);
        }
        //
        [Test]
        public async Task AuthenticateController_Login_NotVerified_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = NSG_Helpers.User_Name2, Password = NSG_Helpers.Password2 };
            IActionResult _results = await _authenticateController.Login(_model);
            // Assert.AreEqual(_results.GetType(), new UnauthorizedResult());
            Assert.AreEqual(_results, false);
        }
        //
        [Test]
        public async Task AuthenticateController_Login_Bad_Test()
        {
            // given
            LoginModel _model = new LoginModel() { Username = "NonUser", Password = "NonUserPassword" };
            IActionResult _results = await _authenticateController.Login(_model);
            Assert.IsInstanceOf<UnauthorizedResult>(_results);
        }
        //
    }
}
