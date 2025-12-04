// CRIA O DTOs (RESPONSAVEL POR TRANSPORTAR OS DADOS ENTRE A O DB E A API)

using System.ComponentModel.DataAnnotations;

namespace Estoque.Api.DTOs;


public record CategoryReadDto(int Id, string Name);

public class CategoryCreateDto
{
    [Required, StringLength(120,MinimumLength = 2)]
    public string Name {get;set;}="";
}

public class CategoryUpdateDto : CategoryCreateDto{}