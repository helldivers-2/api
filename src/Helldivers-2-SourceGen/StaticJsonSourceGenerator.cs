using Helldivers.SourceGen.Contracts;
using Helldivers.SourceGen.Parsers;
using Microsoft.CodeAnalysis;

namespace Helldivers.SourceGen;

/// <summary>
/// A sample source generator that creates C# classes based on the text file (in this case, Domain Driven Design ubiquitous language registry).
/// When using a simple text file as a baseline, we can create a non-incremental source generator.
/// </summary>
[Generator]
public class StaticJsonSourceGenerator : IIncrementalGenerator
{
    private static readonly IJsonParser PlanetParser = new PlanetsParser();
    private static readonly IJsonParser PlanetRegionParser = new PlanetRegionsParser();
    private static readonly IJsonParser BiomesParser = new BiomesParser();
    private static readonly IJsonParser EnvironmentalsParser = new EnvironmentalsParser();
    private static readonly IJsonParser FactionsParser = new FactionsParser();

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var generated = context
            .AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith(".json"))
            .Select(static (file, cancellationToken) =>
            {
                var parser = GetParserForFile(file);

                var name = Path.GetFileNameWithoutExtension(file.Path);

                return (parser.Parse(file, cancellationToken), name);
            });

        context.RegisterSourceOutput(generated, static (context, pair) =>
        {
            var (source, name) = pair;

            try
            {
                context.AddSource(name, source);
            }
            catch (Exception exception)
            {
                context.AddSource($"{name}.g.cs", $"// An exception was thrown processing {name}.json\n{exception.ToString()}");
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            id: "HDJSON", // Unique ID for your error
                            title: "JSON source generator failed", // Title of the error
                            messageFormat: $"An error occured generating C# code from JSON files: {exception}", // Message format
                            category: "HD2", // Category of the error
                            DiagnosticSeverity.Error, // Severity of the error
                            isEnabledByDefault: true // Whether the error is enabled by default
                        ),
                        Location.None // No specific location provided for simplicity
                    )
                );
            }
        });
    }

    private static IJsonParser GetParserForFile(AdditionalText file)
    {
        var name = Path.GetFileNameWithoutExtension(file.Path);
        name = $"{char.ToUpper(name[0])}{name.Substring(1)}";

        return name.ToLowerInvariant() switch
        {
            "planets" => PlanetParser,
            "planetregion" => PlanetRegionParser,
            "biomes" => BiomesParser,
            "environmentals" => EnvironmentalsParser,
            "factions" => FactionsParser,
            _ => throw new Exception($"Generator does not know how to parse {name}")
        };
    }
}
