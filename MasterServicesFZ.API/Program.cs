using MasterServicesFZ.API.Middleware;
using MasterServicesFZ.Application;
using MasterServicesFZ.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Load appsettings.json
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./v1/swagger.json", "Servicios Maestros Api V1");
});

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod());

app.MapControllers();

app.Run();
