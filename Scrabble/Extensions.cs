using DawgResolver;
using Scrabble.Core;
using Scrabble.Core.Tile;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scrabble
{
    public static class Extensions
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public static TileType[,] specialTilePositions { get; set; }
        //public static VirtualTile[,] TileClone(this ScrabbleTile[,] st, ScrabbleForm scrabbleForm)
        //{
        //    var copy = new VirtualTile[ScrabbleForm.BOARD_WIDTH, ScrabbleForm.BOARD_HEIGHT];
        //    for (int x = 1; x <= ScrabbleForm.BOARD_WIDTH; x++)
        //    {
        //        for (int y = 1; y <= ScrabbleForm.BOARD_HEIGHT; y++)
        //        {
        //            copy[x - 1, y - 1] = new VirtualTile(scrabbleForm);
        //            copy[x - 1, y - 1].TileType = st[x - 1, y - 1].TileType;
        //            copy[x - 1, y - 1].TileInPlay = st[x - 1, y - 1].TileInPlay;
        //            copy[x - 1, y - 1].Text = st[x - 1, y - 1].Text;
        //            copy[x - 1, y - 1].XLoc = st[x - 1, y - 1].XLoc;
        //            copy[x - 1, y - 1].YLoc = st[x - 1, y - 1].YLoc;
                   
        //        }
        //    }
        //    return copy;
        //}

    }
}
