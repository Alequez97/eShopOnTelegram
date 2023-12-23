using System.Text;
using System.Text.RegularExpressions;

using eShopOnTelegram.Shop.Worker.Services.Mappers.Enums;

namespace eShopOnTelegram.Shop.Worker.Services.Mappers;

public class EmojiKeyToUnicodeMapper
{
    private readonly Dictionary<EmojiKey, string> _emojiMap = new()
    {
        { EmojiKey.EU_Flag, "\xF0\x9F\x87\xAA\xF0\x9F\x87\xBA" },
    };

    public string GetEmojiUnicode(EmojiKey key)
    {
        if (_emojiMap.TryGetValue(key, out var emojiUtf8Code))
        {
            return Encoding.UTF8.GetString(Array.ConvertAll(Regex.Unescape(emojiUtf8Code).ToCharArray(), c => (byte)c));
        }

        return string.Empty;
    }
}