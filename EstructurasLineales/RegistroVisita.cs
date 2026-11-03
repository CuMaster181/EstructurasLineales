namespace Unidad3;

/// <summary>Una visita al historial: direccion y hora.</summary>
public record RegistroVisita(string Url, DateTime Fecha)
{
    public override string ToString() => $"{Fecha:HH:mm:ss}  {Url}";
}