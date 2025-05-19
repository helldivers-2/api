using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helldivers.Core.Hdml;

/// <summary>
/// Handles the parsing of HDML formatted strings.
///
/// HDML is the acronym for Helldivers Markup Language, a term coined by DetKewlDog.
/// </summary>
/// <seealso href="https://github.com/helldivers-2/api/issues/55#issuecomment-2077491483" />
public sealed class HdmlParser(ILogger<HdmlParser> logger)
{
    /// <summary>The token that indicates an HDML tag is being opened.</summary>
    private const char OPEN_TOKEN = '<';
    /// <summary>The token that indicates an HDML tag is being closed.</summary>
    private const char CLOSE_TOKEN = '>';

    private const string YELLOW_OPENING = "<span data-ah=\"1\">";
    private const string YELLOW_CLOSING = "</span>";

    private const string RED_OPENING = "<span data-ah=\"2\">";
    private const string RED_CLOSING = "</span>";

    private const string BOLD_OPENING = "<span data-ah=\"3\">";
    private const string BOLD_CLOSING = "</span>";

    /// <summary>
    /// A small stack-only data type to contain a single parsed HDML token (eg. `&lt;i=3&gt;lorem ipsum&lt;/i&gt;`).
    /// </summary>
    private ref struct Token
    {
        /// <summary>
        /// The opening tag, eg. `&lt;i=3&gt;`
        /// </summary>
        public ReadOnlySpan<char> Opening;
        /// <summary>
        /// The content of the tag, eg. `lorem ipsum`
        /// </summary>
        public ReadOnlySpan<char> Content;
        /// <summary>
        /// The closing tag, eg. `&lt;/i&gt;`
        /// </summary>
        public ReadOnlySpan<char> Close;
    }

    /// <summary>
    /// Compiles a given HDML string into HTML.
    /// For example, `&lt;i=3&gt;lorem ipsum&lt;/i&gt;` becomes `&lt;i data-ah="3"&gt;lorem ipsum&lt;/i&gt;`
    ///
    /// Compilation may fail silently but should return the original string.
    /// </summary>
    public string Compile(string hdml)
    {
        try
        {
            var builder = new StringBuilder();
            var span = hdml.AsSpan();
            var buffer = new Token();

            var content = 0; // Tracks the last start of content after an HDML block for building a content span.
            for (int index = 0; index < span.Length; index++)
            {
                if (span[index] == OPEN_TOKEN)
                {
                    if (content is not 0)
                        builder.Append(span[content..index]);

                    Tokenize(ref index, ref span, ref buffer);
                    content = index;

                    Parse(ref buffer, ref builder);
                    buffer.Opening = ReadOnlySpan<char>.Empty;
                    buffer.Content = ReadOnlySpan<char>.Empty;
                    buffer.Close = ReadOnlySpan<char>.Empty;
                }
            }

            if (content < span.Length)
                builder.Append(span[content..span.Length]);
            else if (content == span.Length)
                builder.Append(span);

            return builder.ToString();
        }
        catch (Exception exception)
        {
            // TODO: chinese encoding always seems to fail, hard to debug if you can't read the strings though.
            logger.LogError(exception, "Failed to parse HDML");

            return hdml;
        }
    }

    /// <summary>Attempts to extract a token (aka HDML tag) and read it into the buffer.</summary>
    private static void Tokenize(ref int index, ref ReadOnlySpan<char> span, ref Token buffer)
    {
        // first pass, find the opening tag
        int openingStart = index;
        for (; index < span.Length; index++)
        {
            if (span[index] == CLOSE_TOKEN)
                break;
        }

        index++; // include closing tag
        ClampIndex(ref index, ref span);
        buffer.Opening = span[openingStart..index];

        // second pass collect content
        int contentStart = index;
        for (; index < span.Length; index++)
        {
            if (span[index] == OPEN_TOKEN)
                break;
        }

        buffer.Content = span[contentStart..index];

        // final pass, find the closing tag.
        int closeStart = index;
        for (; index < span.Length; index++)
        {
            if (span[index] == CLOSE_TOKEN)
                break;
        }

        index++; // include closing tag
        ClampIndex(ref index, ref span);
        buffer.Close = span[closeStart..index];
        return;
    }

    /// <summary>
    /// Performs the actual conversion of HDML tokens to HTML tokens.
    /// </summary>
    /// <seealso href="https://github.com/helldivers-2/api/issues/55#issuecomment-2077491483" />
    private static void Parse(ref Token token, ref StringBuilder builder)
    {
        if (token.Opening.Length >= 4 && token.Opening[3] == '1')
        {
            builder.Append(YELLOW_OPENING);
            builder.Append(token.Content);
            builder.Append(YELLOW_CLOSING);
        }
        else if (token.Opening.Length >= 4 && token.Opening[3] == '2')
        {
            builder.Append(RED_OPENING);
            builder.Append(token.Content);
            builder.Append(RED_CLOSING);
        }
        else if (token.Opening.Length >= 4 && token.Opening[3] == '3')
        {
            builder.Append(BOLD_OPENING);
            builder.Append(token.Content);
            builder.Append(BOLD_CLOSING);
        }
        else if (token.Opening.Length >= 12 && token.Opening[1] == 'c' && token.Opening[3] == '#')
        {
            builder.Append($"<span data-hex=\"{token.Opening[3..12]}\">");
            builder.Append(token.Content);
            builder.Append("</span>");
        }
        else
        {
            // Fallback, add the entire span as-is.
            builder.Append(token.Opening);
            builder.Append(token.Content);
            builder.Append(token.Close);
        }
    }

    /// <summary>Small helper method to clamp the index NEVER past the span length.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClampIndex(ref int index, ref ReadOnlySpan<char> span)
    {
        if (index >= span.Length)
            index = span.Length - 1;
    }
}
