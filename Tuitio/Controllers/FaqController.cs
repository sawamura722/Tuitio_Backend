using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> GetAllFAQs()
        {
            var faqs = await _faqService.GetAllFAQsAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null)
                return NotFound();

            return Ok(faq);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Faq>> CreateFAQ(Faq faq)
        {
            var createdFaq = await _faqService.CreateFAQAsync(faq);
            return CreatedAtAction(nameof(GetFAQById), new { id = createdFaq.FaqId }, createdFaq);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFAQ(int id, Faq faq)
        {
            if (id != faq.FaqId)
                return BadRequest();

            var updated = await _faqService.UpdateFAQAsync(faq);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFAQ(int id)
        {
            var deleted = await _faqService.DeleteFAQAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
