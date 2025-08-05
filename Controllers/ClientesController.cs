using GerenciamentoPedidos.Data;
using GerenciamentoPedidos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoPedidos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteRepository _repo;
        public ClientesController(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _repo.GetAllAsync();
            return View(clientes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _repo.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            var dto = new ClienteDto { Nome = cliente.Nome, Email = cliente.Email, Telefone = cliente.Telefone };
            ViewBag.ClienteId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _repo.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
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