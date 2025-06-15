using System.Text;
using System.Text.Json;

namespace Helldivers.SourceGen.Parsers;


/// <summary>
/// Handles parsing the planetRegion.json and generating the resulting region data.
/// </summary>
public class PlanetRegionsParser : BaseJsonParser
{
    /// <inheritdoc />
    protected override (string Type, string Source) Parse(string json)
    {
        var builder = new StringBuilder("new Dictionary<ulong, (string Name, string? Description)>()\n\t{\n");
        var document = JsonDocument.Parse(json);
        foreach (var property in document.RootElement.EnumerateObject())
        {
            var index = property.Name;
            var name = property.Value.GetProperty("name").GetString();
            string? description = property.Value.GetProperty("description").GetString();
            if (string.IsNullOrWhiteSpace(description))
                description = "null";
            else
                description = $@"""{description}""";

            builder.AppendLine($@"{'\t'}{'\t'}{{ {index}, (""{name}"", {description}) }},");
        }

        builder.Append("\t}");
        return ("IReadOnlyDictionary<ulong, (string Name, string? Description)>", builder.ToString());
    }
}
