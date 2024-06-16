using Microsoft.Extensions.Options;
using OpenAIDemo.Configurations;

namespace OpenAIDemo.Services
{
    public class OpenAiService:IOpeAiService
    {
        private readonly OpenAiConfig _openAiConfig;
        
        public OpenAiService(IOptionsMonitor<OpenAiConfig> openAiConfig)
        {
            _openAiConfig=openAiConfig.CurrentValue;
        }

        public Task<string> GetCompletion(string prompt)
        {
            var api=new OpenAI_API.OpenAIAPI(_openAiConfig.ApiKey);
            var result= api.Completions.GetCompletion(prompt);
            return result;
        }
    }

    public interface IOpeAiService
    {
        Task<string> GetCompletion(string prompt);
    }
}