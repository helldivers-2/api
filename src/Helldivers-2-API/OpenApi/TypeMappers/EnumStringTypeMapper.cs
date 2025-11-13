#if DEBUG
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using System.Collections.ObjectModel;

namespace Helldivers.API.OpenApi.TypeMappers;

/// <summary>
/// Forces enums to be generated as an enumeration of strings rather than integers.
/// </summary>
/// <typeparam name="T">The enum type to convert.</typeparam>
public sealed class EnumStringTypeMapper<T> : ITypeMapper where T : struct, Enum
{
    /// <inheritdoc />
    public Type MappedType => typeof(T);

    /// <inheritdoc />
    public bool UseReference => false;

    /// <inheritdoc />
    public void GenerateSchema(JsonSchema schema, TypeMapperContext context)
    {
        schema.Type = JsonObjectType.String;
        schema.Format = null;

        var names = Enum.GetNames<T>();

        schema.Enumeration.Clear();
        schema.EnumerationNames.Clear();

        foreach (var name in names)
            schema.Enumeration.Add(name);

        schema.EnumerationNames = new Collection<string>(names.ToList());
        schema.ExtensionData ??= new Dictionary<string, object>()!;
    }
}
#endif
