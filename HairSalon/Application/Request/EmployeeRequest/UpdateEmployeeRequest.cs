using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.EmployeeRequest
{
    public class UpdateEmployeeRequest
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
    }

    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().GreaterThan(0).WithMessage("EmployeeId must be positive");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("FirstName has 100 max length");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("LastName has 100 max length");
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(24).WithMessage("PhoneNumber has 24 max length");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).WithMessage("Email has 100 max length");
            RuleFor(x => x.Position).MaximumLength(255).WithMessage("Position has 255 max length");
        }
    }
}
