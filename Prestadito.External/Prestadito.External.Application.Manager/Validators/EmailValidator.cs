using FluentValidation;
using Prestadito.External.Application.Dto.Email;

namespace Prestadito.External.Application.Manager.Validators
{
    public class EmailValidator : AbstractValidator<EmailRequest>
    {
        public EmailValidator()
        {
            RuleFor(x => x.correo).NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}