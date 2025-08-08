using GerenciamentoPedidos.Data;
using GerenciamentoPedidos.DTOs;
using GerenciamentoPedidos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPedidos.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IPedidoRepository _repo;
        private readonly IProdutoRepository _produtoRepo;
        public PedidosController(IPedidoRepository repo, IProdutoRepository produtoRepo)
        {
            _repo = repo;
            _produtoRepo = produtoRepo;
        }

        public async Task<IActionResult> Index(int? clienteId, string status)
        {
            var pedidos = await _repo.GetFilteredAsync(clienteId, status);
            return View(pedidos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                decimal valorTotal = 0;
                foreach (var item in dto.Itens)
                {
                    var produto = await _produtoRepo.GetByIdAsync(item.ProdutoId);
                    if (produto == null)
                    {
                        ModelState.AddModelError("", $"Produto ID {item.ProdutoId} não encontrado.");
                        return View(dto);
                    }
                    if (!await _produtoRepo.HasStockAsync(item.ProdutoId, item.Quantidade))
                    {
                        ModelState.AddModelError("", $"Estoque insuficiente para o produto {produto.Nome}.");
                        return View(dto);
                    }
                    item.PrecoUnitario = produto.Preco;
                    valorTotal += item.PrecoUnitario * item.Quantidade;
                }
                dto.ValorTotal = valorTotal;
                dto.DataPedido = DateTime.Now;
                var pedidoId = await _repo.AddAsync(dto);
                foreach (var item in dto.Itens)
                {
                    await _produtoRepo.DecrementStockAsync(item.ProdutoId, item.Quantidade);
                    var itemPedido = new ItemPedido
                    {
                        PedidoId = pedidoId,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario
                    };
                    await _repo.AddItemAsync(itemPedido);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            var dto = new PedidoDto { ClienteId = pedido.ClienteId, DataPedido = pedido.DataPedido, ValorTotal = pedido.ValorTotal, Status = pedido.Status };
            ViewBag.PedidoId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PedidoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _repo.UpdateAsync(id, dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}