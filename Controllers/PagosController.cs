using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace imnobiliatiaNet.Controllers
{
    public class PagosController : Controller
    {
        private readonly IPagoRepositorio _pagoRepo;
        private readonly IContratoRepositorio _contratoRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public PagosController(IPagoRepositorio pagoRepo, IContratoRepositorio contratoRepo, IUsuarioRepositorio usuarioRepo)
        {
            _pagoRepo = pagoRepo;
            _contratoRepo = contratoRepo;
            _usuarioRepo = usuarioRepo;
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
                pago.UsuarioAltaId = HttpContext.Session.GetInt32("UsuarioId") ?? 1;


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id, string motivo)
        {
            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();

            var claim = User.FindFirst("Id");
            int usuarioId = claim != null && int.TryParse(claim.Value, out var idUsuario) ? idUsuario : 1;

            await _pagoRepo.AnularAsync(id, usuarioId, motivo);
            return RedirectToAction(nameof(Index), new { contratoId = pago.ContratoId });
        }
        public async Task<IActionResult> Details(int id)
        {
            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();

            // Cargar datos del usuario creador
            if (pago.UsuarioAltaId.HasValue)
                pago.UsuarioAlta = await _usuarioRepo.ObtenerPorIdAsync(pago.UsuarioAltaId.Value);

            // Cargar datos del usuario anulador
            if (pago.UsuarioAnulacionId.HasValue)
                pago.UsuarioAnulacion = await _usuarioRepo.ObtenerPorIdAsync(pago.UsuarioAnulacionId.Value);

            return View(pago);
        }



    }
}
