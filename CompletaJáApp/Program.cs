// Importando as pastas que precisamos (coloque no topo do arquivo)
using Microsoft.EntityFrameworkCore;
using CompletaJaApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. ADICIONANDO OS SERVIÇOS
builder.Services.AddControllersWithViews();

// Configurando a conexão com o Banco de Dados SQL Server
builder.Services.AddDbContext<CompletaJaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilita a memória (Sessão) para o nosso sistema de Login
builder.Services.AddSession();

var app = builder.Build();

// 2. CONFIGURANDO O COMPORTAMENTO DO SITE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ativa a Sessão de fato (Aviso: tem que ficar exatamente aqui!)
app.UseSession();

app.UseAuthorization();

// 3. CONFIGURANDO A TELA INICIAL
// Mudamos aqui para o site abrir direto no Account (Login) ao invés do Home
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();