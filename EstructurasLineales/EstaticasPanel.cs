namespace Unidad3;

/// <summary>Pestana de la Tarea 3.1: Pila, Cola simple y Cola circular con arreglos estaticos.</summary>
public class EstaticasPanel : UserControl
{
    private readonly ComboBox _combo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
    private readonly NumericUpDown _numCap = new() { Minimum = 2, Maximum = 12, Value = 6, Width = 55 };
    private readonly Button _btnCrear = Utilidades.Boton("Crear estructura");
    private readonly TextBox _txtValor = new() { Width = 100 };
    private readonly Button _btnInsertar = Utilidades.Boton("Push");
    private readonly Button _btnQuitar = Utilidades.Boton("Pop");
    private readonly Button _btnVer = Utilidades.Boton("Peek");
    private readonly Button _btnEstado = Utilidades.Boton("isEmpty / isFull");
    private readonly Button _btnLimpiarLog = Utilidades.Boton("Limpiar bitacora");
    private readonly Label _info = new() { AutoSize = true, Font = new Font("Consolas", 10), Padding = new Padding(10, 6, 0, 0) };
    private readonly ArregloVisual _visual = new() { Dock = DockStyle.Fill };
    private readonly TextBox _log = Utilidades.CajaTexto();

    private PilaEstatica<string>? _pila;
    private ColaSimple<string>? _colaSimple;
    private ColaCircular<string>? _colaCircular;
    private int _contador = 1;

