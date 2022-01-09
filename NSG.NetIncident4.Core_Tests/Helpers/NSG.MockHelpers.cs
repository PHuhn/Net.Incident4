using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
//
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
//
using MimeKit;
using MimeKit.NSG;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Domain.Entities;
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
        static public Mock<IServiceProvider> CreateScopedServicesProvider(params (Type @interface, Object service)[] services)
        {
            var scopedServiceProvider = new Mock<IServiceProvider>();

            foreach (var (@interfcae, service) in services)
            {
                scopedServiceProvider
                    .Setup(s => s.GetService(@interfcae))
                    .Returns(service);
            }

            var scope = new Mock<IServiceScope>();
            scope
                .SetupGet(s => s.ServiceProvider)
                .Returns(scopedServiceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(scope.Object);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            return serviceProvider;
        }
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
        //
        // https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
        //
        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Stores.MaxLengthForKeys = 450;
            idOptions.Password.RequireDigit = true;
            idOptions.Password.RequiredLength = 8;
            idOptions.Password.RequireLowercase = true;
            idOptions.Password.RequireUppercase = true;
            idOptions.Password.RequireNonAlphanumeric = true;
            idOptions.SignIn.RequireConfirmedEmail = true;
            // Lockout settings.
            idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            idOptions.Lockout.MaxFailedAccessAttempts = 5;
            idOptions.Lockout.AllowedForNewUsers = true;
            //
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }
        //
        public static SignInManager<TUser> TestSignInManager<TUser>(UserManager<TUser> fakeUserManager) where TUser : class
        {
            // SignInManager<TUser>(
            //  UserManager<TUser>,
            //  IHttpContextAccessor,
            //  IUserClaimsPrincipalFactory<TUser>,
            //  IOptions<IdentityOptions>,
            //  ILogger<SignInManager<TUser>>,
            //  IAuthenticationSchemeProvider,
            //  IUserConfirmation<TUser>)
            IHttpContextAccessor httpContextAccesor = new Mock<IHttpContextAccessor>().Object;
            SignInManager<TUser> _signinManager = new SignInManager<TUser>(
                fakeUserManager,
                httpContextAccesor,
                new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<TUser>>>().Object,
                TestAuthenticationSchemeProvider(),
                new Mock<IUserConfirmation<TUser>>().Object
            );
            return _signinManager;
        }
        //
        public static IAuthenticationSchemeProvider TestAuthenticationSchemeProvider()
        {
            var _authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            _authSchemeProvider.Setup(_ => _.GetDefaultAuthenticateSchemeAsync())
                    .Returns(Task.FromResult(new AuthenticationScheme("idp", "idp", typeof(IAuthenticationHandler))));
            return _authSchemeProvider.Object;
        }
        //
        // SignOutAsync threw the following (thus this):
        // System.InvalidOperationException: HttpContext must not be null.
        // from: https://stackoverflow.com/questions/47198341/how-to-unit-test-httpcontext-signinasync
        public static IServiceProvider TestAuthenticationService()
        {
            var _authServiceMock = new Mock<IAuthenticationService>();
            // setup signin and signout
            // SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties);
            _authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));
            // SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties);
            _authServiceMock
                .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));
            var _authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            _authSchemeProvider.Setup(_ => _.GetDefaultAuthenticateSchemeAsync())
                .Returns(Task.FromResult(new AuthenticationScheme("idp", "idp", typeof(IAuthenticationHandler))));
            var _systemClock = new Mock<ISystemClock>();
            //
            var _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(_authServiceMock.Object);
            _serviceProviderMock
                .Setup(_ => _.GetService(typeof(ISystemClock)))
                .Returns(_systemClock.Object);
            _serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationSchemeProvider)))
                .Returns(TestAuthenticationSchemeProvider());
            //
            return _serviceProviderMock.Object;
        }
        //
    }
    //
    public class FakeNotificationService : INotificationService
    {
        private readonly EmailSettings _emailSettings;
        ILogger _logger;
        //
        public FakeNotificationService(
            EmailSettings emailSettings,
            ILogger logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
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
    }
}