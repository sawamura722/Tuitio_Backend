using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class SchoolService : ISchoolService
    {
        private readonly TutoringSchoolContext _context;

        public SchoolService(TutoringSchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            return await _context.Schools.ToListAsync();
        }

        public async Task<School?> GetSchoolByIdAsync(int id)
        {
            return await _context.Schools.FindAsync(id);
        }

        public async Task<School> CreateSchoolAsync(School school)
        {
            _context.Schools.Add(school);
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task<bool> UpdateSchoolAsync(int id, School school)
        {
            var existingSchool = await _context.Schools.FindAsync(id);
            if (existingSchool == null)
            {
                return false;
            }

            existingSchool.SchoolTitle = school.SchoolTitle;
            existingSchool.Location = school.Location;
            existingSchool.Telephone = school.Telephone;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSchoolAsync(int id)
        {
            var school = await _context.Schools.FindAsync(id);
            if (school == null)
            {
                return false;
            }

            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
