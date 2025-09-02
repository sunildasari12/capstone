//using System;
//using System.Threading.Tasks;
//using OpenAI.GPT3;
//using OpenAI.GPT3.Interfaces;
//using OpenAI.GPT3.Managers;
//using OpenAI.GPT3.ObjectModels;
//using OpenAI.GPT3.ObjectModels.RequestModels;

//namespace AiResumeBuilder.Api.Services
//{
//    public class AiService : IAiService
//    {
//        private readonly IOpenAIService _openAi;

//        public AiService(IOpenAIService openAi)
//        {
//            _openAi = openAi ?? throw new ArgumentNullException(nameof(openAi));
//        }

//        public async Task<string> ImproveSummaryAsync(string summary)
//        {
//            if (string.IsNullOrWhiteSpace(summary))
//                return string.Empty;

//            try
//            {
//                var prompt = $"Rewrite the following text as a professional, concise, and grammatically correct resume summary:\n\n{summary}";

//                var response = await _openAi.Completions.CreateCompletion(new CompletionCreateRequest
//                {
//                    Model = Models.TextDavinciV3,
//                    Prompt = prompt,
//                    MaxTokens = 100,
//                    Temperature = 0.7f
//                });

//                return response.Choices.Count > 0 ? response.Choices[0].Text.Trim() : summary;
//            }
//            catch (Exception ex)
//            {
//                // Log error here if needed
//                return summary; // fallback to original text
//            }
//        }

//        public async Task<string> ImproveExperienceAsync(string experience)
//        {
//            if (string.IsNullOrWhiteSpace(experience))
//                return string.Empty;

//            try
//            {
//                var prompt = $"Rewrite the following text to make it a strong, professional experience statement for a resume:\n\n{experience}";

//                var response = await _openAi.Completions.CreateCompletion(new CompletionCreateRequest
//                {
//                    Model = Models.TextDavinciV3,
//                    Prompt = prompt,
//                    MaxTokens = 150,
//                    Temperature = 0.7f
//                });

//                return response.Choices.Count > 0 ? response.Choices[0].Text.Trim() : experience;
//            }
//            catch (Exception ex)
//            {
//                // Log error here if needed
//                return experience; // fallback to original text
//            }
//        }
//    }
//}

namespace AiResumeBuilder.Api.Services
{
    public class AiService : IAiService
    {
        public Task<string> ImproveSummaryAsync(string summary)
        {
            // Placeholder: call OpenAI or other API here.
            var improved = summary + " (improved by AI)";
            return Task.FromResult(improved);
        }

        public Task<string> ImproveExperienceAsync(string experience)
        {
            var improved = experience + " (improved by AI)";
            return Task.FromResult(improved);
        }
    }
}
