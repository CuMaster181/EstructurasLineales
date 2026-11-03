namespace Unidad3;

/// <summary>Pestana de la Tarea 3.2: mediciones estatico vs. dinamico (tiempo y memoria).</summary>
public class ComparativaPanel : UserControl
{
    private readonly NumericUpDown _numN = new() { Minimum = 10_000, Maximum = 5_000_000, Increment = 100_000, Value = 1_000_000, Width = 100 };
    private readonly Button _btnMedir = Utilidades.Boton("Medir");
    private readonly Label _estado = new() { Text = "Listo.", AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
    private readonly DataGridView _tablaTiempo = Utilidades.Tabla("Estructura", "Tiempo de insercion (ms)", "Memoria ocupada (KB)", "Bytes por elemento");
    private readonly DataGridView _tablaOcupacion = Utilidades.Tabla("Ocupacion de la capacidad", "Elementos", "Estatica (KB, siempre reserva todo)", "Dinamica (KB)", "Menor consumo");

    public ComparativaPanel()
    {
        Dock = DockStyle.Fill;

        var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 4, 6, 0), WrapContents = true };
        fila.Controls.Add(Utilidades.Etiqueta("Elementos (N):"));
        fila.Controls.Add(_numN);
        fila.Controls.Add(_btnMedir);
        fila.Controls.Add(_estado);

        var t1 = new Label
        {
            Text = "1) Insercion de N enteros: tiempo y memoria (usa modo Release para tiempos reales)",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            TextAlign = ContentAlignment.BottomLeft
        };
        var t2 = new Label
        {
            Text = "2) Desperdicio de memoria: pila estatica de capacidad N reservada por completo vs. pila dinamica segun la ocupacion real",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            TextAlign = ContentAlignment.BottomLeft
        };

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.Controls.Add(fila, 0, 0);
        tabla.Controls.Add(t1, 0, 1);
        tabla.Controls.Add(_tablaTiempo, 0, 2);
        tabla.Controls.Add(t2, 0, 3);
        tabla.Controls.Add(_tablaOcupacion, 0, 4);
        Controls.Add(tabla);

        _btnMedir.Click += async (_, _) => await MedirAsync();
    }

    private async Task MedirAsync()
    {
        _btnMedir.Enabled = false;
        _tablaTiempo.Rows.Clear();
        _tablaOcupacion.Rows.Clear();
        _estado.Text = "Midiendo... puede tardar unos segundos.";
        int n = (int)_numN.Value;

        var (tiempos, ocupacion) = await Task.Run(() =>
        {
            Comparativa.MedirInsercion(10_000);               // calentamiento (se descarta)
            return (Comparativa.MedirInsercion(n), Comparativa.MedirOcupacion(n));
        });

        foreach (var f in tiempos)
            _tablaTiempo.Rows.Add(f.Estructura, f.Milisegundos.ToString("F2"), (f.Bytes / 1024).ToString("N0"), (f.Bytes / (double)n).ToString("F1"));

        foreach (var f in ocupacion)
            _tablaOcupacion.Rows.Add($"{f.Porcentaje} %", f.Elementos.ToString("N0"), (f.BytesEstatica / 1024).ToString("N0"),
                                     (f.BytesDinamica / 1024).ToString("N0"), f.BytesEstatica <= f.BytesDinamica ? "Estatica" : "Dinamica");

        _estado.Text = "Medicion terminada.";
        _btnMedir.Enabled = true;
    }
}