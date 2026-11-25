using System.Security.Claims;
using System.Text;
using AgendaKether.Api.Filtros;
using AgendaKether.Api.Middlewares;
using AgendaKether.Application;
using AgendaKether.Application.Auth.Commands.Login;
using AgendaKether.Application.Auth.Commands.RegistrarUsuario;
using AgendaKether.Application.Contatos.Commands.AtualizarContato;
using AgendaKether.Application.Contatos.Commands.CriarContato;
using AgendaKether.Infrastructure;
using AgendaKether.Infrastructure.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ---------- JWT ----------
var secaoJwt = builder.Configuration.GetSection("Jwt");
var jwt = secaoJwt.Get<JwtSettings>()
    ?? throw new InvalidOperationException("Seção 'Jwt' ausente na configuração.");

var bytesChave = Encoding.UTF8.GetBytes(jwt.Key);

if (bytesChave.Length < JwtSettings.KeyTamanhoMinimoBytes)
{
    throw new InvalidOperationException(
        $"Jwt:Key precisa de no mínimo {JwtSettings.KeyTamanhoMinimoBytes} bytes. " +
        $"Atual: {bytesChave.Length}.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(bytesChave),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// ---------- Filtros ----------
builder.Services.AddScoped<ValidationFilter<CriarContatoCommand>>();
builder.Services.AddScoped<ValidationFilter<AtualizarContatoCommand>>();
builder.Services.AddScoped<ValidationFilter<RegistrarUsuarioCommand>>();
builder.Services.AddScoped<ValidationFilter<LoginCommand>>();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExcecaoMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
