using Microsoft.EntityFrameworkCore;
using AutoTTU.Connection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Função auxiliar para obrigar que env var exista
string Req(string name)
{
    var v = Environment.GetEnvironmentVariable(name);
    if (string.IsNullOrWhiteSpace(v))
        throw new InvalidOperationException($"Variável de ambiente '{name}' não definida.");
    return v;
}

var dbHost = Req("DB_HOST"); // ex: autotutsqlserver123.database.windows.net
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
var dbName = Req("DB_NAME");
var dbUser = Req("DB_USER");
var dbPassword = Req("DB_PASSWORD");

// Connection string no formato Azure SQL
var connectionString =
    $"Server=tcp:{dbHost},{dbPort};" +
    $"Initial Catalog={dbName};" +
    $"Persist Security Info=False;" +
    $"User ID={dbUser};" +
    $"Password={dbPassword};" +
    $"MultipleActiveResultSets=False;" +
    $"Encrypt=True;" +
    $"TrustServerCertificate=False;" +
    $"Connection Timeout=30;";

// Para debug (sem senha)
Console.WriteLine($"[DB] Server={dbHost},{dbPort}; Database={dbName}; User={dbUser}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
