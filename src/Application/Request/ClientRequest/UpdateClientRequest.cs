using FluentValidation;

namespace Application.Request.ClientRequest
{
    public class UpdateClientRequest
    {
        public int ClientId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().GreaterThan(0).WithMessage("ClientId must be positive");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxFirstNameLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxLastNameLength);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(ValidationConstants.MaxPhoneNumberLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(ValidationConstants.MaxEmailLength);
            RuleFor(x => x.Note).MaximumLength(ValidationConstants.MaxNotesLength);
        }
    }
}
