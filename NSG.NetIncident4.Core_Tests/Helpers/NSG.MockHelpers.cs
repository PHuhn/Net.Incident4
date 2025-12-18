using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using NUnit.Framework;
//
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
//
using MimeKit;
using MimeKit.NSG;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using MockQueryable.Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
//
namespace NSG.Integration.Helpers
{
    public static class MockHelpers
    {
        //
        /// <summary>
        /// 
        /// <example>
        /// Mock<ILogger<ServicesController>> _mockLogger = LoggerMoq<ServicesController>();
        /// </example>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public Mock<ILogger<T>> LoggerMoq<T>()
        {
            return new Mock<ILogger<T>>();
        }
        //
        public static Mock<ILogger<T>> VerifyLogLevel<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel)
        {
            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
            //
            return logger;
        }
        //
        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, string expectedMessage, Times? times = null)
        {
            // https://adamstorr.azurewebsites.net/blog/mocking-ilogger-with-moq
            times ??= Times.Once();
            Func<object, Type, bool> state = (v, t) => v.ToString().Substring(0, expectedMessage.Length).CompareTo(expectedMessage) == 0;
            //
            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times)times);
            //
            return logger;
        }
        //
        // https://stackoverflow.com/questions/44336489/moq-iserviceprovider-iservicescope
        //static public Mock<IServiceProvider> CreateScopedServicesProvider(params (Type @interface, Object service)[] services)
        //{
        //    var scopedServiceProvider = new Mock<IServiceProvider>();

        //    foreach (var (@interfcae, service) in services)
        //    {
        //        scopedServiceProvider
        //            .Setup(s => s.GetService(@interfcae))
        //            .Returns(service);
        //    }

        //    var scope = new Mock<IServiceScope>();
        //    scope
        //        .SetupGet(s => s.ServiceProvider)
        //        .Returns(scopedServiceProvider.Object);

        //    var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        //    serviceScopeFactory
        //        .Setup(x => x.CreateScope())
        //        .Returns(scope.Object);

        //    var serviceProvider = new Mock<IServiceProvider>();
        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
        //        .Returns(serviceScopeFactory.Object);

        //    return serviceProvider;
        //}
        //
        public static ClaimsPrincipal TestPrincipal(string userName, string role)
        {
            ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, $"{userName}@any.net"),
                new Claim(ClaimTypes.Role, role)
            }, "basic"));
            
            //
            return _user;
        }
        //public static Mock<IUrlHelper> MockUrlHelper(string userName, string role, string path)
        //{
        //    ActionContext context = CreatePageActionContext(userName, role, path, "/Account");
        //    Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
        //    urlHelper.SetupGet(h => h.ActionContext)
        //        .Returns(context);
        //    return urlHelper;
        //}
        //
        //public static ActionContext CreatePageActionContext(string userName, string role, string path, string page)
        //{
        //    RouteValueDictionary rvd = new RouteValueDictionary();
        //    rvd.Add("Account", page);
        //    ActionDescriptor actionDesc = new ActionDescriptor();
        //    ActionContext ac = new ActionContext(
        //        Mock_CreateHttpContext(userName, role, path),
        //        new Microsoft.AspNetCore.Routing.RouteData(rvd),
        //        actionDesc,
        //        new ModelStateDictionary());
        //    return ac;
        //}
        //
        //public static PageContext CreatePageContext(string userName, string role, string path, string page)
        //{
        //    return new PageContext(CreatePageActionContext(userName, role, path, page));
        //}
        //
        /// <summary>
        /// Create a fake http context
        /// <example>
        ///  sut.HttpContext = Fixture_HttpContext("TestUser", "admin", "/UserAdmin/", controllerHeaders);
        /// </example>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="path"></param>
        /// <returns>
        /// HttpContext with a mock HttpContext and an assigned IPrincipal User
        /// </returns>
        //public static HttpContext Mock_CreateHttpContext(string userName, string role, string path)
        //{
        //    //
        //    Mock<HttpContext> httpContext = new Mock<HttpContext>();
        //    //
        //    // https://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
        //    if (userName != "")
        //    {
        //        httpContext.SetupGet(m => m.User).Returns((ClaimsPrincipal)TestPrincipal(userName, role));
        //    }
        //    httpContext.SetupGet(m => m.Request).Returns(
        //        (HttpRequest)Mock_CreateHttpRequest(path));
        //    //
        //    return httpContext.Object;
        //}
        ////
        //public static HttpRequest Mock_CreateHttpRequest(string path)
        //{
        //    // 
        //    // cookie: ai_user=Zn3LO|2021-11-19T15:30:13.062Z; .AspNetCore.Antiforgery.TgedqpnEzL8=CfDJ8MoWY2n3geRBvkgVUWBREdrAbSv3olymADsefAXujoG9VNEcZw3EiwDGCXW4wXuNsrXgp3p7ZTSQAlQvBV5m31kilXUR8tla-lte-Mo9j3HZFbLoXrP9DEhmmJr6wUTqcJd-4uKk5ehaN2u-Za-Jeac; ai_session=xBril|1645283096306.1|1645285831298.5
        //    // user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36 Edg/98.0.1108.50
        //    // Headers = headers, // Dictionary<string, string>
        //    //
        //    Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
        //    httpRequest.SetupGet(m => m.Path).Returns((PathString)new PathString(path));
        //    httpRequest.SetupGet(m => m.PathBase).Returns(new PathString(path));
        //    httpRequest.SetupGet(m => m.Host).Returns((HostString)new HostString("localhost", 44378));
        //    httpRequest.SetupGet(m => m.Scheme).Returns("https");
        //    httpRequest.SetupGet(m => m.QueryString).Returns(new QueryString());
        //    HttpRequest hr = httpRequest.Object;
        //    return hr;
        //}
        //
        // https://github.com/dotnet/aspnetcore/blob/main/src/Identity/test/Shared/MockHelpers.cs
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }
        //
        // https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
        //
        //public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        //{
        //    store = store ?? new Mock<IUserStore<TUser>>().Object;
        //    var options = new Mock<IOptions<IdentityOptions>>();
        //    var idOptions = new IdentityOptions();
        //    idOptions.Stores.MaxLengthForKeys = 450;
        //    idOptions.Password.RequireDigit = true;
        //    idOptions.Password.RequiredLength = 8;
        //    idOptions.Password.RequireLowercase = true;
        //    idOptions.Password.RequireUppercase = true;
        //    idOptions.Password.RequireNonAlphanumeric = true;
        //    idOptions.SignIn.RequireConfirmedEmail = true;
        //    // Lockout settings.
        //    idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    idOptions.Lockout.MaxFailedAccessAttempts = 5;
        //    idOptions.Lockout.AllowedForNewUsers = true;
        //    //
        //    options.Setup(o => o.Value).Returns(idOptions);
        //    var userValidators = new List<IUserValidator<TUser>>();
        //    var validator = new Mock<IUserValidator<TUser>>();
        //    userValidators.Add(validator.Object);
        //    var pwdValidators = new List<PasswordValidator<TUser>>();
        //    pwdValidators.Add(new PasswordValidator<TUser>());
        //    var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
        //        userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
        //        new IdentityErrorDescriber(), null,
        //        new Mock<ILogger<UserManager<TUser>>>().Object);
        //    validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
        //        .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
        //    return userManager;
        //}
        //
        //public static SignInManager<TUser> TestSignInManager<TUser>(UserManager<TUser> fakeUserManager) where TUser : class
        //{
        //    // SignInManager<TUser>(
        //    //  UserManager<TUser>,
        //    //  IHttpContextAccessor,
        //    //  IUserClaimsPrincipalFactory<TUser>,
        //    //  IOptions<IdentityOptions>,
        //    //  ILogger<SignInManager<TUser>>,
        //    //  IAuthenticationSchemeProvider,
        //    //  IUserConfirmation<TUser>)
        //    IHttpContextAccessor httpContextAccesor = new Mock<IHttpContextAccessor>().Object;
        //    SignInManager<TUser> _signinManager = new SignInManager<TUser>(
        //        fakeUserManager,
        //        httpContextAccesor,
        //        new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
        //        new Mock<IOptions<IdentityOptions>>().Object,
        //        new Mock<ILogger<SignInManager<TUser>>>().Object,
        //        TestAuthenticationSchemeProvider(),
        //        new Mock<IUserConfirmation<TUser>>().Object
        //    );
        //    return _signinManager;
        //}
        //
        //public static IAuthenticationSchemeProvider TestAuthenticationSchemeProvider()
        //{
        //    var _authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
        //    _authSchemeProvider.Setup(_ => _.GetDefaultAuthenticateSchemeAsync())
        //            .Returns(Task.FromResult(new AuthenticationScheme("idp", "idp", typeof(IAuthenticationHandler))));
        //    return _authSchemeProvider.Object;
        //}
        //
        // SignOutAsync threw the following (thus this):
        // System.InvalidOperationException: HttpContext must not be null.
        // from: https://stackoverflow.com/questions/47198341/how-to-unit-test-httpcontext-signinasync
        //public static IServiceProvider TestAuthenticationService()
        //{
        //    var _authServiceMock = new Mock<IAuthenticationService>();
        //    // setup signin and signout
        //    // SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties);
        //    _authServiceMock
        //        .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
        //        .Returns(Task.FromResult((object)null));
        //    // SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties);
        //    _authServiceMock
        //        .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
        //        .Returns(Task.FromResult((object)null));
        //    var _authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
        //    _authSchemeProvider.Setup(_ => _.GetDefaultAuthenticateSchemeAsync())
        //        .Returns(Task.FromResult(new AuthenticationScheme("idp", "idp", typeof(IAuthenticationHandler))));
        //    var _systemClock = new Mock<ISystemClock>();
        //    //
        //    var _serviceProviderMock = new Mock<IServiceProvider>();
        //    _serviceProviderMock
        //        .Setup(_ => _.GetService(typeof(IAuthenticationService)))
        //        .Returns(_authServiceMock.Object);
        //    _serviceProviderMock
        //        .Setup(_ => _.GetService(typeof(ISystemClock)))
        //        .Returns(_systemClock.Object);
        //    _serviceProviderMock
        //        .Setup(_ => _.GetService(typeof(IAuthenticationSchemeProvider)))
        //        .Returns(TestAuthenticationSchemeProvider());
        //    //
        //    return _serviceProviderMock.Object;
        //}
        //
        /// <summary>
        /// Create a Moq of Role Manager, passing in the Role Store
        ///  RoleManager<TRole>(IRoleStore<TRole>,
        ///   IEnumerable<IRoleValidator<TRole>>, 
        ///   ILookupNormalizer, 
        ///   IdentityErrorDescriber,
        ///   ILogger<RoleManager<TRole>>)
        /// </summary>
        /// <typeparam name="ApplicationRole"></typeparam>
        /// <param name="store"></param>
        /// <returns></returns>
        public static Mock<RoleManager<ApplicationRole>> GetMockRoleManager<ApplicationRole>(
            IRoleStore<ApplicationRole> store = null) where ApplicationRole : class
        {
            store = store ?? new Mock<IRoleStore<ApplicationRole>>().Object;
            var _roles = new List<IRoleValidator<ApplicationRole>>();
            _roles.Add(new RoleValidator<ApplicationRole>());
            var _mgr = new Mock<RoleManager<ApplicationRole>>(store, _roles,
                MockLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);
            var _mockDbSetRoles = NSG_Helpers.rolesFakeData.BuildMock().BuildMockDbSet();
            _ = _mgr.Setup(x => x.Roles).Returns(_mockDbSetRoles as IQueryable<ApplicationRole>);
            return _mgr;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ApplicationUser"></typeparam>
        /// <returns></returns>
        public static Mock<UserManager<ApplicationUser>> GetMockUserManager<ApplicationUser>(
            IUserStore<ApplicationUser> store = null) where ApplicationUser : class
        {
            store = store ?? new Mock<IUserStore<ApplicationUser>>().Object;
            var _mgr = new Mock<UserManager<ApplicationUser>>(store, null, null, null, null, null, null, null, null);
            _mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            _mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            _ = _mgr.Setup(x => x.Users).Returns(_mockDbSetUsers as IQueryable<ApplicationUser>);
            return _mgr;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ILookupNormalizer MockLookupNormalizer()
        {
            var normalizerFunc = new Func<string, string>(i =>
            {
                if (i == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(i)).ToUpperInvariant();
                }
            });
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(i => i.NormalizeName(It.IsAny<string>())).Returns(normalizerFunc);
            lookupNormalizer.Setup(i => i.NormalizeEmail(It.IsAny<string>())).Returns(normalizerFunc);
            return lookupNormalizer.Object;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Mock of ApplicationDbContext</returns>
        public static Mock<ApplicationDbContext> GetDbContextMock()
        {
            var _databaseName = "Test_" + Guid.NewGuid().ToString();
            var _optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: _databaseName);
            var _dbContextMock = new Mock<ApplicationDbContext>(_optionsBuilder.Options);
            var _database = new Mock<DatabaseFacade>(_dbContextMock.Object);
            _ = _database.Setup(d => d.ToString()).Returns(_databaseName);
            _ = _dbContextMock.Setup(x => x.Database).Returns(_database.Object);
            return _dbContextMock;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSetMock"></param>
        /// <returns></returns>
        public static Mock<DbSet<TEntity>> DbSetAddAsync<TEntity>(this Mock<DbSet<TEntity>> dbSetMock) where TEntity : class
        {
            RuntimeEntityType a;
            // https://stackoverflow.com/questions/68801418/how-to-mock-dbset-addasyncobj-entity
            dbSetMock.Setup(_ => _.AddAsync(It.IsAny<TEntity>(),
                It.IsAny<CancellationToken>()))
                .Returns<TEntity, CancellationToken>((e, c) =>
                {
                    var stateManagerMock = new Mock<IStateManager>();
                    var entityTypeMock = new Mock<IRuntimeEntityType>();
                    entityTypeMock
                        .SetupGet(_ => _.EmptyShadowValuesFactory)
                        .Returns(() => new Mock<ISnapshot>().Object);
                    //entityTypeMock
                    //    .SetupGet(_ => _.Counts)  V10.0 it is private
                    //    .Returns(new PropertyCounts(1, 1, 1, 1, 1, 1, 1, 1));
                    entityTypeMock
                        .Setup(_ => _.GetProperties())
                        .Returns(Enumerable.Empty<IProperty>());
                    InternalEntityEntry internalEntity = new InternalEntityEntry(stateManagerMock.Object,
                        entityTypeMock.Object, e);
                    var entry = new EntityEntry<TEntity>(internalEntity);
                    return ValueTask.FromResult(entry);
                });
            //
            return dbSetMock;
        }
        //
        /// <summary>
        /// Get a complete ApplicationDbContext with mocked data
        /// </summary>
        /// <returns></returns>
        public static Mock<ApplicationDbContext> GetApplicationDbContextMock()
        {
            var _mockDbSetUsers = NSG_Helpers.usersFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetRoles = NSG_Helpers.rolesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetUsrRoles = NSG_Helpers.userRolesFakeData.BuildMock().BuildMockDbSet();
            var _mockDbSetComps = NSG_Helpers.companiesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetComps.DbSetAddAsync<Company>();
            var _mockDbSetSrvrs = NSG_Helpers.serversFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetSrvrs.DbSetAddAsync<Server>();
            var _mockDbSetUsrSrvrs = NSG_Helpers.userSrvrsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetUsrSrvrs.DbSetAddAsync<ApplicationUserServer>();
            var _mockDbSet = NSG_Helpers.incidentsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSet.DbSetAddAsync<Incident>();
            var _mockDbSetNote = NSG_Helpers.incidentNotesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetNote.DbSetAddAsync<IncidentNote>();
            var _mockDbSetNTypes = NSG_Helpers.noteTypesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetNTypes.DbSetAddAsync<NoteType>();
            var _mockDbSetIncNote = NSG_Helpers.incidentIncidentNotesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetIncNote.DbSetAddAsync<IncidentIncidentNote>();
            var _mockDbSetNLogs = NSG_Helpers.networkLogsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetNLogs.DbSetAddAsync<NetworkLog>();
            var _mockDbSetITypes = NSG_Helpers.incidentTypesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetITypes.DbSetAddAsync<IncidentType>();
            var _mockDbSetEmails = NSG_Helpers.emailTemplatesFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetEmails.DbSetAddAsync<EmailTemplate>();
            var _mockDbSetNICs = NSG_Helpers.nicsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetNICs.DbSetAddAsync<NIC>();
            var _mockDbSetLogs = NSG_Helpers.logsFakeData.BuildMock().BuildMockDbSet();
            _ = _mockDbSetLogs.DbSetAddAsync<LogData>();
            Mock<ApplicationDbContext> _contextMock = MockHelpers.GetDbContextMock();
            _contextMock.Setup(x => x.Users).Returns(_mockDbSetUsers.Object);
            _contextMock.Setup(x => x.Roles).Returns(_mockDbSetRoles.Object);
            _contextMock.Setup(x => x.UserRoles).Returns(_mockDbSetUsrRoles.Object);
            _contextMock.Setup(x => x.Companies).Returns(_mockDbSetComps.Object);
            _contextMock.Setup(x => x.Servers).Returns(_mockDbSetSrvrs.Object);
            _contextMock.Setup(x => x.UserServers).Returns(_mockDbSetUsrSrvrs.Object);
            _contextMock.Setup(x => x.Incidents).Returns(_mockDbSet.Object);
            _contextMock.Setup(x => x.IncidentNotes).Returns(_mockDbSetNote.Object);
            _contextMock.Setup(x => x.NoteTypes).Returns(_mockDbSetNTypes.Object);
            _contextMock.Setup(x => x.IncidentIncidentNotes).Returns(_mockDbSetIncNote.Object);
            _contextMock.Setup(x => x.NetworkLogs).Returns(_mockDbSetNLogs.Object);
            _contextMock.Setup(x => x.IncidentTypes).Returns(_mockDbSetITypes.Object);
            _contextMock.Setup(x => x.EmailTemplates).Returns(_mockDbSetEmails.Object);
            _contextMock.Setup(x => x.NICs).Returns(_mockDbSetNICs.Object);
            _contextMock.Setup(x => x.Logs).Returns(_mockDbSetLogs.Object);
            //
            return _contextMock;
        }
        //
    }
    //
    public class FakeNotificationService : INotificationService
    {
        EmailSettings _emailSettings;
        Dictionary<string, MimeKit.NSG.EmailSettings> _emailSettingsDict;
        ILogger _logger;
        //
        public FakeNotificationService(
            Dictionary<string, EmailSettings> emailSettings,
            ILogger logger)
        {
            _emailSettingsDict = emailSettings;
            _emailSettings = emailSettings["Default"];
            _logger = logger;
        }
        //
        public EmailSettings CurrentEmailSettings
        {
            get { return _emailSettings; }
        }
        //
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendEmailAsync(MimeKit.Extensions.NewMimeMessage()
                .From(_emailSettings.UserEmail, _emailSettings.UserName).To(email)
                .Subject(subject).Body(MimeKit.Extensions.TextBody(message)));
        }
        //
        public Task SendEmailAsync(string from, string to, string subject, string message)
        {
            return SendEmailAsync(MimeKit.Extensions.NewMimeMessage()
                .From(from).To(to).Subject(subject).Body(MimeKit.Extensions.TextBody(message)));
        }
        //
        public Task SendEmailAsync(MimeMessage mimeMessage)
        {
            string _msg = mimeMessage.EmailToString();
            _logger.LogWarning(_msg);
            Console.WriteLine("Message sent:\n" + _msg);
            return Task.CompletedTask;
        }
        public Task SendEmailAsync(MimeKit.MimeMessage mimeMessage, string companyShortName)
        {
            if (_emailSettingsDict[companyShortName] == null)
            {
                throw new Exception($"EmailSetting configuration does not contain company {companyShortName}");
            }
            return SendEmailAsync(mimeMessage);
        }
    }
}