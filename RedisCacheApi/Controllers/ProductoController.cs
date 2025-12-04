using Microsoft.AspNetCore.Mvc;
using RedisCacheApi.Services.Interfaces;

namespace RedisCacheApi.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _service;

    //  DI del servicio REAL o del decorator según Program.cs
    public ProductoController(IProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var lista = _service.GetAll();
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var producto = _service.GetById(id);
        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] string nombre)
    {
        var nuevo = _service.Create(nombre);
        return Ok(nuevo);
    }
}
