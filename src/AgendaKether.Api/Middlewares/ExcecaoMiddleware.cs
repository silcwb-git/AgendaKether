using AgendaKether.Domain.Excecoes;
using Microsoft.AspNetCore.Mvc;

namespace AgendaKether.Api.Middlewares;

public class ExcecaoMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExcecaoMiddleware> _logger;
    private readonly IHostEnvironment _ambiente;

    public ExcecaoMiddleware(
        RequestDelegate next,
        ILogger<ExcecaoMiddleware> logger,
        IHostEnvironment ambiente)
    {
        _next = next;
        _logger = logger;
        _ambiente = ambiente;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Regra de negócio violada. TraceId: {TraceId}", context.TraceIdentifier);

            await Responder(context, StatusCodes.Status400BadRequest,
                "Regra de negócio violada", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado. TraceId: {TraceId}", context.TraceIdentifier);

            var detalhe = _ambiente.IsDevelopment()
                ? ex.Message
                : "Ocorreu um erro inesperado. Informe o TraceId ao suporte.";

            await Responder(context, StatusCodes.Status500InternalServerError,
                "Erro interno", detalhe);
        }
    }

    private static async Task Responder(HttpContext context, int status, string titulo, string detalhe)
    {
        if (context.Response.HasStarted)
            return;

        var correlationId = context.TraceIdentifier;

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        // Response.Clear() apaga os headers, inclusive o do CorrelationIdMiddleware.
        // Recolocamos para manter o rastreio também nas respostas de erro.
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        var problema = new ProblemDetails
        {
            Status = status,
            Title = titulo,
            Detail = detalhe,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.io/{status}"
        };

        problema.Extensions["traceId"] = correlationId;

        await context.Response.WriteAsJsonAsync(problema);
    }
}