using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using AgendaKether.Api.Filtros;
using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Contatos.Commands.AtualizarContato;
using AgendaKether.Application.Contatos.Commands.CriarContato;
using AgendaKether.Application.Contatos.Commands.ExcluirContato;
using AgendaKether.Application.Contatos.Queries.ListarContatos;
using AgendaKether.Application.Contatos.Queries.ObterContatoPorId;
using AgendaKether.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AgendaKether.Api.Controllers;

[ApiController]
[Authorize]
[Authorize]
[Route("api/contatos")]
public class ContatosController : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CriarContatoCommand>))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(
        [FromBody] CriarContatoCommand command,
        [FromServices] ICommandHandler<CriarContatoCommand, Guid> handler,
        CancellationToken ct)
    {
        var id = await handler.Handle(command, ct);

        return Created($"/api/contatos/{id}", new { id });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContatoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContatoDto>> ObterPorId(
        Guid id,
        [FromServices] IQueryHandler<ObterContatoPorIdQuery, ContatoDto?> handler,
        CancellationToken ct)
    {
        var dto = await handler.Handle(new ObterContatoPorIdQuery(id), ct);

        if (dto is null)
            return NotFound();

        return Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListarContatosResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<ListarContatosResult>> Listar(
        [FromQuery] string? busca,
        [FromServices] IQueryHandler<ListarContatosQuery, ListarContatosResult> handler,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        CancellationToken ct = default)
    {
        var resultado = await handler!.Handle(
            new ListarContatosQuery(busca, pagina, tamanhoPagina), ct);

        return Ok(resultado);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilter<AtualizarContatoCommand>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        Guid id,
        [FromBody] AtualizarContatoCommand command,
        [FromServices] ICommandHandler<AtualizarContatoCommand, bool> handler,
        CancellationToken ct)
    {
        var atualizou = await handler.Handle(command with { Id = id }, ct);

        if (!atualizou)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(
        Guid id,
        [FromServices] ICommandHandler<ExcluirContatoCommand, bool> handler,
        CancellationToken ct)
    {
        var excluiu = await handler.Handle(new ExcluirContatoCommand(id), ct);

        if (!excluiu)
            return NotFound();

        return NoContent();
    }
}