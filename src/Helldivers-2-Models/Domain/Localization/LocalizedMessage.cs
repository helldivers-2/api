using System.Globalization;
using System.Text.Json.Serialization;

namespace Helldivers.Models.Domain.Localization;

/// <summary>
/// Represents a string that is available in multiple languages.
/// </summary>
/// <param name="Messages">A list of all messages and the <see cref="CultureInfo" /> in which they are available.</param>
[JsonConverter(typeof(LocalizedMessageConverter))]
public sealed record LocalizedMessage(IReadOnlyDictionary<CultureInfo, string> Messages)
{
    /// <summary>
    /// Factory method for creating a <see cref="LocalizedMessage" /> from a list of {language, value} pairs.
    /// </summary>
    public static LocalizedMessage FromStrings(IEnumerable<KeyValuePair<string, string>> values)
    {
        var messages = values.SelectMany(pair =>
        {
            var (key, value) = pair;
            var culture = new CultureInfo(key);
            var parent = culture.Parent;

            return new[]
            {
                new KeyValuePair<CultureInfo, string>(culture, value),
                new KeyValuePair<CultureInfo, string>(parent, value)
            };
        })
        .ToDictionary(pair => pair.Key, pair => pair.Value);

        return new LocalizedMessage(messages);
    }
}
