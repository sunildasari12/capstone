using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AiResumeBuilder.Api.Services;
using System.Threading.Tasks;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AiController : ControllerBase
    {
        private readonly IAiService _ai;

        public AiController(IAiService ai)
        {
            _ai = ai;
        }

        // POST: api/ai/improve-summary
        [HttpPost("improve-summary")]
        public async Task<IActionResult> ImproveSummary([FromBody] string summary)
        {
            var improved = await _ai.ImproveSummaryAsync(summary);
            return Ok(new { improved });
        }

        // POST: api/ai/improve-experience
        [HttpPost("improve-experience")]
        public async Task<IActionResult> ImproveExperience([FromBody] string experience)
        {
            var improved = await _ai.ImproveExperienceAsync(experience);
            return Ok(new { improved });
        }
    }
}
