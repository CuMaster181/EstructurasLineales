namespace Unidad3;

/// <summary>
/// COLA DINAMICA (FIFO) con nodos enlazados. Mantiene dos punteros: 'frente' (de donde se extrae)
/// y 'final' (donde se inserta), de modo que ambas operaciones son O(1) y no hay limite de capacidad.
/// </summary>
public class ColaDinamica<T>
{
    private Nodo<T>? _frente;
    private Nodo<T>? _final;

    public int Cantidad { get; private set; }

    public bool IsEmpty() => _frente == null;

    public void Enqueue(T valor)
    {
        var nuevo = new Nodo<T>(valor);
        if (_final == null)
            _frente = _final = nuevo;
        else
        {
            _final.Siguiente = nuevo;
            _final = nuevo;
        }
        Cantidad++;
    }

    public T Dequeue()
    {
        if (_frente == null)
            throw new InvalidOperationException("Subdesbordamiento (underflow): la cola esta vacia.");
        T valor = _frente.Dato;
        _frente = _frente.Siguiente;
        if (_frente == null) _final = null;
        Cantidad--;
        return valor;
    }

    public T Peek()
    {
        if (_frente == null)
            throw new InvalidOperationException("La cola esta vacia: no hay elemento al frente.");
        return _frente.Dato;
    }

    public void Limpiar()
    {
        _frente = null;
        _final = null;
        Cantidad = 0;
    }

    /// <summary>Recorre los elementos del frente hacia el final.</summary>
    public IEnumerable<T> EnumerarDesdeFrente()
    {
        for (var a = _frente; a != null; a = a.Siguiente) yield return a.Dato;
    }
}