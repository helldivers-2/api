using System.Text;
using System.Text.Json;

namespace Helldivers.SourceGen.Parsers;

/// <summary>
/// Parses JSON data of factions and generates the corresponding C# source code representation.
/// </summary>
public class FactionsParser : BaseJsonParser
{
    /// <inheritdoc />
    protected override (string Type, string Source) Parse(string json)
    {
        var builder = new StringBuilder("new Dictionary<int, string>()\n\t{\n");
        var entries = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
        foreach (var pair in entries)
            builder.AppendLine($@"{'\t'}{'\t'}{{ {pair.Key}, {EscapeString(pair.Value)} }},");

        builder.Append("\t}");
        return ("IReadOnlyDictionary<int, string>", builder.ToString());
    }
}
