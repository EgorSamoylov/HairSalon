using FluentValidation;

namespace Application.Request.EmployeeRequest
{
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Position { get; set; }
    }

    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("FirstName has 100 max length");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("LastName has 100 max length");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(24).WithMessage("PhoneNumber has 24 max length");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).WithMessage("Email has 100 max length");
            RuleFor(x => x.Position).MaximumLength(255).WithMessage("Position has 255 max length");
        }
    }
}
