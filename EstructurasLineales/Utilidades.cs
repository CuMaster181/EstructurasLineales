using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Unidad3;

public static class Utilidades
{
    /// <summary>Crea un TextBox multilinea con fuente monoespaciada.</summary>
    public static TextBox CajaTexto(bool soloLectura = true)
    {
        return new TextBox
        {
            Multiline = true,
            ReadOnly = soloLectura,
            ScrollBars = ScrollBars.Both,
            WordWrap = false,
            Font = new Font("Consolas", 10),
            BackColor = Color.White,
            Dock = DockStyle.Fill
        };
    }

    /// <summary>Crea una tabla (DataGridView) de solo lectura.</summary>
    public static DataGridView Tabla(params string[] columnas)
    {
        var t = new DataGridView
        {
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            RowHeadersVisible = false,
            Dock = DockStyle.Fill
        };
        foreach (string c in columnas) t.Columns.Add(c, c);
        return t;
    }

    public static Label Etiqueta(string texto, int arriba = 6)
    {
        return new Label { Text = texto, AutoSize = true, Padding = new Padding(0, arriba, 0, 0) };
    }

    public static Button Boton(string texto)
    {
        return new Button { Text = texto, AutoSize = true };
    }

    public static string Hora() => DateTime.Now.ToString("HH:mm:ss");
}

/// <summary>Panel sin parpadeo para dibujar con GDI+.</summary>
public class LienzoPanel : Panel
{
    public LienzoPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
    }
}

/// <summary>Dibuja un arreglo (celdas con indice) y marcadores como top, front y rear.</summary>
public class ArregloVisual : LienzoPanel
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string?[] Celdas { get; set; } = Array.Empty<string?>();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Dictionary<int, string> Marcadores { get; set; } = new();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Titulo { get; set; } = "";

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var fuenteTitulo = new Font("Segoe UI", 11, FontStyle.Bold);
        g.DrawString(Titulo, fuenteTitulo, Brushes.DimGray, 10, 8);

        int n = Celdas.Length;
        if (n == 0) return;

        int ancho = Math.Max(30, Math.Min(84, (Width - 40) / n));
        int alto = 54;
        int x0 = (Width - ancho * n) / 2;
        int y0 = Math.Max(40, Height / 2 - alto);

        using var fuente = new Font("Segoe UI", 10, FontStyle.Bold);
        using var fuenteIdx = new Font("Consolas", 10);
        using var fuenteMarca = new Font("Segoe UI", 9, FontStyle.Bold);
        using var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        for (int i = 0; i < n; i++)
        {
            var rect = new Rectangle(x0 + i * ancho, y0, ancho, alto);
            bool ocupada = Celdas[i] != null;
            g.FillRectangle(ocupada ? Brushes.LightSkyBlue : Brushes.WhiteSmoke, rect);
            g.DrawRectangle(Pens.Black, rect);
            if (ocupada) g.DrawString(Celdas[i], fuente, Brushes.Black, rect, formato);

            var rIdx = new RectangleF(rect.X, rect.Bottom + 2, rect.Width, 18);
            g.DrawString(i.ToString(), fuenteIdx, Brushes.Gray, rIdx, formato);

            if (Marcadores.TryGetValue(i, out string? marca))
            {
                int cx = rect.X + rect.Width / 2;
                int yt = rect.Bottom + 26;
                g.FillPolygon(Brushes.Crimson, new[] { new Point(cx, yt), new Point(cx - 7, yt + 11), new Point(cx + 7, yt + 11) });
                var rMarca = new RectangleF(rect.X - 10, yt + 12, rect.Width + 20, 18);
                g.DrawString(marca, fuenteMarca, Brushes.Crimson, rMarca, formato);
            }
        }
    }
}

/// <summary>Dibuja una secuencia de nodos enlazados con flechas (lista, pila o cola dinamica).</summary>
public class ListaVisual : LienzoPanel
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<string> Items { get; set; } = new();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Doble { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string EtiquetaInicio { get; set; } = "cabeza";
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string EtiquetaFin { get; set; } = "";
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Titulo { get; set; } = "";

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var fuenteTitulo = new Font("Segoe UI", 11, FontStyle.Bold);
        g.DrawString(Titulo, fuenteTitulo, Brushes.DimGray, 10, 8);

        using var fuente = new Font("Segoe UI", 10, FontStyle.Bold);
        using var fuenteMarca = new Font("Segoe UI", 9, FontStyle.Bold);
        using var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using var flecha = new Pen(Color.Black, 2) { CustomEndCap = new AdjustableArrowCap(4, 5) };

        int nodoW = 78, nodoH = 42, sep = 48;
        int porFila = Math.Max(1, (Width - 80) / (nodoW + sep));

        if (Items.Count == 0)
        {
            g.DrawString("(estructura vacia: " + EtiquetaInicio + " = null)", fuente, Brushes.Gray, 30, 70);
            return;
        }

        int total = Items.Count + 1;   // el ultimo "nodo" dibujado es 'null'
        for (int i = 0; i < total; i++)
        {
            int fila = i / porFila, col = i % porFila;
            int x = 30 + col * (nodoW + sep);
            int y = 60 + fila * (nodoH + 56);
            var rect = new Rectangle(x, y, nodoW, nodoH);

            if (i == Items.Count)       // nodo "null" al final
            {
                g.DrawString("null", fuente, Brushes.Gray, rect, formato);
            }
            else
            {
                g.FillRectangle(Brushes.LightGoldenrodYellow, rect);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString(Items[i], fuente, Brushes.Black, rect, formato);

                if (i == 0) DibujarMarca(g, EtiquetaInicio, rect, fuenteMarca);
                if (i == Items.Count - 1 && EtiquetaFin != "") DibujarMarca(g, EtiquetaFin, rect, fuenteMarca, i == 0);
            }

            // flecha hacia el siguiente en la misma fila
            if (i < total - 1 && col < porFila - 1)
            {
                int ya = y + (Doble ? nodoH / 3 : nodoH / 2);
                g.DrawLine(flecha, x + nodoW, ya, x + nodoW + sep - 4, ya);
                if (Doble && i < Items.Count - 1)
                {
                    int yb = y + 2 * nodoH / 3;
                    g.DrawLine(flecha, x + nodoW + sep, yb, x + nodoW + 4, yb);
                }
            }
            else if (i < total - 1)
            {
                g.DrawString("continua >", fuenteMarca, Brushes.Gray, x + nodoW + 4, y + nodoH / 2 - 8);
            }
        }
    }

    private static void DibujarMarca(Graphics g, string texto, Rectangle nodo, Font fuente, bool segunda = false)
    {
        int cx = nodo.X + nodo.Width / 2;
        int yTexto = nodo.Y - 34 - (segunda ? 16 : 0);
        g.DrawString(texto, fuente, Brushes.Crimson, cx - 30, yTexto);
        g.FillPolygon(Brushes.Crimson, new[] { new Point(cx, nodo.Y - 2), new Point(cx - 6, nodo.Y - 12), new Point(cx + 6, nodo.Y - 12) });
    }
}