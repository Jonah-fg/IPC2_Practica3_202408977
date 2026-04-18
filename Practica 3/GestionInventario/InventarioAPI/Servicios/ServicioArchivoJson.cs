using InventarioAPI.Modelos;
using System.Text.Json;

namespace InventarioAPI.Servicios
{
    public class ServicioArchivoJson
    {
        private readonly string _rutaArchivo;   

        public ServicioArchivoJson(IWebHostEnvironment entorno)
        {
            _rutaArchivo =Path.Combine(entorno.ContentRootPath, "Data", "inventario.json");

            var directorio= Path.GetDirectoryName(_rutaArchivo);
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio!);
            }

            if (!File.Exists(_rutaArchivo))
            {
                File.WriteAllText(_rutaArchivo,"[]");
            }
        }

        public List<Producto> LeerProductos()
        {
            string contenidoJson =File.ReadAllText(_rutaArchivo);
            List<Producto> productos=JsonSerializer.Deserialize<List<Producto>>(contenidoJson);

            if (productos==null)
            {
                return new List<Producto>();
            }
            return productos;
        }

        public void GuardarProductos(List<Producto> productos)
        {
            JsonSerializerOptions opcionesJson =new JsonSerializerOptions { WriteIndented = true };
            string contenidoJson=JsonSerializer.Serialize(productos, opcionesJson);
            File.WriteAllText(_rutaArchivo, contenidoJson);
        }
    }
}
    

