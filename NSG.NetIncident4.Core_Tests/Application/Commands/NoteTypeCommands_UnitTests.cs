// ===========================================================================
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//
using MockQueryable.Moq;
using Moq;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Application.Commands.NoteTypes;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class NoteTypeCommands_UnitTests
    {
        //
        static CancellationToken _cancelToken = CancellationToken.None;
        static Mock<IMediator> _mediatorMock = null;
        static Mock<ApplicationDbContext> _contextMock = null;
        public NoteTypeCommands_UnitTests()
        {
            Console.WriteLine("NoteTypeCommands_UnitTests ...");
        }
        //
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void NoteTypeCreateCommand_Test()
        {
            // given
            Console.WriteLine("NoteTypeCreateCommand_Test");
            var _mockDbSet = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.Setup(x => x.AddAsync(It.IsAny<NoteType>(), _cancelToken)).ReturnsAsync(() => null);
            // _mockDbSet.DbSetAddAsync<NoteType>();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            NoteTypeCreateCommand _create = new NoteTypeCreateCommand(
                0, "NoteType", "NoteTypeDesc", "");
            // when
            NoteTypeCreateCommandHandler _handler = new NoteTypeCreateCommandHandler(_contextMock.Object);
            Task<NoteType> _createResults = _handler.Handle(_create, CancellationToken.None);
            // then
            NoteType _entity = _createResults.Result;
            Assert.AreEqual(_create.NoteTypeShortDesc, _entity.NoteTypeShortDesc);
        }
        //
        [Test]
        public void NoteTypeUpdateCommand_Test()
        {
            // given
            Console.WriteLine("NoteTypeUpdateCommand_Test");
            int _noteTypeId = 4;
            NoteType? _return = NSG_Helpers.noteTypesFakeData.Find(n => n.NoteTypeId == _noteTypeId);
            var _mockDbSet = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_noteTypeId, _cancelToken)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            NoteTypeUpdateCommand _update = new NoteTypeUpdateCommand()
            {
                NoteTypeId = _noteTypeId,
                NoteTypeShortDesc = "NoteType",
                NoteTypeDesc = "NoteTypeDesc",
                NoteTypeClientScript = ""
            };
            // when
            NoteTypeUpdateCommandHandler _handler = new NoteTypeUpdateCommandHandler(_contextMock.Object);
            Task<int> _updateResults = _handler.Handle(_update, CancellationToken.None);
            // then
            int _count = _updateResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void NoteTypeDeleteCommand_Test()
        {
            // given
            Console.WriteLine("NoteTypeDeleteCommand_Test");
            int _noteTypeId = 5;
            NoteType? _return = NSG_Helpers.noteTypesFakeData.Find(n => n.NoteTypeId == _noteTypeId);
            var _mockDbSet = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_noteTypeId)).ReturnsAsync(_return);
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSet.Object);
            var _saveResult = _contextMock
                .Setup(r => r.SaveChangesAsync(_cancelToken))
                .Returns(Task.FromResult(1));
            _mediatorMock = new Mock<IMediator>();
            // when
            NoteTypeDeleteCommandHandler _handler = new NoteTypeDeleteCommandHandler(_contextMock.Object, _mediatorMock.Object);
            NoteTypeDeleteCommand _delete = new NoteTypeDeleteCommand()
            {
                NoteTypeId = _noteTypeId,
            };
            Task<int> _deleteResults = _handler.Handle(_delete, CancellationToken.None);
            // then
            int _count = _deleteResults.Result;
            Assert.AreEqual(1, _count);
        }
        //
        [Test]
        public void NoteTypeDetailQuery_Test()
        {
            // given
            Console.WriteLine("NoteTypeDetailQuery_Test");
            int _noteTypeId = 1;
            NoteType? _return = NSG_Helpers.noteTypesFakeData.Find(n => n.NoteTypeId == _noteTypeId);
            var _mockDbSet = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _mockDbSet.Setup(x => x.FindAsync(_noteTypeId)).ReturnsAsync(_return);
            //_mockDbSet.Setup(x => x.SingleOrDefaultAsync(
            //    It.IsAny<Func<NoteType, bool>>(), _cancelToken)).ReturnsAsync(
            //    _noteTypeFakeData.Find(n => n.NoteTypeId == 1));
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSet.Object);
            // when
            NoteTypeDetailQueryHandler _handler = new NoteTypeDetailQueryHandler(_contextMock.Object);
            NoteTypeDetailQueryHandler.DetailQuery _detailQuery =
                new NoteTypeDetailQueryHandler.DetailQuery();
            _detailQuery.NoteTypeId = 1;
            Task<NoteTypeDetailQuery> _detailResults =
                _handler.Handle(_detailQuery, CancellationToken.None);
            // then
            NoteTypeDetailQuery _detail = _detailResults.Result;
            Assert.AreEqual(1, _detail.NoteTypeId);
        }
        //
        [Test]
        public void NoteTypeListQuery_Test()
        {
            // given
            Console.WriteLine("NoteTypeListQuery_Test");
            var _mockDbSet = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSet.Object);
            Console.WriteLine(_contextMock.Object.Database.ToString());
            // when
            NoteTypeListQueryHandler _handler = new NoteTypeListQueryHandler(_contextMock.Object);
            NoteTypeListQueryHandler.ListQuery _listQuery =
                new NoteTypeListQueryHandler.ListQuery();
            Task<NoteTypeListQueryHandler.ViewModel> _viewModelResults =
                _handler.Handle(_listQuery, CancellationToken.None);
            // then
            IList<NoteTypeListQuery> _list = _viewModelResults.Result.NoteTypesList;
            Assert.IsTrue(_list.Count > 4);
        }
        //
    }
}
// ===========================================================================
