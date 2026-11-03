namespace Unidad3;

/// <summary>
/// LISTA DOBLEMENTE ENLAZADA generica con nodos dinamicos.
/// Cada nodo guarda referencias al anterior y al siguiente, por lo que se puede recorrer en ambos sentidos
/// y buscar un indice partiendo del extremo mas cercano (cabeza o cola).
/// </summary>
public class ListaDoble<T>
{
    private NodoDoble<T>? _cabeza;
    private NodoDoble<T>? _cola;

    public int Cantidad { get; private set; }
    public bool IsEmpty() => Cantidad == 0;

    /// <summary>Inserta al inicio. O(1).</summary>
    public void AgregarInicio(T valor)
    {
        var nuevo = new NodoDoble<T>(valor);
        if (_cabeza == null)
        {
            _cabeza = _cola = nuevo;
        }
        else
        {
            nuevo.Siguiente = _cabeza;
            _cabeza.Anterior = nuevo;
            _cabeza = nuevo;
        }
        Cantidad++;
    }

    /// <summary>Inserta al final. O(1) porque se conserva la referencia a la cola.</summary>
    public void AgregarFinal(T valor)
    {
        var nuevo = new NodoDoble<T>(valor);
        if (_cola == null)
        {
            _cabeza = _cola = nuevo;
        }
        else
        {
            nuevo.Anterior = _cola;
            _cola.Siguiente = nuevo;
            _cola = nuevo;
        }
        Cantidad++;
    }

    /// <summary>Inserta en una posicion (0..Cantidad). O(n).</summary>
    public void InsertarEn(int indice, T valor)
    {
        if (indice < 0 || indice > Cantidad)
            throw new ArgumentOutOfRangeException(nameof(indice), $"El indice debe estar entre 0 y {Cantidad}.");
        if (indice == 0) { AgregarInicio(valor); return; }
        if (indice == Cantidad) { AgregarFinal(valor); return; }

        NodoDoble<T> actual = NodoEn(indice);           // el nodo que quedara despues del nuevo
        var nuevo = new NodoDoble<T>(valor);
        nuevo.Anterior = actual.Anterior;
        nuevo.Siguiente = actual;
        actual.Anterior!.Siguiente = nuevo;
        actual.Anterior = nuevo;
        Cantidad++;
    }

    /// <summary>Devuelve el nodo en la posicion indicada, recorriendo desde el extremo mas cercano.</summary>
    private NodoDoble<T> NodoEn(int indice)
    {
        NodoDoble<T> actual;
        if (indice < Cantidad / 2)
        {
            actual = _cabeza!;
            for (int i = 0; i < indice; i++) actual = actual.Siguiente!;
        }
        else
        {
            actual = _cola!;
            for (int i = Cantidad - 1; i > indice; i--) actual = actual.Anterior!;
        }
        return actual;
    }

    public T Obtener(int indice)
    {
        ValidarIndice(indice);
        return NodoEn(indice).Dato;
    }

    /// <summary>Elimina el nodo de una posicion y devuelve su dato. O(n), O(1) en los extremos.</summary>
    public T EliminarEn(int indice)
    {
        ValidarIndice(indice);
        NodoDoble<T> nodo = NodoEn(indice);
        Desenlazar(nodo);
        return nodo.Dato;
    }

    public T EliminarInicio() => EliminarEn(0);
    public T EliminarFinal() => EliminarEn(Cantidad - 1);

    /// <summary>Elimina la primera aparicion de un valor. Devuelve false si no existe.</summary>
    public bool Eliminar(T valor)
    {
        int i = Buscar(valor);
        if (i < 0) return false;
        EliminarEn(i);
        return true;
    }

    /// <summary>Devuelve el indice de la primera aparicion del valor o -1. O(n).</summary>
    public int Buscar(T valor)
    {
        var comparador = EqualityComparer<T>.Default;
        int i = 0;
        for (var actual = _cabeza; actual != null; actual = actual.Siguiente, i++)
            if (comparador.Equals(actual.Dato, valor)) return i;
        return -1;
    }

    /// <summary>Invierte la lista intercambiando las referencias de cada nodo. O(n).</summary>
    public void Invertir()
    {
        var actual = _cabeza;
        while (actual != null)
        {
            var siguiente = actual.Siguiente;
            actual.Siguiente = actual.Anterior;
            actual.Anterior = siguiente;
            actual = siguiente;
        }
        (_cabeza, _cola) = (_cola, _cabeza);
    }

    /// <summary>Vacia la lista: los nodos quedan sin referencias y el recolector de basura los libera.</summary>
    public void Limpiar()
    {
        _cabeza = null;
        _cola = null;
        Cantidad = 0;
    }

    public IEnumerable<T> HaciaAdelante()
    {
        for (var a = _cabeza; a != null; a = a.Siguiente) yield return a.Dato;
    }

    public IEnumerable<T> HaciaAtras()
    {
        for (var a = _cola; a != null; a = a.Anterior) yield return a.Dato;
    }

    private void ValidarIndice(int indice)
    {
        if (indice < 0 || indice >= Cantidad)
            throw new ArgumentOutOfRangeException(nameof(indice),
                Cantidad == 0 ? "La lista esta vacia." : $"El indice debe estar entre 0 y {Cantidad - 1}.");
    }

    private void Desenlazar(NodoDoble<T> nodo)
    {
        if (nodo.Anterior != null) nodo.Anterior.Siguiente = nodo.Siguiente; else _cabeza = nodo.Siguiente;
        if (nodo.Siguiente != null) nodo.Siguiente.Anterior = nodo.Anterior; else _cola = nodo.Anterior;
        nodo.Anterior = null;
        nodo.Siguiente = null;
        Cantidad--;
    }
}