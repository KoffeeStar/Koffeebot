using Koffeebot.Repositories.Models;

namespace Koffeebot.Repositories.Interfaces
{
    public interface IChatModelRepository
    {
        Task<List<OpenApiResponse.Message>> GetHistoryAsync(long chatId);
        Task AddMessageAsync(long chatId, OpenApiResponse.Message message);
    }
}