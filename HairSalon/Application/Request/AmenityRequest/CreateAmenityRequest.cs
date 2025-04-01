using FluentValidation;

namespace Application.Request.AmenityRequest
{
    public class CreateAmenityRequest
    {
        public string serviceName { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int Price { get; set; }
        public int DurationMinutes { get; set; }
    }

    public class CreateAmenityRequestValidator : AbstractValidator<CreateAmenityRequest>
    {
        public CreateAmenityRequestValidator()
        {
            RuleFor(x => x.serviceName).NotEmpty().MaximumLength(100).WithMessage("{PropertyName} has 100 max length");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
            RuleFor(x => x.AuthorId).NotEmpty().GreaterThan(0).WithMessage("AuthorId must be positive")
                                               .LessThan(int.MaxValue).WithMessage("AuthorId is too long");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be positive")
                                            .LessThan(int.MaxValue).WithMessage("Price is too big");
            RuleFor(x => x.DurationMinutes).NotEmpty().GreaterThan(0).WithMessage("Duration minutes must be positive")
                                            .LessThan(int.MaxValue).WithMessage("Duration minutes is too big");
        }
    }
}
