using System.Threading.Tasks;

namespace AiResumeBuilder.Api.Services
   
{
    // This is a simple AI service stub. Integrate OpenAI or other provider inside.
    public interface IAiService
    {
        Task<string> ImproveSummaryAsync(string summary);
        Task<string> ImproveExperienceAsync(string experience);
    }
}
