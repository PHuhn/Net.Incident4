﻿// ===========================================================================
// File: NetworkIncidentsController_UnitTests.cs
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
//
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    //
    [TestFixture]
    public class NetworkIncidentsController_UnitTests : UnitTestFixture
    {
        //
        NetworkIncidentsController sut;
        //
        [SetUp]
        public void MySetup()
        {
        }
        //
        [Test]
        public async Task NetworkIncidentsController_GetIncidents_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<NetworkIncidentDetailQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NetworkIncidentDetailQuery())
                .Verifiable("Incident list was not sent.");
            sut = new NetworkIncidentsController(_mediator.Object);
            // when
            ActionResult<NetworkIncidentDetailQuery> _results = await sut.GetIncident(1);
            // then
            Assert.That(_results.Value, Is.InstanceOf<NetworkIncidentDetailQuery>());
            //
        }
        //
        [Test]
        public async Task NetworkNetworkIncidentsController_PutIncident_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<NetworkIncidentSaveQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NetworkIncidentDetailQuery() { message = "" })
                .Verifiable("Incident update was not sent.");
            sut = new NetworkIncidentsController(_mediator.Object);
            // when
            NetworkIncidentDetailQuery _results = await sut.PutIncident(4, new NetworkIncidentSaveQuery());
            // then
            Assert.That(_results, Is.InstanceOf<NetworkIncidentDetailQuery>());
            Assert.That(_results.message, Is.EqualTo(""));
            //
        }
        //
        // EmptyIncident & PostIncident
        #region "Network Incident Create"
        //
        [Test]
        public async Task NetworkIncidentsController_EmptyIncident_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<NetworkIncidentCreateQueryHandler.DetailQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NetworkIncidentDetailQuery())
                .Verifiable("Incident empty was not sent.");
            sut = new NetworkIncidentsController(_mediator.Object);
            // when
            ActionResult<NetworkIncidentDetailQuery> _results = await sut.EmptyIncident("empty",1); // Server id
            // then
            Assert.That(_results.Value, Is.InstanceOf<NetworkIncidentDetailQuery>());
            //
        }
        //
        [Test]
        public async Task NetworkIncidentsController_PostIncident_Test()
        {
            // given
            Mock<IMediator> _mediator = new Mock<IMediator>();
            _mediator
                .Setup(m => m.Send(It.IsAny<NetworkIncidentCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NetworkIncidentDetailQuery() { message = "" })
                .Verifiable("Incident empty was not sent.");
            sut = new NetworkIncidentsController(_mediator.Object);
            // when
            NetworkIncidentDetailQuery _results = await sut.PostIncident(new NetworkIncidentSaveQuery()); // Server id
            // then
            Assert.That(_results, Is.InstanceOf<NetworkIncidentDetailQuery>());
            Assert.That(_results.message, Is.EqualTo(""));
            //
        }
        //
        #endregion // Network Incident Create
        //
    }
}
// ===========================================================================
