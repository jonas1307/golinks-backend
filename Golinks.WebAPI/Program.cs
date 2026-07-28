using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Golinks.Application.Extensions;
using Golinks.Repository;
using Golinks.Repository.Extensions;
using Golinks.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

    builder.Services.AddSerilog((services, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    builder.Services.AddMemoryCache();
    builder.Services.AddApplicationServices();
    builder.Services.AddRepositoryServices(builder.Configuration);
    builder.Services.AddRateLimiting();
    builder.Services.AddProblemDetails();

    builder.Services.AddHealthChecks()
        .AddDbContextCheck<GolinksContext>(name: "database", tags: ["ready"]);

    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddControllers(options =>
        options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer())));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                await ProblemResponse.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication is required to access this resource.");
            },
            OnForbidden = context =>
                ProblemResponse.WriteAsync(context.HttpContext, StatusCodes.Status403Forbidden, "Forbidden", "You don't have the required permission to access this resource.")
        };
    });

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("PermissionPolicy", policy => policy.RequireAuthenticatedUser());

    builder.Services.AddSwaggerConfiguration();

    var app = builder.Build();

    app.UseContextMigrations();

    app.UseSerilogRequestLogging(options =>
    {
        options.GetLevel = (httpContext, _, ex) =>
            ex is not null || httpContext.Response.StatusCode >= 500
                ? Serilog.Events.LogEventLevel.Error
                : httpContext.Request.Path.StartsWithSegments("/health")
                    ? Serilog.Events.LogEventLevel.Verbose
                    : Serilog.Events.LogEventLevel.Information;
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerSetup();
    }
    else
    {
        app.UseExceptionHandler();
    }

    app.UseCors("AllowAllOrigins");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<PermissionMiddleware>();
    app.UseRateLimiter();

    app.MapControllers();

    app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
    app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
