using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoTTU.Connection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1521";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "orcl";

var connectionString = $"User Id={dbUser};Password={dbPassword};Data Source={dbHost}:{dbPort}/{dbName}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
