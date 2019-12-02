using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Dawg.Word;

namespace Dawg
{

    public class Board
    {
        public static Dictionnaire Dico { get; set; } = LoadDico();

        private static Dictionnaire LoadDico()
        {
            var dic = new Dictionnaire();
            dic.ChargerFichierDAWG();


            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("dico_dawg.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();
                dic.Mots = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                int.Parse(dic.Mots[0].Replace("NBMOTS : ", ""));
                dic.NombreNoeuds = dic.Mots.Count - 2;// int.Parse(dic[1].Replace("NBNOEUDS : ", ""));
                var Dictionary = new long[28, dic.NombreNoeuds + 2];
                for (int lineIdx = 2; lineIdx < dic.NombreNoeuds; lineIdx++)
                {
                    var line = dic.Mots[lineIdx];
                    //Remplacement du marqueur de noeud terminal "#" utilisé à la place de "[1" lors de la compilation du dico
                    //Ce remplacement avait deux objectifs : diminuer la taille du fichier tout en améliorant la lisibilité du dawg
                    if (line == "#")
                        line = line.Replace("#", "[1");
                    else
                        line = line.Replace("#", "-[1");

                    // chaque ligne est décomposée pour en extraire les arcs sortants du noeud
                    // un arc est noté par une lettre de A à Z
                    // et par la référence du noeud atteint par l'arc
                    // les arcs sont séparés par le marqueur "-"

                    foreach (var arc in line.Split('-'))
                    {
                        var partie = arc;
                        var letterCode = (int)partie[0] - Dictionnaire.AscShift;
                        var node = int.Parse(partie.Substring(1));
                        Dictionary[letterCode, lineIdx - 1] = node;
                    }

                }
            }

            return dic;
        }

        public static List<Letter> Alphabet { get; set; } = new List<Letter>()
        {
            new Letter('A',1,9),
            new Letter('B',1,2),
            new Letter('C',1,2),
            new Letter('D',1,5),
            new Letter('E',1,13),
            new Letter('F',1,2),
            new Letter('G',1,3),
            new Letter('H',1,4),
            new Letter('I',1,8),
            new Letter('J',8,1),
            new Letter('K',10,1),
            new Letter('L',1,4),
            new Letter('M',1,2),
            new Letter('N',1,5),
            new Letter('O',1,8),
            new Letter('P',1,2),
            new Letter('Q',1,1),
            new Letter('R',1,6),
            new Letter('S',1,5),
            new Letter('T',1,7),
            new Letter('U',1,4),
            new Letter('V',1,2),
            new Letter('W',10,1),
            new Letter('X',10,1),
            new Letter('Y',10,1),
            new Letter('Z',10,1),

        };


        public static int AnchorCount { get; set; } = 0;
        public const int BoardSize = 15;
        private static Tile[,] grid = new Tile[BoardSize, BoardSize];

        public static Tile[,] Grid
        {
            get => grid;
            set
            {
                if (grid == null) grid = value;
            }
        }
    }
    public class Word
    {
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public int Points
        {
            get
            {
                int point = 0;
                if (!IsAllowed) return 0;
                int nbMultiX2 = 0;
                int nbMultiX3 = 0;
                foreach (var t in Tiles)
                {
                    switch (t.TileType)
                    {
                        case TileType.DoubleLetter: point += t.Letter.Value * 2; break;
                        case TileType.TripleLetter: point += t.Letter.Value * 3; break;
                        default: point += t.Letter.Value; break;
                    }
                }
                nbMultiX2 = Tiles.Count(ti => ti.TileType == TileType.DoubleWord);
                nbMultiX3 = Tiles.Count(ti => ti.TileType == TileType.TripleWord);
                if (nbMultiX2 > 0)
                    point *= 2 * nbMultiX2;
                if (nbMultiX3 > 0)
                    point *= 3 * nbMultiX3;

                return point;

            }
        }


        public bool IsAllowed
        {
            get
            {
                return Board.Dico.MotAdmis(new string(this.Tiles.Select(cw => cw.Letter.Char).ToArray()));
            }

        }
        public class Tile
        {



            public Tile()
            {
            }
            public Dictionary<char, int> PossibleLetterPoints { get; set; } = new Dictionary<char, int>();
            public int XLoc { get; set; }
            public int YLoc { get; set; }
            public TileType TileType { get; set; }
            public Letter Letter { get; set; } = new Letter() { Char = char.MinValue };
            public int PrefixMinSize { get; set; } = 0;
            public int PrefixMaxSize { get; set; } = 0;
            public bool IsAnchor
            {
                get
                {
                    return !UpTile.IsEmpty || !DownTile.IsEmpty || !RightTile.IsEmpty || !LeftTile.IsEmpty;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    return Letter.Char == char.MinValue;
                }
            }

            public Tile LeftTile
            {
                get
                {
                    if (XLoc > 0)
                        return Board.Grid[this.XLoc - 1, this.YLoc];
                    return new Tile();
                }
            }
            public Tile RightTile
            {
                get
                {
                    if (XLoc < Board.BoardSize - 1)
                        return Board.Grid[this.XLoc + 1, this.YLoc];
                    return new Tile();
                }
            }
            public Tile DownTile
            {
                get
                {
                    if (YLoc < Board.BoardSize - 1)
                        return Board.Grid[this.XLoc, this.YLoc + 1];
                    return new Tile();
                }
            }
            public Tile UpTile
            {
                get
                {
                    if (XLoc > 0)
                        return Board.Grid[this.XLoc - 1, this.YLoc];
                    return new Tile();
                }
            }
        }
    }
}