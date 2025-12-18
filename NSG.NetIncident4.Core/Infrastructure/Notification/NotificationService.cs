// ===========================================================================
// File: NotificationService.cs
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
//
using MimeKit;
using MimeKit.NSG;
using NSG.NetIncident4.Core.Infrastructure.Common;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
	//
	// ASP.NET Core 2.2 - SMTP EmailSender Implementation
	// https://kenhaggerty.com/articles/article/aspnet-core-22-smtp-emailsender-implementation
	//
	public class NotificationService : INotificationService, IEmailSender
	{
		//private readonly IHostingEnvironment _env;
		//IHostingEnvironment env,
		//_env = env;
		private EmailSettings _emailSettings;
        private readonly Dictionary<string, MimeKit.NSG.EmailSettings> _emailSettingsDict;
        ILogger _logger;
		//
		public NotificationService(
			IOptions<Dictionary<string, EmailSettings>> emailSettings,
			ILogger<NotificationService> logger,
            string companyShortName = "Default")
		{
            _logger = logger;
            _emailSettingsDict = emailSettings.Value;
			// _emailSettings = _emailSettingsDict["Default"];
            SetCompanyNameEmailSettings(companyShortName);
        }
		// just a getter ... can't set a value
		/// <summary>
		/// Return the current email settings fot this instance
		/// </summary>
        public EmailSettings CurrentEmailSettings
        {
            get { return _emailSettings; }
        }
        //
        /// <summary>
        /// The JSON config file for EmailSettings is as follows:
        /// <code>
        /// {
        ///  "group-n": {
        ///    ...
        ///  },
        ///  "EmailSettings": {
        ///    "Default": {
        ///      "SmtpHost": "smtp.gmail.com",
        ///    ...
        ///    },
        ///    "NSG": {
        ///      "SmtpHost": "smtp.mail.yahoo.com",
        ///    ...
        ///    },
        ///    "Company-1": {
        ///      "SmtpHost": "smtp.gmail.com",
        ///    ...
        ///    },
        ///    "Company-n": {
        ///      "SmtpHost": "smtp.gmail.com",
        ///    ...
        ///    }
        ///  }
        /// }
        /// </code>
        /// The index in EmailSettings is the company short name.  So, set the
        /// current EmailSettings appropriately.
        /// </summary>
        /// <param name="companyShortName"></param>
        /// <exception cref="Exception"></exception>
        private void SetCompanyNameEmailSettings(string companyShortName)
        {
            _logger.LogDebug($"CompanyNameSettings: Entered with: {companyShortName}");
            try
            {
                EmailSettings? _newSettings = _emailSettingsDict[companyShortName];
                _emailSettings = _newSettings;
                _logger.LogDebug($"CompanyNameSettings: {companyShortName}\n{_newSettings}");
            }
            catch (System.Exception _ex)
            {
                string _msg = $"CompanyNameSettings: EmailSetting configuration does not contain company {companyShortName}, count: {_emailSettingsDict.Count}";
                _logger.LogError(_msg);
                _emailSettings = new EmailSettings();
                throw new KeyNotFoundException(_msg);
            }
            return;
        }
        //
        /// <summary>
        /// For account controller communications, such as:
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns>void/complete</returns>
        public Task SendEmailAsync(string email, string subject, string message)
		{
			return SendEmailAsync(MimeKit.Extensions.NewMimeMessage()
				.From(_emailSettings.UserEmail, _emailSettings.UserName).To(email)
				.Subject(subject).Body(MimeKit.Extensions.TextBody(message)));
		}
		//
		/// <summary>
		/// For general communications, such as:
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns>void/complete</returns>
		public Task SendEmailAsync(string from, string to, string subject, string message)
		{
			return SendEmailAsync(MimeKit.Extensions.NewMimeMessage()
				.From(from).To(to).Subject(subject).Body(MimeKit.Extensions.TextBody(message)));
		}
		//
		/// <summary>
		/// For very general communications, including attachments and html messages.
		/// </summary>
		/// <param name="mimeMessage"></param>
		/// <returns>void/complete</returns>
		public Task SendEmailAsync(MimeKit.MimeMessage mimeMessage)
		{
			try
			{
				Task<MimeKit.MimeMessage> _email = mimeMessage.SendAsync(_emailSettings);
				_logger.LogInformation(_email.Result.EmailToString());
			}
			catch (System.Exception _ex)
			{
				_logger.LogError(_ex.ToString());
				throw new System.InvalidOperationException(_ex.Message);
			}
			return Task.CompletedTask;
		}
        //
        /// <summary>
        /// For very general communications, including attachments and html messages.
        /// </summary>
        /// <param name="mimeMessage"></param>
        /// <returns>void/complete</returns>
        public Task SendEmailAsync( MimeKit.MimeMessage mimeMessage, string companyShortName )
        {
            _logger.LogDebug($"SendEmailByCompanyAsync: Entered with: {companyShortName}");
            try
            {
                SetCompanyNameEmailSettings(companyShortName);
                Task<MimeKit.MimeMessage> _email = mimeMessage.SendAsync(_emailSettings);
                _logger.LogInformation(_email.Result.EmailToString());
            }
            catch (System.Exception _ex)
            {
                _logger.LogError($"Company: {companyShortName}, {_emailSettings.ToString()}");
                _logger.LogError(_ex.ToString());
                throw new System.InvalidOperationException(_ex.Message);
            }
            return Task.CompletedTask;
        }
        //
    }
}
// ===========================================================================
