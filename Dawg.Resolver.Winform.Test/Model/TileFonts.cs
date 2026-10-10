using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DawgSolver.Model
{
    /// <summary>
    /// Polices libres de droit (SIL OFL / Apache 2.0) embarquées dans Resources\Fonts,
    /// et rendu « lettre de plateau » : remplissage couleur + contour noir tracé géométriquement
    /// (GraphicsPath), ce qui fonctionne avec n'importe quelle police (pas besoin d'une variante Outline).
    /// </summary>
    public static class TileFonts
    {
        private static PrivateFontCollection _pfc;
        private static readonly Dictionary<string, FontFamily> _families = new Dictionary<string, FontFamily>(StringComparer.OrdinalIgnoreCase);

        public const string DefaultFontName = "Titan One";

        /// <summary>Noms des polices disponibles (familles embarquées).</summary>
        public static IReadOnlyList<string> Names => _families.Keys.OrderBy(n => n).ToList();

        public static void EnsureLoaded()
        {
            if (_pfc != null) return;
            _pfc = new PrivateFontCollection();
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                foreach (var res in asm.GetManifestResourceNames().Where(n => n.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase)))
                {
                    byte[] data;
                    using (var s = asm.GetManifestResourceStream(res))
                    using (var ms = new MemoryStream()) { s.CopyTo(ms); data = ms.ToArray(); }
                    // Mémoire volontairement conservée : la police doit rester chargée.
                    IntPtr ptr = Marshal.AllocCoTaskMem(data.Length);
                    Marshal.Copy(data, 0, ptr, data.Length);
                    _pfc.AddMemoryFont(ptr, data.Length);
                }
                foreach (var fam in _pfc.Families) _families[fam.Name] = fam;
                Logger.Log($"Polices plateau chargées : {string.Join(", ", Names)}");
            }
            catch (Exception ex)
            {
                Logger.LogException("TileFonts.EnsureLoaded", ex);
            }
        }

        public static FontFamily Get(string name)
        {
            EnsureLoaded();
            if (!string.IsNullOrWhiteSpace(name) && _families.TryGetValue(name, out var f)) return f;
            return null;
        }

        private static readonly Dictionary<string, float> _capHeightPerEm = new Dictionary<string, float>();

        /// <summary>Hauteur d'une capitale (glyphe "H") pour une taille em donnée, mise en cache par famille.</summary>
        private static float CapHeight(FontFamily f, float em)
        {
            if (!_capHeightPerEm.TryGetValue(f.Name, out var ratio))
            {
                using (var p = new GraphicsPath())
                {
                    p.AddString("H", f, (int)FirstAvailableStyle(f), 100f, Point.Empty, StringFormat.GenericTypographic);
                    ratio = p.GetBounds().Height / 100f;
                }
                _capHeightPerEm[f.Name] = ratio;
            }
            return ratio * em;
        }

        private static FontStyle FirstAvailableStyle(FontFamily f)
        {
            foreach (var st in new[] { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Bold | FontStyle.Italic })
                if (f.IsStyleAvailable(st)) return st;
            return FontStyle.Regular;
        }

        /// <summary>
        /// Dessine <paramref name="text"/> centré dans <paramref name="rect"/> : contour noir
        /// d'épaisseur visible <paramref name="outline"/> px, puis remplissage <paramref name="fill"/>.
        /// Le texte est réduit automatiquement s'il dépasse de la case.
        /// </summary>
        public static void DrawOutlinedCentered(Graphics g, string text, FontFamily family, Rectangle rect, Color fill, float outline)
        {
            if (string.IsNullOrEmpty(text) || family == null || rect.Width <= 2 || rect.Height <= 2) return;
            var oldSmoothing = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                float em = rect.Height * 0.80f;
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, family, (int)FirstAvailableStyle(family), em,
                                   Point.Empty, StringFormat.GenericTypographic);
                    var b = path.GetBounds();
                    if (b.Width <= 0 || b.Height <= 0) return;

                    // Taille homogène : calibrée sur la hauteur d'une capitale ("H") = 64 % de la case,
                    // puis réduite seulement si la lettre déborde (W, M, Q avec sa queue...).
                    float maxW = rect.Width - 2 * outline - 2;
                    float maxH = rect.Height - 2 * outline - 2;
                    float capH = CapHeight(family, em);
                    float scale = capH > 0 ? rect.Height * 0.64f / capH : maxH / b.Height;
                    scale = Math.Min(scale, Math.Min(maxW / b.Width, maxH / b.Height));
                    using (var m = new Matrix())
                    {
                        // (opérations en Prepend : appliquées de la dernière à la première)
                        m.Translate(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
                        m.Scale(scale, scale);
                        m.Translate(-(b.X + b.Width / 2f), -(b.Y + b.Height / 2f));
                        path.Transform(m);
                    }
                    // Trait noir de largeur double PUIS remplissage : seule la moitié extérieure du trait
                    // reste visible -> contour net qui ne « mange » pas la lettre.
                    using (var pen = new Pen(Color.Black, outline * 2) { LineJoin = LineJoin.Round })
                    using (var brush = new SolidBrush(fill))
                    {
                        g.DrawPath(pen, path);
                        g.FillPath(brush, path);
                    }
                }
            }
            finally
            {
                g.SmoothingMode = oldSmoothing;
            }
        }

        /// <summary>Construit le tracé d'une ligne de texte dont la hauteur de capitale vaut <paramref name="capHeightPx"/>.</summary>
        private static GraphicsPath BuildLinePath(string text, FontFamily family, float capHeightPx, out RectangleF bounds)
        {
            const float em = 100f;
            var path = new GraphicsPath();
            path.AddString(text, family, (int)FirstAvailableStyle(family), em, Point.Empty, StringFormat.GenericTypographic);
            float capH = CapHeight(family, em);
            float scale = capH > 0 ? capHeightPx / capH : 1f;
            using (var m = new Matrix()) { m.Scale(scale, scale); path.Transform(m); }
            bounds = path.GetBounds();
            return path;
        }

        /// <summary>Largeur (px) nécessaire pour afficher une ligne de texte de hauteur <paramref name="height"/>.</summary>
        public static int MeasureLineWidth(string text, FontFamily family, int height, float outline)
        {
            if (string.IsNullOrEmpty(text) || family == null || height <= 0) return 0;
            using (BuildLinePath(text, family, height * 0.60f, out var b))
                return (int)Math.Ceiling(b.Right + 2 * outline + 6 + height * 0.15f);   // marge de sécurité
        }

        /// <summary>
        /// Dessine une ligne de texte alignée à gauche et centrée verticalement dans <paramref name="rect"/> :
        /// contour noir + remplissage <paramref name="fill"/> (même rendu que les lettres du plateau).
        /// </summary>
        public static void DrawOutlinedLine(Graphics g, string text, FontFamily family, Rectangle rect, Color fill, float outline)
        {
            if (string.IsNullOrEmpty(text) || family == null || rect.Height <= 2) return;
            var oldSmoothing = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                using (var path = BuildLinePath(text, family, rect.Height * 0.60f, out var b))
                {
                    // Garde-fou : si le texte est plus large que la zone, on le réduit pour qu'il tienne en entier.
                    float avail = rect.Width - 2 * outline - 6;
                    float fit = b.Right > avail && b.Right > 0 ? avail / b.Right : 1f;
                    using (var m = new Matrix())
                    {
                        // (Prepend : la mise à l'échelle s'applique avant la translation)
                        m.Translate(rect.X + outline + 3, rect.Y + rect.Height / 2f);
                        m.Scale(fit, fit);
                        m.Translate(0, -(b.Y + b.Height / 2f));
                        path.Transform(m);
                    }
                    using (var pen = new Pen(Color.Black, outline * 2) { LineJoin = LineJoin.Round })
                    using (var brush = new SolidBrush(fill))
                    {
                        g.DrawPath(pen, path);
                        g.FillPath(brush, path);
                    }
                }
            }
            finally
            {
                g.SmoothingMode = oldSmoothing;
            }
        }
    }

    /// <summary>Style courant des lettres du plateau (alimenté par MainForm depuis AppSettings).</summary>
    public static class TileStyle
    {
        /// <summary>Police utilisée ; null = rendu Windows standard (effet désactivé).</summary>
        public static FontFamily Family { get; set; }
        public static Color Player1Fill { get; set; } = Color.Gold;
        public static Color Player2Fill { get; set; } = Color.MediumSeaGreen;
        /// <summary>Remplissage des lettres non attribuées à un joueur (saisie manuelle, aperçu).</summary>
        public static Color NeutralFill { get; set; } = Color.White;
        /// <summary>Épaisseur visible du contour, en pixels.</summary>
        public static float OutlineWidth { get; set; } = 1.6f;
    }
}
