using System.Collections.Generic;
using System.Linq;

namespace DawgResolver.Model
{

    public interface IBaseTile
    {
        string Name { get; set; }
        TileType TileType { get; }
        int Ligne { get; set; }
        int Col { get; set; }
        Letter Letter { get; set; }
        int LetterMultiplier { get; set; }
        int WordMultiplier { get; set; }
        int AnchorLeftMinLimit { get; set; }
        int AnchorLeftMaxLimit { get; set; }
        bool IsAnchor { get; }
        IDictionary<int, int> Controlers { get; }
        //bool FromJoker { get; set; }
        bool IsEmpty { get; }
        int WordIndex { get; set; }
        bool IsValidated { get; set; }
        IExtendedTile LeftTile { get; }
        IExtendedTile RightTile { get; }
        IExtendedTile UpTile { get; }
        IExtendedTile DownTile { get; }
        IExtendedTile WordMostRightTile { get; }
        IExtendedTile WordMostLeftTile { get; }
        IExtendedTile WordLowerTile { get; }
        IExtendedTile WordUpperTile { get; }
    }
    public interface IExtendedTile : IBaseTile
    {
        bool? IsPlayedByPlayer1 { get; }
        string Serialize { get; }
        System.Drawing.Color BackColor { get; set; }

        Word GetWordFromTile(MovementDirection currentWordDirection);
        void SetWord(string text, MovementDirection direction, bool validate);
        void CopyControllers(IDictionary<int, int> controlers);
        IExtendedTile Copy(bool transpose = false);
        void SetBackColorFrom(IExtendedTile t);
        void SetBackColorFromInnerTile();
        void SetBackColorFromInnerLetterType();
    }

    public class GenericTile : IExtendedTile
    {
        public void SetWord(string word, MovementDirection direction, bool Validate = false)
        {
            
        }
        
        private IExtendedTile SetRightOrDownLetter(char c, Player p, bool validate, MovementDirection direction)
        {
            var nextTile = direction == MovementDirection.Down ? this.DownTile : this.RightTile;

            if (nextTile != null)
            {
                if (nextTile.IsEmpty) nextTile.IsValidated = validate;
                nextTile.Letter = Game.DefaultInstance.Resolver.Find(char.ToUpper(c));

                if (char.IsLower(c)) nextTile.Letter.LetterType = LetterType.Joker;

                if (nextTile.Letter.LetterType == LetterType.Joker)
                    p.Rack.Remove(Game.DefaultInstance.Resolver.Alphabet.ElementAt(26));
                else
                    p.Rack.Remove(Game.DefaultInstance.Resolver.Alphabet.First(a => a.Char == char.ToUpper(c)));
                return nextTile;
            }
            return nextTile;
        }


        public Word GetWordFromTile(MovementDirection direction)
        {
            return null;
        }
        public bool IsValidated { get; set; } = false;
        public GenericTile(int? ligne = null, int? col = null, bool? isPlayedByPlayer1 = null, bool fromJoker = false)
        {
            Controlers = new Dictionary<int, int>(27);

            Ligne = ligne ?? Game.DefaultInstance.BoardCenter;
            Col = col ?? Game.DefaultInstance.BoardCenter;
            Letter = new Letter() { Char = EmptyChar, LetterType = fromJoker ? LetterType.Joker : LetterType.Regular };
            LetterMultiplier = 1;
            WordMultiplier = 1;
            AnchorLeftMinLimit = AnchorLeftMaxLimit = 0;

            if (isPlayedByPlayer1 != null) IsPlayedByPlayer1 = isPlayedByPlayer1;
            else
            {
                if (Game.DefaultInstance.CurrentPlayer.Name == Game.DefaultInstance.Player1.Name)
                    IsPlayedByPlayer1 = true;
                else if (Game.DefaultInstance.CurrentPlayer.Name == Game.DefaultInstance.Player2.Name)
                    IsPlayedByPlayer1 = false;
                else
                    IsPlayedByPlayer1 = null;
            }

        }

        public IExtendedTile Copy(bool transpose = false)
        {
            IExtendedTile newT = this;
            if (transpose) newT = Game.DefaultInstance.Grid[this.Col, this.Ligne];

            IExtendedTile nTile = new GenericTile(newT.Ligne, newT.Col)
            {
                Letter = this.Letter,
                LetterMultiplier = this.LetterMultiplier,
                WordMultiplier = this.WordMultiplier,
                AnchorLeftMinLimit = this.AnchorLeftMinLimit,
                AnchorLeftMaxLimit = this.AnchorLeftMaxLimit,
                IsValidated = this.IsValidated,
            };
            nTile.CopyControllers(this.Controlers);
            return nTile;
        }
        //public Tile(Resolver r, char c, int col) : this(r, c.FindIndex(r), col)
        //{
        //}

