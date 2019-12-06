using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    public static class Extensions
    {

        public static Letter GetRandomLetter(this List<Letter> letters)
        {
            Random rng = new Random();
            int index = rng.Next(letters.Where(l => l.Count > 0).Count());
            letters[index].Count--;
            return letters[index];
        }
        public static string GetLetterByIndex(this List<Letter> l, int index)
        {
            return Game.AlphabetAvecJoker.Find(t => t.Char == (char)(index + Dictionnaire.AscShift)).Char.ToString();
        }

        public static VTile Copy(this VTile t,Game g, VTile[,] grid, bool transpose = false)
        {
            return new Tile(g,transpose ? t.Col : t.Ligne, transpose ? t.Ligne : t.Col)
            {
                Controlers = transpose ? new Dictionary<int, int>(27) : t.Controlers,
                Letter = t.Letter,
                IsAnchor = transpose ? false : t.IsAnchor,
                LetterMultiplier = t.LetterMultiplier,
                WordMultiplier = t.WordMultiplier,
                AnchorLeftMinLimit = transpose ? 0 : t.AnchorLeftMinLimit,
                AnchorLeftMaxLimit = transpose ? 0 : t.AnchorLeftMaxLimit,
                FromJoker = transpose ? false : t.FromJoker
            };
        }
        public static void SetWord(this VTile t, Game g, string word, MovementDirection direction)
        {
            if (t != null)
                t.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(word[0]));
            foreach (var c in word.Skip(1))
            {
                if (direction == MovementDirection.Across)
                    t = t.SetRightLetter(c);
                else
                    t = t.SetDownLetter(c);
            }
        }
        public static VTile SetRightLetter(this VTile t, char c)
        {
            if (t != null && t.RightTile != null)
            {
                t.RightTile.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                return t.RightTile;
            }
            return null;
        }
        public static VTile SetDownLetter(this VTile t, char c)
        {
            if (t != null && t.DownTile != null)
            {
                t.DownTile.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                return t.DownTile;
            }
            return null;
        }

        public static VTile[,] Transpose(this VTile[,] tiles)
        {
            var ret = new VTile[tiles.GetLength(0), tiles.GetLength(1)];
            foreach (var t in tiles)
            {
                ret[t.Col, t.Ligne] = t;
            }

            return ret;
        }
    }
}
