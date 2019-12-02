using DawgResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dawg
{


    [Serializable]
    public class Tile
    {
        public Game Game { get; }
        private bool isAnchor = false;
        public Tile(Game g, int i, int j)
        {
            Game = g;
            XLoc = i;
            YLoc = j;
            IsAnchor = (UpTile != null && !UpTile.IsEmpty) || (DownTile != null && !DownTile.IsEmpty) || (RightTile != null && !RightTile.IsEmpty) || (LeftTile != null && !LeftTile.IsEmpty);
        }


        public Dictionary<char, int> PossibleLetterPoints { get; set; } = new Dictionary<char, int>(27);
        public int XLoc { get; set; }
        public int YLoc { get; set; }
        public TileType TileType { get; set; }
        public Letter Letter { get; set; } = new Letter() { Char = char.MinValue };
        public int PrefixMinSize { get; set; } = 0;
        public int PrefixMaxSize { get; set; } = 0;
        public bool IsAnchor { get => isAnchor; set => isAnchor = value; }


        public bool IsEmpty
        {
            get
            {
                return this == null || Letter.Char == char.MinValue;
            }
        }

        public Tile LeftTile
        {
            get
            {
                if (XLoc > 0)
                    return Game.Grid[this.XLoc - 1, this.YLoc];
                return null;
            }
        }
        public Tile RightTile
        {
            get
            {
                if (XLoc < Game.BoardSize - 1)
                    return Game.Grid[this.XLoc + 1, this.YLoc];
                return null;
            }
        }
        public Tile DownTile
        {
            get
            {
                if (YLoc < Game.BoardSize - 1)
                    return Game.Grid[this.XLoc, this.YLoc + 1];
                return null;
            }
        }
        public Tile UpTile
        {
            get
            {
                if (XLoc > 0)
                    return Game.Grid[this.XLoc - 1, this.YLoc];
                return null;
            }
        }
        public override string ToString()
        {
            return this.Letter.Char.ToString() + "[" + XLoc.ToString() + "," + YLoc.ToString() + "]";
        }
    }

}