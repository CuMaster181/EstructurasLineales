namespace Unidad3;

/// <summary>Pestana de la Tarea 3.2: clases genericas predefinidas de .NET (Stack, Queue, LinkedList, List).</summary>
public class ClasesNetPanel : UserControl
{
    private readonly Stack<string> _stack = new();
    private readonly Queue<string> _queue = new();
    private readonly LinkedList<string> _linked = new();
    private readonly List<string> _list = new();

    private readonly TextBox _txtValor = new() { Width = 100 };
    private readonly TextBox _verStack = Utilidades.CajaTexto();
    private readonly TextBox _verQueue = Utilidades.CajaTexto();
    private readonly TextBox _verLinked = Utilidades.CajaTexto();
    private readonly TextBox _verList = Utilidades.CajaTexto();
    private readonly TextBox _log = Utilidades.CajaTexto();
    private int _contador = 1;

    public ClasesNetPanel()
    {
        Dock = DockStyle.Fill;

        var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 4, 6, 0), WrapContents = true };
        fila.Controls.Add(Utilidades.Etiqueta("Valor:"));
        fila.Controls.Add(_txtValor);

        void Boton(string texto, Action accion)
        {
            var b = Utilidades.Boton(texto);
            b.Click += (_, _) =>
            {
                try { accion(); }
                catch (InvalidOperationException ex) { Escribir("ERROR: " + ex.Message); }
                catch (ArgumentOutOfRangeException ex) { Escribir("ERROR: " + ex.Message); }
                Refrescar();
            };
            fila.Controls.Add(b);
        }

        Boton("Stack.Push", () => { string v = Valor(); _stack.Push(v); Escribir($"Stack.Push(\"{v}\")"); });
        Boton("Stack.Pop", () => Escribir($"Stack.Pop() -> \"{_stack.Pop()}\""));
        Boton("Queue.Enqueue", () => { string v = Valor(); _queue.Enqueue(v); Escribir($"Queue.Enqueue(\"{v}\")"); });
        Boton("Queue.Dequeue", () => Escribir($"Queue.Dequeue() -> \"{_queue.Dequeue()}\""));
        Boton("LinkedList.AddFirst", () => { string v = Valor(); _linked.AddFirst(v); Escribir($"LinkedList.AddFirst(\"{v}\")"); });
        Boton("LinkedList.AddLast", () => { string v = Valor(); _linked.AddLast(v); Escribir($"LinkedList.AddLast(\"{v}\")"); });
        Boton("LinkedList.RemoveFirst", () => { _linked.RemoveFirst(); Escribir("LinkedList.RemoveFirst()"); });
        Boton("List.Add", () => { string v = Valor(); _list.Add(v); Escribir($"List.Add(\"{v}\")"); });
        Boton("List.Insert(0, ...)", () => { string v = Valor(); _list.Insert(0, v); Escribir($"List.Insert(0, \"{v}\")  (desplaza todos los elementos)"); });
        Boton("List.RemoveAt(0)", () => { _list.RemoveAt(0); Escribir("List.RemoveAt(0)  (desplaza todos los elementos)"); });

        var rejilla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4 };
        rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        rejilla.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        rejilla.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        rejilla.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        rejilla.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        rejilla.Controls.Add(Titulo("Stack<T>  -  arreglo interno que se duplica (LIFO)"), 0, 0);
        rejilla.Controls.Add(Titulo("Queue<T>  -  arreglo circular interno que se duplica (FIFO)"), 1, 0);
        rejilla.Controls.Add(_verStack, 0, 1);
        rejilla.Controls.Add(_verQueue, 1, 1);
        rejilla.Controls.Add(Titulo("LinkedList<T>  -  lista doblemente enlazada de nodos"), 0, 2);
        rejilla.Controls.Add(Titulo("List<T>  -  arreglo dinamico (crece x2, acceso por indice O(1))"), 1, 2);
        rejilla.Controls.Add(_verLinked, 0, 3);
        rejilla.Controls.Add(_verList, 1, 3);

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
        tabla.Controls.Add(fila, 0, 0);
        tabla.Controls.Add(rejilla, 0, 1);
        tabla.Controls.Add(_log, 0, 2);
        Controls.Add(tabla);

        Refrescar();
    }

    private static Label Titulo(string texto)
    {
        return new Label { Text = texto, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.BottomLeft };
    }

    private string Valor()
    {
        string v = _txtValor.Text.Trim();
        if (v == "") v = "N" + _contador;
        _contador++;
        _txtValor.Clear();
        return v;
    }

    private void Escribir(string t) => _log.AppendText($"[{Utilidades.Hora()}] {t}{Environment.NewLine}");

    private void Refrescar()
    {
        _verStack.Text = _stack.Count == 0 ? "(vacia)" : "tope -> " + string.Join(", ", _stack);
        _verQueue.Text = _queue.Count == 0 ? "(vacia)" : "frente -> " + string.Join(", ", _queue);
        _verLinked.Text = _linked.Count == 0 ? "(vacia)" : string.Join(" <-> ", _linked);
        _verList.Text = _list.Count == 0 ? "(vacia)" : string.Join(", ", _list.Select((v, i) => $"[{i}]={v}"));
    }
}