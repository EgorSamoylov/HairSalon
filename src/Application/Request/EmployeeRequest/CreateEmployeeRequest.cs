using FluentValidation;

namespace Application.Request.EmployeeRequest
{
    public class CreateEmployeeRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? Position { get; set; }
    }

    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxFirstNameLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxLastNameLength);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(ValidationConstants.MaxPhoneNumberLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(ValidationConstants.MaxEmailLength);
            RuleFor(x => x.Position).MaximumLength(ValidationConstants.MaxPositionLength);
        }
    }
}
