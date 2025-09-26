using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Filters;

namespace imnobiliatiaNet.Controllers
{
    [Autenticado]
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

        public async Task<IActionResult> Index(int contratoId, int pagina = 1, int tamPagina = 10)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(contratoId);
            if (contrato == null) return NotFound();
            ViewBag.Contrato = contrato;

            var resultado = await _pagoRepo.ObtenerPaginadoAsync(contratoId, pagina, tamPagina);
            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = resultado.TotalPaginas;
            ViewBag.ContratoId = contratoId;

            return View(resultado.Items);
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
                //pago.UsuarioAltaId = HttpContext.Session.GetInt32("UsuarioId") ?? 1;
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                if (usuarioId == null) return RedirectToAction("Login", "Auth");
                pago.UsuarioAltaId = usuarioId.Value;



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
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();

            await _pagoRepo.AnularAsync(id, usuarioId.Value, motivo);
            return RedirectToAction(nameof(Index), new { contratoId = pago.ContratoId });
        }


        public async Task<IActionResult> Details(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            var pago = await _pagoRepo.ObtenerPorIdAsync(id);
            if (pago == null) return NotFound();

            // Cargar datos del contrato completo
            pago.Contrato = await _contratoRepo.ObtenerPorIdAsync(pago.ContratoId);

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
