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
        public static string ReverseString(this string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static Letter GetRandomLetter(this List<Letter> letters)
        {
            Random rng = new Random();
            int index = rng.Next(letters.Where(l => l.Count > 0).Count());
            letters[index].Count--;
            return letters[index];
        }
        public static string GetLetterByIndex(this List<Letter> l, int index)
        {
            return Game.AlphabetWWFAvecJoker.Find(t => t.Char == (char)(index + Dictionnaire.AscShift)).Char.ToString();
        }
        public static void ResetCount(this List<Letter> lst)
        {
            foreach (var l in lst)
                l.Count = l.DefaultCount;

        }

        public static VTile Copy(this VTile t, Game g, VTile[,] grid, bool transpose = false)
        {
            VTile newT = t;
            if (transpose) newT = grid[t.Col, t.Ligne];

            return new Tile(g, newT.Ligne, newT.Col)
            {
                Controlers = t.Controlers,
                Letter = t.Letter,
                //IsAnchor = transpose ? false : t.IsAnchor,
                LetterMultiplier = t.LetterMultiplier,
                WordMultiplier = t.WordMultiplier,
                AnchorLeftMinLimit = t.AnchorLeftMinLimit,
                AnchorLeftMaxLimit = t.AnchorLeftMaxLimit,
                FromJoker = transpose ? false : t.FromJoker
            };
        }

        public static Letter DeserializeLetter(this string s)
        {
            var l = s.Split(';');
            return new Letter(l[0].Skip(1).First(), int.Parse(l[1]), int.Parse(l[2]));
        }
        public static VTile DeserializeTile(this string s, Game g)
        {
            var l = s.Split(';');
            var t = new Tile(g, int.Parse(l[0].Substring(1)), int.Parse(l[1]))
            {
                LetterMultiplier = int.Parse(l[2]),
                WordMultiplier = int.Parse(l[3]),
                FromJoker = bool.Parse(l[4]),
                IsValidated = bool.Parse(l[5]),

            };
            if (l.Count() > 6 && l[6] != "")
                t.Letter = t.FromJoker ? Game.AlphabetWWFAvecJoker.Find(c => c.Char == Game.Joker) : Game.AlphabetWWFAvecJoker.Find(c => c.Char == l[6][0]);
            if (l.Count() > 7 && l[7] != "")
                t.IsPlayedByPlayer1 = bool.Parse(l[7]);
            return t;
        }
        public static Word DeserializeMove(this string s, Game g)
        {
            var l = s.Split(';');
            return new Word(g)
            {
                StartTile = new Tile(g, int.Parse(l[0].Substring(2)), int.Parse(l[1])),
                Text = l[2],
                Points = int.Parse(l[3]),
                Direction = (MovementDirection)Enum.Parse(typeof(MovementDirection), l[4]),
                Scramble = l[5].First() == '*'
            };
        }

        public static string String(this List<Letter> lst)
        {
            var ret = "";
            for (int i = 0; i < lst.Count(); i++)
            {
                ret += lst[i].Char;
            }
            return ret;
        }
        public static void SetWord(this VTile t, Player p, string word, MovementDirection direction, bool Validate = false)
        {
            if (word == "") return;
            if (t.Col == Game.BoardSize - 1)
                t = t.LeftTile?.RightTile;
            else
                t = t.RightTile?.LeftTile;
            if (t != null)
            {
                t.Letter = t.SetLetter(word[0], p, Validate);

                foreach (var c in word.Skip(1))
                {
                    if (direction == MovementDirection.Across)
                        t = t.SetRightLetter(c, p, Validate);
                    else
                        t = t.SetDownLetter(c, p, Validate);
                }
            }
        }
        public static Letter SetLetter(this VTile t, char c, Player p, bool Validate)
        {
            if (t != null)
            {
                if (t.IsEmpty)
                    t.IsValidated = Validate;
                t.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                if (char.IsLower(c))
                    t.FromJoker = true;
                if (t.FromJoker)
                    p.Rack.Remove(Game.AlphabetWWFAvecJoker[26]);
                else
                    p.Rack.Remove(t.Letter);

            }
            return t.Letter;
        }
        public static VTile SetRightLetter(this VTile t, char c, Player p, bool Validate)
        {
            if (t != null && t.RightTile != null)
            {
                if (t.RightTile.IsEmpty)
                    t.RightTile.IsValidated = Validate;
                t.RightTile.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                if (char.IsLower(c))
                    t.RightTile.FromJoker = true;
                if (t.RightTile.FromJoker)
                    p.Rack.Remove(Game.AlphabetWWFAvecJoker[26]);
                else
                    p.Rack.Remove(Game.Alphabet.Find(a => a.Char == char.ToUpper(c)));
                return t.RightTile;
            }
            return null;
        }
        public static VTile SetDownLetter(this VTile t, char c, Player p, bool Validate)
        {
            if (t != null && t.DownTile != null)
            {
                if (t.DownTile.IsEmpty)
                    t.DownTile.IsValidated = Validate;
                t.DownTile.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                if (char.IsLower(c))
                    t.DownTile.FromJoker = true;
                if (t.DownTile.FromJoker)
                    p.Rack.Remove(Game.AlphabetWWFAvecJoker[26]);
                else
                    p.Rack.Remove(Game.Alphabet.Find(a => a.Char == char.ToUpper(c)));
                return t.DownTile;
            }
            return null;
        }
        public static VTile[,] Copy(this VTile[,] tiles)
        {
            var ret = new VTile[tiles.GetLength(0), tiles.GetLength(1)];
            for (int ligne = 0; ligne < tiles.GetLength(0); ligne++)
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    ret[ligne, col] = tiles[ligne, col];

                }
            return ret;
        }
        public static VTile[,] Transpose(this VTile[,] tiles, Game g)
        {
            var ret = new VTile[tiles.GetLength(0), tiles.GetLength(1)];
            for (int ligne = 0; ligne < tiles.GetLength(0); ligne++)
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    var source = tiles[col, ligne];
                    ret[ligne, col] = new Tile(g, col, ligne);
                    ret[ligne, col].Ligne = ligne;
                    ret[ligne, col].Col = col;
                    ret[ligne, col].WordMultiplier = source.WordMultiplier;
                    ret[ligne, col].LetterMultiplier = source.LetterMultiplier;
                    ret[ligne, col].Letter = tiles[col, ligne].Letter;

                }
            Game.IsTransposed = !Game.IsTransposed;

            return ret;
        }

        public static List<Letter> RemoveFromRack(this List<Letter> rack, Letter l)
        {
            var ret = rack;
            var idxLetter = rack.Select(ra=>ra.Char).ToList().IndexOf(l.Char);
            ret.RemoveAt(idxLetter);
            return ret;
        }
    }
}
