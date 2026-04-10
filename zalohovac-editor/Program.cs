using zalohovac_editor;
using zalohovac_editor.Presentation.Windows;

namespace zalohovac_editor 
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Application app = new Application();

            //nové okno pro editaci
            JobEditWindow mainWindow = new JobEditWindow(app);

            //sppustit aplikaci -  a předání jí toto okno ke zobrazení
            app.Run(mainWindow);
        }
    }
}