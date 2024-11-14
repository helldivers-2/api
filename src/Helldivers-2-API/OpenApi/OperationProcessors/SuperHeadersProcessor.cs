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
        context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = Constants.CLIENT_HEADER_NAME,
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = true,
                Description = "The name of the header that identifies the client to the API.",
                Default = string.Empty
            }
        );

        context.OperationDescription.Operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = Constants.CONTACT_HEADER_NAME,
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = true,
                Description = "The name of the header with developer contact information.",
                Default = string.Empty
            }
        );

        return true;
    }
}
#endif
