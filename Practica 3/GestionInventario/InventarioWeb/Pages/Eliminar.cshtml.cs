using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventarioWeb.Modelos;
using System.Text.Json;

namespace InventarioWeb.Pages
{
    public class EliminarModel : PageModel
    {
        private readonly IHttpClientFactory _fabricaHttp;

        public EliminarModel(IHttpClientFactory fabricaHttp)
        {
            _fabricaHttp = fabricaHttp;
        }

        public Producto ProductoEliminar { get; set; } = new Producto();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient cliente =_fabricaHttp.CreateClient("InventarioAPI");
            HttpResponseMessage respuesta=await cliente.GetAsync($"api/productos/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                string contenidoJson =await respuesta.Content.ReadAsStringAsync();
                Producto? producto =JsonSerializer.Deserialize<Producto>(contenidoJson);

                if (producto!=null)
                {
                    ProductoEliminar=producto;
                    return Page();
                }
            }
            return RedirectToPage("Index");
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            HttpClient cliente =_fabricaHttp.CreateClient("InventarioAPI");
            HttpResponseMessage respuesta =await cliente.DeleteAsync($"api/productos/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudo eliminar el producto. Intente nuevamente.");

                await OnGetAsync(id);
                return Page();
            }
        }
    }
}