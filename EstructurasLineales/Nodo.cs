namespace Unidad3;

public class Nodo<T>
{
    public T Dato;
    public Nodo<T>? Siguiente;

    public Nodo(T dato)
    {
        Dato = dato;
    }
}

public class NodoDoble<T>
{
    public T Dato;
    public NodoDoble<T>? Anterior;
    public NodoDoble<T>? Siguiente;

    public NodoDoble(T dato)
    {
        Dato = dato;
    }
}