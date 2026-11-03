namespace Unidad3;

/// <summary>
/// COLA SIMPLE (lineal) con arreglo estatico. Politica FIFO: el primero en entrar es el primero en salir.
///
/// Punteros de indice:  front -> SIEMPRE es la posicion [0] (el siguiente en salir)
///                      rear  -> posicion del ultimo elemento insertado (-1 si la cola esta vacia)
///
/// Enqueue inserta en rear + 1.
/// Dequeue saca el elemento de la posicion [0] y RECORRE todos los demas datos una posicion a la
/// izquierda (hacia el indice 0), de modo que el siguiente elemento pasa a ocupar el frente.
/// Por eso Dequeue cuesta O(n); la cola circular evita ese desplazamiento con aritmetica modular.
/// </summary>
public class ColaSimple<T>
{
    private readonly T[] _datos;
    private int _rear = -1;

    public int Capacidad => _datos.Length;

    /// <summary>El frente siempre esta en la posicion 0 (solo tiene sentido si la cola no esta vacia).</summary>
    public int Front => 0;
    public int Rear => _rear;
    public int Cantidad => _rear + 1;

    public ColaSimple(int capacidad)
    {
        if (capacidad < 1) throw new ArgumentException("La capacidad debe ser >= 1.");
        _datos = new T[capacidad];
    }

    /// <summary>Vacia cuando rear vale -1.</summary>
    public bool IsEmpty() => _rear == -1;

    /// <summary>Llena cuando rear esta en la ultima posicion del arreglo.</summary>
    public bool IsFull() => _rear == _datos.Length - 1;

    /// <summary>Inserta al final de la cola. O(1).</summary>
    public void Enqueue(T valor)
    {
        if (IsFull())
            throw new InvalidOperationException("Desbordamiento (overflow): la cola esta llena.");
        _rear++;
        _datos[_rear] = valor;
    }

    /// <summary>
    /// Elimina y devuelve el elemento del frente (posicion 0) y recorre los demas datos
    /// una posicion hacia la izquierda. O(n).
    /// </summary>
    public T Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Subdesbordamiento (underflow): la cola esta vacia.");

        T valor = _datos[0];
        for (int i = 0; i < _rear; i++)
            _datos[i] = _datos[i + 1];     // cada dato se recorre hacia la izquierda
        _datos[_rear] = default!;          // la ultima celda queda libre
        _rear--;
        return valor;
    }

    /// <summary>Devuelve el elemento del frente (posicion 0) sin eliminarlo. O(1).</summary>
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("La cola esta vacia: no hay elemento al frente.");
        return _datos[0];
    }

    public T[] CopiaArreglo() => (T[])_datos.Clone();
}