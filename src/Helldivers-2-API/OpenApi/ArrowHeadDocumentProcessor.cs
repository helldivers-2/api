#if DEBUG
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Helldivers.API.OpenApi;

/// <summary>
/// Handles processing of the OpenAPI specification of the ArrowHead studio API.
/// Specifically, it removes all community API specific aspects (like the '/raw' prefix).
/// </summary>
public class ArrowHeadDocumentProcessor : IDocumentProcessor
{
    // "/raw".length
    private const int PrefixLength = 4;

    private const string ArrowHeadServer = "https://api.live.prod.thehelldiversgame.com/";

    /// <inheritdoc />
    public void Process(DocumentProcessorContext context)
    {
        // First we make a copy of the Paths, since C# doesn't like it when collections are modified during iteration.
        var paths = context.Document.Paths.ToList();
        var server = new OpenApiServer
        {
            Description = "The official ArrowHead server",
            Url = ArrowHeadServer
        };

        foreach (var (url, item) in paths)
        {
            // If it's a '/raw' prefixed URL, we strip the /raw prefix.
            if (string.IsNullOrWhiteSpace(url) is false && url.StartsWith("/raw"))
            {
                item.Servers.Clear();
                item.Servers.Add(server);

                context.Document.Paths[url[PrefixLength..]] = item;
                context.Document.Paths.Remove(url);
            }
        }

        foreach (var operation in context.Document.Operations)
        {
            operation.Operation.OperationId = operation.Operation.OperationId.Replace("Raw", string.Empty);
        }
    }
}
#endif
