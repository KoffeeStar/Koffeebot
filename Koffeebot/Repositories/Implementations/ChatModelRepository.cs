using Microsoft.OpenApi.Models;
using Koffeebot.Repositories.Interfaces;
using Koffeebot.Repositories.Models;
using System.Collections.Concurrent;

namespace Koffeebot.Repositories.Implementations
{
    public class ChatModelRepository : IChatModelRepository
    {
        private readonly ConcurrentDictionary<long, List<OpenApiResponse.Message>> _store = new();

        public Task AddMessageAsync(long chatId, OpenApiResponse.Message message)
        {
            var list = _store.GetOrAdd(chatId, _ => new List<OpenApiResponse.Message>());
            lock (list)
            {
                list.Add(message);
            }
            return Task.CompletedTask;
        }

        public Task<List<OpenApiResponse.Message>> GetHistoryAsync(long chatId)
        {
            _store.TryGetValue(chatId, out var list);
            return Task.FromResult(list == null ? new List<OpenApiResponse.Message>() : new List<OpenApiResponse.Message>(list));
        }
    }
}