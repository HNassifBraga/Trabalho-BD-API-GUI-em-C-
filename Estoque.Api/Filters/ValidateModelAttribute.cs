//Cria os Action Filters, caso o DTO não passe nas DataAnnotations([requires],[minLength],etc) o filtro interrompe a execução

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Estoque.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
// Cria uma classe que herda de ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    // Sobrescreve o “gancho” que roda ANTES da action do controller.
    {
        if(!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
}