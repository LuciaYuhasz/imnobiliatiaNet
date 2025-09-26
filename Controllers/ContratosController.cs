using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using imnobiliatiaNet.Filters;


namespace imnobiliatiaNet.Controllers
{
    [Autenticado]
    public class ContratosController : Controller
    {
        private readonly IContratoRepositorio _contratoRepo;
        private readonly IInmuebleRepositorio _inmuebleRepo;
        private readonly IInquilinoRepositorio _inquilinoRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;


        public ContratosController(IContratoRepositorio contratoRepo, IInmuebleRepositorio inmuebleRepo, IInquilinoRepositorio inquilinoRepo, IUsuarioRepositorio usuarioRepo)
        {
            _contratoRepo = contratoRepo;
            _inmuebleRepo = inmuebleRepo;
            _inquilinoRepo = inquilinoRepo;
            _usuarioRepo = usuarioRepo;
        }

        // GET: Contratos
        public async Task<IActionResult> Index(string? filtro, int pagina = 1, int tamPagina = 10)
        {
            var resultado = await _contratoRepo.ObtenerPaginadoAsync(filtro, pagina, tamPagina);
            ViewBag.Filtro = filtro;
            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = resultado.TotalPaginas;
            return View(resultado.Items);
        }

        // GET: Contratos/Details/5

        public async Task<IActionResult> Details(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            // Cargar auditoría
            if (contrato.UsuarioCreadorId > 0)
                contrato.UsuarioCreador = await _usuarioRepo.ObtenerPorIdAsync(contrato.UsuarioCreadorId);
            if (contrato.UsuarioTerminadorId.HasValue)
                contrato.UsuarioTerminador = await _usuarioRepo.ObtenerPorIdAsync(contrato.UsuarioTerminadorId.Value);




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
                int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 1;


                contrato.UsuarioCreadorId = usuarioId;
                // Verificar superposición de fechas
                if (await _contratoRepo.ExisteSuperposicionAsync(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin))
                {
                    TempData["Error"] = "Ya existe un contrato para ese inmueble en ese rango de fechas.";
                    await CargarListasAsync();
                    return View(contrato);
                }

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
                // Verificar superposición de fechas excluyendo el contrato actual
                if (await _contratoRepo.ExisteSuperposicionAsync(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin, contrato.Id))
                {
                    TempData["Error"] = "Ya existe un contrato para ese inmueble en ese rango de fechas.";
                    await CargarListasAsync();
                    return View(contrato);
                }

                var actualizado = await _contratoRepo.ModificarAsync(contrato);
                if (!actualizado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync();
            return View(contrato);
        }



        public async Task<IActionResult> Terminar(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            return View(contrato); // Vista con confirmación y fecha de terminación
        }

        // POST: Contratos/Terminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Terminar(int id, DateTime fechaTerminacion)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            // Validación de fecha
            if (fechaTerminacion <= contrato.FechaInicio || fechaTerminacion > contrato.FechaFin)
            {
                TempData["Error"] = "La fecha de terminación debe estar entre el inicio y el fin del contrato.";
                return RedirectToAction("Terminar", new { id });
            }

            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 1;


            var resultado = await _contratoRepo.TerminarAnticipadamenteAsync(id, fechaTerminacion, usuarioId);
            if (!resultado)
            {
                TempData["Error"] = "No se pudo registrar la terminación anticipada.";
                return RedirectToAction("Terminar", new { id });
            }

            TempData["Exito"] = "Contrato terminado anticipadamente.";
            return RedirectToAction(nameof(Details), new { id });
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
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Renovar(int id)
        {
            var contrato = await _contratoRepo.ObtenerPorIdAsync(id);
            if (contrato == null)
                return NotFound();

            // Validar que el contrato ya haya finalizado
            if (contrato.FechaFin >= DateTime.Today)
            {
                TempData["Error"] = "El contrato aún está vigente. No se puede renovar.";
                return RedirectToAction(nameof(Index));
            }

            var nuevoContrato = new Contrato
            {
                InquilinoId = contrato.InquilinoId,
                InmuebleId = contrato.InmuebleId,
                FechaInicio = contrato.FechaFin.AddDays(1),
                FechaFin = contrato.FechaFin.AddYears(1),
                Monto = contrato.Monto
            };

            ViewBag.ContratoOriginal = contrato;
            return View(nuevoContrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renovar(Contrato c)
        {
            if (!ModelState.IsValid) return View(c);

            var haySuperposicion = await _contratoRepo.ExisteSuperposicionAsync(c.InmuebleId, c.FechaInicio, c.FechaFin);
            if (haySuperposicion)
            {
                ModelState.AddModelError("", "Ya existe un contrato activo para ese inmueble en ese rango de fechas.");
                return View(c);
            }

            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 1;
            c.UsuarioCreadorId = usuarioId;

            var id = await _contratoRepo.AltaAsync(c);
            return RedirectToAction("Editar", new { id });
        }

    }
}
