using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Create database if not exists
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    
    // Seed data if empty
    if (!context.WeatherForecasts.Any())
    {
        context.WeatherForecasts.AddRange(
            new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 20,
                Summary = "Warm"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(1),
                TemperatureC = 25,
                Summary = "Hot"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(2),
                TemperatureC = 15,
                Summary = "Cool"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(3),
                TemperatureC = 30,
                Summary = "Sunny"
            },
            new WeatherForecast
            {
                Date = DateTime.Now.AddDays(4),
                TemperatureC = 10,
                Summary = "Rainy"
            }
        );
        context.SaveChanges();
    }
}

app.Run();