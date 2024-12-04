namespace Tuitio.DTOs
{
    public class UpdateUserDTO
    {
        public int? UserId { get; set; }

        public string Username { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FullName { get; set; }

        public string? ProfileImage { get; set; }
        public IFormFile? Image { get; set; }
    }
}
