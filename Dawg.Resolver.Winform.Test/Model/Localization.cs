using System;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace DawgSolver.Model
{
    /// <summary>
    /// Accès aux traductions de l'interface.
    /// Fichiers : Resources\Strings.resx (anglais, langue neutre / repli)
    ///            Resources\Strings.fr.resx (français, compilé en assembly satellite "fr").
    /// Pour ajouter un texte : ajouter la même clé dans les deux .resx.
    /// Les clés "Ctl_NomDuControle" sont appliquées automatiquement aux contrôles (MainForm.ApplyLanguage).
    /// </summary>
    public static class L
    {
        private static ResourceManager _rm;
        private static bool _rmResolved;

        public static CultureInfo Culture { get; private set; } = new CultureInfo("fr");

        /// <summary>Code de langue courant : "fr" ou "en".</summary>
        public static string Language => Culture.TwoLetterISOLanguageName;

        private static ResourceManager RM
        {
            get
            {
                if (!_rmResolved)
                {
                    _rmResolved = true;
                    // Nom de ressource résolu dynamiquement (indépendant du RootNamespace).
                    var asm = typeof(L).Assembly;
                    var name = asm.GetManifestResourceNames()
                                  .FirstOrDefault(n => n.EndsWith(".Strings.resources", StringComparison.OrdinalIgnoreCase));
                    if (name != null)
                        _rm = new ResourceManager(name.Substring(0, name.Length - ".resources".Length), asm);
                    else
                        Logger.Log("Traductions : ressource Strings introuvable, textes d'origine conservés.");
                }
                return _rm;
            }
        }

        public static void SetLanguage(string lang)
        {
            Culture = new CultureInfo(string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase) ? "en" : "fr");
            CultureInfo.CurrentUICulture = Culture;
        }

        /// <summary>Texte traduit, ou null si la clé n'existe pas.</summary>
        public static string TryT(string key)
        {
            try { return RM?.GetString(key, Culture); }
            catch (MissingManifestResourceException) { return null; }
        }

        /// <summary>Texte traduit (la clé elle-même si absente).</summary>
        public static string T(string key) => TryT(key) ?? key;

        /// <summary>Texte traduit et formaté (string.Format).</summary>
        public static string F(string key, params object[] args) => string.Format(Culture, T(key), args);
    }
}
