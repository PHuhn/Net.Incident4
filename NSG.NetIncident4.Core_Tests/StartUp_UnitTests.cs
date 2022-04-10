using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using MimeKit.NSG;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//
namespace NSG.NetIncident4.Core_Tests
{
    [TestFixture]
    public class StartUp_UnitTests : UnitTestFixture
    {
        Startup _startUp = null;
        //
        [SetUp]
        public void Setup()
        {
            SetupConfiguration("appsettings.json");
            _startUp = new Startup(configuration);
        }
        //
        [Test]
        public void Startup_LoggingService_Test()
        {
            Console.WriteLine("LoggingService_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            ILoggerFactory _loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();
            ILoggerProvider _loggerProvider = services.BuildServiceProvider().GetService<ILoggerProvider>();
            //
            Assert.NotNull(_loggerFactory);
            Assert.NotNull(_loggerProvider);
        }
        //
        [Test]
        public void Startup_ViewLocations_Test()
        {
            Console.WriteLine("Startup_ViewLocations_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureViewLocation(services);
            IConfigureOptions<RazorPagesOptions> _configureRazorPages = services.BuildServiceProvider().GetService<IConfigureOptions<RazorPagesOptions>>();
            IConfigureOptions<RazorViewEngineOptions> _configureRazorView = services.BuildServiceProvider().GetService<IConfigureOptions<RazorViewEngineOptions>>();
            //
            Assert.NotNull(_configureRazorPages);
            Assert.NotNull(_configureRazorView);
        }
        //
        [Test]
        public void Startup_NotificationServices_Test()
        {
            Console.WriteLine("Startup_NotificationServices_Test");
            // given
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // when
            _startUp.ConfigureNotificationServices(services);
            // then
            IOptions<MimeKit.NSG.EmailSettings> _emailSettings = services.BuildServiceProvider().GetService<IOptions<MimeKit.NSG.EmailSettings>>();
            IOptions<ServicesSettings> _serviceSettings = services.BuildServiceProvider().GetService<IOptions<ServicesSettings>>();
            IOptions<ApplicationSettings> _applicationSettings = services.BuildServiceProvider().GetService<IOptions<ApplicationSettings>>();
            IOptions<AuthSettings> _authSettings = services.BuildServiceProvider().GetService<IOptions<AuthSettings>>();
            //
            INotificationService _notification = services.BuildServiceProvider().GetService<INotificationService>();
            IEmailSender _emailSender = services.BuildServiceProvider().GetService<IEmailSender>();
            IApplication _application = services.BuildServiceProvider().GetService<IApplication>();
            IMediator _mediator = services.BuildServiceProvider().GetService<IMediator>();
            // then
            Assert.NotNull(_emailSettings.Value);
            Assert.NotNull(_serviceSettings.Value);
            Assert.NotNull(_applicationSettings.Value);
            Assert.NotNull(_authSettings.Value);
            //
            Assert.NotNull(_notification);
            Assert.NotNull(_emailSender);
            Assert.NotNull(_application);
            Assert.NotNull(_mediator);
        }
        //
        [Test]
        public void Startup_AuthorizationPolicy_Test()
        {
            // given
            Console.WriteLine("Startup_AuthorizationPolicy_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            // when
            _startUp.ConfigureAuthorizationPolicyServices(services);
            // then
            IAuthorizationService _authorization = services.BuildServiceProvider().GetService<IAuthorizationService>();
            Assert.NotNull(_authorization);
        }
        //
        [Test]
        public void Startup_Session_Test()
        {
            // given
            Console.WriteLine("Startup_Session_Test");
            ServiceCollection services = new ServiceCollection();
            // when
            _startUp.ConfigureSessionServices(services);
            // then
            IKeyManager _keyManager = services.BuildServiceProvider().GetService<IKeyManager>();
            // ServiceType = { Name = "ISessionStore" FullName = "Microsoft.AspNetCore.Session.ISessionStore"}
            // ISessionStore _session = services.BuildServiceProvider().GetService<ISessionStore>();
            Assert.NotNull(_keyManager);
        }
        //
        [Test]
        public void Startup_Cors_Test()
        {
            // given
            Console.WriteLine("Startup_Cors_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            _startUp.authSettings =
                configuration.GetSection("AuthSettings").Get<AuthSettings>();
            // when
            _startUp.ConfigureCorsServices(services);
            // then
            ICorsService _corsService = services.BuildServiceProvider().GetService<ICorsService>();
            Assert.NotNull(_corsService);
        }
        //
        [Test]
        public void Startup_SwaggerService_Test()
        {
            // given
            Console.WriteLine("Startup_SwaggerService_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            services.AddMvc();
            // when
            _startUp.ConfigureSwaggerServices(services);
            // then
            IConfigureOptions<MvcOptions> _swaggerMvc = services.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>();
            // ISwaggerProvider _swagger = services.BuildServiceProvider().GetService<ISwaggerProvider>();
            Assert.NotNull(_swaggerMvc);
        }
        //
    }
}