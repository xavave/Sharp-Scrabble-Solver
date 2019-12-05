using DawgResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DawgResolver
{
 
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
        public Tile(int ligne, int col)
        {
            Ligne = ligne;
            Col = col;
            Letter = new Letter() { Char = char.MinValue };
            LetterMultiplier = 1;
            WordMultiplier = 1;
            AnchorLeftMinLimit = AnchorLeftMaxLimit = 0;
            IsAnchor = (UpTile != null && !UpTile.IsEmpty) || (DownTile != null && !DownTile.IsEmpty) || (RightTile != null && !RightTile.IsEmpty) || (LeftTile != null && !LeftTile.IsEmpty);
        }

        internal void Clear()
        {
            IsAnchor = false;
            AnchorLeftMinLimit = 0;
            AnchorLeftMaxLimit = 0;
            Controlers = new Dictionary<int, int>(27);
        }

        public bool FromJoker { get; set; } = false;
        public Dictionary<int, int> Controlers { get; set; } = new Dictionary<int, int>(27);
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; set; }
        public int AnchorLeftMaxLimit { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public bool IsAnchor { get => isAnchor; set => isAnchor = value; }


        public bool IsEmpty
        {
            get
            {
                return this == null || !Letter.HasValue();
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
            //var c = $"{Letter} min / max:{AnchorLeftMinLimit};{AnchorLeftMaxLimit}";
            //var cont = "";
            //foreach (var co in Controlers)
            //    cont += $"{Game.AlphabetAvecJoker[co.Key + Dictionnaire.AscShiftBase0]}{co.Value};";
            //return $"{ c} [{Game.AlphabetAvecJoker[Ligne]}{ Col + 1}] {(isAnchor ? "*" : "")} {cont}";
            return $"{Letter.Char}";
        }
    }

}