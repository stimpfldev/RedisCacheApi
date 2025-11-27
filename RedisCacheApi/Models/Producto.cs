namespace RedisCacheApi.Models;

// 👉 Modelo simple que representa un producto real.
//    Esto sería equivalente a una tabla de base de datos.
public class Producto
{
    public int Id { get; set; }        // 👉 Identificador único
    public string Nombre { get; set; } // 👉 Nombre del producto
}
