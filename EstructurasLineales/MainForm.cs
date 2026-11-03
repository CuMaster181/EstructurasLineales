namespace Unidad3;

/// <summary>Ventana principal: una pestana por cada tarea de la Unidad 3.</summary>
public class MainForm : Form
{
    public MainForm()
    {
        Text = "Unidad 3 - Estructuras lineales (Tareas 3.1, 3.2 y 3.3)";
        Width = 1150;
        Height = 780;
        StartPosition = FormStartPosition.CenterScreen;

        // Pestanas internas de la Tarea 3.2
        var dinamicas = new TabControl { Dock = DockStyle.Fill };
        AgregarPestana(dinamicas, "Lista doblemente enlazada", new ListaDoblePanel());
        AgregarPestana(dinamicas, "Pila dinamica", new PilaColaDinamicaPanel(true));
        AgregarPestana(dinamicas, "Cola dinamica", new PilaColaDinamicaPanel(false));
        AgregarPestana(dinamicas, "Clases .NET (Stack, Queue, LinkedList, List)", new ClasesNetPanel());
        AgregarPestana(dinamicas, "Comparativa estatico vs. dinamico", new ComparativaPanel());

        var principal = new TabControl { Dock = DockStyle.Fill };
        AgregarPestana(principal, "3.1  Pila y Colas estaticas", new EstaticasPanel());
        AgregarPestana(principal, "3.2  Estructuras dinamicas", dinamicas);
        AgregarPestana(principal, "3.3  Navegador web (Pilas)", new NavegadorPanel());
        Controls.Add(principal);
    }

    private static void AgregarPestana(TabControl tabs, string titulo, Control contenido)
    {
        var pagina = new TabPage(titulo);
        contenido.Dock = DockStyle.Fill;
        pagina.Controls.Add(contenido);
        tabs.TabPages.Add(pagina);
    }
}