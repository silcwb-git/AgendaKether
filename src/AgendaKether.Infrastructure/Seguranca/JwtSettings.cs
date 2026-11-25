namespace AgendaKether.Infrastructure.Seguranca;

public class JwtSettings
{
    public const int KeyTamanhoMinimoBytes = 32;

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "AgendaKether";
    public string Audience { get; set; } = "AgendaKether";
    public int ExpiraEmMinutos { get; set; } = 60;
}