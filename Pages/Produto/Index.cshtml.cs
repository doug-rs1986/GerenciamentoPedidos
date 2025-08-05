using GerenciamentoPedidos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciamentoPedidos.Pages.Produto
{
    public class IndexModel : PageModel
    {
        private readonly Data.IProdutoRepository _repo;
        public List<GerenciamentoPedidos.Models.Produto> Produtos { get; set; } = new();

        public IndexModel(Data.IProdutoRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            Produtos = (await _repo.GetAllAsync()).ToList();
        }
    }
}