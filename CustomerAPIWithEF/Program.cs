using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CustomerAPIWithEF.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EmployeeDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
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