using System;
using System.IO;
using System.Text;

namespace DawgSolver.Model
{
    /// <summary>
    /// Journal d'événements simple et thread-safe, écrit dans un fichier texte
    /// à côté de l'exécutable (scrabble.log). Utilisé notamment pour capturer les
    /// exceptions d'AutoPlay qui, sinon, sont avalées silencieusement sur le thread
    /// d'arrière-plan.
    /// </summary>
    public static class Logger
    {
        private static readonly object _lock = new object();
        public static string LogFilePath { get; private set; }
        public static bool Enabled { get; set; } = true;

        public static void Init()
        {
            try
            {
                LogFilePath = Path.Combine(AppContext.BaseDirectory, "scrabble.log");
                // Nouvelle session : on repart d'un fichier propre.
                File.WriteAllText(LogFilePath, $"=== Scrabble log - session {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==={Environment.NewLine}", Encoding.UTF8);
            }
            catch
            {
                LogFilePath = null; // journalisation best-effort : on n'empêche jamais l'app de tourner
            }
        }

        public static void Log(string message)
        {
            Write($"{DateTime.Now:HH:mm:ss.fff}  {message}");
        }

        public static void LogException(string context, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{DateTime.Now:HH:mm:ss.fff}  [ERREUR] {context}: {ex?.GetType().FullName}: {ex?.Message}");
            var inner = ex?.InnerException;
            int depth = 0;
            while (inner != null && depth++ < 5)
            {
                sb.AppendLine($"    Inner: {inner.GetType().FullName}: {inner.Message}");
                inner = inner.InnerException;
            }
            if (ex?.StackTrace != null) sb.AppendLine(ex.StackTrace);
            Write(sb.ToString());
        }

        private static void Write(string text)
        {
            if (!Enabled) return;
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(LogFilePath ?? "scrabble.log", text + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // On n'interrompt jamais l'application pour un problème de log.
            }
        }
    }
}
