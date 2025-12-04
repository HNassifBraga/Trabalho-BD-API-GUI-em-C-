// cria os controllers da api, cria os HTPPs para fazer o crud

using Estoque.Api.Data;
using Estoque.Api.DTOs;
using Estoque.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Api.Controllers;


[ApiController]
// um atributo aplicado a classe controller, ativa os comportamentos de API no asp.net core
[Route("api/[controller]")]
//atributo de Route na classe controler; Define a rota como api/[controller] [controler] controler é um token, ProductsController -> [controller] -> api/products
[ServiceFilter(typeof(ValidateModelAttribute))]
//aplica o filtro validateModelAttribute via DI (dependecies injection) executando as validaçoes antes da action

public class ProductsController(AppDbContext db) : ControllerBase
// cria um classe , ProductsController, que recebe a instância AppDbContext e herda da classe COntrollerBase
{
    [HttpGet]
    //marca o metodo como um endpoint get na api
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll([FromQuery] string? name)
    // traz todos os itens da entidade Produto usando o ProductReadDto,se nome vier na query, filtra antes de executar a partir do nome
    {
        var q = db.Products.AsNoTracking();
        //var q = produtos sem registrar as entidades em cache no DbCOntext ou ObjectContext
        if(!string.IsNullOrWhiteSpace(name))
        {
            q=q.Where(p=> EF.Functions.Like(p.Name, $"%{name}%"));
        }
        //caso a query chegue sem nome não tem filtro, caso chegue com nome, traga q onde produto.name like query.name
        var list = await q
            .Include(p=>p.Category)// para fazer o join com a tabela category
            .OrderByDescending(p => p.Id)
            .Select( p=> p.ToReadDto())
            .ToListAsync();
        return Ok(list);
        //monta a consulta ordenando por id descrescente, projeta caca produto para PRoductReadDto, executa a query com ToListAsync e quando o retornar 200, ok, ele entrega a lista
    }

    
    [HttpGet("{id:int}")]
    //define uma rota com o prametro id
    public async Task<ActionResult<ProductReadDto>> GetById(int id)
    // busca por produto id; retorna 200 caso exista o id ou 404 caso nao exista
    {
        var p = await  db.Products
            .AsNoTracking()
            .Include(p=> p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        // ↑ Busca ASSÍNCRONA pela **chave primária**.
        //   1) Primeiro procura no ChangeTracker (já carregado no contexto).
        //   2) Se não achar, consulta o banco.
        //   3) Retorna **null** se não existir.
        //   4) É **tracked** (sem AsNoTracking), útil se você for editar/deletar depois.
        return p is null ? NotFound() : Ok(p.ToReadDto());
    }

    [HttpPost]
    // marca um metodo como um endpoint post na api
    public async Task<ActionResult<ProductReadDto>> Create ([FromBody] ProductCreateDto dto)
    //cria uma row no DB e após criada retorna a row criada no DB
    {
        var entity = dto.ToEntity();
        // chama toEntity par converter os valores que chegam do dto em entidade
        db.Products.Add(entity);
        try { await db.SaveChangesAsync();}
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE",StringComparison.OrdinalIgnoreCase)== true)
        { return Conflict(new { message = "Nome do produto deve ser único"}); }

        return CreatedAtAction(nameof(GetById), new {id = entity.Id }, entity.ToReadDto());
    }
    // Pega o valor convertido do DTO e manda o db adicionar ele; espera ser salvo; caso de um erro em que contenha uniue na mensagem, ele avisa que o nome precisa ser único; caso não de esse erro ele retorna a row adicionada
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
    // recebe ProductUpdateDto no body, atualiza o produto e retorna 204 ou 404.
    {
        var entity = await db.Products.FindAsync(id);
        if(entity is null) return NotFound();

        dto.UpdateEntity((entity));
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)== true)
        { return Conflict(new { message = "Nome do produto deve ser único"}); }

        return Ok(new { message = "Atualizado com sucesso", data = entity.ToReadDto() });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    // Deleta um iten, podendo varios tipos de retornos actionResult, deletando a partir do id
    {
        var entity = await db.Products.FindAsync(id);
        if(entity is null) return NotFound();

        db.Products.Remove(entity);
        await db.SaveChangesAsync();
        return Ok(new { message = "Deletado com sucesso", data = entity.ToReadDto() });
    }

}

