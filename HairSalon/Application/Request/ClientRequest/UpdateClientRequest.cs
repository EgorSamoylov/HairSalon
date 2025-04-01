using FluentValidation;

namespace Application.Request.ClientRequest
{
    public class UpdateClientRequest
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().GreaterThan(0).WithMessage("ClientId must be positive");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("FirstName has 100 max length");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("LastName has 100 max length");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(24).WithMessage("PhoneNumber has 24 max length");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).WithMessage("Email has 100 max length");
            RuleFor(x => x.Note).MaximumLength(255).WithMessage("Note has 255 max length");
        }
    }
}
