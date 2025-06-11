

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderProcessing.Application.Interface;
using OrderProcessing.Application.Service;
using OrderProcessing.Infrastructure.Data;
using OrderProcessing.Infrastructure.Repository;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Logging
builder.Logging.AddConsole();

// Add DbContext - must come before services that depend on it
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost", 
        Port = 5672
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<RabbitMqService>();

// Controllers
builder.Services.AddControllers();

// Swagger & API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order Processing API",
        Version = "v1",
        Description = "API to process and track customer orders"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like this: Bearer {your token here}"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Processing API v1");
    c.RoutePrefix = "swagger";
});


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

builder.WebHost.UseUrls("http://0.0.0.0:80");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
