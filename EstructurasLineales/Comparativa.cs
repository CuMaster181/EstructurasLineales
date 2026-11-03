using System.Diagnostics;

namespace Unidad3;

public record FilaTiempo(string Estructura, double Milisegundos, long Bytes);
public record FilaOcupacion(int Porcentaje, int Elementos, long BytesEstatica, long BytesDinamica);

/// <summary>
/// Mediciones para el documento comparativo (Tarea 3.2): tiempo y memoria de las
/// estructuras estaticas, las dinamicas propias y las clases predefinidas de .NET.
/// </summary>
public static class Comparativa
{
    private static (double ms, long bytes) Medir(Func<object> crear)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long antes = GC.GetTotalMemory(true);

        var sw = Stopwatch.StartNew();
        object o = crear();
        sw.Stop();

        long despues = GC.GetTotalMemory(true);   // 'o' sigue alcanzable hasta KeepAlive
        GC.KeepAlive(o);
        return (sw.Elapsed.TotalMilliseconds, despues - antes);
    }

    /// <summary>Inserta n enteros en cada estructura y mide tiempo y memoria ocupada.</summary>
    public static List<FilaTiempo> MedirInsercion(int n)
    {
        var filas = new List<FilaTiempo>();

        void Agregar(string nombre, Func<object> crear)
        {
            var r = Medir(crear);
            filas.Add(new FilaTiempo(nombre, r.ms, r.bytes));
        }

        Agregar("PilaEstatica<int> (arreglo)", () => { var p = new PilaEstatica<int>(n); for (int i = 0; i < n; i++) p.Push(i); return p; });
        Agregar("PilaDinamica<int> (nodos)", () => { var p = new PilaDinamica<int>(); for (int i = 0; i < n; i++) p.Push(i); return p; });
        Agregar("Stack<int> de .NET", () => { var p = new Stack<int>(); for (int i = 0; i < n; i++) p.Push(i); return p; });
        Agregar("ColaCircular<int> (arreglo)", () => { var c = new ColaCircular<int>(n); for (int i = 0; i < n; i++) c.Enqueue(i); return c; });
        Agregar("ColaDinamica<int> (nodos)", () => { var c = new ColaDinamica<int>(); for (int i = 0; i < n; i++) c.Enqueue(i); return c; });
        Agregar("Queue<int> de .NET", () => { var c = new Queue<int>(); for (int i = 0; i < n; i++) c.Enqueue(i); return c; });
        Agregar("ListaDoble<int> (nodos dobles)", () => { var l = new ListaDoble<int>(); for (int i = 0; i < n; i++) l.AgregarFinal(i); return l; });
        Agregar("LinkedList<int> de .NET", () => { var l = new LinkedList<int>(); for (int i = 0; i < n; i++) l.AddLast(i); return l; });
        return filas;
    }

    /// <summary>
    /// Memoria de una pila estatica (capacidad fija, siempre reservada completa) frente a una dinamica,
    /// segun el porcentaje de la capacidad que realmente se usa.
    /// </summary>
    public static List<FilaOcupacion> MedirOcupacion(int capacidad)
    {
        var filas = new List<FilaOcupacion>();
        foreach (int pct in new[] { 1, 5, 10, 25, 50, 100 })
        {
            int n = capacidad * pct / 100;
            var est = Medir(() => { var p = new PilaEstatica<int>(capacidad); for (int i = 0; i < n; i++) p.Push(i); return p; });
            var din = Medir(() => { var p = new PilaDinamica<int>(); for (int i = 0; i < n; i++) p.Push(i); return p; });
            filas.Add(new FilaOcupacion(pct, n, est.bytes, din.bytes));
        }
        return filas;
    }
}