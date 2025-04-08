using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
