namespace Unidad3;

/// <summary>
/// PILA (Stack) implementada con un arreglo estatico. Politica LIFO:
/// el ultimo elemento en entrar es el primero en salir.
///
/// Puntero de indice:  top  -> posicion del ultimo elemento insertado (-1 si esta vacia).
/// No se usan colecciones nativas de .NET: solo un arreglo de tamano fijo.
/// </summary>
public class PilaEstatica<T>
{
    private readonly T[] _datos;   // memoria estatica: el tamano se fija al crear la pila
    private int _top = -1;         // indice del tope

    public int Capacidad => _datos.Length;
    public int Cantidad => _top + 1;
    public int Top => _top;

    public PilaEstatica(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentException("La capacidad debe ser >= 1.");
        _datos = new T[capacidad];
    }

    /// <summary>La pila esta vacia cuando top vale -1.</summary>
    public bool IsEmpty() => _top == -1;

    /// <summary>La pila esta llena cuando top apunta a la ultima posicion del arreglo.</summary>
    public bool IsFull() => _top == _datos.Length - 1;

    /// <summary>Inserta un elemento en el tope. O(1).</summary>
    public void Push(T valor)
    {
        if (IsFull())
            throw new InvalidOperationException("Desbordamiento (overflow): la pila esta llena.");
        _top++;
        _datos[_top] = valor;
    }

    /// <summary>Elimina y devuelve el elemento del tope. O(1).</summary>
    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Subdesbordamiento (underflow): la pila esta vacia.");
        T valor = _datos[_top];
        _datos[_top] = default!;   // se limpia la celda para no conservar referencias
        _top--;
        return valor;
    }

    /// <summary>Devuelve el elemento del tope sin eliminarlo. O(1).</summary>
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("La pila esta vacia: no hay elemento en el tope.");
        return _datos[_top];
    }

    /// <summary>Copia del arreglo interno (solo para dibujarlo en la interfaz).</summary>
    public T[] CopiaArreglo() => (T[])_datos.Clone();
}