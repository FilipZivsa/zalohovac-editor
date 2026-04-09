using zalohovac_editor;
using zalohovac_editor.Presentation.Windows;

namespace zalohovac_editor // Tvůj hlavní namespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vytvoříme hlavní smyčku aplikace
            Application app = new Application();

            // Vytvoříme naše nové okno pro editaci
            JobEditWindow mainWindow = new JobEditWindow(app);

            // Spustíme aplikaci a předáme jí toto okno ke zobrazení
            app.Run(mainWindow);
        }
    }
}