using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;

public class UsuariosController : Controller
{
    private readonly IUsuarioRepositorio _usuarioRepo;

    public UsuariosController(IUsuarioRepositorio usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IActionResult> Index()
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        if (rol != "Administrador") return Unauthorized();

        var usuarios = await _usuarioRepo.ObtenerTodosAsync();
        return View(usuarios);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        if (rol != "Administrador") return Unauthorized();

        var usuario = await _usuarioRepo.ObtenerPorIdAsync(id);
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Usuario usuario)
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        if (rol != "Administrador") return Unauthorized();

        await _usuarioRepo.ActualizarAsync(usuario);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (rol != "Administrador" || usuarioId == id) return Unauthorized();

        await _usuarioRepo.BorrarAsync(id);
        return RedirectToAction("Index");
    }
}
