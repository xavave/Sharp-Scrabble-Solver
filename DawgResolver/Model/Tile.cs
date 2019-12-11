using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DawgResolver.Model
{

    public interface VTile
    {
        bool IsValidated { get; set; }
        TileType TileType { get; }
        int Ligne { get; set; }
        int Col { get; set; }
        Letter Letter { get; set; }
        int LetterMultiplier { get; set; }
        int WordMultiplier { get; set; }
        int AnchorLeftMinLimit { get; set; }
        int AnchorLeftMaxLimit { get; set; }
        bool IsAnchor { get; }
        Dictionary<int, int> Controlers { get; set; }
        bool FromJoker { get; set; }
        bool IsEmpty { get; }
        VTile LeftTile { get; }
        VTile RightTile { get; }
        VTile UpTile { get; }
        VTile DownTile { get; }
        string Serialize { get; }
    }
    public class Tile : VTile
    {
        public System.Drawing.Color Background { get; set; }
        public bool IsValidated { get; set; } = false;

        public Game Game { get; }

        public Tile(Game g, int ligne, int col)
        {
            Game = g;
            Ligne = ligne;
            Col = col;
            Letter = new Letter() { Char = char.MinValue };
            LetterMultiplier = 1;
            WordMultiplier = 1;
            AnchorLeftMinLimit = AnchorLeftMaxLimit = 0;

        }
        public Tile(Game g, char c, int col) : this(g, Game.Alphabet.IndexOf(Game.Alphabet.Find(l => l.Char == c)), col)
        {

        }

        internal void Clear()
        {
            //IsAnchor = false;
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
        public bool IsAnchor
        {
            get
            {
                return (IsEmpty && TileType == TileType.Center) || (IsEmpty && (
                    (UpTile != null && !UpTile.IsEmpty) ||
                    (DownTile != null && !DownTile.IsEmpty) ||
                    (RightTile != null && !RightTile.IsEmpty) ||
                    (LeftTile != null && !LeftTile.IsEmpty)));
            }
        }
        public TileType TileType
        {
            get
            {
                if (Ligne == (int)(Game.BoardSize/2) && Col == (int)(Game.BoardSize / 2))
                    return TileType.Center;
                else if (WordMultiplier == 2) return TileType.DoubleWord;
                else if (LetterMultiplier == 2) return TileType.DoubleLetter;
                else if (LetterMultiplier == 3) return TileType.TripleLetter;
                else if (WordMultiplier == 3) return TileType.TripleWord;
                return TileType.Regular;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this == null || Letter==null || !Letter.HasValue();
            }
        }

        public VTile LeftTile
        {
            get
            {
                if (Col > 0)
                    return Game.Grid[this.Ligne, this.Col - 1];
                return null;
            }
        }
        public VTile RightTile
        {
            get
            {
                if (Col < Game.BoardSize - 1)
                    return Game.Grid[this.Ligne, this.Col + 1];
                return null;
            }
        }
        public VTile DownTile
        {
            get
            {
                if (Ligne < Game.BoardSize - 1)
                    return Game.Grid[this.Ligne + 1, this.Col];
                return null;
            }
        }
        public VTile UpTile
        {
            get
            {
                if (Ligne > 0)
                    return Game.Grid[this.Ligne - 1, this.Col];
                return null;
            }
        }

        public string Serialize
        {
            get
            {
                return $"T{Ligne};{Col};{LetterMultiplier};{WordMultiplier};{FromJoker};{IsValidated}";
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