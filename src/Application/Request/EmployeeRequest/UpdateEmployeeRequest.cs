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
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? Position { get; set; }
    }

    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().GreaterThan(0).WithMessage("EmployeeId must be positive");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxFirstNameLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxLastNameLength);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(ValidationConstants.MaxPhoneNumberLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(ValidationConstants.MaxEmailLength);
            RuleFor(x => x.Position).MaximumLength(ValidationConstants.MaxPositionLength);
        }
    }
}
