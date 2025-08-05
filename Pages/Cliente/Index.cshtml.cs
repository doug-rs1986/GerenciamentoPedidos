using GerenciamentoPedidos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciamentoPedidos.Pages.Cliente
{
    public class IndexModel : PageModel
    {
        private readonly Data.IClienteRepository _repo;
        public List<GerenciamentoPedidos.Models.Cliente> Clientes { get; set; } = new List<GerenciamentoPedidos.Models.Cliente>();
        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        public IndexModel(Data.IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(Filtro))
                Clientes = (await _repo.SearchAsync(Filtro)).ToList();
            else
                Clientes = (await _repo.GetAllAsync()).ToList();
        }
    }
}