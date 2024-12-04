using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class FaqService : IFaqService
    {
        private readonly TutoringSchoolContext _context;

        public FaqService(TutoringSchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faq>> GetAllFAQsAsync()
        {
            return await _context.Faqs.ToListAsync();
        }

        public async Task<Faq> GetFAQByIdAsync(int id)
        {
            return await _context.Faqs.FindAsync(id);
        }

        public async Task<Faq> CreateFAQAsync(Faq faq)
        {
            _context.Faqs.Add(faq);
            await _context.SaveChangesAsync();
            return faq;
        }

        public async Task<bool> UpdateFAQAsync(Faq faq)
        {
            _context.Entry(faq).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteFAQAsync(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
                return false;

            _context.Faqs.Remove(faq);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