// ______________________________________________________________________________________________________________________


[ApiController]
// um atributo aplicado a classe controller, ativa os comportamentos de API no asp.net core
[Route("api/[controller]")]
//atributo de Route na classe controler; Define a rota como api/[controller] [controler] controler é um token, ProductsController -> [controller] -> api/products
[ServiceFilter(typeof(ValidateModelAttribute))]
public class CategoryController(AppDbContext db) : ControllerBase
// cria um classe , CategoryController, que recebe a instância AppDbContext e herda da classe COntrollerBase
{
    [HttpGet]
    //marca o metodo como um endpoint get na api
    public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetAll([FromQuery] string? name)
    // traz todos os itens da entidade Category usando o CategoryReadDto,se nome vier na query, filtra antes de executar a partir do nome
    {
        var q = db.Category.AsNoTracking();
        //var q = Categorys sem registrar as entidades em cache no DbCOntext ou ObjectContext
        if(!string.IsNullOrWhiteSpace(name))
        {
            q=q.Where(p=> EF.Functions.Like(p.Name, $"%{name}%"));
        }
        //caso a query chegue sem nome não tem filtro, caso chegue com nome, traga q onde Category.name like query.name
        var list = await q
            .OrderByDescending(p => p.Id)
            .Select( p=> p.ToReadDto())
            .ToListAsync();
        return Ok(list);
        //monta a consulta ordenando por id descrescente, projeta caca Category para PRoductReadDto, executa a query com ToListAsync e quando o retornar 200, ok, ele entrega a lista
    }
    [HttpGet("{id:int}")]
    //define uma rota com o prametro id
    public async Task<ActionResult<CategoryReadDto>> GetById(int id)
    // busca por Category id; retorna 200 caso exista o id ou 404 caso nao exista
    {
        var p = await db.Category.FindAsync(id);
        // ↑ Busca ASSÍNCRONA pela **chave primária**.
        //   1) Primeiro procura no ChangeTracker (já carregado no contexto).
        //   2) Se não achar, consulta o banco.
        //   3) Retorna **null** se não existir.
        //   4) É **tracked** (sem AsNoTracking), útil se você for editar/deletar depois.
        return p is null ? NotFound() : Ok(p.ToReadDto());
    }

    [HttpPost]
    // marca um metodo como um endpoint post na api
    public async Task<ActionResult<CategoryReadDto>> Create ([FromBody] CategoryCreateDto dto)
    //cria uma row no DB e após criada retorna a row criada no DB
    {
        var entity = dto.ToEntity();
        // chama toEntity par converter os valores que chegam do dto em entidade
        db.Category.Add(entity);
        try { await db.SaveChangesAsync();}
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE",StringComparison.OrdinalIgnoreCase)== true)
        { return Conflict(new { message = "Nome da categoria deve ser único"}); }

        return CreatedAtAction(nameof(GetById), new {id = entity.Id }, entity.ToReadDto());
    }
    // Pega o valor convertido do DTO e manda o db adicionar ele; espera ser salvo; caso de um erro em que contenha uniue na mensagem, ele avisa que o nome precisa ser único; caso não de esse erro ele retorna a row adicionada
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
    // recebe ProductUpdateDto no body, atualiza o Category e retorna 204 ou 404.
    {
        var entity = await db.Category.FindAsync(id);
        if(entity is null) return NotFound();

        dto.UpdateEntity((entity));
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)== true)
        { return Conflict(new { message = "Nome da categoria deve ser único"}); }

        return Ok(new { message = "Atualizado com sucesso", data = entity.ToReadDto() });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    // Deleta um iten, podendo varios tipos de retornos actionResult, deletando a partir do id
    {
        var entity = await db.Category.FindAsync(id);
        if(entity is null) return NotFound();

        db.Category.Remove(entity);
        await db.SaveChangesAsync();
        return Ok(new { message = "Deletado com sucesso", data = entity.ToReadDto() });
    }

}
