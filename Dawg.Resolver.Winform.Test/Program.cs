using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using DawgSolver.Model;

namespace Dawg.Solver.Winform.Test
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.Init();

            // Capture globale des exceptions (UI, threads de fond, tâches non observées).
            Application.ThreadException += (s, e) => Logger.LogException("Application.ThreadException", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => Logger.LogException("AppDomain.UnhandledException", e.ExceptionObject as Exception);
            TaskScheduler.UnobservedTaskException += (s, e) => { Logger.LogException("UnobservedTaskException", e.Exception); e.SetObserved(); };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Logger.Log("Démarrage de l'application.");
            Application.Run(new MainForm());
            Logger.Log("Fermeture de l'application.");
        }
    }
}
