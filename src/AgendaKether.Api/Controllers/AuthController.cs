using AgendaKether.Api.Filtros;
using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Auth.Commands.Login;
using AgendaKether.Application.Auth.Commands.RegistrarUsuario;
using AgendaKether.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaKether.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    [HttpPost("registrar")]
    [ServiceFilter(typeof(ValidationFilter<RegistrarUsuarioCommand>))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarUsuarioCommand command,
        [FromServices] ICommandHandler<RegistrarUsuarioCommand, Guid> handler,
        CancellationToken ct)
    {
        var id = await handler.Handle(command, ct);
        return Created($"/api/usuarios/{id}", new { id });
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilter<LoginCommand>))]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> Login(
        [FromBody] LoginCommand command,
        [FromServices] ICommandHandler<LoginCommand, TokenDto> handler,
        CancellationToken ct)
    {
        var token = await handler.Handle(command, ct);
        return Ok(token);
    }
}
