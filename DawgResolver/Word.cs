using Dawg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    [Serializable]
    public class Word
    {
        public Game Game { get; set; }
        public Word(Game g)
        {
            Game = g;

        }
        public bool Scramble
        {
            get
            {
                return Tiles.Count() == 7;
            }
        }
        public MovementDirection Direction { get; set; }
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public int Points
        {
            get; set;
            //get
            //{
            //    int point = 0;
            //    if (!IsAllowed) return 0;
            //    int nbMultiX2 = 0;
            //    int nbMultiX3 = 0;
            //    foreach (var t in Tiles)
            //    {
            //        switch (t.TileType)
            //        {
            //            case TileType.DoubleLetter: point += t.Letter.Value * 2; break;
            //            case TileType.TripleLetter: point += t.Letter.Value * 3; break;
            //            default: point += t.Letter.Value; break;
            //        }
            //    }
            //    nbMultiX2 = Tiles.Count(ti => ti.TileType == TileType.DoubleWord);
            //    nbMultiX3 = Tiles.Count(ti => ti.TileType == TileType.TripleWord);
            //    if (nbMultiX2 > 0)
            //        point *= 2 * nbMultiX2;
            //    if (nbMultiX3 > 0)
            //        point *= 3 * nbMultiX3;

            //    return point;

            //}
        }
        //public string Text { get; set; }

        public string Text
        {
            get
            {
                return new string(Tiles.Select(t => t.Letter.Char).ToArray());
            }
        }
        public string DisplayText
        {
            get => $"[{Game.Alphabet[Tiles[0].YLoc].Char}{Tiles[0].XLoc + 1}] " + Text + $" ({Points})";
        }
        public bool IsAllowed
        {
            get
            {
                return Game.Dico.MotAdmis(Text);
            }

        }
        public override string ToString()
        {
            return DisplayText;
        }
    }
}
