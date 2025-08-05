using GerenciamentoPedidos.Data;
using GerenciamentoPedidos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPedidos.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IPedidoRepository _repo;
        public PedidosController(IPedidoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _repo.GetAllAsync();
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
            await _repo.AddAsync(dto);
            return RedirectToAction(nameof(Index));
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
            await _repo.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
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
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}