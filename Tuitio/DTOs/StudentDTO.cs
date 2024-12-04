namespace Tuitio.DTOs
{
    public class StudentDTO
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FullName { get; set; }

        public string? ProfileImage { get; set; }
    }
}
