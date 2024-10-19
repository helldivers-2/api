#if DEBUG
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Helldivers.API.OpenApi.DocumentProcessors;

/// <summary>
/// Handles processing of the OpenAPI specification of the Production Helldivers API.
/// </summary>
public class HelldiversDocumentProcessor : IDocumentProcessor
{
    private const string HelldiversFlyServer = "https://api.helldivers2.dev/";

    /// <inheritdoc />
    public void Process(DocumentProcessorContext context)
    {
        // First we make a copy of the Paths, since C# doesn't like it when collections are modified during iteration.
        var paths = context.Document.Paths.ToList();
        var server = new OpenApiServer
        {
            Description = "The dotnet helldivers server",
            Url = HelldiversFlyServer
        };

        foreach (var (_, item) in paths)
        {
            item.Servers.Clear();
            item.Servers.Add(server);
        }
    }
}
#endif
