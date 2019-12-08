﻿using DawgResolver;
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

        public static VTile Copy(this VTile t, Game g, VTile[,] grid, bool transpose = false)
        {
            VTile newT = t;
            if (transpose) newT = grid[t.Col, t.Ligne];
           
            return new Tile(g, newT.Ligne,newT.Col)
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

        public static string String(this List<Letter> lst)
        {
            return new string(lst.Select(c => c.Char).ToArray());
        }
        public static void SetWord(this VTile t, Player p, string word, MovementDirection direction, bool Validate = false)
        {

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
            if (t != null && t.IsEmpty)
            {
                if (t.IsEmpty)
                    t.IsValidated = Validate;
                t.Letter = Game.Alphabet.Find(a => a.Char == char.ToUpper(c));
                if (char.IsLower(c))
                    t.FromJoker = true;
                if (t.FromJoker)
                    p.Rack.Remove(Game.AlphabetAvecJoker[26]);
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
                    t.FromJoker = true;
                if (t.RightTile.FromJoker)
                    p.Rack.Remove(Game.AlphabetAvecJoker[26]);
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
                    t.FromJoker = true;
                if (t.DownTile.FromJoker)
                    p.Rack.Remove(Game.AlphabetAvecJoker[26]);
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
                    ret[ligne, col].Letter = tiles[col, ligne].Letter;

                }
            Game.IsTransposed = !Game.IsTransposed;

            return ret;
        }
    }
}
