using GerenciamentoPedidos.Data;
using GerenciamentoPedidos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPedidos.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _repo;
        public ProdutosController(IProdutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _repo.GetAllAsync();
            return View(produtos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var produto = await _repo.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _repo.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _repo.GetByIdAsync(id);
            if (produto == null) return NotFound();
            var dto = new ProdutoDto { Nome = produto.Nome, Descricao = produto.Descricao, Preco = produto.Preco, QuantidadeEstoque = produto.QuantidadeEstoque };
            ViewBag.ProdutoId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _repo.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _repo.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}