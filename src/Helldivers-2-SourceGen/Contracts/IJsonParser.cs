using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Helldivers.SourceGen.Contracts;

/// <summary>
/// Interface for parsing JSON strings and generating corresponding C# source code.
/// </summary>
public interface IJsonParser
{
    /// <summary>
    /// Parses the given source text and generates corresponding C# source code.
    /// </summary>
    /// <param name="file">The source file to be parsed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> for cancelling the parse process.</param>
    /// <returns>A <see cref="SourceText"/> object representing the generated C# source code.</returns>
    SourceText Parse(AdditionalText file, CancellationToken cancellationToken = default);
}
