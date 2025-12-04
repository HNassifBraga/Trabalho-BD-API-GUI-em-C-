// CRIAR O CONTEXTO (O QUE CONECTA O DB COM O C#)

using Estoque.Api.Models;
//Aponta para as classes dentro do diretório Models
using Microsoft.EntityFrameworkCore;
//Importa o namespace de Entity Framework Core

namespace Estoque.Api.Data;
//Tudo abaixo a partir de agora sera referente ao namespace Estoque.Api.Data

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
// Derfine um construtor que recebe DbCOntextOptions<AppDbContext> e chama o construtor da classe base (DbContext) passando options
{
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    // Propriedade que expõe o conjunto Product no DbContext. Chama DbContext.Set<Product>() e retorna Dbset<Product>
    protected override void OnModelCreating(ModelBuilder mb) 
    //OnModelCreating(ModelBuilder mb) é o gancho do EF Core para configurar o modelo (mapeamentos) via Fluent API
    {
        mb.Entity<Product>(e => {
            //pega o builder da entidade Product
            e.HasIndex(p => p.Name).IsUnique();
            e.Property(p => p.Price).HasColumnType("decimal(12,2)");
        });

        mb.Entity<Category>(e => {
            //pega o builder da entidade Product
            e.HasIndex(p => p.Name).IsUnique();
        });
    }


    


}