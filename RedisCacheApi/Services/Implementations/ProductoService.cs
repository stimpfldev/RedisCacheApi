using RedisCacheApi.Models;
using RedisCacheApi.Services.Interfaces;

namespace RedisCacheApi.Services.Implementations;

// servicio REAL (sin caché). 

public class ProductoService : IProductoService
{
    //  Lista en memoria simulando base de datos
    private readonly List<Producto> _productos = new();
    private int _nextId = 1;

    public List<Producto> GetAll()
    {
        Console.WriteLine(" ProductoService.GetAll ejecutado (NO usa cache)");
        return _productos.ToList();
    }

    public Producto GetById(int id)
    {
        Console.WriteLine($" ProductoService.GetById({id}) ejecutado (NO usa cache)");
        return _productos.FirstOrDefault(p => p.Id == id);
    }

    public Producto Create(string nombre)
    {
        var nuevo = new Producto
        {
            Id = _nextId++,
            Nombre = nombre
        };

        _productos.Add(nuevo);

        Console.WriteLine(" ProductoService.Create ejecutado (NO usa cache)");

        return nuevo;
    }
}
