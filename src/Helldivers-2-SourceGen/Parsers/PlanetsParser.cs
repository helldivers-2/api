using System.Text;
using System.Text.Json;

namespace Helldivers.SourceGen.Parsers;

/// <summary>
/// Handles parsing the planets.json and generating the resulting planet data.
/// </summary>
public class PlanetsParser : BaseJsonParser
{
    /// <inheritdoc />
    protected override (string Type, string Source) Parse(string json)
    {
        var builder = new StringBuilder("new Dictionary<int, (LocalizedMessage Name, string Sector, string Biome, List<string> Environmentals)>()\n\t{\n");
        var document = JsonDocument.Parse(json);
        foreach (var property in document.RootElement.EnumerateObject())
        {
            var index = property.Name;
            var name = property.Value.GetProperty("name").GetString();
            var names = property
                .Value
                .GetProperty("names")
                .EnumerateObject()
                .ToDictionary(prop => prop.Name, prop => prop.Value.GetString()!);
            var sector = property.Value.GetProperty("sector").GetString();
            var biome = property.Value.GetProperty("biome").GetString();
            var environmentals = property
                .Value
                .GetProperty("environmentals")
                .EnumerateArray()
                .Select(prop => $@"""{prop.GetString()!}""")
                .ToList();

            builder.AppendLine($@"{'\t'}{'\t'}{{ {index}, (LocalizedMessage.FromStrings([{string.Join(", ", names.Select(pair => $@"new KeyValuePair<string, string>(""{pair.Key}"", ""{pair.Value}"")"))}]), ""{sector}"", ""{biome}"", [{string.Join(", ", environmentals)}]) }},");
        }

        builder.Append("\t}");
        return ("IReadOnlyDictionary<int, (LocalizedMessage Name, string Sector, string Biome, List<string> Environmentals)>", builder.ToString());
    }
}
