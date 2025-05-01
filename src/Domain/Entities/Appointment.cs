using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        [Column("client_id")]
        public int ClientId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }
        [Column("appointment_datetime")]
        public DateTime AppointmentDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
