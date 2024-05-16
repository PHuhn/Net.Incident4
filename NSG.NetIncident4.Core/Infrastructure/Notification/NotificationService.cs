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
		private readonly IApplication _application;
        ILogger _logger;
		//
		public NotificationService(
			IOptions<Dictionary<string, EmailSettings>> emailSettings,
			ILogger<NotificationService> logger)
		{
            _emailSettingsDict = emailSettings.Value;
			_emailSettings = _emailSettingsDict["Default"];
            _logger = logger;
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
            EmailSettings _tempSettings = _emailSettings;
            EmailSettings? _newSettings = _emailSettingsDict[companyShortName];
			if (_newSettings != null)
			{
				_emailSettings = _newSettings;
                _logger.LogDebug($"SendEmailByCompanyAsync: {companyShortName}\n{_newSettings}");
            }
            else
			{
				string _msg = $"EmailSetting configuration does not contain company {companyShortName}, count: {_emailSettingsDict.Count}";
                _logger.LogError(_msg);
                throw new Exception(_msg);
            }
            try
            {
                Task<MimeKit.MimeMessage> _email = mimeMessage.SendAsync(_emailSettings);
                _logger.LogInformation(_email.Result.EmailToString());
            }
            catch (System.Exception _ex)
            {
                _logger.LogError($"Company: {companyShortName}, {_emailSettings.ToString()}");
                _logger.LogError(_ex.ToString());
                throw new System.InvalidOperationException(_ex.Message);
            }
            _emailSettings = _tempSettings;
            return Task.CompletedTask;
        }
        //
    }
}
// ===========================================================================
