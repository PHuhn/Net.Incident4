//
// ---------------------------------------------------------------------------
// Log create command.
//
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
//
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Common;
//
namespace NSG.NetIncident4.Core.Application.Commands.Logs
{
	//
	/// <summary>
	/// 'Log' create command, handler and handle.
	/// </summary>
	public class LogCreateCommand : IRequest<LogData>
	{
        //
        public byte LogLevel { get; set; }
        public string Level { get; set; }
        public string Method { get; set; }
		public string Message { get; set; }
		public string Exception { get; set; }
        //
        public LogCreateCommand(LoggingLevel level, MethodBase? method, string message, Exception? exception = null)
        {
            LogLevel = (byte)level;
            Level = level.GetName();
            Method =  (method == null ? "-unknown-" : method.DeclaringType.FullName + "." + method.Name);
            Message = message;
            Exception = (exception == null ? "" : exception.ToString());
        }
        //
        public LogCreateCommand(byte severity, string method, string message, string exception = "")
        {
            LoggingLevel _logLevel = (LoggingLevel)severity;
            LogLevel = severity;
            Level = _logLevel.GetName();
            if (Level is null)
            {
                Level = $"Level-{severity}";
            }
            Method = method;
            Message = message;
            Exception = (exception == null ? "" : exception);
        }
        //
    }
    //
    /// <summary>
    /// 'Log' create command handler.
    /// </summary>
    public class LogCreateCommandHandler : IRequestHandler<LogCreateCommand, LogData>
	{
		private readonly ApplicationDbContext _context;
        private IApplication _application;
        //
        /// <summary>
        ///  The constructor for the inner handler class, to create the Log entity.
        /// </summary>
        /// <param name="context">The database interface context.</param>
        public LogCreateCommandHandler(ApplicationDbContext context, IApplication application)
        {
			_context = context;
            _application = application;
        }
		//
		/// <summary>
		/// 'Log' command handle method, passing two interfaces.
		/// </summary>
		/// <param name="request">This create command request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>The Log entity class.</returns>
		public async Task<LogData> Handle(LogCreateCommand request, CancellationToken cancellationToken)
		{
			string codeName = "LogCreateCommandHandler.Handle";
			Validator _validator = new Validator();
			ValidationResult _results = _validator.Validate(request);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new CreateCommandValidationException(_results.FluentValidationErrors());
			}
			//
			// Move from create command class to entity class.
			var _entity = new LogData
			{
				Date = _application.Now(),
				Application = _application.GetApplicationName(),
				Method = (request.Method.Length > 255 ? request.Method.Substring(0, 255) : request.Method),
				LogLevel = request.LogLevel,
				Level = request.Level,
				UserAccount = _application.GetUserAccount(),
				Message = (request.Message.Length > 4000 ? request.Message.Substring(0, 4000) : request.Message),
				Exception = request.Exception.Length > 4000 ? request.Exception.Substring(0, 4000) : request.Exception
			};
			try
            {
				_context.Logs.Add(_entity);
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (Exception _ex)
			{
				System.Diagnostics.Debug.WriteLine(_ex.ToString());
				throw (new Exception($"{codeName}: add failed: {_ex.Message}", _ex));
			}
			// Return the entity class.
			return _entity;
		}
		//
		/// <summary>
		/// FluentValidation of the 'LogCreateCommand' class.
		/// </summary>
		public class Validator : AbstractValidator<LogCreateCommand>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'LogCreateCommand' validator.
			/// </summary>
			public Validator()
			{
				//
				//RuleFor(x => x.UserAccount).NotEmpty().MaximumLength(255);
				RuleFor(x => x.Message).NotEmpty();
				//RuleFor(x => x.Exception).MaximumLength(4000);
				//
			}
			//
		}
		//
	}
	//
	/// <summary>
	/// Custom LogCreateCommand validation exception.
	/// </summary>
	public class CreateCommandValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of LogCreateCommand validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public CreateCommandValidationException(string errorMessage)
			: base($"LogCreateCommand validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
