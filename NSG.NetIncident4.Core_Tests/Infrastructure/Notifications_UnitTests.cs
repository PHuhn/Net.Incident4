// ===========================================================================
// File: Notification_UnitTests.cs
using NUnit.Framework;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//
using Moq;
using MimeKit;
//
using MimeKit.NSG;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class Notification_UnitTests
    {
        /*
        ** !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        **
        ** This test requires some ability to send emails.
        ** I use fake-smtp-server, which is a nodejs application.
        ** The project does have a 'fake-smtp.bat', that invokes
        ** the globally installed node package.
        **
        ** !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        */
        public IConfiguration Configuration { get; set;  }
        IOptions<Dictionary<string, EmailSettings>> emailSettings = null;
        Mock<ILogger<NotificationService>> mockLogger = null;
        //
        public Notification_UnitTests()
        {
            Console.WriteLine("Notification_UnitTests ...");
            string _appSettings = "appsettings.json";
            if (_appSettings != "")
                if (!File.Exists(_appSettings))
                    throw new FileNotFoundException($"Settings file: {_appSettings} not found.");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(_appSettings, optional: true, reloadOnChange: false)
                .AddUserSecrets<Notification_UnitTests>()
                .Build();
            emailSettings =
                Options.Create<Dictionary<string, EmailSettings>>(Configuration.GetSection("EmailSettings").Get<Dictionary<string, EmailSettings>>());
            Console.WriteLine( Configuration.GetSection("EmailSettings").Get<Dictionary<string, EmailSettings>>() );
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
        public void SetCompanyNameEmailSettings_Default_Test()
        {
            // given
            Console.WriteLine("SetCompanyNameEmailSettings_Default_Test ...");
            mockLogger.Reset();
            // when
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            // then
            EmailSettings current = _notificationService.CurrentEmailSettings;
            Assert.That<EmailSettings>(current, Is.Not.Null);
            Console.WriteLine(current.ToString());
            Assert.That<int>(current.SmtpPort, Is.Not.EqualTo(0));
        }
        //
        [Test()]
        public void SetCompanyNameEmailSettings_NSG_Test()
        {
            // given
            Console.WriteLine("SetCompanyNameEmailSettings_NSG_Test ...");
            mockLogger.Reset();
            // when
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object, "NSG");
            // then
            EmailSettings current = _notificationService.CurrentEmailSettings;
            Assert.That<EmailSettings>(current, Is.Not.Null);
            Console.WriteLine(current.ToString());
            Assert.That<int>(current.SmtpPort, Is.Not.EqualTo(0));
        }
        //
        [Test()]
        public void SetCompanyNameEmailSettings_Bad_Test()
        {
            // given
            Console.WriteLine("SetCompanyNameEmailSettings_Bad_Test ...");
            mockLogger.Reset();
            try
            {
                INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object, "Bad");
            }
            catch (KeyNotFoundException _ex)
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
        [Test()]
        public async Task SendEmailAsyncEmail_Test()
        {
            // given
            Console.WriteLine("SendEmailAsyncEmail_Test ...");
            mockLogger.Reset();
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            // when
            await _notificationService.SendEmailAsync("Email@anybody.net", "Email Testing", "Email testing message.");
            // then
            // https://adamstorr.azurewebsites.net/blog/mocking-ilogger-with-moq
            // for VerifyLogging see Helpers/MockHelpers.cs
            mockLogger.VerifyLogging(LogLevel.Information, "From: ");
        }
        //
        [Test()]
        public async Task SendEmailAsyncFromTo_Test()
        {
            // given
            Console.WriteLine("SendEmailAsyncFromTo_Test ...");
            mockLogger.Reset();
            INotificationService _notificationService = new NotificationService(emailSettings, mockLogger.Object);
            try
            {
                Console.WriteLine(_notificationService.CurrentEmailSettings.SmtpHost);
                Console.WriteLine(_notificationService.CurrentEmailSettings.SmtpPort);
                await _notificationService.SendEmailAsync("FromTo@site.net", "FromTo@anybody.net", "FromTo Testing", "FromTo testing message.");
                Console.WriteLine("Finished SendEmailAsync");
                // for VerifyLogging see Helpers/MockHelpers.cs
                mockLogger.VerifyLogging(LogLevel.Information, "From: ");
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message);
                mockLogger.VerifyLogLevel(LogLevel.Error);
                Console.WriteLine(_ex.InnerException != null ? _ex.InnerException.Message: "No inner");
                Assert.Fail(_ex.Message);
            }
        }
        //
        [Test()]
        public async Task SendEmailAsyncMimeMessage_Test()
        {
            // given
            Console.WriteLine("SendEmailAsyncMimeMessage_Test ...");
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
            // given
            Console.WriteLine("SendEmailAsyncMimeMessageBad_Test ...");
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
// ===========================================================================
