using Helldivers.SourceGen.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Helldivers.SourceGen.Parsers;

/// <summary>
/// An abstract base class for parsing JSON data and generating corresponding C# source code.
/// </summary>
public abstract class BaseJsonParser : IJsonParser
{
    private const string TEMPLATE = @"// <auto-generated />
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using global::System.Collections.Generic;
using global::Helldivers.Models.Domain.Localization;

namespace Helldivers.Models;

public static partial class Static
{{
    /// <summary>Public list of {0} entries from {1}</summary>
    public static {2} {0} = {3};
}}";

    /// <inheritdoc />
    public SourceText Parse(AdditionalText file, CancellationToken cancellationToken = default)
    {
        var filename = Path.GetFileName(file.Path);
        var json = file.GetText(cancellationToken)?.ToString();

        var name = Path.GetFileNameWithoutExtension(file.Path);
        name = $"{char.ToUpper(name[0])}{name.Substring(1)}";

        if (string.IsNullOrWhiteSpace(json) is false)
        {
            var (type, csharp) = Parse(json!);

            var output = string.Format(TEMPLATE, name, filename, type, csharp);
            return SourceText.From(output, Encoding.UTF8);
        }

        return SourceText.From("// Could not read JSON file");
    }

    /// <summary>
    /// Convert the JSON string into C# code that can be injected.
    /// </summary>
    protected abstract (string Type, string Source) Parse(string json);
}
