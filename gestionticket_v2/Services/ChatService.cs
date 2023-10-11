using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace gestionticket_v2.Services
{
    public class OpenAiResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public string text { get; set; }
    }

    public class ChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetResponse(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/engines/davinci-codex/completions", new { prompt = input, max_tokens = 2000 });
            var data = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            return data.choices[0].text;
        }
    }
}
