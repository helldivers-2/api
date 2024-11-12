using System.Text;
using System.Text.Json;

namespace Helldivers.SourceGen.Parsers;

/// <summary>
/// The EnvironmentalsParser class is responsible for parsing JSON strings
/// representing environmental hazards and converting them into C# source code.
/// </summary>
public class EnvironmentalsParser : BaseJsonParser
{
    /// <inheritdoc />
    protected override (string Type, string Source) Parse(string json)
    {
        var builder = new StringBuilder("new Dictionary<string, Helldivers.Models.V1.Planets.Hazard>()\n\t{\n");
        var entries = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)!;
        foreach (var pair in entries)
            builder.AppendLine($@"{'\t'}{'\t'}{{ ""{pair.Key}"", new Helldivers.Models.V1.Planets.Hazard(""{pair.Value["name"]}"", ""{pair.Value["description"]}"") }},");

        builder.Append("\t}");
        return ("IReadOnlyDictionary<string, Helldivers.Models.V1.Planets.Hazard>", builder.ToString());
    }
}
