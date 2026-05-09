using Microsoft.EntityFrameworkCore;
using CompletaJaApp.Models;

namespace CompletaJaApp.Data
{
    // O ": DbContext" faz com que a nossa classe herde os "superpoderes" do Entity Framework
    public class CompletaJaContext : DbContext
    {
        // Esse é o construtor. Ele serve para receber as configurações que fizemos
        // lá no arquivo appsettings.json (como o endereço do banco de dados).
        public CompletaJaContext(DbContextOptions<CompletaJaContext> options) : base(options)
        {
        }

        // Aqui nós apresentamos nossos Modelos para o banco de dados!
        // 'DbSet' significa 'Conjunto de Dados'. Cada um vai virar uma Tabela no SQL Server.
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Album> Albuns { get; set; }
        public DbSet<Local> Locais { get; set; }
    }
}