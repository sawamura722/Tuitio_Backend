using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.Models;

namespace Tuitio.Services.IService
{
    public interface IFaqService
    {
        Task<IEnumerable<Faq>> GetAllFAQsAsync();
        Task<Faq> GetFAQByIdAsync(int id);
        Task<Faq> CreateFAQAsync(Faq faq);
        Task<bool> UpdateFAQAsync(Faq faq);
        Task<bool> DeleteFAQAsync(int id);
    }
}
