using System.Diagnostics.CodeAnalysis;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

        app.UseMiddleware<PermissionMiddleware>();
    }
}
