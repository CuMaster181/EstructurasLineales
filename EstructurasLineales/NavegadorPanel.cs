namespace Unidad3;

/// <summary>Pestana de la Tarea 3.3: simulador del historial de navegacion web con pilas.</summary>
public class NavegadorPanel : UserControl
{
    private readonly HistorialNavegador _nav = new();

    private readonly ComboBox _url = new() { Width = 380, DropDownStyle = ComboBoxStyle.DropDown };
    private readonly Button _btnIr = Utilidades.Boton("Visitar");
    private readonly Button _btnAtras = Utilidades.Boton("< Atras");
    private readonly Button _btnAdelante = Utilidades.Boton("Adelante >");
    private readonly Button _btnReiniciar = Utilidades.Boton("Reiniciar");

    private readonly Label _actual = new()
    {
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 13, FontStyle.Bold),
        TextAlign = ContentAlignment.MiddleCenter,
        BackColor = Color.WhiteSmoke,
        BorderStyle = BorderStyle.FixedSingle
    };
    private readonly ListBox _listaAtras = new() { Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
    private readonly ListBox _listaAdelante = new() { Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
    private readonly ListBox _listaHistorial = new() { Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
    private readonly TextBox _log = Utilidades.CajaTexto();

    public NavegadorPanel()
    {
        Dock = DockStyle.Fill;

        _url.Items.AddRange(new object[]
        {
            "https://www.wikipedia.org", "https://github.com", "https://learn.microsoft.com",
            "https://www.youtube.com", "https://www.google.com", "https://stackoverflow.com"
        });
        _url.Text = "https://www.wikipedia.org";

        var barra = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6, 6, 6, 0) };
        barra.Controls.Add(_btnAtras);
        barra.Controls.Add(_btnAdelante);
        barra.Controls.Add(Utilidades.Etiqueta("Direccion:"));
        barra.Controls.Add(_url);
        barra.Controls.Add(_btnIr);
        barra.Controls.Add(_btnReiniciar);

        var cuerpo = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 4 };
        cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31));
        cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31));
        cuerpo.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        cuerpo.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        cuerpo.Controls.Add(Titulo("PILA ATRAS  (tope arriba)"), 0, 0);
        cuerpo.Controls.Add(Titulo("PAGINA ACTUAL"), 1, 0);
        cuerpo.Controls.Add(Titulo("PILA ADELANTE  (tope arriba)"), 2, 0);
        cuerpo.Controls.Add(_listaAtras, 0, 1);
        cuerpo.Controls.Add(_actual, 1, 1);
        cuerpo.Controls.Add(_listaAdelante, 2, 1);
        var tituloHistorial = Titulo("HISTORIAL COMPLETO  (lista doblemente enlazada, de la mas reciente a la mas antigua)");
        cuerpo.Controls.Add(tituloHistorial, 0, 2);
        cuerpo.SetColumnSpan(tituloHistorial, 2);
        cuerpo.Controls.Add(Titulo("BITACORA DE OPERACIONES"), 2, 2);
        cuerpo.Controls.Add(_listaHistorial, 0, 3);
        cuerpo.SetColumnSpan(_listaHistorial, 2);
        cuerpo.Controls.Add(_log, 2, 3);

        var raiz = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        raiz.Controls.Add(barra, 0, 0);
        raiz.Controls.Add(cuerpo, 0, 1);
        Controls.Add(raiz);

        _btnIr.Click += (_, _) => Visitar();
        _btnAtras.Click += (_, _) => Navegar(true);
        _btnAdelante.Click += (_, _) => Navegar(false);
        _btnReiniciar.Click += (_, _) => { _nav.Reiniciar(); _log.Clear(); Escribir("Historial reiniciado."); Refrescar(); };
        _url.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Visitar(); }
        };

        Refrescar();
    }

    private static Label Titulo(string texto)
    {
        return new Label { Text = texto, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.BottomLeft };
    }

    private void Escribir(string t) => _log.AppendText($"[{Utilidades.Hora()}] {t}{Environment.NewLine}");

    private void Visitar()
    {
        try
        {
            string anterior = _nav.Actual ?? "(ninguna)";
            _nav.Visitar(_url.Text);
            Escribir($"VISITAR {_nav.Actual}  | Push({anterior}) en ATRAS, se vacia ADELANTE");
        }
        catch (ArgumentException ex)
        {
            Escribir("ERROR: " + ex.Message);
        }
        Refrescar();
    }

    private void Navegar(bool atras)
    {
        try
        {
            string origen = _nav.Actual ?? "";
            if (atras) { _nav.Atras(); Escribir($"ATRAS  | Push({origen}) en ADELANTE, Pop de ATRAS -> {_nav.Actual}"); }
            else { _nav.Adelante(); Escribir($"ADELANTE  | Push({origen}) en ATRAS, Pop de ADELANTE -> {_nav.Actual}"); }
        }
        catch (InvalidOperationException ex)
        {
            Escribir("ERROR: " + ex.Message);
        }
        Refrescar();
    }

    private void Refrescar()
    {
        _actual.Text = _nav.Actual ?? "(pagina en blanco)";

        _listaAtras.BeginUpdate();
        _listaAtras.Items.Clear();
        foreach (string s in _nav.PaginasAtras()) _listaAtras.Items.Add(s);
        _listaAtras.EndUpdate();

        _listaAdelante.BeginUpdate();
        _listaAdelante.Items.Clear();
        foreach (string s in _nav.PaginasAdelante()) _listaAdelante.Items.Add(s);
        _listaAdelante.EndUpdate();

        _listaHistorial.BeginUpdate();
        _listaHistorial.Items.Clear();
        foreach (var r in _nav.HistorialRecienteAPrimero()) _listaHistorial.Items.Add(r.ToString());
        _listaHistorial.EndUpdate();

        _btnAtras.Enabled = _nav.PuedeIrAtras;
        _btnAdelante.Enabled = _nav.PuedeIrAdelante;
    }
}