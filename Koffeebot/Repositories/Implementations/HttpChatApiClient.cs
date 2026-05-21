using Koffeebot.Repositories.Interfaces;
using Koffeebot.Repositories.Models;
using Koffeebot.Settings;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Koffeebot.Repositories.Implementations
{
    public class HttpChatApiClient : IChatApiClient
    {
        private readonly HttpClient _httpClient;

        public HttpChatApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ChatApiSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChatApiSettings.ApiKey);
        }

        public async Task<string> SendMessageAsync(string userMessage, IEnumerable<OpenApiResponse.Message> history)
        {
            var messagesList = history.ToList();

            Console.WriteLine("=== Sending to OpenRouter ===");
            foreach (var msg in messagesList)
            {
                Console.WriteLine($"[{msg.Role}]: {msg.Content}");
            }
            Console.WriteLine("===========================");

            var payload = new OpenApiRequest()
            {
                Model = ChatApiSettings.DefaultModel,
                Messages = messagesList,
                MaxTokens = 1000
            };

            var response = await _httpClient.PostAsJsonAsync("", payload);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<OpenApiResponse?>();
            if (body?.Choices != null && body.Choices.Length > 0)
            {
                return body.Choices[0].Message.Content;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}