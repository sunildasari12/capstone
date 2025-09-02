using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AiResumeBuilder.Api.Services;
using System.Text;
using System.Threading.Tasks;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        public ExportController(IResumeService resumeService) => _resumeService = resumeService;

        // GET: api/export/pdf/{resumeId}
        [HttpGet("pdf/{resumeId}")]
        public async Task<IActionResult> GetPdf(int resumeId)
        {
            var r = await _resumeService.GetByIdAsync(resumeId);
            if (r == null) return NotFound();

            // Simple text-based PDF stub (replace with QuestPDF/DinkToPdf/iText in production)
            var content = $"Resume: {r.Title}\n\nSummary:\n{r.Summary}";
            var bytes = Encoding.UTF8.GetBytes(content);

            return File(bytes, "application/pdf", $"resume_{resumeId}.pdf");
        }
    }
}
