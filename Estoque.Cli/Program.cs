using System.Globalization;
using Estoque.Api.Data;
using Estoque.Api.Models;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../Estoque.Api/estoque.db") // usa o mesmo DB da API
    .Options;

using var db = new AppDbContext(options);
await db.Database.EnsureCreatedAsync();

if (args.Length == 0)
{
    Console.WriteLine("""
Uso:
  list
  get <id>
  add "<name>" <price> [category]
  update <id> "<name>" <price> [category]
  delete <id>
Exemplos:
  add "Chocolate 90g" 5.99 "Doces"
  list
  get 2
  update 2 "Chocolate 90g Amargo" 6.49 "Doces"
  delete 2
""");
    return;
}

switch (args[0].ToLowerInvariant())
{
    case "list":
        foreach (var p in db.Products.AsNoTracking().OrderByDescending(p => p.Id))
            Console.WriteLine($"{p.Id}: {p.Name} - R${p.Price} ({p.Category})");
        break;

    case "get":
        if (args.Length < 2 || !int.TryParse(args[1], out var idG)) { Console.WriteLine("ID inválido."); return; }
        var g = await db.Products.FindAsync(idG);
        Console.WriteLine(g is null ? "Não encontrado." : $"{g.Id}: {g.Name} - R${g.Price} ({g.Category})");
        break;

    case "add":
        if (args.Length < 3) { Console.WriteLine("Uso: add \"<name>\" <price> [category]"); return; }
        var nameA = args[1];
        if (!decimal.TryParse(args[2], NumberStyles.Number, CultureInfo.InvariantCulture, out var priceA))
        { Console.WriteLine("Preço inválido (use ponto decimal)."); return; }
        var catA = args.Length >= 4 ? args[3] : null;
        db.Products.Add(new Product { Name = nameA, Price = priceA, Category = catA });
        try { await db.SaveChangesAsync(); Console.WriteLine("Criado."); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true)
        { Console.WriteLine("Conflito: nome do produto deve ser único."); }
        break;

    case "update":
        if (args.Length < 4 || !int.TryParse(args[1], out var idU))
        { Console.WriteLine("Uso: update <id> \"<name>\" <price> [category]"); return; }
        var u = await db.Products.FindAsync(idU);
        if (u is null) { Console.WriteLine("Não encontrado."); return; }
        var nameU = args[2];
        if (!decimal.TryParse(args[3], NumberStyles.Number, CultureInfo.InvariantCulture, out var priceU))
        { Console.WriteLine("Preço inválido (use ponto decimal)."); return; }
        var catU = args.Length >= 5 ? args[4] : null;
        u.Name = nameU; u.Price = priceU; u.Category = catU;
        try { await db.SaveChangesAsync(); Console.WriteLine("Atualizado."); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true)
        { Console.WriteLine("Conflito: nome do produto deve ser único."); }
        break;

    case "delete":
        if (args.Length < 2 || !int.TryParse(args[1], out var idD)) { Console.WriteLine("ID inválido."); return; }
        var d = await db.Products.FindAsync(idD);
        if (d is null) { Console.WriteLine("Não encontrado."); return; }
        db.Products.Remove(d);
        await db.SaveChangesAsync();
        Console.WriteLine("Removido.");
        break;

    default:
        Console.WriteLine("Comando desconhecido.");
        break;
}
