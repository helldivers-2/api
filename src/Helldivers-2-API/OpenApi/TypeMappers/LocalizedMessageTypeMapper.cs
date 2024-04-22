#if DEBUG
using Helldivers.Models.Domain.Localization;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Helldivers.API.OpenApi.TypeMappers;

/// <summary>
/// Maps the LocalizedMessage type to a JSON schema describing what values it serializes to.
/// </summary>
/// <param name="languages">Contains all languages that are supported (and thus should be OpenAPI documented).</param>
public sealed class LocalizedMessageTypeMapper(IReadOnlyList<string> languages) : ITypeMapper
{
    /// <inheritdoc />
    public Type MappedType => typeof(LocalizedMessage);

    /// <inheritdoc />
    public bool UseReference => false;

    /// <summary>
    /// Gets the generated schema for the LocalizedMessage type.
    /// </summary>
    public void GenerateSchema(JsonSchema schema, TypeMapperContext context)
    {
        var dictionarySchema = new JsonSchema
        {
            Title = nameof(LocalizedMessage),
            Type = JsonObjectType.Object,
            Description = "When passing in ivl-IV as Accept-Language, all available languages are returned"
        };

        // NJsonSchema's Properties is read-only, so we can't use the object assignment syntax.
        foreach (string language in languages)
        {
            dictionarySchema.Properties.Add(language, new JsonSchemaProperty
            {
                Type = JsonObjectType.String,
                Description = $"The message in {language}"
            });
        }

        schema.OneOf.Add(JsonSchema.FromType(typeof(string)));
        schema.OneOf.Add(dictionarySchema);
    }
}
#endif
