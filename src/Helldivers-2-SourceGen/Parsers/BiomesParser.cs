using System.Text;
using System.Text.Json;

namespace Helldivers.SourceGen.Parsers;

/// <summary>
/// BiomesParser is responsible for parsing a JSON string representation of biomes.
/// </summary>
public class BiomesParser : BaseJsonParser
{
    /// <inheritdoc />
    protected override (string Type, string Source) Parse(string json)
    {
        var builder = new StringBuilder("new Dictionary<string, Helldivers.Models.V1.Planets.Biome>()\n\t{\n");
        var entries = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)!;
        foreach (var pair in entries)
            builder.AppendLine($@"{'\t'}{'\t'}{{ ""{pair.Key}"", new Helldivers.Models.V1.Planets.Biome(""{pair.Value["name"]}"", ""{pair.Value["description"]}"") }},");

        builder.Append("\t}");
        return ("IReadOnlyDictionary<string, Helldivers.Models.V1.Planets.Biome>", builder.ToString());
    }
}
