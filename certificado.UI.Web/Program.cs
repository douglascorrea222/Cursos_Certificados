using Certificado.RepositorioEF.Data;
using Certificado.Aplicacao;
using Microsoft.EntityFrameworkCore;
using certificado.Dominio.Contrato;
using Certificado.Dominio.Entities;
using certificado.RepositorioEF.Repositorios;
using Microsoft.AspNetCore.Identity;
using certificado.UI.Web.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Adicionar a string de conex�o a partir do arquivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar o DbContext para usar o SQL Server
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

// Configurar Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<Contexto>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Adicionar servi�os de logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Configurar MVC e Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configurar inje��o de depend�ncia para reposit�rios e servi�os de aplica��o
// builder.Services.AddScoped<InterfaceBase<Tb_Usuario>, Tb_UsuarioRepositorioEF>();
// builder.Services.AddScoped<Tb_UsuarioAplicacao>(provider =>
//     Tb_UsuarioAplicacaoConstrutor.Tb_UsuarioRepositorioEF(provider.GetService<Contexto>()));

var app = builder.Build();

// Inicializar dados (roles e usu�rios)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<SeedData>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var seedData = new SeedData(logger, userManager, roleManager);
    await seedData.Initialize();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adiciona a autentica��o ao pipeline
app.UseAuthentication();

// Adiciona a autoriza��o ao pipeline
app.UseAuthorization();

// Configurar rotas padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
