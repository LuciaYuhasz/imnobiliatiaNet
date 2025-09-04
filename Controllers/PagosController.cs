using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace imnobiliatiaNet.Controllers
{
    public class PagosController : Controller
    {
        private readonly IPagoRepositorio _pagoRepo;
        private readonly IContratoRepositorio _contratoRepo;

        public PagosController(IPagoRepositorio pagoRepo, IContratoRepositorio contratoRepo)
        {
            _pagoRepo = pagoRepo;
            _contratoRepo = contratoRepo;
        }

        // Listar pagos de un contrato
        public async Task<IActionResult> Index(int contratoId)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(contratoId);
            if (contrato == null) return NotFound();

            ViewBag.Contrato = contrato;
            var pagos = await _pagoRepo.ObtenerPorContratoAsync(contratoId);
            return View(pagos);
        }

        // GET: Crear pago
        public IActionResult Create(int contratoId)
        {
            var pago = new Pago { ContratoId = contratoId, FechaPago = DateTime.Today };
            return View(pago);
        }

        // POST: Crear pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                await _pagoRepo.CrearAsync(pago);
                return RedirectToAction(nameof(Index), new { contratoId = pago.ContratoId });
            }
            return View(pago);
        }

        // GET: Editar concepto
        public async Task<IActionResult> Edit(int id)
        {
            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null || pago.Anulado) return NotFound();
            return View(pago);
        }

        // POST: Editar concepto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Pago pago)
        {
            if (ModelState.IsValid)
            {
                await _pagoRepo.ActualizarConceptoAsync(pago);
                return RedirectToAction(nameof(Index), new { contratoId = pago.ContratoId });
            }
            return View(pago);
        }

        // GET: Confirmar anulación
        public async Task<IActionResult> Anular(int id)
        {
            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();
            return View(pago);
        }

        // POST: Anular pago
        [HttpPost, ActionName("Anular")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnularConfirmed(int id)
        {
            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();

            await _pagoRepo.AnularAsync(id);
            return RedirectToAction(nameof(Index), new { contratoId = pago.ContratoId });
        }
    }
}
