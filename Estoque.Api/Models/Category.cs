using System.ComponentModel.DataAnnotations;
// importa os atributos de validação/anotação mais comuns de .NET, (minlength, maxlength, keyatribute, etc)

namespace Estoque.Api.Models;
// tudo que vem abaixo entra no namespace em Estoque.Api.Models

public class Category
{
    public int Id {get;set;}
    public string Name {get;set;} = "";

    public ICollection<Product> Products {get;set;} = new List<Product>();
}