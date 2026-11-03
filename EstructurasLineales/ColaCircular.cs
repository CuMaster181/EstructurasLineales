namespace Unidad3;

/// <summary>
/// COLA CIRCULAR con arreglo estatico.
///
/// Se conserva el contador (count) y la formula circular para calcular rear:
///     rear = (front + count) % capacidad
/// pero, como en la cola simple, cada Dequeue RECORRE los datos restantes una posicion a la izquierda
/// (hacia el indice 0), de modo que front SIEMPRE es la posicion [0].
///
/// Consecuencia: como front nunca se desplaza, rear nunca da la vuelta al arreglo y el modulo no llega a
/// "envolver" (el comportamiento resulta equivalente al de la cola simple; Dequeue cuesta O(n)).
/// </summary>
public class ColaCircular<T>
{
    private readonly T[] _datos;
    private int _front = 0;    // siempre 0: los datos se recorren a la izquierda en cada Dequeue
    private int _count = 0;    // cantidad de elementos almacenados

    public int Capacidad => _datos.Length;
    public int Front => _front;
    public int Cantidad => _count;

    /// <summary>Posicion del ultimo elemento insertado (-1 si la cola esta vacia).</summary>
    public int Rear => _count == 0 ? -1 : (_front + _count - 1) % _datos.Length;

    public ColaCircular(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentException("La capacidad debe ser >= 1.");
        _datos = new T[capacidad];
    }

    public bool IsEmpty() => _count == 0;
    public bool IsFull() => _count == _datos.Length;

    /// <summary>Inserta al final: rear = (front + count) % capacidad. O(1).</summary>
    public void Enqueue(T valor)
    {
        if (IsFull())
            throw new InvalidOperationException("Desbordamiento (overflow): la cola circular esta llena.");
        int posicion = (_front + _count) % _datos.Length;
        _datos[posicion] = valor;
        _count++;
    }

    /// <summary>
    /// Elimina el elemento del frente (posicion 0) y recorre los demas datos
    /// una posicion hacia la izquierda. O(n).
    /// </summary>
    public T Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Subdesbordamiento (underflow): la cola circular esta vacia.");

        T valor = _datos[_front];
        for (int i = 0; i < _count - 1; i++)
            _datos[(_front + i) % _datos.Length] = _datos[(_front + i + 1) % _datos.Length];   // recorrer a la izquierda
        _datos[(_front + _count - 1) % _datos.Length] = default!;                              // ultima celda libre
        _count--;
        return valor;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("La cola esta vacia: no hay elemento al frente.");
        return _datos[_front];
    }

    public T[] CopiaArreglo() => (T[])_datos.Clone();
}