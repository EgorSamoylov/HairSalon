using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.AppointmentRequest
{
    public class CreateAppointmentRequest
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int AmenityId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().GreaterThan(0).WithMessage("ClientId must be positive")
                                               .LessThan(int.MaxValue).WithMessage("clientId is too long");
            RuleFor(x => x.EmployeeId).NotEmpty().GreaterThan(0).WithMessage("EmployeeId must be positive")
                                               .LessThan(int.MaxValue).WithMessage("EmployeeId is too long");
            RuleFor(x => x.AmenityId).NotEmpty().GreaterThan(0).WithMessage("AmenityId must be positive")
                                               .LessThan(int.MaxValue).WithMessage("AmenityId is too long");
            RuleFor(x => x.AppointmentDateTime).NotEmpty().GreaterThan(DateTime.Now).WithMessage("AppointmentDateTime must be in the future");
            RuleFor(x => x.Notes).MaximumLength(ValidationConstants.MaxNotesLength);
        }
    }
}
