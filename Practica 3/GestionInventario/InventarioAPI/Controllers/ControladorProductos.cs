using InventarioAPI.Modelos;
using InventarioAPI.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorProductos : ControllerBase
    {
        private readonly ServicioArchivoJson _servicioArchivo;

        public ControladorProductos(ServicioArchivoJson servicioArchivo)
        {
            _servicioArchivo =servicioArchivo;
        }

        // GET: 
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Producto> listaProductos=_servicioArchivo.LeerProductos();
            return Ok(listaProductos);
        }

        // GET: 
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            List<Producto> listaProductos=_servicioArchivo.LeerProductos();
            Producto? productoEncontrado =listaProductos.FirstOrDefault(p => p.Id==id);

            if (productoEncontrado ==null)
            {
                return NotFound($"Producto con ID {id} no encontrado.");
            }
            return Ok(productoEncontrado);
        }

        // POST: 
        [HttpPost]
        public IActionResult Crear([FromBody] Producto nuevoProducto)
        {
            List<Producto> listaProductos =_servicioArchivo.LeerProductos();

            int nuevoId=1;
            if (listaProductos.Count> 0)
            {
                nuevoId =listaProductos.Max(p=>p.Id) + 1;
            }
            nuevoProducto.Id =nuevoId;

            listaProductos.Add(nuevoProducto);
            _servicioArchivo.GuardarProductos(listaProductos);

            return CreatedAtAction(nameof(ObtenerPorId), new { id= nuevoProducto.Id }, nuevoProducto);
        }

        // PUT: 
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Producto productoActualizado)
        {
            List<Producto> listaProductos= _servicioArchivo.LeerProductos();
            Producto? productoExistente = listaProductos.FirstOrDefault(p => p.Id == id);

            if (productoExistente== null)
            {
                return NotFound($"Producto con ID {id} no encontrado.");
            }
            productoExistente.Nombre=productoActualizado.Nombre;
            productoExistente.Categoria =productoActualizado.Categoria;
            productoExistente.Descripcion =productoActualizado.Descripcion;
            productoExistente.Precio =productoActualizado.Precio;
            productoExistente.CantidadStock= productoActualizado.CantidadStock;
            productoExistente.FechaVencimiento= productoActualizado.FechaVencimiento;

            _servicioArchivo.GuardarProductos(listaProductos);
            return NoContent(); 
        }

        // DELETE: 
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            List<Producto> listaProductos=_servicioArchivo.LeerProductos();
            Producto? productoAEliminar=listaProductos.FirstOrDefault(p => p.Id == id);

            if (productoAEliminar ==null)
            {
                return NotFound($"Producto con ID {id} no encontrado.");
            }

            listaProductos.Remove(productoAEliminar);
            _servicioArchivo.GuardarProductos(listaProductos);
            return NoContent();
        }
    }
}