    public EstaticasPanel()
    {
        Dock = DockStyle.Fill;
        _combo.Items.AddRange(new object[] { "Pila (Stack)", "Cola simple", "Cola circular" });
        _combo.SelectedIndex = 0;

        var fila1 = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 4, 6, 0) };
        fila1.Controls.Add(Utilidades.Etiqueta("Estructura:"));
        fila1.Controls.Add(_combo);
        fila1.Controls.Add(Utilidades.Etiqueta("Capacidad:"));
        fila1.Controls.Add(_numCap);
        fila1.Controls.Add(_btnCrear);

        var fila2 = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 0, 6, 0) };
        fila2.Controls.Add(Utilidades.Etiqueta("Valor:"));
        fila2.Controls.Add(_txtValor);
        fila2.Controls.Add(_btnInsertar);
        fila2.Controls.Add(_btnQuitar);
        fila2.Controls.Add(_btnVer);
        fila2.Controls.Add(_btnEstado);
        fila2.Controls.Add(_btnLimpiarLog);

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.Controls.Add(fila1, 0, 0);
        tabla.Controls.Add(fila2, 0, 1);
        tabla.Controls.Add(_info, 0, 2);
        tabla.Controls.Add(_visual, 0, 3);
        tabla.Controls.Add(_log, 0, 4);
        Controls.Add(tabla);

        _combo.SelectedIndexChanged += (_, _) => Crear();
        _btnCrear.Click += (_, _) => Crear();
        _btnInsertar.Click += (_, _) => Insertar();
        _btnQuitar.Click += (_, _) => Quitar();
        _btnVer.Click += (_, _) => Ver();
        _btnEstado.Click += (_, _) => Consultar();
        _btnLimpiarLog.Click += (_, _) => _log.Clear();
        _txtValor.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Insertar(); }
        };

        Crear();
    }

    private int Modo => _combo.SelectedIndex;

    private void Crear()
    {
        int cap = (int)_numCap.Value;
        _pila = null; _colaSimple = null; _colaCircular = null;
        _contador = 1;
        switch (Modo)
        {
            case 0:
                _pila = new PilaEstatica<string>(cap);
                _btnInsertar.Text = "Push"; _btnQuitar.Text = "Pop"; _btnVer.Text = "Peek";
                _visual.Titulo = "PILA estatica (LIFO)  -  indice 0 = base, 'top' = ultimo insertado";
                break;
            case 1:
                _colaSimple = new ColaSimple<string>(cap);
                _btnInsertar.Text = "Enqueue"; _btnQuitar.Text = "Dequeue"; _btnVer.Text = "Peek";
                _visual.Titulo = "COLA SIMPLE estatica (FIFO)  -  front siempre es el indice 0; al hacer Dequeue los datos se recorren a la izquierda";
                break;
            default:
                _colaCircular = new ColaCircular<string>(cap);
                _btnInsertar.Text = "Enqueue"; _btnQuitar.Text = "Dequeue"; _btnVer.Text = "Peek";
                _visual.Titulo = "COLA CIRCULAR estatica (FIFO)  -  rear = (front + count) % capacidad; los datos se recorren a la izquierda";
                break;
        }
        _log.Clear();
        Escribir($"Se creo {_combo.Text} con capacidad {cap}.");
        Refrescar();
    }

    private void Escribir(string texto) => _log.AppendText($"[{Utilidades.Hora()}] {texto}{Environment.NewLine}");

    private void Insertar()
    {
        string valor = _txtValor.Text.Trim();
        if (valor == "") valor = "V" + _contador;

        try
        {
            switch (Modo)
            {
                case 0: _pila!.Push(valor); Escribir($"Push(\"{valor}\")  -> top = {_pila.Top}"); break;
                case 1: _colaSimple!.Enqueue(valor); Escribir($"Enqueue(\"{valor}\")  -> rear = {_colaSimple.Rear}"); break;
                default: _colaCircular!.Enqueue(valor); Escribir($"Enqueue(\"{valor}\")  -> rear = {_colaCircular.Rear}"); break;
            }
            _contador++;
            _txtValor.Clear();
        }
        catch (InvalidOperationException ex)
        {
            Escribir("ERROR: " + ex.Message);
        }
        Refrescar();
        _txtValor.Focus();
    }

    private void Quitar()
    {
        try
        {
            switch (Modo)
            {
                case 0: { string v = _pila!.Pop(); Escribir($"Pop()  -> \"{v}\"   (top = {_pila.Top})"); break; }
                case 1: { string v = _colaSimple!.Dequeue(); Escribir($"Dequeue()  -> \"{v}\"   (los datos se recorrieron a la izquierda; rear = {_colaSimple.Rear})"); break; }
                default: { string v = _colaCircular!.Dequeue(); Escribir($"Dequeue()  -> \"{v}\"   (los datos se recorrieron a la izquierda; front = {_colaCircular.Front})"); break; }
            }
        }
        catch (InvalidOperationException ex)
        {
            Escribir("ERROR: " + ex.Message);
        }
        Refrescar();
    }

    private void Ver()
    {
        try
        {
            string v = Modo switch
            {
                0 => _pila!.Peek(),
                1 => _colaSimple!.Peek(),
                _ => _colaCircular!.Peek()
            };
            Escribir($"Peek()  -> \"{v}\"   (no se elimina)");
        }
        catch (InvalidOperationException ex)
        {
            Escribir("ERROR: " + ex.Message);
        }
    }

    private void Consultar()
    {
        switch (Modo)
        {
            case 0: Escribir($"isEmpty() = {_pila!.IsEmpty()} | isFull() = {_pila.IsFull()}"); break;
            case 1: Escribir($"isEmpty() = {_colaSimple!.IsEmpty()} | isFull() = {_colaSimple.IsFull()}"); break;
            default: Escribir($"isEmpty() = {_colaCircular!.IsEmpty()} | isFull() = {_colaCircular.IsFull()}"); break;
        }
    }

    private static void Marcar(Dictionary<int, string> marcas, int indice, int longitud, string texto)
    {
        if (indice < 0 || indice >= longitud) return;
        marcas[indice] = marcas.TryGetValue(indice, out string? previo) ? previo + "/" + texto : texto;
    }

    private void Refrescar()
    {
        var marcas = new Dictionary<int, string>();
        string?[] celdas;
        string estado;

        switch (Modo)
        {
            case 0:
                {
                    var p = _pila!;
                    celdas = p.CopiaArreglo();
                    if (!p.IsEmpty()) Marcar(marcas, p.Top, celdas.Length, "top");
                    estado = $"top = {p.Top} | cantidad = {p.Cantidad}/{p.Capacidad} | isEmpty = {p.IsEmpty()} | isFull = {p.IsFull()}";
                    break;
                }
            case 1:
                {
                    var c = _colaSimple!;
                    celdas = c.CopiaArreglo();
                    if (!c.IsEmpty())
                    {
                        Marcar(marcas, c.Front, celdas.Length, "front");
                        Marcar(marcas, c.Rear, celdas.Length, "rear");
                    }
                    estado = $"front = {c.Front} (fijo) | rear = {c.Rear} | cantidad = {c.Cantidad}/{c.Capacidad} | " +
                             $"isEmpty = {c.IsEmpty()} | isFull = {c.IsFull()}";
                    break;
                }
            default:
                {
                    var c = _colaCircular!;
                    celdas = c.CopiaArreglo();
                    if (!c.IsEmpty())
                    {
                        Marcar(marcas, c.Front, celdas.Length, "front");
                        Marcar(marcas, c.Rear, celdas.Length, "rear");
                    }
                    estado = $"front = {c.Front} | rear = {c.Rear} | cantidad = {c.Cantidad}/{c.Capacidad} | " +
                             $"isEmpty = {c.IsEmpty()} | isFull = {c.IsFull()}";
                    break;
                }
        }

        _visual.Celdas = celdas;
        _visual.Marcadores = marcas;
        _visual.Invalidate();
        _info.Text = estado;
    }
}