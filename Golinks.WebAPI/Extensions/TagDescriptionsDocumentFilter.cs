using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags =
        [
            new OpenApiTag { Name = "Links", Description = "Authenticated CRUD operations for managing links." },
            new OpenApiTag { Name = "Metrics", Description = "Public access metrics aggregated per link." },
            new OpenApiTag { Name = "Redirect", Description = "Public redirection from a slug to its original URL." }
        ];
    }
}
