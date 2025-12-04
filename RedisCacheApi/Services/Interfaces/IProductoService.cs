
using RedisCacheApi.Models;

namespace RedisCacheApi.Services.Interfaces;

// contrato q define lo que ofrece el servicio de productos.
//    Redis se integrará son tocar estos métodos (decorator), asi mantengo  desacoplamiento.
public interface IProductoService
{
    List<Producto> GetAll();
    Producto GetById(int id);
    Producto Create(string nombre);
}
