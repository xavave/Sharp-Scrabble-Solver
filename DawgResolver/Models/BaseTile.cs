using System.Collections.Generic;
using System.Linq;

namespace Dawg.Scrabble.Model.Models
{
    public interface IBaseTile : ITileLetterPosition
    {
        string Name { get; set; }
        TileType TileType { get; }

        int LetterMultiplier { get; set; }
        int WordMultiplier { get; set; }
        int AnchorLeftMinLimit { get; set; }
        int AnchorLeftMaxLimit { get; set; }
        bool IsAnchor { get; }
        IDictionary<int, int> Controlers { get; }
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
    public abstract class BaseTile : IExtendedTile
    {
        public void SetWord(Game g, string word, MovementDirection direction, bool Validate = false)
        {
            if (word == "") return;


            this.SetFirstLetter(g.Resolver, word.First(), g.CurrentPlayer, Validate);

            foreach (var c in word.Skip(1))
            {
                this.SetRightOrDownLetter(g.Resolver, c, g.CurrentPlayer, Validate, direction);

            }
        }
        public IExtendedTile SetFirstLetter(Resolver r, char c, Player p, bool validate)
        {
            //IExtendedTile nextTile = null;
            //if (this.Col == r.game.BoardSize - 1)
            //    nextTile = this.LeftTile.RightTile;
            //else nextTile = this.RightTile.LeftTile;

            if (this.IsEmpty) this.IsValidated = validate;

            this.Letter = r.Find(char.ToUpper(c));
            if (char.IsLower(c))
                this.Letter.LetterType = LetterType.Joker;
            if (this.Letter.LetterType == LetterType.Joker)
                p.Rack.Remove(r.Alphabet.ElementAt(26));
            else
                p.Rack.Remove(this.Letter);
            return this;

        }
        private IExtendedTile SetRightOrDownLetter(Resolver r, char c, Player p, bool validate, MovementDirection direction)
        {
            var nextTile = direction == MovementDirection.Down ? this.DownTile : this.RightTile;

            if (nextTile != null)
            {
                if (nextTile.IsEmpty) nextTile.IsValidated = validate;
                nextTile.Letter = r.Find(char.ToUpper(c));

                if (char.IsLower(c)) nextTile.Letter.LetterType = LetterType.Joker;

                if (nextTile.Letter.LetterType == LetterType.Joker)
                    p.Rack.Remove(r.Alphabet.ElementAt(26));
                else
                    p.Rack.Remove(r.Alphabet.First(a => a.Char == char.ToUpper(c)));
                return nextTile;
            }
            return nextTile;
        }


        public Word GetWordFromTile(Game g, MovementDirection direction)
        {
            return null;
        }
        public bool IsValidated { get; set; } = false;
        private Resolver resolver { get; }
        public BaseTile(Resolver r, int ligne, int col, bool? isPlayedByPlayer1 = null, bool fromJoker = false)
        {
            Controlers = new Dictionary<int, int>(27);
            resolver = r;
            Ligne = ligne;
            Col = col;
            Letter = new Letter(r) { Char = EmptyChar, LetterType = fromJoker ? LetterType.Joker : LetterType.Regular };
            LetterMultiplier = 1;
            WordMultiplier = 1;
            AnchorLeftMinLimit = AnchorLeftMaxLimit = 0;

            if (isPlayedByPlayer1 != null) IsPlayedByPlayer1 = isPlayedByPlayer1;
            else
            {
                if (r.game.CurrentPlayer.Name == r.game.Player1.Name)
                    IsPlayedByPlayer1 = true;
                else if (r.game.CurrentPlayer.Name == r.game.Player2.Name)
                    IsPlayedByPlayer1 = false;
                else
                    IsPlayedByPlayer1 = null;
            }

        }

        public abstract IExtendedTile Copy(Resolver r, bool transpose = false);

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
        public int Ligne { get; private set; }
        public int Col { get; private set; }
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

                if (Ligne == resolver.game.BoardCenter && Col == resolver.game.BoardCenter)
                    return TileType.Center;
                else if (WordMultiplier == 2) return TileType.DoubleWord;
                else if (LetterMultiplier == 2) return TileType.DoubleLetter;
                else if (LetterMultiplier == 3) return TileType.TripleLetter;
                else if (WordMultiplier == 3) return TileType.TripleWord;
                return TileType.Regular;
            }
        }

        public bool IsEmpty => this == null || Letter == null || !Letter.HasValue() || Letter.Char == Game.EmptyChar;
        public IExtendedTile LeftTile => Col > 0 ? resolver.game.Grid[this.Ligne, this.Col - 1] : null;
        public IExtendedTile RightTile => Col < resolver.game.BoardSize - 1 ? resolver.game.Grid[this.Ligne, this.Col + 1] : null;
        public IExtendedTile DownTile => Ligne < resolver.game.BoardSize - 1 ? resolver.game.Grid[this.Ligne + 1, this.Col] : null;
        public IExtendedTile UpTile => Ligne > 0 ? resolver.game.Grid[this.Ligne - 1, this.Col] : null;
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

        public override string ToString()
        {
            //var c = $"{Letter} min / max:{AnchorLeftMinLimit};{AnchorLeftMaxLimit}";
            //var cont = "";
            //foreach (var co in Controlers)
            //    cont += $"{Game.AlphabetAvecJoker[co.Key + Dictionnaire.AscShiftBase0]}{co.Value};";
            //return $"{ c} [{Game.AlphabetAvecJoker[Ligne]}{ Col + 1}] {(isAnchor ? "*" : "")} {cont}";
            return $"{this?.Letter?.Char}";
        }

        public void SetTilePos(int ligne, int col)
        {
            Ligne = ligne;
            Col = col;

        }

        //public void Initialize()
        //{
        //}


    }

}