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
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    //
    [TestFixture]
    public class IncidentsController_UnitTests : UnitTestFixture
    {
        //
        IncidentsController sut;
        //
        [SetUp]
        public void MySetup()
        {
        }
        //
        [Test]
        public async Task IncidentsController_GetIncidents_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<IncidentListQueryHandler.ListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new IncidentListQueryHandler.ViewModel())
                .Verifiable("Incident list was not sent.");
            sut = new IncidentsController(_mediator.Object);
            // when
            ActionResult<IncidentListQueryHandler.ViewModel> _results = await sut.GetIncidents("");
            // then
            Assert.IsInstanceOf<IncidentListQueryHandler.ViewModel>(_results.Value);
            //
        }
        //
        [Test]
        public async Task IncidentsController_DeleteIncident_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<IncidentDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable("Incident list was not sent.");
            sut = new IncidentsController(_mediator.Object);
            // when
            int _results = await sut.DeleteIncident(1);
            // then
            Assert.AreEqual(_results, 1);
            //
        }
        //
    }
}
