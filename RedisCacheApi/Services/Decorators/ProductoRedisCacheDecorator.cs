using RedisCacheApi.Models;
using RedisCacheApi.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisCacheApi.Services.Decorators;

// 👉 Este DECORATOR envuelve a IProductoService y agrega CACHÉ REDIS.
//    Redis = caché distribuido (ideal para load balancing y microservicios)

public class ProductoRedisCacheDecorator : IProductoService
{
    private readonly IProductoService _inner;            // 👉 Service REAL (sin cache)
    private readonly IDatabase _redis;                   // 👉 Base de datos de Redis

    public ProductoRedisCacheDecorator(
        IProductoService inner,
        IConnectionMultiplexer connection)
    {
        _inner = inner;
        _redis = connection.GetDatabase();               // 👉 Creamos una DB de Redis
    }

    // ================================================
    // GET ALL (cacheado en Redis)
    // ================================================
    public List<Producto> GetAll()
    {
        var key = "productos_todos";

        // 👉 1) Intentamos leer desde Redis
        var valor = _redis.StringGet(key);

        if (!valor.IsNullOrEmpty)
        {
            Console.WriteLine("👉 GetAll desde REDIS (decorator)");

            // 👉 Redis solo guarda STRINGS -> deserializamos JSON
            return JsonSerializer.Deserialize<List<Producto>>(valor);
        }

        Console.WriteLine("⚠️ GetAll SIN cache (decorator)");

        // 👉 2) Llamamos al servicio real
        var lista = _inner.GetAll();

        // 👉 3) Guardamos en Redis como JSON
        _redis.StringSet(
            key,
            JsonSerializer.Serialize(lista),
            TimeSpan.FromSeconds(30)   // TTL = 30 segundos
        );

        return lista;
    }

    // ================================================
    // GET BY ID (cacheado en Redis)
    // ================================================
    public Producto GetById(int id)
    {
        // 👉 Clave única por producto
        var key = $"producto_{id}";

        // 👉 1) Intentar leer desde Redis
        var valor = _redis.StringGet(key);

        if (!valor.IsNullOrEmpty)
        {
            Console.WriteLine($"👉 GetById({id}) desde REDIS (decorator)");

            // 👉 Redis guarda texto → deserializamos
            return JsonSerializer.Deserialize<Producto>(valor);
        }

        Console.WriteLine($"⚠️ GetById({id}) SIN cache (decorator)");

        // 👉 2) No está en cache → pedimos al service real
        var producto = _inner.GetById(id);

        if (producto != null)
        {
            // 👉 3) Guardamos en cache con TTL
            _redis.StringSet(
                key,
                JsonSerializer.Serialize(producto),
                TimeSpan.FromSeconds(30) // TTL de 30s
            );
        }

        return producto;
    }


    // ================================================
    // CREATE (invalidar cache)
    // ================================================
    public Producto Create(string nombre)
    {
        var nuevo = _inner.Create(nombre);

        // 👉 Invalidamos caches relacionados
        _redis.KeyDelete("productos_todos");
        _redis.KeyDelete($"producto_{nuevo.Id}");

        Console.WriteLine("❌ CREATE: cache invalidado (decorator)");

        return nuevo;
    }
}
