using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class AuthService : IAuthService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(TutoringSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Register(UserDTO userDto)
        {
            // Validate that the email and username are unique
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                return "Email is already in use";
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                return "Username is already taken";

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            // Get the role ID dynamically
            var userRoleId = await _context.Roles
                .Where(r => r.RoleName == "USER")
                .Select(r => r.RoleId)
                .SingleOrDefaultAsync();

            // Create a new user entity
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = passwordHash,
                RoleId = userRoleId,
                CreatedAt = DateTime.Now
            };

            // Add the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<string> RegisterTeacher(UserDTO userDto)
        {
            // Validate that the email and username are unique
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                return "Email is already in use";
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                return "Username is already taken";

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            // Get the role ID dynamically
            var userRoleId = await _context.Roles
                .Where(r => r.RoleName == "TEACHER")
                .Select(r => r.RoleId)
                .SingleOrDefaultAsync();

            // Create a new user entity
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = passwordHash,
                RoleId = userRoleId,
                CreatedAt = DateTime.Now
            };

            // Add the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Teacher registered successfully";
        }


        public async Task<string> Login(string username, string password)
        {
            // Find the user by email
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return "Invalid credentials";

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return "Bearer " + tokenHandler.WriteToken(token);
        }
    }
}
