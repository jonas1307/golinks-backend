using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Repository.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static void UseContextMigrations(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<GolinksContext>();
            context.Database.Migrate();
        }
    }
}
