using System;
using System.Collections.Generic;
using System.Linq;

namespace DawgSolver.Model
{
    /// <summary>Un mot trouvé par <see cref="WordSearch"/>.</summary>
    public sealed class FoundWord
    {
        /// <summary>Mot affiché : les lettres jouées par un joker sont en minuscules.</summary>
        public string Display { get; set; }
        /// <summary>Mot en majuscules (pour le dictionnaire / la définition).</summary>
        public string Text => Display.ToUpperInvariant();
        public int Points { get; set; }
        public int JokersUsed { get; set; }
        public override string ToString() => $"{Display}  ({Points} pts)";
    }

    /// <summary>
    /// Chercheur de mots indépendant du plateau, qui parcourt directement le DAWG
    /// (tableau Legacy du dictionnaire chargé : Legacy[1..26, nœud] = nœud suivant pour A..Z,
    /// Legacy[27, nœud] = 1 si le nœud termine un mot ; nœud racine = 1).
    ///
    /// - Tirage : lettres disponibles, chacune utilisable une fois ; '?' (ou '*', ' ') = joker.
    ///   Tirage vide = lettres illimitées (utile avec un motif).
    /// - Motif (facultatif) : lettre = lettre imposée (déjà posée, ne consomme pas le tirage),
    ///   '?' ou '.' = une case à remplir, '*' = zéro, une ou plusieurs cases.
    ///   Ex. « B??JOUR », « *TION », « RE* ».
    /// </summary>
    public static class WordSearch
    {
        public const int MaxLength = 15;
        public const int MaxResults = 5000;

        public static List<FoundWord> Search(int[,] legacy, string rack, string pattern, int length, bool useAllLetters,
                                             Func<char, int> letterValue)
        {
            if (legacy == null) throw new InvalidOperationException("Aucun dictionnaire chargé.");
            var counts = new int[27];
            int jokers = 0, rackSize = 0;
            foreach (var ch in (rack ?? "").ToUpperInvariant())
            {
                if (ch >= 'A' && ch <= 'Z') { counts[ch - 'A' + 1]++; rackSize++; }
                else if (ch == '?' || ch == '*' || ch == ' ' || ch == '.') { jokers++; rackSize++; }
            }
            bool unlimited = rackSize == 0;

            var tokens = (pattern ?? "").Trim().ToUpperInvariant().Replace('.', '?')
                                        .Where(c => (c >= 'A' && c <= 'Z') || c == '?' || c == '*').ToArray();
            bool hasPattern = tokens.Length > 0;
            if (!hasPattern && unlimited) return new List<FoundWord>();   // il faut au moins un tirage ou un motif

            var ctx = new Ctx
            {
                Legacy = legacy, Counts = counts, Jokers = jokers, Unlimited = unlimited,
                Tokens = hasPattern ? tokens : null,
                Length = hasPattern ? 0 : length,
                RequiredFromRack = useAllLetters && !unlimited ? rackSize : -1,
                Value = letterValue,
            };
            ctx.Walk(1, 0, new char[MaxLength], 0, 0, 0);

            return ctx.Results.Values
                      .OrderByDescending(w => w.Display.Length)
                      .ThenByDescending(w => w.Points)
                      .ThenBy(w => w.Text, StringComparer.Ordinal)
                      .Take(MaxResults)
                      .ToList();
        }

        private sealed class Ctx
        {
            public int[,] Legacy;
            public int[] Counts;
            public int Jokers;
            public bool Unlimited;
            public char[] Tokens;          // null = pas de motif
            public int Length;             // 0 = toutes longueurs (sans motif)
            public int RequiredFromRack;   // -1 = pas d'exigence
            public Func<char, int> Value;
            public readonly Dictionary<string, FoundWord> Results = new Dictionary<string, FoundWord>();

            int Next(int node, int letter) => Legacy[letter, node];
            bool Terminal(int node) => Legacy[27, node] != 0;

            /// <param name="tok">position dans le motif</param>
            /// <param name="fromRack">nombre de lettres prises dans le tirage (jokers compris)</param>
            public void Walk(int node, int tok, char[] buf, int len, int fromRack, int points)
            {
                if (Results.Count >= MaxResults * 2) return;

                // Fin éventuelle du mot
                bool patternDone = Tokens == null || tok == Tokens.Length
                                   || (tok == Tokens.Length - 1 && Tokens[tok] == '*');
                if (patternDone && len >= 2 && Terminal(node)
                    && (Length == 0 || len == Length)
                    && (RequiredFromRack < 0 || fromRack == RequiredFromRack)
                    && (Tokens != null || fromRack > 0))
                    Record(buf, len, points);

                if (len >= MaxLength) return;
                if (Tokens == null && Length > 0 && len >= Length) return;

                if (Tokens == null)
                {
                    FillFromRack(node, tok, buf, len, fromRack, points);
                    return;
                }
                if (tok >= Tokens.Length) return;

                char t = Tokens[tok];
                if (t >= 'A' && t <= 'Z')
                {
                    int nx = Next(node, t - 'A' + 1);
                    if (nx == 0) return;
                    buf[len] = t;
                    Walk(nx, tok + 1, buf, len + 1, fromRack, points + Value(t));
                }
                else if (t == '?')
                {
                    FillFromRack(node, tok + 1, buf, len, fromRack, points);
                }
                else // '*' : soit on passe le jeton, soit on remplit une case et on reste dessus
                {
                    Walk(node, tok + 1, buf, len, fromRack, points);
                    FillFromRack(node, tok, buf, len, fromRack, points);
                }
            }

            /// <summary>Remplit la case courante avec une lettre du tirage (ou un joker).</summary>
            void FillFromRack(int node, int nextTok, char[] buf, int len, int fromRack, int points)
            {
                for (int i = 1; i <= 26; i++)
                {
                    int nx = Next(node, i);
                    if (nx == 0) continue;
                    char c = (char)('A' + i - 1);
                    if (Unlimited)
                    {
                        buf[len] = c;
                        Walk(nx, nextTok, buf, len + 1, fromRack, points + Value(c));
                    }
                    else if (Counts[i] > 0)
                    {
                        // Une vraie lettre vaut toujours mieux qu'un joker pour la même case.
                        Counts[i]--; buf[len] = c;
                        Walk(nx, nextTok, buf, len + 1, fromRack + 1, points + Value(c));
                        Counts[i]++;
                    }
                    else if (Jokers > 0)
                    {
                        Jokers--; buf[len] = char.ToLowerInvariant(c);
                        Walk(nx, nextTok, buf, len + 1, fromRack + 1, points);   // joker = 0 point
                        Jokers++;
                    }
                }
            }

            void Record(char[] buf, int len, int points)
            {
                var display = new string(buf, 0, len);
                var key = display.ToUpperInvariant();
                int jokers = display.Count(char.IsLower);
                if (Results.TryGetValue(key, out var existing) && existing.Points >= points) return;
                Results[key] = new FoundWord { Display = display, Points = points, JokersUsed = jokers };
            }
        }
    }
}
