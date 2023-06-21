using System.Text;
using System.Text.RegularExpressions;

namespace TelegramBot.Services.Mappers;

public class EmojiKeyToUnicodeMapper
{
    private readonly Dictionary<string, string> _emojiMap = new Dictionary<string, string>()
{
    { "EU-Flag", "\xF0\x9F\x87\xAA\xF0\x9F\x87\xBA" },
};

    public string GetEmoji(string key)
    {
        if (_emojiMap.TryGetValue(key, out var emojiUtf8Code))
        {
            return Encoding.UTF8.GetString(Array.ConvertAll(Regex.Unescape(emojiUtf8Code).ToCharArray(), c => (byte)c));
        }

        return string.Empty;
    }
}