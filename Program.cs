var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// 🧠 Sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// ADO.NET helper
builder.Services.AddSingleton<imnobiliatiaNet.Data.Db>();
builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IPropietarioRepositorio,
                           imnobiliatiaNet.Repositorios.PropietarioRepositorio>();
builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IInquilinoRepositorio,
                           imnobiliatiaNet.Repositorios.InquilinoRepositorio>();

builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IInmuebleRepositorio,
                           imnobiliatiaNet.Repositorios.InmuebleRepositorio>();
builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IContratoRepositorio,
                           imnobiliatiaNet.Repositorios.ContratoRepositorio>();
builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IPagoRepositorio,
                           imnobiliatiaNet.Repositorios.PagoRepositorio>();
builder.Services.AddScoped<imnobiliatiaNet.Repositorios.IUsuarioRepositorio,
                           imnobiliatiaNet.Repositorios.UsuarioRepositorio>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();



