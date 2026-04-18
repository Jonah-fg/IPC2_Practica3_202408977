using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventarioWeb.Modelos;
using System.Text;
using System.Text.Json;

namespace InventarioWeb.Pages
{
    public class EditarModel : PageModel
    {
        private readonly IHttpClientFactory _fabricaHttp;

        public EditarModel(IHttpClientFactory fabricaHttp)
        {
            _fabricaHttp =fabricaHttp;
        }

        [BindProperty]
        public Producto ProductoActualizar { get; set; }=new Producto();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient cliente= _fabricaHttp.CreateClient("InventarioAPI");
            HttpResponseMessage respuesta=await cliente.GetAsync($"api/productos/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                string contenidoJson =await respuesta.Content.ReadAsStringAsync();
                Producto? producto =JsonSerializer.Deserialize<Producto>(contenidoJson);

                if (producto!=null)
                {
                    ProductoActualizar=producto;
                    return Page();
                }
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpClient cliente= _fabricaHttp.CreateClient("InventarioAPI");
            string contenidoJson=JsonSerializer.Serialize(ProductoActualizar);
            StringContent cuerpoHttp =new StringContent(contenidoJson, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta =await cliente.PutAsync($"api/productos/{id}", cuerpoHttp);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");
                return Page();
            }
        }
    }
}