namespace Scrabble2018
{


    //public class BoardTiles
    //{
    //    //private const TileType __ = TileType.Regular;
    //    //private const TileType DL = TileType.DoubleLetter;
    //    //private const TileType TL = TileType.TripleLetter;
    //    //private const TileType DW = TileType.DoubleWord;
    //    //private const TileType TW = TileType.TripleWord;
    //    //private const TileType ST = TileType.Center;

    //    // Reference BoardTiles and utility
    //    // Reference is for cancelling used colors
    //    //public static TileType[,] Placement =
    //    //{
    //    //    {TW,__,__,DL,__,__,__,TW,__,__,__,DL,__,__,TW},
    //    //    {__,DW,__,__,__,TL,__,__,__,TL,__,__,__,DW,__},
    //    //    {__,__,DW,__,__,__,DL,__,DL,__,__,__,DW,__,__},
    //    //    {DL,__,__,DW,__,__,__,DL,__,__,__,DW,__,__,DL},
    //    //    {__,__,__,__,DW,__,__,__,__,__,DW,__,__,__,__ },
    //    //    {__,TL,__,__,__,TL,__,__,__,TL,__,__,__,TL,__},
    //    //    {__,__,DL,__,__,__,DL,__,DL,__,__,__,DL,__,__},
    //    //    //midDLe
    //    //    {TW,__,__,DL,__,__,__,ST,__,__,__,DL,__,__,TL},
    //    //    //midDLe
    //    //    {__,__,DL,__,__,__,DL,__,DL,__,__,__,DL,__,__},
    //    //    {__,TL,__,__,__,TL,__,__,__,TL,__,__,__,TL,__},
    //    //    {__,__,__,__,DW,__,__,__,__,__,DW,__,__,__,__ },
    //    //    {DL,__,__,DW,__,__,__,DL,__,__,__,DW,__,__,DL},
    //    //    {__,__,DW,__,__,__,DL,__,DL,__,__,__,DW,__,__},
    //    //    { __,DW,__,__,__,TL,__,__,__,TL,__,__,__,DW,__},
    //    //    {TW,__,__,DL,__,__,__,TW,__,__,__,DL,__,__,TW}
    //    //};

    //    public TileType[,] PlaceInUse;
    //    private bool[,] Visited;

    //    public BoardTiles()
    //    {
    //        //PlaceInUse = Placement;
    //        Visited = new bool[15, 15];
    //    }

    //    //public int WordMultiplier(Tile t)
    //    //{
    //    //    switch (t.TileType)
    //    //    {
    //    //        case TileType.TripleWord:
    //    //            Visited[t.Ligne, t.Col] = true;
    //    //            return 3;
    //    //        case TileType.DoubleWord:
    //    //            Visited[t.Ligne, t.Col] = true;
    //    //            return 2;
    //    //        default:
    //    //            return 1;
    //    //    }
    //    //}

    //    //public int LetterMultiplier(int i, int j)
    //    //{
    //    //    switch (PlaceInUse[i, j])
    //    //    {
    //    //        case TileType.TripleLetter:
    //    //            Visited[i, j] = true;
    //    //            return 3;
    //    //        case TileType.DoubleLetter:
    //    //            Visited[i, j] = true;
    //    //            return 2;
    //    //        default:
    //    //            return 1;
    //    //    }
    //    //}

    //    //public void ApplyVisited()
    //    //{
    //    //    for (int i = 0; i < Visited.GetLength(0); i++)
    //    //    {
    //    //        for (int j = 0; j < Visited.GetLength(1); j++)
    //    //        {
    //    //            if (Visited[i, j])
    //    //            {
    //    //                PlaceInUse[i, j] = TileType.Regular;
    //    //                Visited[i, j] = false;
    //    //            }
    //    //        }
    //    //    }

    //    //}

    //    //public void CleanVisited()
    //    //{
    //    //    for (int i = 0; i < Visited.GetLength(0); i++)
    //    //    {
    //    //        for (int j = 0; j < Visited.GetLength(1); j++)
    //    //        {
    //    //            Visited[i, j] = false;
    //    //        }
    //    //    }
    //    //}

    //    public static SolidColorBrush DetermineColor(VTile t)
    //    {
    //        switch (t.TileType)
    //        {
    //            case TileType.TripleWord:
    //                return Brushes.OrangeRed;
    //            case TileType.DoubleWord:
    //                return Brushes.Coral;
    //            case TileType.DoubleLetter:
    //                return Brushes.LightSkyBlue;
    //            case TileType.TripleLetter:
    //                return Brushes.MediumBlue;
    //            case TileType.Center:
    //                return Brushes.Gold;
    //            default:
    //                return Brushes.Bisque;
    //        }
    //    }


    //}
}
