// ===========================================================================
// File: StartUp_UnitTests.cs
using NUnit.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//
using MediatR;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using MimeKit.NSG;
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
            Microsoft.Extensions.Logging.ILoggerFactory?
                _loggerFactory = services.BuildServiceProvider().GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            ILoggerProvider? _loggerProvider = services.BuildServiceProvider().GetService<ILoggerProvider>();
            //
            Assert.That(_loggerFactory, Is.Not.Null);
            Assert.That(_loggerProvider, Is.Not.Null);
        }
        //
        [Test]
        public void Startup_ViewLocations_Test()
        {
            Console.WriteLine("Startup_ViewLocations_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureViewLocation(services);
            IConfigureOptions<RazorPagesOptions>? _configureRazorPages = services.BuildServiceProvider().GetService<IConfigureOptions<RazorPagesOptions>>();
            IConfigureOptions<RazorViewEngineOptions>? _configureRazorView = services.BuildServiceProvider().GetService<IConfigureOptions<RazorViewEngineOptions>>();
            //
            Assert.That(_configureRazorPages, Is.Not.Null);
            Assert.That(_configureRazorView, Is.Not.Null);
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
            IOptions<MimeKit.NSG.EmailSettings>? _emailSettings = services.BuildServiceProvider().GetService<IOptions<MimeKit.NSG.EmailSettings>>();
            IOptions<ServicesSettings>? _serviceSettings = services.BuildServiceProvider().GetService<IOptions<ServicesSettings>>();
            IOptions<ApplicationSettings>? _applicationSettings = services.BuildServiceProvider().GetService<IOptions<ApplicationSettings>>();
            IOptions<AuthSettings>? _authSettings = services.BuildServiceProvider().GetService<IOptions<AuthSettings>>();
            //
            INotificationService? _notification = services.BuildServiceProvider().GetService<INotificationService>();
            IEmailSender? _emailSender = services.BuildServiceProvider().GetService<IEmailSender>();
            IApplication? _application = services.BuildServiceProvider().GetService<IApplication>();
            IMediator? _mediator = services.BuildServiceProvider().GetService<IMediator>();
            // then
            Assert.That(_emailSettings.Value, Is.Not.Null);
            Assert.That(_serviceSettings.Value, Is.Not.Null);
            Assert.That(_applicationSettings.Value, Is.Not.Null);
            Assert.That(_authSettings.Value, Is.Not.Null);
            //
            Assert.That(_notification, Is.Not.Null);
            Assert.That(_emailSender, Is.Not.Null);
            Assert.That(_application, Is.Not.Null);
            Assert.That(_mediator, Is.Not.Null);
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
            IAuthorizationService? _authorization = services.BuildServiceProvider().GetService<IAuthorizationService>();
            Assert.That(_authorization, Is.Not.Null);
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
            IKeyManager? _keyManager = services.BuildServiceProvider().GetService<IKeyManager>();
            // ServiceType = { Name = "ISessionStore" FullName = "Microsoft.AspNetCore.Session.ISessionStore"}
            // ISessionStore _session = services.BuildServiceProvider().GetService<ISessionStore>();
            Assert.That(_keyManager, Is.Not.Null);
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
            ICorsService? _corsService = services.BuildServiceProvider().GetService<ICorsService>();
            Assert.That(_corsService, Is.Not.Null);
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
            IConfigureOptions<MvcOptions>? _swaggerMvc = services.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>();
            // ISwaggerProvider _swagger = services.BuildServiceProvider().GetService<ISwaggerProvider>();
            Assert.That(_swaggerMvc, Is.Not.Null);
        }
        //
    }
}
// ===========================================================================
