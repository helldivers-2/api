using System.Globalization;

namespace Helldivers.Core.Localization;

/// <summary>
/// Specialized version of <see cref="Dictionary{TKey,TValue}" /> that intelligently maps <see cref="CultureInfo" /> as
/// keys (eg, if "en-UK" does not exist but "en-US" it'll match).
/// </summary>
public class CultureDictionary<T> where T : class
{
    private readonly Dictionary<CultureInfo, T> _items;

    /// <summary>Creates a new instance of <see cref="CultureDictionary{T}" />,</summary>
    public CultureDictionary(IEnumerable<KeyValuePair<string, T>> items)
    {
        _items = items
            .Select(pair => new KeyValuePair<CultureInfo, T>(new CultureInfo(pair.Key), pair.Value))
            .SelectMany(pair => new List<KeyValuePair<CultureInfo, T>>([
                new KeyValuePair<CultureInfo, T>(pair.Key, pair.Value),
                new KeyValuePair<CultureInfo, T>(pair.Key.Parent, pair.Value)
            ]))
            .DistinctBy(pair => pair.Key)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    /// <summary>
    /// Attempts to resolve a <typeparamref name="T" /> by it's closest common value or parent.
    /// </summary>
    public T? Get(CultureInfo? cultureInfo = default)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;

        return _items.GetValueOrDefault(cultureInfo)
            ?? _items.GetValueOrDefault(cultureInfo.Parent);
    }
}
