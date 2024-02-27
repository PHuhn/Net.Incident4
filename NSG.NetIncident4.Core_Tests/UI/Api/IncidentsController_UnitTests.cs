using NUnit.Framework;
using System;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Moq;
using MediatR;
//
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
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
        public void IncidentsController_LazyLoadEvent2_Test()
        {
            // given
            string lazyLoadJSON = "{\"first\":0,\"rows\":5,\"sortOrder\":1,\"filters\":{" +
                "\"NoteTypeDesc\":[{\"value\":\"F\",\"matchMode\":\"startsWith\",\"operator\":\"and\"},{\"value\":\"1\",\"matchMode\":\"contains\",\"operator\":\"and\"}]," +
                "\"NoteTypeShortDesc\":[{\"value\":\"S\",\"matchMode\":\"startsWith\",\"operator\":\"and\"}]},\"globalFilter\":null}";
            // {\"first\":0,\"rows\":5,\"sortOrder\":1,\"filters\":{\"ServerId\":{\"value\":1,\"matchMode\":\"equals\"},\"Mailed\":{\"value\":false,\"matchMode\":\"equals\"},\"Closed\":{\"value\":false,\"matchMode\":\"equals\"},\"Special\":{\"value\":false,\"matchMode\":\"equals\"}},\"globalFilter\":null}
            //// when
            LazyLoadEvent2 loadEvent = JsonConvert.DeserializeObject<LazyLoadEvent2>( lazyLoadJSON );
            // then
            Assert.That(loadEvent, Is.Not.Null);
            //
        }
        //
        [Test]
        public void IncidentsController_LazyLoadEvent_Test()
        {
            // given
            string lazyLoadJSON = "{\"first\":0,\"rows\":5,\"sortOrder\":1,\"filters\":{" +
                "\"ServerId\":{\"value\":1,\"matchMode\":\"equals\"}," +
                "\"Mailed\":{\"value\":false,\"matchMode\":\"equals\"}," +
                "\"Closed\":{\"value\":false,\"matchMode\":\"equals\"}," +
                "\"Special\":{\"value\":false,\"matchMode\":\"equals\"}},\"globalFilter\":null}";
            //// when
            LazyLoadEvent loadEvent = JsonConvert.DeserializeObject<LazyLoadEvent>(lazyLoadJSON);
            // then
            Assert.That(loadEvent, Is.Not.Null );
            //
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
            System.Text.Json.JsonElement lazyEvent = new System.Text.Json.JsonElement();
            using (JsonDocument _docLazy = JsonDocument.Parse("{\"first\":0,\"rows\":5,\"sortOrder\":1}"))
            {
                lazyEvent = _docLazy.RootElement;
                ActionResult<IncidentListQueryHandler.ViewModel> _results = await sut.GetIncidents(lazyEvent);
                // then
                Assert.That(_results.Value, Is.InstanceOf<IncidentListQueryHandler.ViewModel>());
            }
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
            Assert.That(_results, Is.EqualTo(1));
            //
        }
        //
    }
}
