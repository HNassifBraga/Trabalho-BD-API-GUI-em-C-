// CRIAR O MODELO (descreve como os tipos de entidade do aplicativo são mapeados para o banco de dados subjacente)


using System.ComponentModel.DataAnnotations;
// importa os atributos de validação/anotação mais comuns de .NET, (minlength, maxlength, keyatribute, etc)

namespace Estoque.Api.Models;
// tudo que vem abaixo entra no namespace em Estoque.Api.Models

public class Product
{
    public int Id {get; set;}

    [Required, StringLength(120, MinimumLength = 2)]
    public string Name {get;set;} = "";

    [Range(0, 1_000_000)]
    public decimal Price {get;set;}

    [Required]
    public int CategoryId {get;set;}

    public Category? Category {get;set;}

    public DateTime Created_at {get;set;} = DateTime.UtcNow;
}