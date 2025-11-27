//IProductoService cumple 3 roles 
//1 Define el contrato del servicio (qué debe hacer, no cómo)
//2 Permite cambiar la implementación sin romper nada (desacoplamiento)
//3 Hace posible el Decorator Pattern (que vas a usar con Redis)
using RedisCacheApi.Models;

namespace RedisCacheApi.Services.Interfaces;

// 👉 Este contrato define lo que ofrece el servicio de productos.
//    Redis se integrará SIN tocar estos métodos (decorator), manteniendo desacoplamiento.
public interface IProductoService
{
    List<Producto> GetAll();
    Producto GetById(int id);
    Producto Create(string nombre);
}
