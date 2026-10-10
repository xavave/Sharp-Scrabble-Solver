using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace DawgSolver.Model
{
    /// <summary>
    /// Réglages persistants de l'application, stockés en JSON dans
    /// %AppData%\ScrabbleSolver\settings.json.
    /// </summary>
    public class AppSettings
    {
        /// <summary>Nom du fichier DAWG du dictionnaire choisi (ex. dico_dawgODS9.txt).</summary>
        public string Dico { get; set; } = "dico_dawgODS9.txt";

        /// <summary>Couleur de remplissage du texte du joueur 1 (format #RRGGBB).</summary>
        public string Player1Color { get; set; } = "#FFD700";

        /// <summary>Couleur de remplissage du texte du joueur 2 (format #RRGGBB).</summary>
        public string Player2Color { get; set; } = "#3CB371";

        /// <summary>
        /// Police des lettres du plateau (contour noir + remplissage couleur du joueur).
        /// Une des polices libres embarquées (ex. "Titan One") ; vide = rendu Windows standard.
        /// </summary>
        public string BoardFontName { get; set; } = TileFonts.DefaultFontName;

        /// <summary>Épaisseur visible du contour des lettres du plateau (pixels).</summary>
        public float BoardOutlineWidth { get; set; } = 1.6f;

        /// <summary>Taille (en points) de la police de l'historique des coups.</summary>
        public float HistoryFontSizePt { get; set; } = 9f;

        /// <summary>Langue de l'interface : "fr" ou "en".</summary>
        public string Language { get; set; } = "fr";

        public static string SettingsPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "ScrabbleSolver", "settings.json");

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                    return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new AppSettings();
            }
            catch (Exception ex)
            {
                Logger.LogException("AppSettings.Load", ex);
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
            }
            catch (Exception ex)
            {
                Logger.LogException("AppSettings.Save", ex);
            }
        }

        // Conversions pratiques couleur <-> chaîne
        public Color GetPlayerColor(bool player1) => ParseColor(player1 ? Player1Color : Player2Color, player1 ? Color.Gold : Color.MediumSeaGreen);

        public void SetPlayerColor(bool player1, Color c)
        {
            var html = ColorTranslator.ToHtml(Color.FromArgb(c.R, c.G, c.B));
            if (player1) Player1Color = html; else Player2Color = html;
        }

        private static Color ParseColor(string s, Color fallback)
        {
            try { return string.IsNullOrWhiteSpace(s) ? fallback : ColorTranslator.FromHtml(s); }
            catch { return fallback; }
        }
    }
}
