using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scrabble2018.Model
{
    public class TextBoxTile : TextBox, VTile
    {
        private Letter letter;
        private int ligne;
        private int col;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public TextBoxTile(VTile t)
        {
            TileType = t.TileType;
            Background = UpdateColor(t);
            Col = t.Col;
            Ligne = t.Ligne;
            Letter = t.Letter;
            WordMultiplier = t.IsEmpty ? t.WordMultiplier : 1;
            LetterMultiplier = t.IsEmpty ? t.LetterMultiplier : 1;
        }
        public SolidColorBrush UpdateColor(VTile t)
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

        public DawgResolver.Model.Word GetWordFromTile(MovementDirection direction)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
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

        public VTile LeftTile { get; }

        public VTile RightTile { get; }

        public VTile UpTile { get; }

        public VTile DownTile { get; }

        public string Serialize => throw new NotImplementedException();

        public bool? IsPlayedByPlayer1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public VTile WordMostRightTile => throw new NotImplementedException();

        public VTile WordMostLeftTile => throw new NotImplementedException();

        public VTile WordLowerTile => throw new NotImplementedException();

        public VTile WordUpperTile => throw new NotImplementedException();
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
