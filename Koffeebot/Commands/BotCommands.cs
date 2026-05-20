using Koffeebot.Dtos;
using Koffeebot.Repositories.Interfaces;
using Koffeebot.Repositories.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Koffeebot.Commands
{
    public interface IBotCommand
    {
        string Trigger { get; }
        Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId);
    }

    public class StartCommand : IBotCommand
    {
        public string Trigger => "/start";
        public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
        {
            await bot.SendMessage(chatId, "Привет! Я OpenAI-бот. Отправь сообщение —— я передам его сторонней модели и верну ответ.\n/help для списка команд.");
        }
    }

    public class HelpCommand : IBotCommand
    {
        public string Trigger => "/help";
        public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
        {
            await bot.SendMessage(chatId, "Список доступных команд:\n/start - начало работы.");
        }
    }

    public class TelegramUpdateProcessor
    {
        private readonly IEnumerable<IBotCommand> _commands;
        private readonly ITelegramBotClient _botClient;
        private readonly IChatApiClient _chatClient;
        private readonly IChatModelRepository _chatModelRepository;

        public TelegramUpdateProcessor(
            IEnumerable<IBotCommand> commands,
            ITelegramBotClient botClient,
            IChatApiClient chatClient,
            IChatModelRepository chatModelRepository)
        {
            _commands = commands;
            _botClient = botClient;
            _chatClient = chatClient;
            _chatModelRepository = chatModelRepository;
        }

        public async Task HandleAsync(TelegramUpdate update)
        {
            if (update.Message == null)
                return;

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text?.Trim();

            if (string.IsNullOrEmpty(text))
                return;

            if (text.StartsWith("/"))
            {
                var cmd = text.Split(' ', 2)[0];
                var command = _commands.FirstOrDefault(c => c.Trigger.Equals(cmd, StringComparison.OrdinalIgnoreCase));
                if (command != null)
                {
                    await command.ExecuteAsync(update, _botClient, chatId);
                    return;
                }
                else
                {
                    await _botClient.SendMessage(chatId, "Неизвестная команда. Используйте /help");
                    return;
                }
            }

            await _chatModelRepository.AddMessageAsync(chatId, new OpenApiResponse.Message { Role = "user", Content = text });
            var history = await _chatModelRepository.GetHistoryAsync(chatId);

            try
            {
                var result = await _chatClient.SendMessageAsync(text, history);
                await _chatModelRepository.AddMessageAsync(chatId, new OpenApiResponse.Message { Role = "assistant", Content = result });
                await _botClient.SendMessage(chatId, result);
            }
            catch (Exception ex)
            {
                await _botClient.SendMessage(chatId, "Ошибка при вызове Chat API: " + ex.Message);
            }
        }
    }
}