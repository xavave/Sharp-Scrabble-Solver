using DawgResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dawg
{
    public static class TileFluentExtentions
    {
        public static Tile Copy(this Tile t, bool transpose = false)
        {
            return new Tile(t.Game, transpose ? t.Col : t.Ligne, transpose ? t.Ligne : t.Col)
            {
                Controlers = transpose ? new Dictionary<char, int>(27) : t.Controlers,
                Letter = t.Letter,
                IsAnchor = transpose ? false : t.IsAnchor,
                LetterMultiplier = t.LetterMultiplier,
                WordMultiplier = t.WordMultiplier,
                AnchorLimit1 = transpose ? 0 : t.AnchorLimit1,
                AnchorLimit2 = transpose ? 0 : t.AnchorLimit2,
                FromJoker = transpose ? false : t.FromJoker
            };
        }
        public static void SetWord(this Tile t, Game g, string word, MovementDirection direction)
        {
            if (t != null)
                t.Letter = g.Alphabet.Find(a => a.Char == char.ToUpper(word[0]));
            foreach (var c in word.Skip(1))
            {
                if (direction == MovementDirection.Across)
                    t = t.SetRightLetter(g, c);
                else
                    t = t.SetDownLetter(g, c);
            }
        }
        public static Tile SetRightLetter(this Tile t, Game g, char c)
        {
            if (t.RightTile != null)
            {
                t.RightTile.Letter = g.Alphabet.Find(a => a.Char == char.ToUpper(c));
                return t.RightTile;
            }
            return null;
        }
        public static Tile SetDownLetter(this Tile t, Game g, char c)
        {
            if (t.DownTile != null)
            {
                t.DownTile.Letter = g.Alphabet.Find(a => a.Char == char.ToUpper(c));
                return t.DownTile;
            }
            return null;
        }

        public static Tile[,] Transpose(this Tile[,] tiles)
        {
            var ret = new Tile[tiles.GetLength(0),tiles.GetLength(1)];
            foreach (var t in tiles)
            {
                ret[t.Col, t.Ligne] = t;
            }

            return ret;
        }
    }
    public interface VTile
    {
        int Ligne { get; set; }
        int Col { get; set; }
        Letter Letter { get; set; }
    }
    public class Tile : VTile
    {
        public Game Game { get; }
        bool isAnchor = false;
        public Tile(Game g, int ligne, int col)
        {
            Game = g;
            Ligne = ligne;
            Col = col;
            Letter = new Letter() { Char = char.MinValue };
            LetterMultiplier = 1;
            WordMultiplier = 1;
            AnchorLimit1 = AnchorLimit2 = 0;
            IsAnchor = (UpTile != null && !UpTile.IsEmpty) || (DownTile != null && !DownTile.IsEmpty) || (RightTile != null && !RightTile.IsEmpty) || (LeftTile != null && !LeftTile.IsEmpty);
        }

        public bool FromJoker { get; set; } = false;
        public Dictionary<char, int> Controlers { get; set; } = new Dictionary<char, int>(27);
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; set; }
        public int AnchorLimit2 { get; set; }
        public int AnchorLimit1 { get; set; }
        public bool IsAnchor { get => isAnchor; set => isAnchor = value; }


        public bool IsEmpty
        {
            get
            {
                return this == null || Letter == null || Letter.Char == char.MinValue;
            }
        }

        public Tile LeftTile
        {
            get
            {
                if (Col > 0)
                    return Game.Grid[this.Ligne, this.Col - 1];
                return null;
            }
        }
        public Tile RightTile
        {
            get
            {
                if (Col < Game.BoardSize - 1)
                    return Game.Grid[this.Ligne, this.Col + 1];
                return null;
            }
        }
        public Tile DownTile
        {
            get
            {
                if (Ligne < Game.BoardSize - 1)
                    return Game.Grid[this.Ligne + 1, this.Col];
                return null;
            }
        }
        public Tile UpTile
        {
            get
            {
                if (Ligne > 0)
                    return Game.Grid[this.Ligne - 1, this.Col];
                return null;
            }
        }
        public override string ToString()
        {
            return $"{this.Letter.Char} [{Game.Alphabet[Col]}{ Ligne}] {(isAnchor ? " Ancre" : "")}";
        }
    }

}