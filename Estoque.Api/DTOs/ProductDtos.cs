// CRIA O DTOs (RESPONSAVEL POR TRANSPORTAR OS DADOS ENTRE A O DB E A API)

using System.ComponentModel.DataAnnotations;

namespace Estoque.Api.DTOs;

public record ProductReadDto(int Id, string Name, decimal Price, int CategoryId, DateTime Created_at, string? CategoryName);//para apresentar o nome da category
// define a o tipo DTo de leitura, as propriedades e seus tipos

public class ProductCreateDto
// define o tipo de DTO de create, as propriedades, seus tipos e suas regras
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name {get; set;} = "";
    [Range(0,1_000_000)]
    public decimal Price {get; set;}

    public int CategoryId {get;set;}
    
}

public class ProductUpdateDto : ProductCreateDto{}
// Define o tipo de DTO update e herda do DTO create as propriedades, tipo e regras

// ___________________________________________________________________________________________________________
