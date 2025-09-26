using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Models;

using imnobiliatiaNet.Repositorios;
using imnobiliatiaNet.Filters;



namespace imnobiliatiaNet.Controllers
{
    [Autenticado]
    public class PropietariosController : Controller
    {
        private readonly IPropietarioRepositorio _repo;
        public PropietariosController(IPropietarioRepositorio repo) => _repo = repo;

        /* public async Task<IActionResult> Index(string? q)
         {
             var lista = await _repo.ListarAsync(q);
             return View(lista);
         }*/
        public async Task<IActionResult> Index(string? q, int pagina = 1, int tamPagina = 10)
        {
            var lista = await _repo.ListarPaginadoAsync(q, pagina, tamPagina);
            ViewBag.Pagina = pagina;
            ViewBag.TamPagina = tamPagina;
            ViewBag.TotalPaginas = lista.TotalPaginas;
            ViewBag.Filtro = q;
            return View(lista.Items);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Propietario p)
        {
            if (!ModelState.IsValid) return View(p);
            var id = await _repo.CrearAsync(p);
            return RedirectToAction(nameof(Editar), new { id });
        }

        public async Task<IActionResult> Editar(int id)
        {
            var p = await _repo.ObtenerPorIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Propietario p)
        {
            if (!ModelState.IsValid) return View(p);
            var ok = await _repo.ActualizarAsync(p);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(int id)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Administrador")
                return Unauthorized();

            try
            {
                var borrado = await _repo.BorrarAsync(id);
                if (!borrado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Number == 1451)
            {
                TempData["Error"] = "No se puede eliminar el propietario porque tiene inmuebles asociados.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al intentar eliminar el propietario.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var p = await _repo.ObtenerPorIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }
    }
}
