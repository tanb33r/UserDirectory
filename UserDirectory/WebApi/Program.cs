using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Dtos;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain;
using UserDirectory.Infrastructure.Sql;
using UserDirectory.Application.Services;
using UserDirectory.Infrastructure.Sql.Services;
using UserDirectory.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<UserDirectoryDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Sql")));


builder.Services.AddAutoMapper(typeof(IUserService).Assembly,
                               typeof(UserService).Assembly,
                               typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserRepository, SqlUserRepository>();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDto>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200") // update the url as per your need
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                 .AllowCredentials());
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();

/*
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
var forecast = Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    ))
    .ToArray();
return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/
