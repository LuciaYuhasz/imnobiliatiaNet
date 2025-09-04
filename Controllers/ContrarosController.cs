using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace imnobiliatiaNet.Controllers
{
    public class ContratosController : Controller
    {
        private readonly IContratoRepositorio _contratoRepo;
        private readonly IInmuebleRepositorio _inmuebleRepo;
        private readonly IInquilinoRepositorio _inquilinoRepo;

        public ContratosController(IContratoRepositorio contratoRepo, IInmuebleRepositorio inmuebleRepo, IInquilinoRepositorio inquilinoRepo)
        {
            _contratoRepo = contratoRepo;
            _inmuebleRepo = inmuebleRepo;
            _inquilinoRepo = inquilinoRepo;
        }

        // GET: Contratos
        public async Task<IActionResult> Index()
        {
            var lista = await _contratoRepo.ObtenerTodosAsync();
            return View(lista);
        }

        // GET: Contratos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            return View(contrato);
        }

        // GET: Contratos/Create
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                await _contratoRepo.AltaAsync(contrato);
                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync();
            return View(contrato);
        }

        // GET: Contratos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            await CargarListasAsync();
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contrato contrato)
        {
            if (id != contrato.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var actualizado = await _contratoRepo.ModificarAsync(contrato);
                if (!actualizado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync();
            return View(contrato);
        }

        // GET: Contratos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eliminado = await _contratoRepo.BajaAsync(id);
            if (!eliminado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Método auxiliar para cargar listas de inmuebles e inquilinos
        private async Task CargarListasAsync()
        {
            var inmuebles = await _inmuebleRepo.ObtenerTodosAsync();
            ViewBag.Inmuebles = inmuebles.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.Direccion} - {i.Tipo}"
            }).ToList();

            var inquilinos = await _inquilinoRepo.ObtenerTodosAsync();
            ViewBag.Inquilinos = inquilinos.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.Apellido}, {i.Nombre}"
            }).ToList();
        }
    }
}
