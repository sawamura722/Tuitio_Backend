using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class RegistrationService : IRegistrationService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public RegistrationService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync()
        {
            var registrations = await _context.Registrations.ToListAsync();

            return _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
        }

        public async Task<IEnumerable<RegistrationDTO>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            var registrations = await _context.Registrations
                .Include(r => r.Course)
                .Where(r => r.StudentId == studentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
        }

        public async Task<RegistrationDTO> RegisterStudentInCourseAsync(int studentId, int courseId)
        {
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);

            if (existingRegistration != null)
            {
                throw new Exception("Student is already registered in this course.");
            }

            var registration = new Registration
            {
                StudentId = studentId,
                CourseId = courseId,
                RegistrationDate = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return _mapper.Map<RegistrationDTO>(registration);
        }

        public async Task<bool> UnregisterStudentFromCourseAsync(int studentId, int courseId)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);

            if (registration == null) return false;

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RegistrationDTO>> GetStudentsByCourseIdAsync(int courseId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.CourseId == courseId)
                .Include(r => r.Student) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
        }

    }
}
