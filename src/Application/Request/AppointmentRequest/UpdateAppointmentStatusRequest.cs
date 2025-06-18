namespace Application.Request.AppointmentRequest
{
    public class UpdateAppointmentStatusRequest
    {
        public bool? IsCompleted { get; set; }
        public bool? IsCancelled { get; set; }
    }
}
