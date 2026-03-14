#if DEBUG
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Helldivers.API.OpenApi.OperationProcessors;

/// <summary>
/// The <see cref="SuperHeadersProcessor"/> is used to add headers that are required for the API to identify
/// the client and to provide developer contact information.
/// </summary>
public class SuperHeadersProcessor : IOperationProcessor
{
    /// <inheritdoc />
    public bool Process(OperationProcessorContext context)
    {
        context.OperationDescription.Operation.Security ??= [];
        context.OperationDescription.Operation.Security.Add(
            new OpenApiSecurityRequirement
            {
                { Constants.CLIENT_HEADER_NAME, [] },
                { Constants.CONTACT_HEADER_NAME, [] }
            }
        );

        return true;
    }
}
#endif
