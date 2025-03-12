using Microsoft.EntityFrameworkCore;
using ProductEFAPI.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<EmployeeDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee");
        c.RoutePrefix = string.Empty; // Access Swagger UI at root URL
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
