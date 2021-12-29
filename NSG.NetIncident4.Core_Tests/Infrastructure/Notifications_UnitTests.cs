using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
//
using Moq;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using MimeKit;
using NSG.NetIncident4.Core_Tests.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class Notification_UnitTests
    {
        //
        public IConfiguration Configuration { get; set;  }
        IOptions<MimeKit.NSG.EmailSettings> emailSettings = null;
        Mock<ILogger<NotificationService>> mockLogger = null;
        //
        public Notification_UnitTests()
        {
            string _appSettings = "appsettings.json";
            if (_appSettings != "")
                if (!File.Exists(_appSettings))
                    throw new FileNotFoundException($"Settings file: {_appSettings} not found.");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
                .Build();
            emailSettings =
                Options.Create<MimeKit.NSG.EmailSettings>(Configuration.GetSection("EmailSettings").Get<MimeKit.NSG.EmailSettings>());
            //
            mockLogger = new Mock<ILogger<NotificationService>>();
            //
        }
        //
        [SetUp]
        public void MySetup()
        {
        }
        //
        [Test()]
        public async Task SendEmailAsyncEmail_Test()
        {
            mockLogger.Reset();
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            await _notificationService.SendEmailAsync("Email@anybody.net", "Email Testing", "Email testing message.");
            // asserts
            // https://adamstorr.azurewebsites.net/blog/mocking-ilogger-with-moq
            // for VerifyLogging see Helpers/MockHelpers.cs
            mockLogger.VerifyLogging(LogLevel.Information, "From: ");
        }
        //
        [Test()]
        public async Task SendEmailAsyncFromTo_Test()
        {
            mockLogger.Reset();
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            await _notificationService.SendEmailAsync("FromTo@site.net", "FromTo@anybody.net", "FromTo Testing", "FromTo testing message.");
            // for VerifyLogging see Helpers/MockHelpers.cs
            mockLogger.VerifyLogging(LogLevel.Information, "From: ");
        }
        //
        [Test()]
        public async Task SendEmailAsyncMimeMessage_Test()
        {
            mockLogger.Reset();
            MimeMessage _mimeMessage = MimeKit.Extensions.NewMimeMessage()
                .From("mm@site.net").To("mm@anybody.net").Subject("mm Testing").Body(MimeKit.Extensions.TextBody("mm testing message."));
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            await _notificationService.SendEmailAsync(_mimeMessage);
            // for VerifyLogging see Helpers/MockHelpers.cs
            mockLogger.VerifyLogging(LogLevel.Information, "From: ");
        }
        //
        [Test()]
        public async Task SendEmailAsyncMimeMessageBad_Test()
        {
            mockLogger.Reset();
            MimeMessage _mimeMessage = MimeKit.Extensions.NewMimeMessage();
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            try
            {
                await _notificationService.SendEmailAsync(_mimeMessage);
            }
            catch (InvalidOperationException _ex)
            {
                Console.WriteLine(_ex.Message);
                mockLogger.VerifyLogLevel(LogLevel.Error);
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message);
                Assert.Fail(_ex.Message);
            }
        }
        //
    }
}
