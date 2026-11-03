namespace Unidad3;

/// <summary>
/// PILA DINAMICA (LIFO) con nodos enlazados. No tiene capacidad maxima: cada Push crea un nodo con 'new'
/// y cada Pop libera uno. El unico puntero necesario es 'tope', que apunta al ultimo nodo insertado.
/// </summary>
public class PilaDinamica<T>
{
    private Nodo<T>? _tope;

    public int Cantidad { get; private set; }

    public bool IsEmpty() => _tope == null;

    /// <summary>Crea un nodo que apunta al tope actual y lo convierte en el nuevo tope. O(1).</summary>
    public void Push(T valor)
    {
        var nuevo = new Nodo<T>(valor);
        nuevo.Siguiente = _tope;
        _tope = nuevo;
        Cantidad++;
    }

    public T Pop()
    {
        if (_tope == null)
            throw new InvalidOperationException("Subdesbordamiento (underflow): la pila esta vacia.");
        T valor = _tope.Dato;
        _tope = _tope.Siguiente;     // el nodo anterior queda sin referencias
        Cantidad--;
        return valor;
    }

    public T Peek()
    {
        if (_tope == null)
            throw new InvalidOperationException("La pila esta vacia: no hay elemento en el tope.");
        return _tope.Dato;
    }

    public void Limpiar()
    {
        _tope = null;
        Cantidad = 0;
    }

    /// <summary>Recorre los elementos del tope hacia la base.</summary>
    public IEnumerable<T> EnumerarDesdeTope()
    {
        for (var a = _tope; a != null; a = a.Siguiente) yield return a.Dato;
    }
}