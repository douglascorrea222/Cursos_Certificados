using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Certificado.RepositorioEF.Data
{
    public class ContextoFactory : IDesignTimeDbContextFactory<Contexto>
    {
        public Contexto CreateDbContext(string[] args)
        {
            // Diretório atual do projeto
            var basePath = Directory.GetCurrentDirectory();

            // Verifica se o arquivo appsettings.json está presente no diretório atual
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                // Se não encontrado, ajusta para o diretório do projeto de inicialização
                basePath = Path.GetFullPath(Path.Combine(basePath, "..", "Certificado.UI.Web"));
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<Contexto>();
            optionsBuilder.UseSqlServer(connectionString);

            return new Contexto(optionsBuilder.Options);
        }
    }
}
