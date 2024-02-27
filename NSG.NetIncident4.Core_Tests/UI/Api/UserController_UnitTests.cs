using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using MediatR;
//
using NSG.NetIncident4.Core.UI.Api;
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    //
    [TestFixture]
    public class UserController_UnitTests : UnitTestFixture
    {
        //
        UserController sut;
        //
        [Test]
        public async Task UserController_GetAsync_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<ApplicationUserServerDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApplicationUserServerDetailQuery())
                .Verifiable("User get was not sent.");
            sut = new UserController(_mediator.Object);
            // when
            ApplicationUserServerDetailQuery _results = await sut.GetAsync("TestUser", "Server1");
            // then
            Assert.That(_results, Is.InstanceOf<ApplicationUserServerDetailQuery>());
            //
        }
        //
    }
}
