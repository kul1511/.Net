using Microsoft.OpenApi.Models;
using Bus_Booking_Web_Api.Data;
using Bus_Booking_Web_Api.Interface;
using Bus_Booking_Web_Api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IBus, BusRepository>();
builder.Services.AddScoped<ITrip, TripRepository>();
builder.Services.AddScoped<IBooking, BookingRepository>();

builder.Services.AddDbContext<BusBookingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee");
        c.RoutePrefix = string.Empty; // Access Swagger UI at root URL
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
