using Koffeebot.Repositories.Models;

namespace Koffeebot.Repositories.Interfaces
{
    public interface IChatApiClient
    {
        Task<string> SendMessageAsync(string userMessage, IEnumerable<OpenApiResponse.Message> history);
    }
}