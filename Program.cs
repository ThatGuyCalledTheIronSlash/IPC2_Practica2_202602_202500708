namespace IPC2_Practica2_202602_202500708;

static class Program
{
// Punto de entrada principal para la aplicación.
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}