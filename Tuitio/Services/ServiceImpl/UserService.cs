using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class UserService : IUserService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public UserService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDTO>(user);
        }


        public async Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            // Handle the profile image
            if (updateUserDTO.Image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(user.ProfileImage))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ProfileImages", user.ProfileImage);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                var imageFileName = Guid.NewGuid() + Path.GetExtension(updateUserDTO.Image.FileName);
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ProfileImages");
                var imageFilePath = Path.Combine(imagesFolder, imageFileName);

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Save the new image file
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await updateUserDTO.Image.CopyToAsync(stream);
                }

                user.ProfileImage = imageFileName;
            }

            // Update other properties
            user.Username = updateUserDTO.Username;

            // Hash the password if it has been changed
            if (!string.IsNullOrEmpty(updateUserDTO.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDTO.Password);
            }

            user.Email = updateUserDTO.Email;
            user.FullName = updateUserDTO.FullName;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
    }
}
