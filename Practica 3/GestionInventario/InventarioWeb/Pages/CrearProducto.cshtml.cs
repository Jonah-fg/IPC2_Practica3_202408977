using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventarioWeb.Modelos;
using System.Text;
using System.Text.Json;

namespace InventarioWeb.Pages
{
    public class CrearModel : PageModel
    {
        private readonly IHttpClientFactory _fabricaHttp;

        public CrearModel(IHttpClientFactory fabricaHttp)
        {
            _fabricaHttp=fabricaHttp;
        }

        [BindProperty]
        public Producto ProductoNuevo { get; set; } =new Producto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            Console.WriteLine($"Producto recibido: Nombre={ProductoNuevo.Nombre}, Precio={ProductoNuevo.Precio}");

            if (!ModelState.IsValid)
            {
                // También muestra errores de validación
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error de validación: {error.ErrorMessage}");
                }
                return Page();
            }

            HttpClient cliente =_fabricaHttp.CreateClient("InventarioAPI");

            string contenidoJson =JsonSerializer.Serialize(ProductoNuevo);
            Console.WriteLine($"JSON enviado a API: {contenidoJson}");
            StringContent cuerpoHttp=new StringContent(contenidoJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta =await cliente.PostAsync("api/productos", cuerpoHttp);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el producto.");
                return Page();
            }
        }
    }
}
