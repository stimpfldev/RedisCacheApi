using RedisCacheApi.Services.Decorators;
using RedisCacheApi.Services.Implementations;
using RedisCacheApi.Services.Interfaces;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// 👉 REGISTRAR REDIS (Singleton, conexión global)
// ===========================================
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IProductoService, ProductoService>(); // 👉 Inyectamos el servicio REAL directamente.

// 👉 Registramos el service REAL
builder.Services.AddSingleton<IProductoService, ProductoService>();

// 👉 Decoramos el service con Redis
builder.Services.Decorate<IProductoService, ProductoRedisCacheDecorator>();



var app = builder.Build();


// ===========================================
// PRUEBA DE REDIS
// ===========================================
var redis = app.Services.GetRequiredService<IConnectionMultiplexer>();
var db = redis.GetDatabase();

db.StringSet("prueba", "Redis funcionando!");
var valor = db.StringGet("prueba");

Console.WriteLine($"🔵 Redis dice: {valor}");

//---- FIN PRUEBA REDIS ----//

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