        //internal void Clear()
        //{
        //    //IsAnchor = false;
        //    AnchorLeftMinLimit = 0;
        //    AnchorLeftMaxLimit = 0;
        //    Controlers.Clear();
        //}

        public void CopyControllers(IDictionary<int, int> source)
        {
            foreach (var k in source.Keys)
            {
                this.Controlers[k] = source[k];
            }
        }
        public bool? IsPlayedByPlayer1 { get; }
        //public bool FromJoker { get; set; } = false;
        public IDictionary<int, int> Controlers { get; }
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; set; }
        public int AnchorLeftMaxLimit { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public bool IsAnchor => (IsEmpty && TileType == TileType.Center)
                || (IsEmpty && (
                    (UpTile != null && !UpTile.IsEmpty) ||
                    (DownTile != null && !DownTile.IsEmpty) ||
                    (RightTile != null && !RightTile.IsEmpty) ||
                    (LeftTile != null && !LeftTile.IsEmpty))
                );
        public TileType TileType
        {
            get
            {

                if (Ligne == Game.DefaultInstance.BoardCenter && Col == Game.DefaultInstance.BoardCenter)
                    return TileType.Center;
                else if (WordMultiplier == 2) return TileType.DoubleWord;
                else if (LetterMultiplier == 2) return TileType.DoubleLetter;
                else if (LetterMultiplier == 3) return TileType.TripleLetter;
                else if (WordMultiplier == 3) return TileType.TripleWord;
                return TileType.Regular;
            }
        }

        public bool IsEmpty => this == null || Letter == null || !Letter.HasValue() || Letter.Char == Game.EmptyChar;
        public IExtendedTile LeftTile => Col > 0 ? Game.DefaultInstance.Grid[this.Ligne, this.Col - 1] : null;
        public IExtendedTile RightTile => Col < Game.DefaultInstance.BoardSize - 1 ? Game.DefaultInstance.Grid[this.Ligne, this.Col + 1] : null;
        public IExtendedTile DownTile => Ligne < Game.DefaultInstance.BoardSize - 1 ? Game.DefaultInstance.Grid[this.Ligne + 1, this.Col] : null;
        public IExtendedTile UpTile => Ligne > 0 ? Game.DefaultInstance.Grid[this.Ligne - 1, this.Col] : null;
        public string Serialize => $"T{Ligne};{Col};{LetterMultiplier};{WordMultiplier};{(Letter.LetterType == LetterType.Joker ? "true" : "false")};{IsValidated};{Letter?.Char};{IsPlayedByPlayer1}";
        public char EmptyChar => ' ';

        public IExtendedTile WordMostRightTile
        {
            get
            {
                IExtendedTile t = this;

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
        public IExtendedTile WordMostLeftTile
        {
            get
            {
                IExtendedTile t = this;
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
        public IExtendedTile WordLowerTile
        {
            get
            {
                IExtendedTile t = this;
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
        public IExtendedTile WordUpperTile
        {
            get
            {
                IExtendedTile t = this;
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
        public int WordIndex { get; set; } = 0;
        public string Name { get; set; }
        public System.Drawing.Color BackColor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override string ToString()
        {
            //var c = $"{Letter} min / max:{AnchorLeftMinLimit};{AnchorLeftMaxLimit}";
            //var cont = "";
            //foreach (var co in Controlers)
            //    cont += $"{Game.AlphabetAvecJoker[co.Key + Dictionnaire.AscShiftBase0]}{co.Value};";
            //return $"{ c} [{Game.AlphabetAvecJoker[Ligne]}{ Col + 1}] {(isAnchor ? "*" : "")} {cont}";
            return $"{this?.Letter?.Char}";
        }

        public void SetBackColorFrom(IExtendedTile t)
        {
            //rien
        }

        public void SetBackColorFromInnerTile()
        {
            SetBackColorFrom(this);
        }

        public void SetBackColorFromInnerLetterType()
        {
            throw new System.NotImplementedException();
        }

        //public void Initialize()
        //{
        //}


    }

}