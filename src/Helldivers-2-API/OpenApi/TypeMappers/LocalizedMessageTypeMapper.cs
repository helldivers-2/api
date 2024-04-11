using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Helldivers.API.OpenApi.TypeMappers;

/// <summary>
/// Maps the LocalizedMessage type to a JSON schema describing what values it serializes to.
/// </summary>
public sealed class LocalizedMessageTypeMapper : ITypeMapper
{
    /// <summary>
    /// Initialises a new instance of the <see cref="LocalizedMessageTypeMapper"/> class. 
    /// </summary>
    /// <param name="mappedType"></param>
    /// <param name="languages"></param>
    public LocalizedMessageTypeMapper(Type mappedType, List<string> languages)
    {
        MappedType = mappedType;
        Languages = languages;
        UseReference = false;
    }

    private List<string> Languages { get; set; }

    /// <summary>Gets the mapped type.</summary>
    public Type MappedType { get; }

    /// <summary>Gets a value indicating whether to use a JSON Schema reference for the type.</summary>
    public bool UseReference { get; }

    /// <summary>
    /// Gets the generated schema for the LocalizedMessage type.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void GenerateSchema(JsonSchema schema, TypeMapperContext context)
    {
        schema.OneOf.Add(JsonSchema.FromType(typeof(string)));

        var stringType = new JsonSchemaProperty { Type = JsonObjectType.String };
        
        var dictionarySchema = new JsonSchema
        {
            Type = JsonObjectType.Object
        };
        
        // Do it in NJsonSchema's *dirty* way because we can't set it in the constructor
        foreach (string language in Languages)
        {
            dictionarySchema.Properties.Add(language, stringType);
        }
        
        dictionarySchema.Description =
            "When the correct header is passed we return a dictionary of all the available languages";
        
        schema.OneOf.Add(dictionarySchema);
    }
}
