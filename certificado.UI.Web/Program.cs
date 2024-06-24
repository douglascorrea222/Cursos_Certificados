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

// Adicionar a string de conexão a partir do arquivo appsettings.json
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

// Adicionar serviços de logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Configurar MVC e Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configurar injeção de dependência para repositórios e serviços de aplicação
// builder.Services.AddScoped<InterfaceBase<Tb_Usuario>, Tb_UsuarioRepositorioEF>();
// builder.Services.AddScoped<Tb_UsuarioAplicacao>(provider =>
//     Tb_UsuarioAplicacaoConstrutor.Tb_UsuarioRepositorioEF(provider.GetService<Contexto>()));

var app = builder.Build();

// Inicializar dados (roles e usuários)
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

// Adiciona a autenticação ao pipeline
app.UseAuthentication();

// Adiciona a autorização ao pipeline
app.UseAuthorization();

// Configurar rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
