//
// ---------------------------------------------------------------------------
// EmailTemplate insert duplicate validation.
//
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Entity class of a company specific e-mail templates.
    /// </summary>
    public class EmailTemplate
    {
        //
        [Required(ErrorMessage = "CompanyId is required.")]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "IncidentTypeId is required.")]
        public int IncidentTypeId { get; set; }
        [Required(ErrorMessage = "SubjectLine is required."), MaxLength(1073741823, ErrorMessage = "'SubjectLine' must be 1073741823 or less characters.")]
        public string SubjectLine { get; set; }
        [Required(ErrorMessage = "EmailBody is required."), MaxLength(1073741823, ErrorMessage = "'EmailBody' must be 1073741823 or less characters.")]
        public string EmailBody { get; set; }
        [Required(ErrorMessage = "TimeTemplate is required."), MaxLength(1073741823, ErrorMessage = "'TimeTemplate' must be 1073741823 or less characters.")]
        public string TimeTemplate { get; set; }
        [Required(ErrorMessage = "ThanksTemplate is required."), MaxLength(1073741823, ErrorMessage = "'ThanksTemplate' must be 1073741823 or less characters.")]
        public string ThanksTemplate { get; set; }
        [Required(ErrorMessage = "LogTemplate is required."), MaxLength(1073741823, ErrorMessage = "'LogTemplate' must be 1073741823 or less characters.")]
        public string LogTemplate { get; set; }
        [Required(ErrorMessage = "Template is required."), MaxLength(1073741823, ErrorMessage = "'Template' must be 1073741823 or less characters.")]
        public string Template { get; set; }
        [Required(ErrorMessage = "FromServer is required.")]
        public bool FromServer { get; set; }
        //
        public Company? Company { get; set;  }
        public IncidentType? IncidentType { get; set; }
        //
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EmailTemplate()
        {
            CompanyId = 0;
            IncidentTypeId = 0;
            SubjectLine = "";
            EmailBody = "";
            TimeTemplate = "";
            ThanksTemplate = "";
            LogTemplate = "";
            Template = "";
            FromServer = false;
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("CompanyId: {0}, ", CompanyId.ToString());
            _return.AppendFormat("IncidentTypeId: {0}, ", IncidentTypeId.ToString());
            _return.AppendFormat("SubjectLine: {0}, ", SubjectLine);
            _return.AppendFormat("EmailBody: {0}, ", EmailBody);
            _return.AppendFormat("TimeTemplate: {0}, ", TimeTemplate);
            _return.AppendFormat("ThanksTemplate: {0}, ", ThanksTemplate);
            _return.AppendFormat("LogTemplate: {0}, ", LogTemplate);
            _return.AppendFormat("Template: {0}, ", Template);
            _return.AppendFormat("FromServer: {0}, ", FromServer.ToString());
            _return.AppendFormat("]");
            return _return.ToString();
        }
    }
}
// ---------------------------------------------------------------------------
