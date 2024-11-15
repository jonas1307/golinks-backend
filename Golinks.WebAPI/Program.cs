using Golinks.Application.Extensions;
using Golinks.WebAPI.Extensions;
using Golinks.Repository.Extensions;
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

builder.Services.AddAutoMapperConfiguration();
builder.Services.RegisterRepositoryServices(builder.Configuration);

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
    .AddPolicy("PermissionPolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var userPermissions = context.User.FindFirst("permissions")?.Value;
            if (string.IsNullOrEmpty(userPermissions))
                return false;

            var requiredPermission = context.Resource as string;
            return userPermissions.Split(',').Contains(requiredPermission);
        }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseContextMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
