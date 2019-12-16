using System.Collections.Generic;

namespace DawgResolver.Model
{

    public interface VTile
    {

        void Initialize();
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
        bool? IsPlayedByPlayer1 { get; set; }
        VTile LeftTile { get; }
        VTile RightTile { get; }
        VTile UpTile { get; }
        VTile DownTile { get; }

        VTile WordMostRightTile { get; }
        VTile WordMostLeftTile { get; }
        VTile WordLowerTile { get; }
        VTile WordUpperTile { get; }
        string Serialize { get; }
        Word GetWordFromTile(MovementDirection direction);
    }
    public class Tile : VTile
    {
        //public Color Background { get; set; }
        public bool IsValidated { get; set; } = false;

        public Game Game { get; }

        public Tile(Game g, int ligne, int col)
        {
            Game = g;
            Ligne = ligne;
            Col = col;
            Letter = new Letter() { Char = EmptyChar };
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
        public bool? IsPlayedByPlayer1 { get; set; }
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
                if (Ligne == (int)(Game.BoardSize / 2) && Col == (int)(Game.BoardSize / 2))
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
                return this == null || Letter == null || !Letter.HasValue() || Letter.Char == Game.EmptyChar;
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
                return $"T{Ligne};{Col};{LetterMultiplier};{WordMultiplier};{FromJoker};{IsValidated};{Letter?.Char};{IsPlayedByPlayer1}";
            }
        }

        public char EmptyChar { get => ' '; }

        public VTile WordMostRightTile
        {
            get
            {
                VTile t = this;
               
                if (IsEmpty || t.RightTile == null || t.RightTile.IsEmpty)
                    return t;
                else
                    while (t.RightTile != null && !t.RightTile.IsEmpty)
                    {
                        t = t.RightTile;
                    }
                return t;
            }
        }
        public VTile WordMostLeftTile
        {
            get
            {
                VTile t = this;
                if (IsEmpty || t.LeftTile == null || t.LeftTile.IsEmpty)
                    return t;
                else
                    while (t.LeftTile != null && !t.LeftTile.IsEmpty)
                    {
                        t = t.LeftTile;
                    }
                return t;
            }
        }
        public VTile WordLowerTile
        {
            get
            {
                VTile t = this;
                if (IsEmpty || t.DownTile == null || t.DownTile.IsEmpty)
                    return t;
                else
                    while (t.DownTile != null && !t.DownTile.IsEmpty)
                    {
                        t = t.DownTile;
                    }
                return t;
            }
        }
        public VTile WordUpperTile
        {
            get
            {
                VTile t = this;
                if (IsEmpty || t.UpTile == null || t.UpTile.IsEmpty)
                    return t;
                else
                    while (t.UpTile != null && !t.UpTile.IsEmpty)
                    {
                        t = t.UpTile;
                    }
                return t;
            }
        }

        public Word GetWordFromTile(MovementDirection direction)
        {
            var word = new Word(Game);
            VTile tile = this;
            string text = "";
            int? points = 0;
            int wordmulti = 1;
            if (direction == MovementDirection.Across)
            {
                var rTile = tile.WordMostRightTile;
                var lTile = tile.WordMostLeftTile;
                var wordLength = rTile.Col - lTile.Col;
                for (int i = lTile.Col; i <= lTile.Col + wordLength; i++)
                {
                    text += Game.Grid[lTile.Ligne, i].Letter.Char;
                }
                word.Text = text;
                word.StartTile = lTile;
            }
            else
            {
                var dTile = tile.WordLowerTile;
                var uTile = tile.WordUpperTile;
                var wordLength = dTile.Ligne - uTile.Ligne;
                for (int i = uTile.Ligne; i <= uTile.Ligne + wordLength; i++)
                {
                    text += Game.Grid[i, tile.Col].Letter.Char;
                }
                word.Text = text;
                word.StartTile = uTile;

            }
            points = points * wordmulti;
            if (points.HasValue)
                word.Points = points.Value;
            word.Direction = direction;

            return word;
        }
        public override string ToString()
        {
            //var c = $"{Letter} min / max:{AnchorLeftMinLimit};{AnchorLeftMaxLimit}";
            //var cont = "";
            //foreach (var co in Controlers)
            //    cont += $"{Game.AlphabetAvecJoker[co.Key + Dictionnaire.AscShiftBase0]}{co.Value};";
            //return $"{ c} [{Game.AlphabetAvecJoker[Ligne]}{ Col + 1}] {(isAnchor ? "*" : "")} {cont}";
            return $"{this?.Letter?.Char}";
        }

        public void Initialize()
        {
        }
    }

}