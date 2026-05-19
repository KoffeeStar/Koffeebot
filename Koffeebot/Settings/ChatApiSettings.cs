namespace Koffeebot.Settings
{
    public static class ChatApiSettings
    {
        public static string BaseUrl { get; } = "https://openrouter.ai/api/v1/chat/completions";
        public static string ApiKey { get; } = "тут был мой ключ, но с ним на гитхаб нельзя было выгрузить";
        public static string DefaultModel { get; } = "gpt-3.5-turbo";
    }
}