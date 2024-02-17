using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//
using MockQueryable.Moq;
using Moq;
//
using NSG.NetIncident4.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NSG.NetIncident4.Core.Application.Commands.NoteTypes;
using SendGrid.Helpers.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NSG.Integration.Helpers;
using System.Text;
using NSG.NetIncident4.Core.UI.ViewModels;
// using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests
{
	[TestFixture]
	public class Test_Tests
	{
		ApplicationDbContext _context = null;
		static CancellationToken _cancelToken = CancellationToken.None;
		[SetUp]
		public void Setup()
		{
			Company _company1 = new Company()
			{
				CompanyId = 1,
				CompanyShortName = "NSG",
				CompanyName = "Northern Software Group",
				Address = "123 Any St.",
				City = "Ann Arbor",
				State = "MI",
				PostalCode = "48104",
				Country = "USA",
				PhoneNumber = "(734)662-1688",
				Notes = "Nothing of note."
			};
			//var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			//_context = new ApplicationDbContext(optionsBuilder.Options);
			//_context.Companies.Add(_company1);
			//Console.WriteLine(_context);
		}
		//
		[Test]
		public void AlertMessageNumberFormat_Test()
		{
			Console.WriteLine("NumberFormat_Test");
			int _n1 = 0;
			string _n1Str = _n1.ToString("d3");
			Console.WriteLine(_n1Str);
			Assert.AreEqual(_n1Str, "000");
			int _n2 = 301;
			string _n2Str = _n2.ToString("d3");
			Console.WriteLine(_n2Str);
			Assert.AreEqual(_n2Str, "301");
			List<AlertMessage> Alerts = new List<AlertMessage>()
			{
				new AlertMessage( "001", "Warn", "Message 001"),
				new AlertMessage( "ABC", "Error", "Message ABC")
			};
			string _id = (Alerts.Count + 1).ToString( "d3" );
			Alerts.Add(new AlertMessage(_id, "Info", "Message " + _id));
			Console.WriteLine(_id);
			Assert.AreEqual(_id, "003");
			foreach( AlertMessage _alert in Alerts)
			{
				Console.WriteLine(_alert.ToString());
			}
		}
		//
		[Test]
		public async Task TestingCreate_Test()
		{
			// given
			Console.WriteLine("TestingCreate_Test");
			var _optionsBuilder = new DbContextOptionsBuilder<TestingContext>()
				.UseInMemoryDatabase(databaseName: "Test_" + Guid.NewGuid().ToString());
			var _contextMock = new Mock<TestingContext>(_optionsBuilder.Options);
			var _mockDbSet = new List<Testing>().BuildMock().BuildMockDbSet();
			// _ = _mockDbSet.Setup(x => x.AddAsync(It.IsAny<Testing>(), _cancelToken)).ReturnsAsync(() => null);
			_mockDbSet.DbSetAddAsync<Testing>();
			_contextMock.Setup(x => x.Testings).Returns(_mockDbSet.Object);
			var _saveResult = _contextMock
				.Setup(r => r.SaveChangesAsync(_cancelToken))
				.Returns(Task.FromResult(1));
			Testing _create = new Testing()
			{
				// Id = 0,
				TestingShortDesc = "Short",
				Description = "Long description"
			};
			// when
			TestingCreateCommand _handler = new TestingCreateCommand(_contextMock.Object);
			Task<Testing> _createResults = _handler.Create(_create, _cancelToken);
			// then
			Testing _entity = _createResults.Result;
			Console.WriteLine(_entity);
			Assert.AreEqual(_create.TestingShortDesc, _entity.TestingShortDesc);
		}
		//
		[Test]
		public async Task Test1()
		{
			//ApplicationRole _entity = new ApplicationRole()
			//{
			//	Id = "pub",
			//	Name = "Name"
			//};
			//var _roles = new List<ApplicationRole>() { _entity, new ApplicationRole { Id = "Test", Name = "User" } }
			//		.AsQueryable().BuildMock();
			//Console.WriteLine("Setup");
			//Console.WriteLine(_roles.ToArray());
			//
			Assert.Pass();
		}
		//
	}
	public class Testing
	{
		//
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required(ErrorMessage = "'Id' is required.")]
		public int Id { get; set; }
		[Required(ErrorMessage = "'Short Desc' is required."), MaxLength(8, ErrorMessage = "'Short Desc' must be 8 or less characters.")]
		public string TestingShortDesc { get; set; }
		[Required(ErrorMessage = "'Description' is required."), MaxLength(50, ErrorMessage = "'Description' must be 50 or less characters.")]
		public string Description { get; set; }
		//
		public virtual ICollection<Company> Companies { get; } = new List<Company>();
		// Parameterless constructor
		public Testing()
		{
			Id = 0;
			TestingShortDesc = "";
			Description = "";
		}
		public override string ToString()
		{
			//
			StringBuilder _return = new StringBuilder("record:[");
			_return.AppendFormat("Id: {0}, ", Id);
			_return.AppendFormat("TestingShortDesc: {0}, ", TestingShortDesc);
			_return.AppendFormat("Description: {0}]", Description);
			return _return.ToString();
		}
	}
	//
	public class TestingContext : IdentityDbContext<IdentityUser>
	{
		public TestingContext(DbContextOptions options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
		public virtual DbSet<Testing> Testings { get; set; } = default!;
		public virtual DbSet<Company> Companies { get; set; } = default!;
	}
	public class TestingCreateCommand
	{
		private readonly TestingContext _context;
		//
		public TestingCreateCommand(TestingContext context)
		{
			_context = context;
		}
		public async Task<Testing> Create(Testing request, CancellationToken cancellationToken)
		{
			await _context.Testings.AddAsync(request, cancellationToken);
			try
			{
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException upExc)
			{
				Console.WriteLine(upExc.ToString());
			}
			return request;
		}
	}
}