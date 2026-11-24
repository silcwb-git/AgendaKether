using AgendaKether.Api.Filtros;
using AgendaKether.Api.Middlewares;
using AgendaKether.Application;
using AgendaKether.Application.Contatos.Commands.AtualizarContato;
using AgendaKether.Application.Contatos.Commands.CriarContato;
using AgendaKether.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Filtros de validação: um por command que tem validator
builder.Services.AddScoped<ValidationFilter<CriarContatoCommand>>();
builder.Services.AddScoped<ValidationFilter<AtualizarContatoCommand>>();

var app = builder.Build();

// Ordem importa: correlação primeiro, para que o erro já tenha TraceId
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExcecaoMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();