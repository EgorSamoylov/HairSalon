﻿using FluentValidation;

namespace Application.Request.AmenityRequest
{
    public class UpdateAmenityRequest
    {
        public int AmenityId { get; set; }
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public int AuthorId { get; set; }
        public int Price { get; set; }
        public int DurationMinutes { get; set; }
    }

    public class UpdateAmenityRequestValidator : AbstractValidator<UpdateAmenityRequest>
    {
        public UpdateAmenityRequestValidator()
        {
            RuleFor(x => x.AmenityId).NotEmpty().GreaterThan(0).WithMessage("AmenityId must be positive");
            RuleFor(x => x.ServiceName).NotEmpty().MaximumLength(ValidationConstants.MaxServiceNameLength);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(ValidationConstants.MaxDescriptionLength);
            RuleFor(x => x.AuthorId).NotEmpty().GreaterThan(0).WithMessage("AuthorId must be positive")
                                               .LessThan(int.MaxValue).WithMessage("AuthorId is too long");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be positive")
                                            .LessThan(int.MaxValue).WithMessage("Price is too big");
            RuleFor(x => x.DurationMinutes).NotEmpty().GreaterThan(0).WithMessage("Duration minutes must be positive")
                                            .LessThan(int.MaxValue).WithMessage("Duration minutes is too big");
        }
    }
}
