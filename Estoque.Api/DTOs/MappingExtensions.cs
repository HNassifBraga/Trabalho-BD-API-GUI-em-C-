// Cria extensões de mapeamento entre entidades e DTO, criar funções (métodos de extensão) que convertem entre tipos

using Estoque.Api.Models;
using Estoque.Api.DTOs;

namespace Estoque.Api.DTOs;

public static class Mapping
{
    public static ProductReadDto ToReadDto(this Product p) => 
        new(p.Id, p.Name, p.Price, p.CategoryId, p.Created_at, p.Category?.Name);//para apresentar o nome da category
    // pega um product que temos na memoria e transforma em ProductReadDto

    public static Product ToEntity(this ProductCreateDto dto) => new()
    {
        Name = dto.Name,
        Price = dto.Price,
        CategoryId = dto.CategoryId   
    };
    // ProductCreateDto chega via API e o metodo de extensão (toEntity) converte o dto em uma entidade Product para salva-la no DB com EF Core

    public static void UpdateEntity(this ProductUpdateDto dto, Product entity)
    {
        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.CategoryId = dto.CategoryId;
    }
    // Pega um produto que temos na memória e substituimos seus valores pelos valores chegados via API, valores DTO

// ______________________________________________________________________


    public static CategoryReadDto ToReadDto(this Category c) =>
        new(c.Id, c.Name);
    
    public static Category ToEntity(this CategoryCreateDto dto)=>new()
    {
        Name = dto.Name
    };

    public static void UpdateEntity(this CategoryUpdateDto dto, Category entity)
    {
        entity.Name = dto.Name;
    }

}