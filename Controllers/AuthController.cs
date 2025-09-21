using imnobiliatiaNet.Models;
using imnobiliatiaNet.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
public class AuthController : Controller
{
    private readonly IUsuarioRepositorio _usuarioRepo;

    public AuthController(IUsuarioRepositorio usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    // GET: Login
    public IActionResult Login() => View();

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string clave)
    {
        var usuario = await _usuarioRepo.ObtenerPorEmailAsync(email);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(clave, usuario.ClaveHash))
        {
            TempData["Error"] = "Email o contraseña incorrectos.";
            return View();
        }

        // Guardar datos en sesión
        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
        HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
        HttpContext.Session.SetString("UsuarioAvatar", usuario.AvatarUrl ?? "");

        return RedirectToAction("Panel", "Home");
    }

    // GET: Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    public async Task<IActionResult> SeedAdmin()
    {
        var nuevoUsuario = new Usuario
        {
            Email = "admin@ejemplo.com",
            Nombre = "Administrador",
            Rol = "Administrador",
            ClaveHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            AvatarUrl = null
        };

        await _usuarioRepo.CrearAsync(nuevoUsuario);
        return Content("Usuario administrador creado");
    }



    // GET: Register
    //public IActionResult Register() => View();
    public IActionResult Register()
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        if (rol != "Administrador")
            return RedirectToAction("Login");

        return View();
    }

    // POST: Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string email, string nombre, string clave, string rol)
    {

        var rolActual = HttpContext.Session.GetString("UsuarioRol");
        if (rolActual != "Administrador")
        {
            return RedirectToAction("Login");
        }
        var existente = await _usuarioRepo.ObtenerPorEmailAsync(email);
        if (existente != null)
        {
            TempData["Error"] = "Ya existe un usuario con ese email.";
            return View();
        }

        var nuevoUsuario = new Usuario
        {
            Email = email,
            Nombre = nombre,
            Rol = rol,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(clave),
            AvatarUrl = null
        };

        await _usuarioRepo.CrearAsync(nuevoUsuario);
        TempData["Success"] = "Usuario registrado correctamente.";
        return RedirectToAction("Login");
    }
    public IActionResult SoloAdmin()
    {
        var rol = HttpContext.Session.GetString("UsuarioRol");
        if (rol != "Administrador")
            return Unauthorized(); // o RedirectToAction("Login")

        // lógica solo para admins
        return View();
    }
    public async Task<IActionResult> Perfil()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login");

        var usuario = await _usuarioRepo.ObtenerPorIdAsync(usuarioId.Value);
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Perfil(string nombre, string clave, string avatarUrl)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login");

        var usuario = await _usuarioRepo.ObtenerPorIdAsync(usuarioId.Value);
        usuario.Nombre = nombre;
        if (!string.IsNullOrEmpty(clave))
            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(clave);
        usuario.AvatarUrl = avatarUrl;

        await _usuarioRepo.ActualizarAsync(usuario);
        TempData["Success"] = "Perfil actualizado correctamente.";
        return RedirectToAction("Perfil");
    }
    [HttpPost]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarAvatar()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login");

        var usuario = await _usuarioRepo.ObtenerPorIdAsync(usuarioId.Value);
        if (usuario == null) return NotFound();

        usuario.AvatarUrl = null;
        await _usuarioRepo.ActualizarAsync(usuario); // o ModificarAsync si usás ese nombre

        TempData["Success"] = "Avatar eliminado correctamente.";
        return RedirectToAction("Perfil");
    }



}
