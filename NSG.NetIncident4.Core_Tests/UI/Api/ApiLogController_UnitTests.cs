using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Moq;
using MediatR;
//
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    //
    [TestFixture]
    public class ApiLogController_UnitTests : UnitTestFixture
    {
        //
        [Test]
        public async Task ApiLogController_PostAsync_Test()
        {
            // given
            Mock<IMediator> _mediator =
                new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<LogCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LogData())
                .Verifiable("Log was not sent.");
            LogController sut = new LogController(_mediator.Object);
            // when
            LogData _results = await sut.PostAsync(0, "method", "message");
            // then
            Assert.IsInstanceOf<LogData>(_results);
            //
        }
    }
}
