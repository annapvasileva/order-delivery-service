using System.Text.Json;
using System.Text.Json.Serialization;
using OrderDeliveryService.Application.Extensions;
using OrderDeliveryService.Infrastructure.Persistence.Extensions;
using OrderDeliveryService.Infrastructure.Persistence.Options.Postgres;
using OrderDeliveryService.Presentation.Controllers;
using OrderDeliveryService.Presentation.Extensions;
using OrderDeliveryService.Presentation.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services
    .AddOptions<PostgresOptions>()
    .Bind(builder.Configuration.GetSection(PostgresOptions.SectionName));
builder.Services
    .AddPostgresDbContext()
    .AddPostgresMigrations()
    .AddPostgresRepositories()
    .AddServices();

builder.Services.AddSingleton(_ =>
{
    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    options.Converters.Add(new JsonStringEnumConverter());
    return options;
});

builder.Services.AddScoped<ExceptionFormattingMiddleware>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(OrderController).Assembly);

builder.Services.AddSwagger();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseMiddleware<ExceptionFormattingMiddleware>();
app.MapControllers();

await app.RunAsync();