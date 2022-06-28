//
// ---------------------------------------------------------------------------
// EmailTemplates create command.
//
using System;
using System.Threading;
using System.Threading.Tasks;
//
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core.Application.Commands.CompanyEmailTemplates
{
	//
	/// <summary>
	/// 'EmailTemplate' create command, handler and handle.
	/// </summary>
	public class CompanyEmailTemplateCreateCommand : IRequest<EmailTemplate>
	{
		public int CompanyId { get; set; }
		public int IncidentTypeId { get; set; }
		public string SubjectLine { get; set; }
		public string EmailBody { get; set; }
		public string TimeTemplate { get; set; }
		public string ThanksTemplate { get; set; }
		public string LogTemplate { get; set; }
		public string Template { get; set; }
		public bool FromServer { get; set; }
		//
		public CompanyEmailTemplateCreateCommand()
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
	}
	//
	/// <summary>
	/// 'EmailTemplate' create command handler.
	/// </summary>
	public class CompanyEmailTemplateCreateCommandHandler : IRequestHandler<CompanyEmailTemplateCreateCommand, EmailTemplate>
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<CompanyEmailTemplateCreateCommandHandler> _logger;
		//
		//
		/// <summary>
		///  The constructor for the inner handler class, to create the EmailTemplate entity.
		/// </summary>
		/// <param name="context">The database interface context.</param>
		public CompanyEmailTemplateCreateCommandHandler(ApplicationDbContext context, ILogger<CompanyEmailTemplateCreateCommandHandler> logger)
		{
			_context = context;
			_logger = logger;
		}
		//
		/// <summary>
		/// 'EmailTemplate' command handle method, passing two interfaces.
		/// </summary>
		/// <param name="request">This create command request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>The EmailTemplate entity class.</returns>
		public async Task<EmailTemplate> Handle(CompanyEmailTemplateCreateCommand request, CancellationToken cancellationToken)
		{
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(request);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new CreateCommandValidationException(_results.FluentValidationErrors());
			}
			var _duplicate = await _context.EmailTemplates
				.SingleOrDefaultAsync(r => r.IncidentTypeId == request.IncidentTypeId, cancellationToken);
			if (_duplicate != null)
			{
				throw new CreateCommandDuplicateException(request.IncidentTypeId);
			}
			_logger.Log(LogLevel.Information, request.EmailBody);
			// Move from create command class to entity class.
			var _entity = new EmailTemplate
			{
				CompanyId = request.CompanyId,
				IncidentTypeId = request.IncidentTypeId,
				SubjectLine = request.SubjectLine,
				EmailBody = request.EmailBody,
				TimeTemplate = request.TimeTemplate,
				ThanksTemplate = request.ThanksTemplate,
				LogTemplate = request.LogTemplate,
				Template = request.Template,
				FromServer = request.FromServer,
			};
			_context.EmailTemplates.Add(_entity);
            try
            {
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException upExc)
			{
				throw _context.HandleDbUpdateException(upExc);
			}
			catch (Exception)
			{
				throw;
			}
			// Return the entity class.
			return _entity;
		}
		//
		/// <summary>
		/// FluentValidation of the 'EmailTemplateCreateCommand' class.
		/// </summary>
		public class Validator : AbstractValidator<CompanyEmailTemplateCreateCommand>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'EmailTemplateCreateCommand' validator.
			/// </summary>
			public Validator()
			{
				//
				RuleFor(x => x.CompanyId).NotNull();
				RuleFor(x => x.IncidentTypeId).NotNull();
				RuleFor(x => x.SubjectLine).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.EmailBody).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.TimeTemplate).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.ThanksTemplate).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.LogTemplate).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.Template).NotEmpty().MaximumLength(1073741823);
				RuleFor(x => x.FromServer).NotNull();
				//
			}
			//
		}
		//
	}
	//
	/// <summary>
	/// Custom EmailTemplateCreateCommand duplicate exception.
	/// </summary>
	public class CreateCommandDuplicateException: Exception
	{
		//
		/// <summary>
		/// Implementation of EmailTemplateCreateCommand duplicate exception.
		/// </summary>
		/// <param name="id">The key for the record.</param>
		public CreateCommandDuplicateException(int id)
			: base($"EmailTemplateCreateCommand duplicate id exception: Id: {id}")
		{
		}
	}
	//
	/// <summary>
	/// Custom EmailTemplateCreateCommand validation exception.
	/// </summary>
	public class CreateCommandValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of EmailTemplateCreateCommand validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public CreateCommandValidationException(string errorMessage)
			: base($"EmailTemplateCreateCommand validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
