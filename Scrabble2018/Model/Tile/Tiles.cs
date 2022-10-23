using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using DawgResolver.Model;

namespace Scrabble2018.Model
{
    public class TextBoxTile : TextBox, IBaseTile
    {
        private Letter letter;
        private int ligne;
        private int col;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public TextBoxTile(IExtendedTile t)
        {
            TileType = t.TileType;
            Background = UpdateColor(t);
            Col = t.Col;
            Ligne = t.Ligne;
            Letter = t.Letter;
            WordMultiplier = t.IsEmpty ? t.WordMultiplier : 1;
            LetterMultiplier = t.IsEmpty ? t.LetterMultiplier : 1;
        }
        public SolidColorBrush UpdateColor(IExtendedTile t)
        {
            switch (t.TileType)
            {
                case TileType.TripleWord:
                    return Brushes.OrangeRed;
                case TileType.DoubleWord:
                    return Brushes.Coral;
                case TileType.DoubleLetter:
                    return Brushes.LightSkyBlue;
                case TileType.TripleLetter:
                    return Brushes.MediumBlue;
                case TileType.Center:
                    return Brushes.Gold;
                default:
                    return Brushes.Bisque;
            }
        }
        //public void Initialize()
        //{
        //    throw new NotImplementedException();
        //}

        public DawgResolver.Model.Word GetWordFromTile(DawgResolver.Model.Game g, MovementDirection direction)
        {
            var word = new DawgResolver.Model.Word(g);
            IExtendedTile tile = this;
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
                    text += g.Grid[lTile.Ligne, i].Letter.Char;
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
                    text += g.Grid[i, tile.Col].Letter.Char;
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

        public void CopyControllers(Dictionary<int, int> source)
        {
            throw new NotImplementedException();
        }

        public int Ligne
        {
            get => ligne; set { ligne = value; OnPropertyChanged(); }
        }
        public int Col { get => col; set { col = value; OnPropertyChanged(); } }
        public Letter Letter
        {
            get => letter; set
            {
                letter = value;
                OnPropertyChanged();
            }
        }
        public TileType TileType { get; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public int AnchorLeftMaxLimit { get; set; }
        public bool IsAnchor { get; set; }
        public Dictionary<int, int> Controlers { get; set; }
        public bool FromJoker { get; set; }
        public bool IsEmpty { get; }
        public bool IsValidated { get; set; }

        public IExtendedTile LeftTile { get; }

        public IExtendedTile RightTile { get; }

        public IExtendedTile UpTile { get; }

        public IExtendedTile DownTile { get; }

        public string Serialize { get; }

        public bool? IsPlayedByPlayer1 { get; set; }

        public IExtendedTile WordMostRightTile { get; }

        public IExtendedTile WordMostLeftTile { get; }

        public IExtendedTile WordLowerTile { get; }

        public IExtendedTile WordUpperTile { get; }

        public int WordIndex { get; set; }
    }
    //public class Tile : IComparable
    //{
    //    // Tile data
    //    private char tileChar;

    //    public char TileChar
    //    {
    //        get { return tileChar; }
    //        set { tileChar = value; }
    //    }
    //    private int tileScore;

    //    public int TileScore
    //    {
    //        get { return tileScore; }
    //        set { tileScore = value; }
    //    }

    //    public Tile(char c, int s)
    //    {
    //        tileChar = c;
    //        tileScore = s;
    //    }

    //    public int CompareTo(object obj)
    //    {
    //        if( obj == null ) return 1;

    //        Tile OtherTile = obj as Tile;
    //        if( OtherTile != null )
    //            return this.TileChar.CompareTo(OtherTile.TileChar);
    //        else
    //            throw new ArgumentException("Tiles Comparison Exception");
    //    }
    //}
}
