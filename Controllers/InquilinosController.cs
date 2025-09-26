
using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;

using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Filters;

namespace inmobiliatiaNet.Controllers
{
    [Autenticado]
    public class InquilinosController : Controller
    {
        private readonly IInquilinoRepositorio _repo;

        public InquilinosController(IInquilinoRepositorio repo)
        {
            _repo = repo;
        }

        // GET: /Inquilinos
        public async Task<IActionResult> Index(string? filtro, int pagina = 1, int tamPagina = 10)
        {
            var resultado = await _repo.ListarPaginadoAsync(filtro, pagina, tamPagina);
            ViewBag.Filtro = filtro;
            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = resultado.TotalPaginas;
            return View(resultado.Items);
        }


        // GET: /Inquilinos/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Inquilinos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                await _repo.CrearAsync(inquilino);
                return RedirectToAction("Index");
            }
            return View(inquilino);
        }

        // GET: /Inquilinos/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var inquilino = await _repo.ObtenerPorIdAsync(id);
            if (inquilino == null) return NotFound();
            return View(inquilino);
        }

        // POST: /Inquilinos/Editar/5
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var actualizado = await _repo.ActualizarAsync(inquilino);
                if (!actualizado) return NotFound();
                return RedirectToAction("Index");
            }
            return View(inquilino);
        }

        // GET: /Inquilinos/Detalles/5
        public async Task<IActionResult> Detalle(int id)
        {
            var inquilino = await _repo.ObtenerPorIdAsync(id);
            if (inquilino == null) return NotFound();
            return View(inquilino);
        }

        // GET: /Inquilinos/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var inquilino = await _repo.ObtenerPorIdAsync(id);
            if (inquilino == null) return NotFound();
            return View(inquilino);
        }
        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
                return Unauthorized();

            try
            {
                var borrado = await _repo.BorrarAsync(id);
                if (!borrado)
                    return NotFound();

                return RedirectToAction("Index");
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Number == 1451)
            {
                TempData["Error"] = "No se puede eliminar el inquilino porque tiene contratos asociados.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al intentar eliminar el inquilino.";
                return RedirectToAction("Index");
            }
        }


    }
}

