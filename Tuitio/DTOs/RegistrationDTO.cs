namespace Tuitio.DTOs
{
    public class RegistrationDTO
    {
        public int? RegistrationId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public DateTime? RegistrationDate { get; set; }
        public UserDTO Student { get; set; }
    }
}
