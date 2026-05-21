namespace Koffeebot.Settings
{
    public static class ChatApiSettings
    {
        public static string BaseUrl { get; } = "https://openrouter.ai/api/v1/chat/completions";
        public static string ApiKey { get; } = "ключ";
        public static string DefaultModel { get; } = "baidu/cobuddy:free";
    }
}