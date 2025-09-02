using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        // ✅ Homepage visible to Guests (no login required)
        [HttpGet("homepage")]
        [AllowAnonymous]
        public IActionResult GetHomePage()
        {
            return Ok(new
            {
                message = "Welcome to AI Resume Builder",
                features = new[] { "AI-powered resume suggestions", "Create Resumes","Edit resumes", "Download as PDF" }
            });
        }
    }
}
