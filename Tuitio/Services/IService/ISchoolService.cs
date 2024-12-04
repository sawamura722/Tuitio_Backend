using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.Models;

namespace Tuitio.Services.IService
{
    public interface ISchoolService
    {
        Task<IEnumerable<School>> GetAllSchoolsAsync();
        Task<School?> GetSchoolByIdAsync(int id);
        Task<School> CreateSchoolAsync(School school);
        Task<bool> UpdateSchoolAsync(int id, School school);
        Task<bool> DeleteSchoolAsync(int id);
    }
}
