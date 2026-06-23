namespace Golinks.Application.Requests;

public class LinkRequest
{
    /// <summary>The destination URL the slug redirects to.</summary>
    /// <example>https://github.com/jonas1307/golinks-backend</example>
    public required string Url { get; set; }

    /// <summary>The unique short slug used in the public redirect URL.</summary>
    /// <example>golinks-repo</example>
    public required string Slug { get; set; }

    /// <summary>An optional human-friendly description of the link.</summary>
    /// <example>Golinks backend repository</example>
    public string? Description { get; set; }

    /// <summary>Optional UTC expiration. After this moment the link returns 410 Gone on redirect.</summary>
    /// <example>2026-12-31T23:59:59Z</example>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>Optional maximum number of accesses. Once reached the link returns 410 Gone on redirect.</summary>
    /// <example>100</example>
    public int? MaxUsage { get; set; }
}
