#nullable enable
using System.Text.RegularExpressions;

namespace Golinks.WebAPI.Extensions;

public class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
        => value is null ? null : Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
}
