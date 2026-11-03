namespace Unidad3;

/// <summary>
/// TAREA 3.3 - Historial de navegacion web con PILAS.
///
/// Reglas (igual que un navegador real):
///   - Visitar(url): la pagina actual se apila en ATRAS, la nueva pasa a ser la actual
///                   y la pila ADELANTE se vacia (la rama "futura" ya no existe).
///   - Atras():      la pagina actual se apila en ADELANTE y se desapila ATRAS para ser la actual.
///   - Adelante():   la pagina actual se apila en ATRAS y se desapila ADELANTE para ser la actual.
///
/// Se usan DOS pilas porque "volver" es un comportamiento LIFO: la primera pagina a la que se regresa
/// es siempre la ultima que se visito. Ademas se guarda el historial completo (cronologico) en una
/// lista doblemente enlazada para poder recorrerlo del mas reciente al mas antiguo.
/// </summary>
public class HistorialNavegador
{
    private readonly PilaDinamica<string> _atras = new();
    private readonly PilaDinamica<string> _adelante = new();
    private readonly ListaDoble<RegistroVisita> _historial = new();

    public string? Actual { get; private set; }

    public bool PuedeIrAtras => !_atras.IsEmpty();
    public bool PuedeIrAdelante => !_adelante.IsEmpty();
    public int CantidadAtras => _atras.Cantidad;
    public int CantidadAdelante => _adelante.Cantidad;
    public int CantidadHistorial => _historial.Cantidad;

    public void Visitar(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Escribe una direccion (URL) para visitar.");

        if (Actual != null) _atras.Push(Actual);   // la pagina actual pasa a la pila ATRAS
        Actual = url.Trim();
        _adelante.Limpiar();                       // al navegar a algo nuevo se pierde el "adelante"
        _historial.AgregarFinal(new RegistroVisita(Actual, DateTime.Now));
    }

    public string Atras()
    {
        if (!PuedeIrAtras)
            throw new InvalidOperationException("No hay paginas anteriores (la pila ATRAS esta vacia).");
        _adelante.Push(Actual!);
        Actual = _atras.Pop();
        return Actual;
    }

    public string Adelante()
    {
        if (!PuedeIrAdelante)
            throw new InvalidOperationException("No hay paginas siguientes (la pila ADELANTE esta vacia).");
        _atras.Push(Actual!);
        Actual = _adelante.Pop();
        return Actual;
    }

    public void Reiniciar()
    {
        _atras.Limpiar();
        _adelante.Limpiar();
        _historial.Limpiar();
        Actual = null;
    }

    /// <summary>Contenido de la pila ATRAS, del tope (pagina a la que regresaria "Atras") hacia la base.</summary>
    public IEnumerable<string> PaginasAtras() => _atras.EnumerarDesdeTope();

    /// <summary>Contenido de la pila ADELANTE, del tope hacia la base.</summary>
    public IEnumerable<string> PaginasAdelante() => _adelante.EnumerarDesdeTope();

    /// <summary>Historial completo, de la visita mas reciente a la mas antigua (recorrido hacia atras de la lista doble).</summary>
    public IEnumerable<RegistroVisita> HistorialRecienteAPrimero() => _historial.HaciaAtras();
}