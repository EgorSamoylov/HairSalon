using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class RegistrationRequest
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }
    }

    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(ValidationConstants.MinPasswordLength);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
