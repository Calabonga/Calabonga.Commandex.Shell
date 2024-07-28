using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Calabonga.Commandex.Contracts;

public static class JsonSerializerOptionsExt
{
    public static JsonSerializerOptions Cyrillic =>
        new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
}