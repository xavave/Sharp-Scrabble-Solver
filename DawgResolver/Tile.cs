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
        public static Tile Copy(this Tile t)
        {
            return new Tile(t.Game, t.Ligne, t.Col)
            {
                PossibleLetterPoints = t.PossibleLetterPoints,
                Letter = t.Letter,
                IsAnchor = t.IsAnchor,
                LetterMultiplier = t.LetterMultiplier,
                WordMultiplier = t.WordMultiplier,
                AnchorLimit1 = t.AnchorLimit1,
                AnchorLimit2 = t.AnchorLimit2,
                FromJoker = t.FromJoker
            };
        }
    }
    [Serializable]
    public class Tile
    {
        public Game Game { get; }
        private bool isAnchor = false;
        public Tile(Game g, int ligne, int col)
        {
            Game = g;
            Ligne = ligne;
            Col = col;
            IsAnchor = (UpTile != null && !UpTile.IsEmpty) || (DownTile != null && !DownTile.IsEmpty) || (RightTile != null && !RightTile.IsEmpty) || (LeftTile != null && !LeftTile.IsEmpty);
        }
       
        public bool FromJoker { get; set; } = false;
        public Dictionary<char, int> PossibleLetterPoints { get; set; } = new Dictionary<char, int>(27);
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; } = 1;
        public int WordMultiplier { get; set; } = 1;
        public Letter Letter { get; set; } = new Letter() { Char = char.MinValue };
        public int AnchorLimit2 { get; set; } = 0;
        public int AnchorLimit1 { get; set; } = 0;
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