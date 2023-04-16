using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
//
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
//
using MockQueryable.Moq;
using MediatR;
using Moq;
using FluentValidation.Results;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.UI.Controllers;
using NSG.NetIncident4.Core.UI.ViewModels;
using FluentValidation.AspNetCore;
using System.Data;
using System.IO;
//
namespace NSG.NetIncident4.Core_Tests.UI.Controller
{
    [TestFixture]
    public class BaseController_UnitTests : UnitTestFixture
    {
        //
        public string _testName = "TestUser";
        //
        [SetUp]
        public void Setup()
        {
            // Fixture_UnitTestSetup();
        }
        //  Error
        //  Warning
        //  Success
        //  Information
        //  AddAlertMessage
        //  AddAlertMessage
        //  Base_AddErrors(Exception except)
        //  Base_AddErrors(ValidationResult modelState)
        //  Base_AddErrors(IdentityResult modelState)
        //  Base_AddErrors(ModelStateDictionary modelState)
        //
        #region "alert message"
        //
        [Test]
        public void BaseController_Error_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some error message";
            BaseController sut = new BaseController(mockMediator.Object);
            BaseController.Alerts.Clear();
            // when
            sut.Error(_message);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "error");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        [Test]
        public void BaseController_Warning_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some warning message";
            BaseController sut = new BaseController(mockMediator.Object);
            BaseController.Alerts.Clear();
            // when
            sut.Warning(_message);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "warn");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        [Test]
        public void BaseController_Success_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some success message";
            BaseController sut = new BaseController(mockMediator.Object);
            BaseController.Alerts.Clear();
            // when
            sut.Success(_message);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "success");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        [Test]
        public void BaseController_Information_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some information message";
            BaseController sut = new BaseController(mockMediator.Object);
            BaseController.Alerts.Clear();
            // when
            sut.Information(_message);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "info");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        [Test]
        public void BaseController_AddErrors_Exception_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some add errors exception message";
            BaseController sut = new BaseController(mockMediator.Object);
            BaseController.Alerts.Clear();
            // when
            sut.Base_AddErrors(new Exception(_message));
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "error");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        [Test]
        public void BaseController_AddErrors_ValidationResult_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _propName = "SomeField";
            string _message = "Some add errors validation result message";
            BaseController sut = new BaseController(mockMediator.Object);
            FluentValidation.Results.ValidationResult _modelState = 
                new FluentValidation.Results.ValidationResult(
                    new List<ValidationFailure>()
                        { new ValidationFailure(_propName, _message) }
                    );
            BaseController.Alerts.Clear();
            // when
            sut.Base_AddErrors(_modelState);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "SomeField");
            Assert.AreEqual(_alertMessage.Level, "error");
            Assert.AreEqual(_alertMessage.Message, $"{_propName}: {_message}\n");
        }
        //
        [Test]
        public void BaseController_AddErrors_IdentityResult_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _code = "nsg001";
            string _message = "Some add errors identity result message";
            BaseController sut = new BaseController(mockMediator.Object);
            var _results = IdentityResult.Failed( new IdentityError() { Code = _code, Description = _message } );
            BaseController.Alerts.Clear();
            // when
            sut.Base_AddErrors(_results);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "error");
            Assert.AreEqual(_alertMessage.Message, $"{_code}: {_message}\n");
        }
        //  Base_AddErrors(ModelStateDictionary modelState)
        //
        [Test]
        public void BaseController_AddErrors_ModelStateDictionary_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            string _message = "Some add errors identity result message";
            BaseController sut = new BaseController(mockMediator.Object);
            var _modelState = new ModelStateDictionary();
            _modelState.TryAddModelException("nsg001", new Exception(_message));
            BaseController.Alerts.Clear();
            // when
            sut.Base_AddErrors(_modelState);
            // then
            Assert.AreEqual(BaseController.Alerts.Count, 1);
            AlertMessage _alertMessage = BaseController.Alerts.FirstOrDefault();
            Assert.IsNotNull(_alertMessage);
            Assert.AreEqual(_alertMessage.Id, "001");
            Assert.AreEqual(_alertMessage.Level, "error");
            Assert.AreEqual(_alertMessage.Message, _message);
        }
        //
        #endregion // alert message
        //
        [Test]
        public void BaseController_GetUserAccount_Auth_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            BaseController sut = new BaseController(mockMediator.Object)
            {
                ControllerContext = Fixture_ControllerContext(_testName, "pub", "", new Dictionary<string, string>())
            };
            //public ControllerContext Fixture_ControllerContext(string userName, string role, string path, Dictionary<string, string> headers)
            // when
            string _userName = sut.Base_GetUserAccount();
            // then
            Assert.AreEqual(_userName, _testName);
        }
        //
        [Test]
        public void BaseController_GetUserAccount_UnAuth_Test()
        {
            // given
            Mock<IMediator> mockMediator = new Mock<IMediator>();
            BaseController sut = new BaseController(mockMediator.Object);
            // when
            string _userName = sut.Base_GetUserAccount();
            // then
            Assert.AreEqual(_userName, "- Not Authenticated -");
        }
        //
    }
}
//