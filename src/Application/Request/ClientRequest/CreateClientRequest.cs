using FluentValidation;

namespace Application.Request.ClientRequest
{
    public class CreateClientRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? Note { get; set; }
    }

    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxFirstNameLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxLastNameLength);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(ValidationConstants.MaxPhoneNumberLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(ValidationConstants.MaxEmailLength);
            RuleFor(x => x.Note).MaximumLength(ValidationConstants.MaxNotesLength);
        }
    }
}
