//
// ---------------------------------------------------------------------------
// NoteType create command.
//
using System;
using System.Threading;
using System.Threading.Tasks;
//
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Application.Commands.NoteTypes
{
	//
	/// <summary>
	/// 'NoteType' create command, handler and handle.
	/// </summary>
	public class NoteTypeCreateCommand : IRequest<NoteType>
	{
		public int NoteTypeId { get; set; }
		public string NoteTypeShortDesc { get; set; }
		public string NoteTypeDesc { get; set; }
		[System.ComponentModel.DataAnnotations.DisplayFormat(ConvertEmptyStringToNull = false)]
		public string NoteTypeClientScript { get; set; }
		//
		public NoteTypeCreateCommand()
        {
			NoteTypeId = 0;
			NoteTypeShortDesc = "";
			NoteTypeDesc = "";
			NoteTypeClientScript = "";
        }
	}
    //
    /// <summary>
    /// 'NoteType' create command handler.
    /// </summary>
    public class NoteTypeCreateCommandHandler : IRequestHandler<NoteTypeCreateCommand, NoteType>
	{
		private readonly ApplicationDbContext _context;
		//
		//
		/// <summary>
		///  The constructor for the inner handler class, to create the NoteType entity.
		/// </summary>
		/// <param name="context">The database interface context.</param>
		public NoteTypeCreateCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}
		//
		/// <summary>
		/// 'NoteType' command handle method, passing two interfaces.
		/// </summary>
		/// <param name="request">This create command request.</param>
		/// <param name="cancellationToken">Cancel token.</param>
		/// <returns>The NoteType entity class.</returns>
		public async Task<NoteType> Handle(NoteTypeCreateCommand request, CancellationToken cancellationToken)
		{
			Validator _validator = new Validator();
			if (request.NoteTypeClientScript == null) request.NoteTypeClientScript = "";
			ValidationResult _results = _validator.Validate(request);
			if (!_results.IsValid)
			{
				// Call the FluentValidationErrors extension method.
				throw new CreateCommandValidationException(_results.FluentValidationErrors());
			}
            // Move from create command class to entity class.
            var _entity = new NoteType
            {
                NoteTypeId = 0,
                NoteTypeShortDesc = request.NoteTypeShortDesc,
                NoteTypeDesc = request.NoteTypeDesc,
                NoteTypeClientScript = request.NoteTypeClientScript.ToLower()
            };
			_context.NoteTypes.Add(_entity);
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
		/// FluentValidation of the 'NoteTypeCreateCommand' class.
		/// </summary>
		public class Validator : AbstractValidator<NoteTypeCreateCommand>
		{
			//
			/// <summary>
			/// Constructor that will invoke the 'NoteTypeCreateCommand' validator.
			/// </summary>
			public Validator()
			{
				//
				RuleFor(x => x.NoteTypeShortDesc).NotEmpty().MaximumLength(8);
				RuleFor(x => x.NoteTypeDesc).NotEmpty().MaximumLength(50);
                RuleFor(x => x.NoteTypeClientScript).MaximumLength(12);
            //
	        }
        //
		}
		//
	}
	//
	/// <summary>
	/// Custom NoteTypeCreateCommand validation exception.
	/// </summary>
	public class CreateCommandValidationException: Exception
	{
		//
		/// <summary>
		/// Implementation of NoteTypeCreateCommand validation exception.
		/// </summary>
		/// <param name="errorMessage">The validation error messages.</param>
		public CreateCommandValidationException(string errorMessage)
			: base($"NoteTypeCreateCommand validation exception: errors: {errorMessage}")
		{
		}
	}
	//
}
// ---------------------------------------------------------------------------
