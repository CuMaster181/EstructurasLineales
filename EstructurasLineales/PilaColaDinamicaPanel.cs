namespace Unidad3;

/// <summary>Pestana de la Tarea 3.2: pila dinamica o cola dinamica con nodos enlazados.</summary>
public class PilaColaDinamicaPanel : UserControl
{
    private readonly bool _esPila;
    private readonly PilaDinamica<string> _pila = new();
    private readonly ColaDinamica<string> _cola = new();
    private readonly TextBox _txtValor = new() { Width = 100 };
    private readonly ListaVisual _visual = new() { Dock = DockStyle.Fill };
    private readonly Label _info = new() { AutoSize = true, Font = new Font("Consolas", 10), Padding = new Padding(10, 6, 0, 0) };
    private readonly TextBox _log = Utilidades.CajaTexto();
    private int _contador = 1;

    public PilaColaDinamicaPanel(bool esPila)
    {
        _esPila = esPila;
        Dock = DockStyle.Fill;

        string agregar = esPila ? "Push" : "Enqueue";
        string quitar = esPila ? "Pop" : "Dequeue";
        var b1 = Utilidades.Boton(agregar);
        var b2 = Utilidades.Boton(quitar);
        var b3 = Utilidades.Boton("Peek");
        var b4 = Utilidades.Boton("Limpiar estructura");

        var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 4, 6, 0) };
        fila.Controls.Add(Utilidades.Etiqueta("Valor:"));
        fila.Controls.Add(_txtValor);
        fila.Controls.Add(b1);
        fila.Controls.Add(b2);
        fila.Controls.Add(b3);
        fila.Controls.Add(b4);

        if (esPila)
        {
            _visual.Titulo = "PILA DINAMICA (LIFO): solo existe el puntero 'tope'; cada Push crea un nodo con new";
            _visual.EtiquetaInicio = "tope";
        }
        else
        {
            _visual.Titulo = "COLA DINAMICA (FIFO): punteros 'frente' (sale) y 'final' (entra); sin limite de capacidad";
            _visual.EtiquetaInicio = "frente";
            _visual.EtiquetaFin = "final";
        }

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        tabla.Controls.Add(fila, 0, 0);
        tabla.Controls.Add(_info, 0, 1);
        tabla.Controls.Add(_visual, 0, 2);
        tabla.Controls.Add(_log, 0, 3);
        Controls.Add(tabla);

        b1.Click += (_, _) => Insertar();
        b2.Click += (_, _) => Quitar();
        b3.Click += (_, _) => Ver();
        b4.Click += (_, _) => { _pila.Limpiar(); _cola.Limpiar(); Escribir("Limpiar()"); Refrescar(); };
        _txtValor.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Insertar(); }
        };

        Refrescar();
    }

    private void Escribir(string t) => _log.AppendText($"[{Utilidades.Hora()}] {t}{Environment.NewLine}");

    private void Insertar()
    {
        string v = _txtValor.Text.Trim();
        if (v == "") v = "N" + _contador;
        _contador++;
        _txtValor.Clear();
        if (_esPila) { _pila.Push(v); Escribir($"Push(\"{v}\")  -> nuevo nodo; tope = \"{v}\""); }
        else { _cola.Enqueue(v); Escribir($"Enqueue(\"{v}\")  -> nuevo nodo al final"); }
        Refrescar();
        _txtValor.Focus();
    }

    private void Quitar()
    {
        try
        {
            if (_esPila) Escribir($"Pop()  -> \"{_pila.Pop()}\"");
            else Escribir($"Dequeue()  -> \"{_cola.Dequeue()}\"");
        }
        catch (InvalidOperationException ex) { Escribir("ERROR: " + ex.Message); }
        Refrescar();
    }

    private void Ver()
    {
        try
        {
            if (_esPila) Escribir($"Peek()  -> \"{_pila.Peek()}\"");
            else Escribir($"Peek()  -> \"{_cola.Peek()}\"");
        }
        catch (InvalidOperationException ex) { Escribir("ERROR: " + ex.Message); }
    }

    private void Refrescar()
    {
        _visual.Items = _esPila ? _pila.EnumerarDesdeTope().ToList() : _cola.EnumerarDesdeFrente().ToList();
        _visual.Invalidate();
        int n = _esPila ? _pila.Cantidad : _cola.Cantidad;
        _info.Text = $"cantidad = {n} | isEmpty = {(_esPila ? _pila.IsEmpty() : _cola.IsEmpty())} | sin limite de capacidad (memoria dinamica)";
    }
}