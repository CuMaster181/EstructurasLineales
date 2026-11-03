namespace Unidad3;

/// <summary>Pestana de la Tarea 3.2: lista doblemente enlazada con nodos dinamicos.</summary>
public class ListaDoblePanel : UserControl
{
    private readonly ListaDoble<string> _lista = new();
    private readonly TextBox _txtValor = new() { Width = 100 };
    private readonly NumericUpDown _numIndice = new() { Minimum = 0, Maximum = 999, Width = 55 };
    private readonly ListaVisual _visual = new()
    {
        Dock = DockStyle.Fill,
        Doble = true,
        EtiquetaInicio = "cabeza",
        EtiquetaFin = "cola",
        Titulo = "LISTA DOBLEMENTE ENLAZADA (cada nodo guarda anterior y siguiente)"
    };
    private readonly TextBox _adelante = Utilidades.CajaTexto();
    private readonly TextBox _atras = Utilidades.CajaTexto();
    private readonly TextBox _log = Utilidades.CajaTexto();
    private int _contador = 1;

    public ListaDoblePanel()
    {
        Dock = DockStyle.Fill;

        var b1 = Utilidades.Boton("Agregar inicio");
        var b2 = Utilidades.Boton("Agregar final");
        var b3 = Utilidades.Boton("Insertar en indice");
        var b4 = Utilidades.Boton("Eliminar en indice");
        var b5 = Utilidades.Boton("Eliminar valor");
        var b6 = Utilidades.Boton("Buscar valor");
        var b7 = Utilidades.Boton("Invertir");
        var b8 = Utilidades.Boton("Limpiar");

        var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 4, 6, 0), WrapContents = true };
        fila.Controls.Add(Utilidades.Etiqueta("Valor:"));
        fila.Controls.Add(_txtValor);
        fila.Controls.Add(Utilidades.Etiqueta("Indice:"));
        fila.Controls.Add(_numIndice);
        foreach (var b in new[] { b1, b2, b3, b4, b5, b6, b7, b8 }) fila.Controls.Add(b);

        var recorridos = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
        recorridos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        recorridos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
        recorridos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
        recorridos.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
        recorridos.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        recorridos.Controls.Add(new Label { Text = "Recorrido hacia adelante (cabeza -> cola)", Dock = DockStyle.Fill }, 0, 0);
        recorridos.Controls.Add(new Label { Text = "Recorrido hacia atras (cola -> cabeza)", Dock = DockStyle.Fill }, 1, 0);
        recorridos.Controls.Add(new Label { Text = "Bitacora", Dock = DockStyle.Fill }, 2, 0);
        recorridos.Controls.Add(_adelante, 0, 1);
        recorridos.Controls.Add(_atras, 1, 1);
        recorridos.Controls.Add(_log, 2, 1);

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        tabla.Controls.Add(fila, 0, 0);
        tabla.Controls.Add(_visual, 0, 1);
        tabla.Controls.Add(recorridos, 0, 2);
        Controls.Add(tabla);

        b1.Click += (_, _) => Accion(() => { string v = Valor(); _lista.AgregarInicio(v); Escribir($"AgregarInicio(\"{v}\")"); });
        b2.Click += (_, _) => Accion(() => { string v = Valor(); _lista.AgregarFinal(v); Escribir($"AgregarFinal(\"{v}\")"); });
        b3.Click += (_, _) => Accion(() => { string v = Valor(); int i = (int)_numIndice.Value; _lista.InsertarEn(i, v); Escribir($"InsertarEn({i}, \"{v}\")"); });
        b4.Click += (_, _) => Accion(() => { int i = (int)_numIndice.Value; string v = _lista.EliminarEn(i); Escribir($"EliminarEn({i}) -> \"{v}\""); });
        b5.Click += (_, _) => Accion(() => { string v = _txtValor.Text.Trim(); Escribir($"Eliminar(\"{v}\") -> {(_lista.Eliminar(v) ? "eliminado" : "no encontrado")}"); });
        b6.Click += (_, _) => Accion(() => { string v = _txtValor.Text.Trim(); int i = _lista.Buscar(v); Escribir($"Buscar(\"{v}\") -> {(i >= 0 ? "indice " + i : "no encontrado")}"); });
        b7.Click += (_, _) => Accion(() => { _lista.Invertir(); Escribir("Invertir()"); });
        b8.Click += (_, _) => Accion(() => { _lista.Limpiar(); Escribir("Limpiar()"); });

        Refrescar();
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

    private void Accion(Action a)
    {
        try { a(); }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            Escribir("ERROR: " + ex.Message);
        }
        Refrescar();
    }

    private void Refrescar()
    {
        _visual.Items = _lista.HaciaAdelante().ToList();
        _visual.Invalidate();
        _adelante.Text = _lista.Cantidad == 0 ? "(vacia)" : string.Join(" <-> ", _lista.HaciaAdelante());
        _atras.Text = _lista.Cantidad == 0 ? "(vacia)" : string.Join(" <-> ", _lista.HaciaAtras());
    }
}