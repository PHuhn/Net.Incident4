//
using System;
using System.Threading.Tasks;
//
namespace NSG.NetIncident4.Core.Infrastructure.Notification
{
	public interface INotificationService
	{
		Task SendEmailAsync(string email, string subject, string message);
		Task SendEmailAsync(string to, string from, string subject, string message);
		Task SendEmailAsync(MimeKit.MimeMessage mimeMessage);
        Task SendEmailAsync(MimeKit.MimeMessage mimeMessage, string companyShortName);
    }
}
//
