using GerenciamentoPedidos.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciamentoPedidos.Pages.Pedido
{
    public class IndexModel : PageModel
    {
        private readonly Data.IPedidoRepository _repo;
        public List<GerenciamentoPedidos.Models.Pedido> Pedidos { get; set; } = new List<GerenciamentoPedidos.Models.Pedido>();

        public IndexModel(Data.IPedidoRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            Pedidos = (await _repo.GetAllAsync()).ToList();
        }
    }
}