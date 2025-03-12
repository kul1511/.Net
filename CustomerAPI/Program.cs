using Microsoft.OpenApi.Models;
using CustomerAPI.Interface;
using CustomerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using CustomerAPI;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<DbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container
builder.Services.AddControllers();

builder.Services.AddScoped<ICustomerV1Repository, CustomerV1Repository>();
builder.Services.AddScoped<ICustomerV2Repository, CustomerV2Repository>();

// API Versioning
// builder.Services.AddApiVersioning(options =>
// {
//     options.ReportApiVersions = true;
//     options.AssumeDefaultVersionWhenUnspecified = true;
//     options.DefaultApiVersion = new ApiVersion(1, 0);
//     options.ApiVersionReader = ApiVersionReader.Combine(
//         new UrlSegmentApiVersionReader(),   // URI-based versioning (e.g., /api/v1/customers)
//         new QueryStringApiVersionReader("api-version")  // Query parameter versioning (e.g., /api/customers?api-version=2.0)
//     );
// });

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customers",
        Version = "v1",
        Description = "Customer API",
    });
});

var app = builder.Build();

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web API v1");
        c.RoutePrefix = string.Empty; // Access Swagger UI at root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
