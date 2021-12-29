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

        //ConfigureLoggingServices(services);
        //ConfigureViewLocation(services);
        //ConfigureNotificationServices(services);
        //ConfigureAuthorizationPolicyServices(services);
        //ConfigureSessionServices(services);
        //ConfigureCorsServices(services);
        //ConfigureSwaggerServices(services);

        //
        [Test]
        public void Startup_LoggingService_Test()
        {
            Console.WriteLine("LoggingService_Test");
            ServiceCollection services = new ServiceCollection();
            _startUp.ConfigureLoggingServices(services);
            ILoggerFactory _loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();
            ILoggerProvider _loggerProvider = services.BuildServiceProvider().GetService<ILoggerProvider>();
            //Microsoft.Extensions.Logging.Console.ConsoleLogger _consoleLoggerProvider = services.BuildServiceProvider().GetService<ILoggerProviderConfiguration<ConsoleLogger>>();
            //ILoggerProviderConfigurationFactory < ConsoleLogger >> _loggerProvider = services.BuildServiceProvider().GetService<ILoggerProviderConfiguration<ConsoleLogger>>();
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
            // ImplementationInstance = {Microsoft.Extensions.Options.ConfigureNamedOptions<Microsoft.AspNetCore.Mvc.RazorPages.RazorPagesOptions>}
            IConfigureOptions<RazorPagesOptions> _configureRazorPages = services.BuildServiceProvider().GetService<IConfigureOptions<RazorPagesOptions>>();
            // ImplementationInstance = {Microsoft.Extensions.Options.ConfigureNamedOptions<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>}
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
            //services.Configure<ServicesSettings>(Configuration.GetSection("ServicesSettings"));
            //services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            //AuthSettings _authSettings = Options.Create<AuthSettings>(
            //    Configuration.GetSection("AuthSettings").Get<AuthSettings>()).Value;
            //services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            // services.Configure<MimeKit.NSG.EmailSettings>(Configuration.GetSection("EmailSettings"));
            // ImplementationInstance = { Microsoft.Extensions.Options.ConfigurationChangeTokenSource<NSG.NetIncident4.Core.Infrastructure.Services.ServicesSettings>}
            ServicesSettings _serviceSettings = services.BuildServiceProvider().GetService<ServicesSettings>();
            ApplicationSettings _applicationSettings = services.BuildServiceProvider().GetService<ApplicationSettings>();
            AuthSettings _authSettings = services.BuildServiceProvider().GetService<AuthSettings>();
            //
            INotificationService _notification = services.BuildServiceProvider().GetService<INotificationService>();
            MimeKit.NSG.EmailSettings _emailSender = services.BuildServiceProvider().GetService<MimeKit.NSG.EmailSettings>();
            IApplication _application = services.BuildServiceProvider().GetService<IApplication>();
            //
            // Assert.NotNull(_serviceSettings);
            Assert.NotNull(_notification);
            Assert.NotNull(_emailSender);
            Assert.NotNull(_application);
        }
        //
        //
    }
}