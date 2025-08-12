using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CROPDEAL.Services;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning).WriteTo.File("Logs/logs_.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

// Configure JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddControllers();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddScoped<IAuth, RegisterRepository>();
builder.Services.AddScoped<ICrops, CropRepository>();
builder.Services.AddScoped<IOrders, OrderRepository>();
builder.Services.AddScoped<IBankAccount, BankAccountRepository>();
builder.Services.AddScoped<ISubscription, SubscriptionRepository>();
builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IInvoice, InvoiceRepository>();

builder.Services.AddDbContext<CropDealDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token (e.g., Bearer your-token-here)"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddOpenApi();
builder.Services.AddAuthorization();

var app = builder.Build();

// Development setup for Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
