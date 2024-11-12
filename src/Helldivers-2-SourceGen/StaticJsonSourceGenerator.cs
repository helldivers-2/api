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
            "biomes" => BiomesParser,
            "environmentals" => EnvironmentalsParser,
            "factions" => FactionsParser,
            _ => throw new Exception($"Generator does not know how to parse {name}")
        };
    }



#if false
    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var file in context.AdditionalFiles)
        {
            if (file is null)
                continue;

            var name = Path.GetFileNameWithoutExtension(file.Path);
            name = $"{char.ToUpper(name[0])}{name.Substring(1)}";

            try
            {
                var json = file.GetText(context.CancellationToken)?.ToString() ?? throw new InvalidOperationException($"Cannot generate C# from missing JSON file {file.Path}");

                var (type, value) = name.ToLowerInvariant() switch
                {
                    "planets" => ParsePlanetsDictionary(json),
                    "biomes" => ParseBiomesDictionary(json),
                    "environmentals" => ParseEnvironmentalsDictionary(json),
                    "factions" => ParseFactionsDictionary(json),
                    _ => throw new Exception($"Generator does not know how to parse {name}")
                };

                var source = $@"// <auto-generated />
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using global::System.Collections.Generic;
using global::Helldivers.Models.Domain.Localization;

namespace Helldivers.Models;

public static partial class Static
{{
    /// <summary>Public list of {name} entries from {Path.GetFileName(file.Path)}</summary>
    public static {type} {name} = {value};
}}
";

                context.AddSource($"{name}.g.cs", source);
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
        }
    }
#endif
}
