using Koffeebot.Dto;

namespace Koffeebot.Dto
{
    public class TelegramChat
    {
        public long Id { get; set; }
    }
}
namespace Koffeebot.Dtos
{
    public class TelegramMessage
    {
        public int MessageId { get; set; }
        public TelegramChat Chat { get; set; } = new TelegramChat();
        public string? Text { get; set; }
        public int Date { get; set; }
    }
}


namespace Koffeebot.Dtos
{
    public class TelegramUpdate
    {
        public int UpdateId { get; set; }
        public TelegramMessage? Message { get; set; }
        public TelegramMessage? EditedMessage { get; set; }
    }
}