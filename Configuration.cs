namespace BlogApi;

public static class Configuration
{
    public static string JwtKey = "XLZ8cTzQyQcor1EK5Fu6liQbOHZ0dxDEyj6112KlxoFAu0v4bH";

    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_OHZ0dxDEyj6112KlxoFAu0v4bH";
    public static SmtpSettings Smtp = new();

    public class SmtpSettings()
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


}