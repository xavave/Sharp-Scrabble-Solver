using Dawg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    [Serializable]
    public class Game
    {
        public void PrintGrid(Tile[,] Tiles, bool printAnchor)
        {
            var txt = "";
            Debug.WriteLine("123456789ABCDEF");
            for (int y = 0; y < Game.BoardSize; y++)
            {
                for (int x = 0; x < Game.BoardSize; x++)
                {

                    var tile = Grid[x, y];
                    if (printAnchor)
                    {
                        txt = tile.IsAnchor ? "@" : tile.IsEmpty ? "0" : tile.Letter.ToString();
                    }
                    else
                    {
                        txt = tile.IsEmpty ? "0" : tile.Letter.ToString();
                    }
                    Debug.Write(txt);
                }
                Debug.WriteLine($"|{Alphabet[y].Char}");
            }
            Debug.WriteLine("_____________________________________");
        }
        public Dictionnaire Dico { get; }
        public const char Joker = '*';

        public Resolver Resolver { get; }
        public Bag Bag { get; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Game()
        {
            Dico = LoadDico();
            InitBoard();
            Player1 = new Player(this);
            Player2 = new Player(this);
            Resolver = new Resolver(this);
            Bag = new Bag(this);

        }
        private Dictionnaire LoadDico()
        {
            var dic = new Dictionnaire();
            dic.ChargerFichierDAWG();

            return dic;
        }

        public List<Letter> Alphabet { get; } = new List<Letter>()
        {
            new Letter('A',1,9),
            new Letter('B',5,2),
            new Letter('C',4,2),
            new Letter('D',3,5),
            new Letter('E',1,13),
            new Letter('F',5,2),
            new Letter('G',5,3),
            new Letter('H',5,4),
            new Letter('I',1,8),
            new Letter('J',8,1),
            new Letter('K',10,1),
            new Letter('L',2,4),
            new Letter('M',4,2),
            new Letter('N',1,5),
            new Letter('O',4,8),
            new Letter('P',4,2),
            new Letter('Q',10,1),
            new Letter('R',1,6),
            new Letter('S',1,5),
            new Letter('T',1,7),
            new Letter('U',2,4),
            new Letter('V',8,2),
            new Letter('W',10,1),
            new Letter('X',10,1),
            new Letter('Y',10,1),
            new Letter('Z',10,1),
            new Letter(Joker,0,2),

        };



        public const int BoardSize = 15;
        private Tile[,] grid = new Tile[BoardSize+1, BoardSize+1];
        public void InitBoard()
        {
            // Définition des cases bonus
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\initial_board.txt");

            int row = 0;
            int col = 0;
            foreach (var w in File.ReadAllLines(path))
            {
                foreach (var tp in w.Trim().Split(','))
                {
                    grid[row, col] = new Tile(this, row, col);

                    if (string.IsNullOrEmpty(tp))
                        continue;

                    switch (tp.Trim())
                    {
                        case "RE":
                            grid[row, col].TileType = TileType.Regular;
                            break;
                        case "CE":
                            grid[row, col].TileType = TileType.Center;
                            break;
                        case "TW":
                            grid[row, col].TileType = TileType.TripleWord;
                            break;
                        case "TL":
                            grid[row, col].TileType = TileType.TripleLetter;
                            break;
                        case "DW":
                            grid[row, col].TileType = TileType.DoubleWord;
                            break;
                        case "DL":
                            grid[row, col].TileType = TileType.DoubleLetter;
                            break;
                        default:
                            throw new Exception($"Unknown tile type in inital_board file: {tp}");
                    }
                    col += 1;
                }

                col = 0;
                row += 1;
            }



            ////'Initialisation du contenu du sac d'où sont tirées les lettres
            //for (int nl = 0; nl < 27; nl++)
            //{
            //    BagContent[nl] = LettersCount[nl];
            //    //LetterValue[nl] = LetterPoints[nl];
            //}
            //Afficher_Contenu_Sac


        }
        public Tile[,] Grid
        {
            get => grid;
            set
            {
                if (grid == null) grid = value;
            }
        }

        public bool FirstMove
        {
            get
            {
                return grid[7, 7].IsEmpty;
            }
        }
    }
}
