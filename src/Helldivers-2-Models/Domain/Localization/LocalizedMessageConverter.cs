using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Helldivers.Models.Domain.Localization;

internal sealed class LocalizedMessageConverter : JsonConverter<LocalizedMessage>
{
    /// <inheritdoc />
    public override LocalizedMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new InvalidOperationException($"Unable to deserialize {nameof(LocalizedMessage)} from JSON");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, LocalizedMessage value, JsonSerializerOptions options)
    {
        // If the invariant culture is selected, serialize a dictionary with all languages.
        if (Equals(CultureInfo.CurrentCulture, LocalizedMessage.InvariantCulture))
        {
            writer.WriteStartObject();

            // We keep both country language and global language (eg. 'en-US' and 'en'), so we filter out the globals.
            foreach (var (language, message) in value.Messages.Where(culture => culture.Key.Name.Contains('-')))
                writer.WriteString(language.Name, message);

            writer.WriteEndObject();

            // Don't write out the translated string
            return;
        }

        if (value.Messages.TryGetValue(CultureInfo.CurrentCulture, out var result) is false)
        {
            if (value.Messages.TryGetValue(CultureInfo.CurrentCulture.Parent, out result) is false)
            {
                // Fall back to the configured default culture if one is available
                value.Messages.TryGetValue(LocalizedMessage.FallbackCulture, out result);
            }
        }

        if (string.IsNullOrWhiteSpace(result) is false)
            writer.WriteStringValue(result);
        else
            writer.WriteNullValue();
    }
}
