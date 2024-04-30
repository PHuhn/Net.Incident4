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
using Microsoft.Extensions.Configuration;
using MimeKit.NSG;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.DependencyInjection;
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
			Assert.That(_n1Str, Is.EqualTo("000"));
			int _n2 = 301;
			string _n2Str = _n2.ToString("d3");
			Console.WriteLine(_n2Str);
			Assert.That(_n2Str, Is.EqualTo("301"));
			List<AlertMessage> Alerts = new List<AlertMessage>()
			{
				new AlertMessage( "001", "Warn", "Message 001"),
				new AlertMessage( "ABC", "Error", "Message ABC")
			};
			string _id = (Alerts.Count + 1).ToString( "d3" );
			Alerts.Add(new AlertMessage(_id, "Info", "Message " + _id));
			Console.WriteLine(_id);
			Assert.That(_id, Is.EqualTo("003"));
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
			Assert.That(_entity.TestingShortDesc, Is.EqualTo(_create.TestingShortDesc));
		}
        //
        //
        public Dictionary<string, EmailSettings> GetEmailSettings()
        {
            // MimeKit.NSG.
            Dictionary<string, EmailSettings> _emailSettings;
            Console.WriteLine("GetEmailSettings: Entering ...");
            string _appSettings = "appSettings.json";
            try
            {
                IConfiguration _config = new ConfigurationBuilder()
                    .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
                    .AddUserSecrets<Test_Tests>()
                    .Build();
                Console.WriteLine($"GetEmailSettings: After config: {_config}");
                //
                if (_config != null)
                {
                    _emailSettings =
                        _config.GetSection("EmailSettings").Get<Dictionary<string, EmailSettings>>();
                    Console.WriteLine($"EmailSettings: {_emailSettings}");
                }
                else
                {
                    var _msg = "GetEmailSettings: ConfigurationBuilder, Could not find EmailSettings in secrets.json";
                    Console.WriteLine(_msg);
                    throw new Exception(_msg);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message);
                throw;
            }
            return _emailSettings;
        }
		//
        [Test]
        public async Task Read_EmailSettings_Test()
        {
			Dictionary<string, EmailSettings> _emailSettings = GetEmailSettings();
            Assert.That(_emailSettings.Count, Is.GreaterThan(0));
            EmailSettings _defaultSettings = _emailSettings["Default"];
            Assert.That(_defaultSettings.UserName, Is.EqualTo("Administrator"));
            Console.WriteLine($"Default: {_defaultSettings}");
            EmailSettings _nsgSettings = _emailSettings["NSG"];
            Assert.That(_nsgSettings.UserName, Is.EqualTo("Phil (NSG)"));
            Console.WriteLine($"NSG: {_nsgSettings}");
            //foreach (KeyValuePair<string, EmailSettings> entry in _emailSettings)
            //{
            //    Console.WriteLine(entry.Key);
            //    Console.WriteLine(entry.Value.ToString());
            //    Console.WriteLine("");
            //}
            Assert.Pass();
        }
        //
        [Test]
		public async Task Test1()
		{
			string Password = "1111 2222 3333 4444";
            Console.WriteLine(Password.Substring(Math.Max(0, Password.Length - 4)));
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
        // [Test]
        public async Task GetIncident_with_Note_Company_Test()
        {
			// this is to test a block of code to get it right
			long _incidentId = 74;
            ServiceCollection _services = new ServiceCollection();
			ApplicationDbContext _context = NSG_Helpers.GetDbContext(_services);
            var _emailNoteTypeId = await _context.NoteTypes.Where(_it => _it.NoteTypeClientScript == "email").Select(_it => _it.NoteTypeId).FirstOrDefaultAsync();
            Incident? _entity = await _context.Incidents
                .Include(_i => _i.IncidentIncidentNotes)
                .ThenInclude(IncidentIncidentNotes => IncidentIncidentNotes.IncidentNote)
                .Include(_i => _i.Server)
                .ThenInclude(Servers => Servers.Company)
                .SingleOrDefaultAsync(_r => _r.IncidentId == _incidentId);
			//
			if (_entity != null)
			{
				//
				IncidentNote? _note = _entity.IncidentIncidentNotes.Where(_inn => _inn.IncidentNote.NoteTypeId == _emailNoteTypeId).Select(_in => _in.IncidentNote).FirstOrDefault();
                string? _companyShortName = _entity.Server.Company.CompanyShortName;
				Assert.That<IncidentNote>(_note, Is.Not.Null);
                Assert.That<string>(_companyShortName, Is.Not.Null);
                Assert.That<string>(_companyShortName, Is.Not.EqualTo(""));
            }
            else
			{
                Assert.Fail();
            }
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