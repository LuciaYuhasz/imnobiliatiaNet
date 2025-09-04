using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Repositorios;
using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IInmuebleRepositorio _repo;
        private readonly IPropietarioRepositorio propietarioRepositorio;



        public InmueblesController(IInmuebleRepositorio repo, IPropietarioRepositorio propietarioRepo)
        {
            _repo = repo;
            propietarioRepositorio = propietarioRepo;
        }


        public async Task<IActionResult> Index(string? filtro, bool? disponibles)
        {
            var lista = await _repo.ObtenerTodosAsync(filtro, disponibles);
            return View(lista);
        }


        // Método GET: muestra el formulario
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var propietarios = await propietarioRepositorio.ObtenerTodosAsync(); // tu método real
            ViewBag.Propietarios = propietarios;
            return View();
        }

        // Método POST: recibe el formulario enviado
        [HttpPost]
        public async Task<IActionResult> Crear(Inmueble i)
        {
            if (!ModelState.IsValid)
            {
                var propietarios = await propietarioRepositorio.ObtenerTodosAsync();
                ViewBag.Propietarios = propietarios;
                return View(i);
            }

            await _repo.AltaAsync(i);
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<IActionResult> Editar(int id)
        {
            var i = await _repo.ObtenerPorIdAsync(id);
            if (i == null) return NotFound();
            return View(i);
        }*/
        public async Task<IActionResult> Editar(int id)
        {
            var i = await _repo.ObtenerPorIdAsync(id);
            if (i == null) return NotFound();

            var propietarios = await propietarioRepositorio.ObtenerTodosAsync();
            ViewBag.Propietarios = propietarios;

            return View(i);
        }
        /*
                [HttpPost]
                public async Task<IActionResult> Editar(Inmueble i)
                {
                    if (!ModelState.IsValid) return View(i);
                    await _repo.ModificarAsync(i);
                    return RedirectToAction(nameof(Index));
                }
        */
        [HttpPost]
        public async Task<IActionResult> Editar(Inmueble i)
        {
            if (!ModelState.IsValid)
            {
                var propietarios = await propietarioRepositorio.ObtenerTodosAsync();
                ViewBag.Propietarios = propietarios;
                return View(i);
            }

            await _repo.ModificarAsync(i);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var i = await _repo.ObtenerPorIdAsync(id);
            if (i == null) return NotFound();
            return View(i);
        }

        /*public async Task<IActionResult> Eliminar(int id)
        {
            await _repo.BajaAsync(id);
            return RedirectToAction(nameof(Index));
        }*/
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _repo.BajaAsync(id);
                if (!eliminado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Number == 1451)
            {
                TempData["Error"] = "No se puede eliminar el inmueble porque está vinculado a uno o más contratos.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al intentar eliminar el inmueble.";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
