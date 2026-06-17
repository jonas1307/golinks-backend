using FluentValidation.AspNetCore;
using Golinks.Application.Extensions;
using Golinks.Repository.Extensions;
using Golinks.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddRepositoryServices(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("PermissionPolicy", policy => policy.RequireAuthenticatedUser());

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseContextMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissionMiddleware>();

app.MapControllers();

app.Run();
