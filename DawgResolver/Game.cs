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
    public class Game
    {
        public void PrintGrid(Tile[,] Tiles, bool printAnchor, bool printLetterValue = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("123456789012345");
            for (int ligne = 0; ligne <= Tiles.GetUpperBound(0); ligne++)
            {
                for (int col = 0; col <= Tiles.GetUpperBound(1); col++)
                {

                    var tile = Tiles[ligne, col];

                    if (printAnchor)
                    {
                        var lm = tile.LetterMultiplier == 2 ? (char)0xB2 : tile.LetterMultiplier == 3 ? (char)0XB3 : (char)0XB7;
                        var wm = tile.WordMultiplier == 2 ? '2' : tile.WordMultiplier == 3 ? '3' : (char)0XB7;
                        sb.Append(tile.IsAnchor ? "@" : tile.IsEmpty ? tile.LetterMultiplier > 1 ? lm.ToString() : wm.ToString() : printLetterValue? (tile.Letter.Value*tile.LetterMultiplier).ToString(): tile.Letter.ToString());
                    }
                    else
                    {
                        sb.Append(tile.IsEmpty ? "0" : tile.Letter.ToString());
                    }

                }
                sb.AppendLine($"|{Alphabet[ligne].Char}");
            }
            sb.AppendLine("_____________________________________");
            Debug.WriteLine(sb.ToString());
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
            Bag = new Bag();


        }
        private Dictionnaire LoadDico()
        {
            var dic = new Dictionnaire();
            dic.ChargerFichierDAWG();

            return dic;
        }

        public static  List<Letter> Alphabet { get; } = new List<Letter>()
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
                    Grid[row, col] = new Tile(row, col);

                    if (string.IsNullOrEmpty(tp))
                        continue;

                    switch (tp.Trim())
                    {
                        case "RE":

                            break;
                        case "CE":

                            break;
                        case "TW":
                            Grid[row, col].WordMultiplier = 3;
                            break;
                        case "TL":
                            Grid[row, col].LetterMultiplier = 3;
                            break;
                        case "DW":
                            Grid[row, col].WordMultiplier = 2;
                            break;
                        case "DL":
                            Grid[row, col].LetterMultiplier = 2;
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
        public static Tile[,] Grid { get; set; } = new Tile[Game.BoardSize, Game.BoardSize];
       

        public bool FirstMove
        {
            get
            {
                return Grid[7, 7].IsEmpty;
            }
        }
    }
}
