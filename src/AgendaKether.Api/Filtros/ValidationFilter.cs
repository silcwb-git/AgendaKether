using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgendaKether.Api.Filtros;

public class ValidationFilter<T> : IAsyncActionFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator) => _validator = validator;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var argumento = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (argumento is not null)
        {
            var resultado = await _validator.ValidateAsync(argumento, context.HttpContext.RequestAborted);

            if (!resultado.IsValid)
            {
                var erros = resultado.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                var problema = new ValidationProblemDetails(erros)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Erro de validação",
                    Instance = context.HttpContext.Request.Path,
                    Type = "https://httpstatuses.io/400"
                };

                problema.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                context.Result = new BadRequestObjectResult(problema);
                return;
            }
        }

        await next();
    }
}