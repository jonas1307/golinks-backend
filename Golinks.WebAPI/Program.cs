using Golinks.Application.Extensions;
using Golinks.WebAPI.Extensions;
using Golinks.Repository.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

builder.Services.AddAutoMapperConfiguration();
builder.Services.RegisterRepositoryServices(builder.Configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
