using NUnit.Framework;
using System;
using System.Text;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
//
namespace NSG.NetIncident4.Core_Tests.Persistence
{
    [TestFixture]
    public class ApplicationDbContext_UnitTests : UnitTestFixture
    {
        //
        [SetUp]
        public void Setup()
        {
            Fixture_UnitTestSetup();
        }
        //
        [Test]
        public async Task ApplicationDbContext_DuplicateCreateApplicationUser_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: ApplicationUser. Duplicate Username: Test-User, Duplicate Email: Test-User@any.com, Duplicate Full Name: Test User, ";
            await SeedData.SeedCompany(db_context);
            Console.WriteLine("ApplicationDbContext_DuplicateCreateApplicationUser_Test");
            ApplicationUser _new = new ApplicationUser()
            {
                UserName = "Test-User",
                NormalizedUserName = "Test-User".ToUpper(),
                Email = "Test-User@any.com",
                NormalizedEmail = "Test-User@any.com".ToUpper(),
                EmailConfirmed = false,
                PasswordHash = "",
                SecurityStamp = "",
                PhoneNumber = "555-1212",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                //
                FirstName = "Test",
                LastName = "User",
                FullName = "Test User",
                UserNicName = "Test",
                CompanyId = 1,
                CreateDate = DateTime.Now
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                _new.Id = Guid.NewGuid().ToString();
                // _new.UserName = "User-Test";
                // _new.Email = "User-Test@any.com";
                // _new.NormalizedEmail = _new.Email.ToUpper();
                // _new.FullName = "User-Test";
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            foreach (ApplicationUser _au in db_context.Users)
            {
                Console.WriteLine($"{_au.UserName} {_au.Email} {_au.FullName} {_au.Id}");
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public async Task ApplicationDbContext_DuplicateUpdateApplicationUser_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: ApplicationUser. Duplicate Username: Phil, and/or Email: author@any.net, and/or Name: Author Huhn, ";
            await SeedData.SeedCompany(db_context);
            await SeedData.SeedServer(db_context, 1);
            await SeedData.SeedRole(roleManager);
            DatabaseSeeder seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            await seeder.Seed_Users();
            Console.WriteLine("ApplicationDbContext_DuplicateUpdateApplicationUser_Test");
            // create duplicate
            try
            {
                ApplicationUser? _user1 = db_context.Users.FirstOrDefault(u => u.UserName == NSG_Helpers.User_Name);
                ApplicationUser? _user2 = db_context.Users.FirstOrDefault(u => u.UserName == NSG_Helpers.User_Name2);
                if( _user2 != null )
                {
                    _user2.UserName = _user1?.UserName;
                    _user2.NormalizedUserName = _user2?.UserName.ToUpper();
                    db_context.SaveChanges();
                }
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            foreach (ApplicationUser _au in db_context.Users)
            {
                Console.WriteLine($"{_au.UserName} {_au.Email} {_au.FullName} {_au.Id}");
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateCreateCompany_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: Company. Duplicate Short: TestCmpny and/or Discription: Test Company. ";
            Console.WriteLine("ApplicationDbContext_DuplicateCreateCompany_Test");
            Company _new = new Company()
            {
                CompanyShortName = "TestCmpny",
                CompanyName = "Test Company",
                Address = "-",
                City = "-",
                State = "MI",
                PostalCode = "48555",
                Country = "USA",
                PhoneNumber = "555-1212",
                Notes = "",
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateUpdateCompany_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: Company. Duplicate Short: TestCmpny2 and/or Discription: Test Company5. ";
            Console.WriteLine("ApplicationDbContext_DuplicateUpdateCompany_Test");
            Company _new = new Company();
            foreach (int _idx in new int[]{1, 2, 3, 4 ,5})
            {
                _new = new Company()
                {
                    CompanyShortName = $"TestCmpny{_idx}",
                    CompanyName = $"Test Company{_idx}",
                    Address = "-",
                    City = "-",
                    State = "MI",
                    PostalCode = "48555",
                    Country = "USA",
                    PhoneNumber = "555-1212",
                    Notes = "",
                };
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            // create duplicate
            try
            {
                _new.CompanyShortName = $"TestCmpny2";
                // _new.CompanyName = "Test Company2";
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public async Task ApplicationDbContext_DuplicateCreateServer_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: Server. Duplicate Short: TestSrv and/or Name: Test Server. ";
            await SeedData.SeedCompany(db_context);
            Console.WriteLine("ApplicationDbContext_DuplicateCreateServer_Test");
            Server _new = new Server()
            {
                CompanyId = 1,
                ServerShortName = "TestSrv",
                ServerName = "Test Server",
                ServerDescription = "Test Server",
                WebSite = "-",
                ServerLocation = "-",
                FromName = "-",
                FromNicName = "-",
                FromEmailAddress = "-",
                TimeZone = "EST",
                DST = false,
                TimeZone_DST = "",
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public async Task ApplicationDbContext_DuplicateUpdateServer_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: Server. Duplicate Short: TestSrv1 and/or Name: Test Server3. ";
            await SeedData.SeedCompany(db_context);
            Console.WriteLine("ApplicationDbContext_DuplicateCreateServer_Test");
            Server _lastNew = new Server();
            foreach (int _idx in new int[] { 1, 2, 3})
            {
                Server _new = new Server()
                {
                    // ServerId <- IDENTITY(1,1) primary key
                    CompanyId = 1,  // <- foreign key
                    ServerShortName = $"TestSrv{_idx}",
                    ServerName = $"Test Server{_idx}",
                    ServerDescription = $"Test Server{_idx}",
                    WebSite = "-",
                    ServerLocation = "-",
                    FromName = "-",
                    FromNicName = "-",
                    FromEmailAddress = "-",
                    TimeZone = "EST",
                    DST = false,
                    TimeZone_DST = "",
                };
                db_context.Add(_new);
                try
                {
                    db_context.SaveChanges();
                    Console.WriteLine($"New Server: {_new.ServerId}, {_new.ServerShortName}, {_new.ServerName}, Company: {_new.CompanyId} ");
                    _lastNew = _new;
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex.GetBaseException().ToString());
                    Assert.Fail(_ex.Message);
                }
            }
            // create duplicate
            try
            {
                foreach( Server _srv in db_context.Servers)
                {
                    Console.WriteLine($"{_srv.ServerId}, {_srv.ServerShortName}, {_srv.ServerName}");
                }
                _lastNew.ServerId = 0;
                _lastNew.ServerShortName = $"TestSrv1";
                // _new.ServerName = $"Test Server1";
                db_context.Add(_lastNew);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail("Duplicate exception failed");
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateCreateIncidentType_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: IncidentType. Duplicate Short: TestIT and/or Discription: Test IncidentType. ";
            Console.WriteLine("ApplicationDbContext_DuplicateCreateIncidentType_Test");
            IncidentType _new = new IncidentType()
            {
                IncidentTypeShortDesc = "TestIT",
                IncidentTypeDesc = "Test IncidentType",
                IncidentTypeFromServer = false,
                IncidentTypeSubjectLine = "-",
                IncidentTypeEmailTemplate = "-",
                IncidentTypeTimeTemplate = "-",
                IncidentTypeThanksTemplate = "-",
                IncidentTypeLogTemplate = "-",
                IncidentTypeTemplate = "-",
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateUpdateIncidentType_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: IncidentType. Duplicate Short: TestIT1 and/or Discription: Test IncidentType5. ";
            Console.WriteLine("ApplicationDbContext_DuplicateUpdateIncidentType_Test");
            IncidentType _new = new IncidentType();
            foreach (int _idx in new int[] { 1, 2, 3, 4, 5 })
            {
                _new = new IncidentType()
                {
                    IncidentTypeShortDesc = $"TestIT{_idx}",
                    IncidentTypeDesc = $"Test IncidentType{_idx}",
                    IncidentTypeFromServer = false,
                    IncidentTypeSubjectLine = "-",
                    IncidentTypeEmailTemplate = "-",
                    IncidentTypeTimeTemplate = "-",
                    IncidentTypeThanksTemplate = "-",
                    IncidentTypeLogTemplate = "-",
                    IncidentTypeTemplate = "-",
                };
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            // create duplicate
            try
            {
                _new.IncidentTypeShortDesc = $"TestIT1";
                // _new.IncidentTypeDesc = $"Test IncidentType1";
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public async Task ApplicationDbContext_DuplicateCreateEmailTemplate_Test()
        {
            Console.WriteLine("ApplicationDbContext_DuplicateCreateEmailTemplate_Test");
            string expected = "DbUpdateException: Problem updating entity: EmailTemplate. Duplicate Company: NSG-1 and IncidentType: DIR-7. ";
            await SeedData.SeedCompany(db_context);
            await SeedData.SeedIncidentType(db_context, false);
            // 1 Unk/2 Multiple/3 SQL/4 PHP/5 XSS/6 VS/7 DIR/8 DoS
            EmailTemplate _new = new EmailTemplate()
            {
                CompanyId = 1,
                IncidentTypeId = 7,
                SubjectLine = "-",
                EmailBody = "-",
                TimeTemplate = "-",
                ThanksTemplate = "-",
                LogTemplate = "-",
                Template = "-",
                FromServer = false
            };
            Console.WriteLine($"Count: {db_context.IncidentTypes.Count()}");
            foreach ( IncidentType _it in db_context.IncidentTypes )
            {
                Console.WriteLine($"{_it.IncidentTypeId} {_it.IncidentTypeShortDesc}");
            }
            Console.WriteLine(_new);
            db_context.EmailTemplates.Add(_new);
            db_context.SaveChanges();
            foreach (EmailTemplate _et in db_context.EmailTemplates)
            {
                Console.WriteLine($" {_et.CompanyId} {_et.IncidentTypeId}");
            }
            // create duplicate
            try
            {
                db_context.EmailTemplates.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateCreateNIC_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: NIC. Duplicate Id: TestNIC and/or Discription: Test NIC. ";
            Console.WriteLine("ApplicationDbContext_DuplicateNIC_Test");
            NIC _new = new NIC()
            {
                NIC_Id = "TestNIC",
                NICDescription = "Test NIC"
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
        [Test]
        public void ApplicationDbContext_DuplicateCreateNoteType_Test()
        {
            string expected = "DbUpdateException: Problem updating entity: NoteType. Duplicate Short: Test_NT and/or Discription: Test NT. ";
            Console.WriteLine("ApplicationDbContext_DuplicateNoteType_Test");
            NoteType _new = new NoteType()
            {
                NoteTypeShortDesc = "Test_NT",
                NoteTypeDesc = "Test NT",
            };
            db_context.Add(_new);
            db_context.SaveChanges();
            // create duplicate
            try
            {
                db_context.Add(_new);
                db_context.SaveChanges();
            }
            catch (DbUpdateException upExc)
            {
                string actual = db_context.HandleDbUpdateExceptionString(upExc);
                Console.WriteLine(actual);
                Assert.AreEqual(expected, actual);
                return;
            }
            Assert.Fail();
            //
        }
        //
    }
}