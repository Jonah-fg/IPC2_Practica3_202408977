using InventarioWeb.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace InventarioWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _fabricaHttp;

        public IndexModel(IHttpClientFactory fabricaHttp)
        {
            _fabricaHttp = fabricaHttp;
        }

        public List<Producto> ListaProductos { get; set; }=new List<Producto>();

        public async Task OnGetAsync()
        {
            HttpClient cliente= _fabricaHttp.CreateClient("InventarioAPI");
            HttpResponseMessage respuesta =await cliente.GetAsync("api/productos");

            if (respuesta.IsSuccessStatusCode)
            {
                string contenidoJson =await respuesta.Content.ReadAsStringAsync();
                JsonSerializerOptions opcionesJson =new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive=true
                };
                List<Producto>? productos=JsonSerializer.Deserialize<List<Producto>>(contenidoJson, opcionesJson);

                if (productos!=null)
                {
                    ListaProductos =productos;
                }
            }
        }
    }
}
