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
        if (value.Messages.TryGetValue(CultureInfo.CurrentCulture, out var result) is false)
            value.Messages.TryGetValue(CultureInfo.CurrentCulture.Parent, out result);

        if (string.IsNullOrWhiteSpace(result) is false)
            writer.WriteStringValue(result);
        else writer.WriteNullValue();
    }
}
