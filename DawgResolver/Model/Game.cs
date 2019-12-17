using DawgResolver;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace DawgResolver.Model
{
    public class Game
    {
        public bool Stop { get; set; } = false;

        public int MoveIndex { get; set; } = 1;
        public static char EmptyChar { get; } = ' ';
        public static MovementDirection CurrentWordDirection { get; set; } = MovementDirection.Across;
        public static int BoardSize { get; set; } = 15;

        public static bool HintSortByBestScore { get; set; } = true;

        public static char GameStyle { get; set; } = 'S';

        private VTile[,] grid = new VTile[Game.BoardSize, Game.BoardSize];
        public string GenerateHtml(VTile[,] Tiles)
        {
            StringBuilder sb = new StringBuilder("<html><div>");
            for (int ligne = 0; ligne < Tiles.GetLength(0); ligne++)
            {

                sb.Append("|");
                for (int col = 0; col < Tiles.GetLength(1); col++)
                {
                    //TODO 
                }
                sb.Append("|<br/>");
            }
            sb.Append("</div></html>");
            return sb.ToString();
        }

        public string GenerateTextGrid(VTile[,] Tiles, bool? printAnchor = false, bool printLetterValue = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("123456789012345");
            sb.AppendLine("_______________");
            for (int ligne = 0; ligne < Tiles.GetLength(0); ligne++)
            {
                sb.Append($"{Alphabet[ligne].Char}|");
                for (int col = 0; col < Tiles.GetLength(1); col++)
                {

                    var tile = Tiles[ligne, col];

                    if (printAnchor.HasValue && printAnchor.Value)
                    {
                        if (!tile.IsEmpty)
                        {

                        }
                        sb.Append(tile.IsAnchor ? "@" : tile.IsEmpty ? "0" : tile.Letter.Char.ToString());
                        //var lm = tile.LetterMultiplier == 2 ? (char)0xB2 : tile.LetterMultiplier == 3 ? (char)0XB3 : '0';
                        //var wm = tile.WordMultiplier == 2 ? '2' : tile.WordMultiplier == 3 ? '3' : '0';
                        //sb.Append(tile.IsAnchor ? "@" : tile.IsEmpty ? tile.LetterMultiplier > 1 ? lm.ToString() : wm.ToString() : printLetterValue ? (tile.Letter.Value * tile.LetterMultiplier).ToString() : tile.Letter.ToString());
                    }
                    else if (printAnchor.HasValue)
                    {
                        sb.Append(tile.IsEmpty ? "0" : tile.Letter.ToString());
                    }
                    else
                    {
                        var lm = tile.LetterMultiplier == 2 ? (char)0xB2 : tile.LetterMultiplier == 3 ? (char)0XB3 : '0';
                        var wm = tile.WordMultiplier == 2 ? '2' : tile.WordMultiplier == 3 ? '3' : '0';
                        sb.Append(tile.IsValidated ? "V" : "0");
                    }

                }
                sb.AppendLine();
            }
            sb.AppendLine("_______________");
            return sb.ToString();
        }
        public Dictionnaire Dico { get; }
        public const char Joker = '?';
        public bool IsPlayer1 { get; set; } = true;
        public bool EndGame { get; set; } = false;

        public Resolver Resolver { get; }
        public Bag Bag { get; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Game(bool initBoard = true)
        {
            if (Dico == null)
                Dico = LoadDico();
            if (initBoard) InitBoard();
            Player1 = new Player(this);
            Player2 = new Player(this);
            Resolver = new Resolver(this);
            Bag = new Bag();
            //Bag.Letters = new List<Letter>(Game.AlphabetAvecJoker);
            Bag.Letters.ResetCount();
            NoMoreMovesCount = 0;
            EndGame = false;
            IsPlayer1 = true;

        }
        private Dictionnaire LoadDico()
        {
            var dic = new Dictionnaire();
            //dic.DAWG = dic.ChargerFichierDAWG();
            //dic.ChargerFichierDAWG();

            return dic;
        }
        public static List<Letter> Alphabet
        {
            get { return GameStyle == 'S' ? AlphabetScrabbleAvecJoker.Take(26).ToList() : AlphabetWWFAvecJoker.Take(26).ToList(); }
        }

        //public List<Letter> ClearTilesInPlay(Player p)
        //{

        //    for (int i = 0; i < Grid.LongLength; i++)
        //    {
        //        var tile = Grid.OfType<VTile>().ElementAt(i);
        //        if (!tile.IsValidated)
        //        {
        //            if (!tile.IsEmpty)
        //            {
        //                if (tile.FromJoker)
        //                {
        //                    p.Rack.Add(GameStyle == 'S' ? AlphabetScrabbleAvecJoker[26]: Game.AlphabetWWFAvecJoker[26]);
        //                }
        //                else
        //                {
        //                    p.Rack.Add(tile.Letter);
        //                }
        //                tile.IsValidated = true;
        //            }
        //            else
        //                Grid[tile.Ligne, tile.Col].Letter = new Letter();

        //        }
        //    }
        //    return p.Rack;
        //}

        public static List<Letter> AlphabetWWFAvecJoker { get; } = new List<Letter>()
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
        public static List<Letter> AlphabetScrabbleAvecJoker { get; } = new List<Letter>()
        {
            new Letter('A',1,9),
            new Letter('B',3,2),
            new Letter('C',3,2),
            new Letter('D',2,3),
            new Letter('E',1,15),
            new Letter('F',4,2),
            new Letter('G',2,2),
            new Letter('H',4,2),
            new Letter('I',1,8),
            new Letter('J',8,1),
            new Letter('K',10,1),
            new Letter('L',1,5),
            new Letter('M',2,3),
            new Letter('N',1,6),
            new Letter('O',1,6),
            new Letter('P',3,2),
            new Letter('Q',8,1),
            new Letter('R',1,6),
            new Letter('S',1,6),
            new Letter('T',1,6),
            new Letter('U',1,6),
            new Letter('V',4,2),
            new Letter('W',10,1),
            new Letter('X',10,1),
            new Letter('Y',10,1),
            new Letter('Z',10,1),
            new Letter(Joker,0,2),

        };

        public VTile[,] InitBoard(int newBoardSize = 0)
        {
            if (newBoardSize > 0)
                Grid = new VTile[Game.BoardSize, Game.BoardSize];
            // Définition des cases bonus
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"initial_board{BoardSize}{GameStyle}.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();

                int row = 0;
                int col = 0;
                foreach (var w in content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (var tp in w.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (Grid[row, col] == null)
                            Grid[row, col] = new Tile(this, row, col);
                        else
                        {

                        }

                        if (string.IsNullOrEmpty(tp))
                            continue;
                        else


                            switch (tp.Trim())
                            {
                                case "RE":
                                case "__":
                                    Grid[row, col].WordMultiplier = 1;
                                    Grid[row, col].LetterMultiplier = 1;
                                    break;
                                case "CE":

                                    break;
                                case "TW":
                                case "3W":
                                    Grid[row, col].WordMultiplier = 3;
                                    break;
                                case "TL":
                                case "3L":
                                    Grid[row, col].LetterMultiplier = 3;
                                    break;
                                case "DW":
                                case "2W":
                                    Grid[row, col].WordMultiplier = 2;
                                    break;
                                case "DL":
                                case "2L":
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
                return Grid;
            }
        }
        public VTile[,] Grid
        {
            get => grid; set { grid = value; }
        }
        public static bool IsTransposed { get; set; } = false;
        public bool FirstMove
        {
            get
            {
                return Grid.OfType<VTile>().First(t => t.TileType == TileType.Center).IsEmpty;
            }
        }

        public int NoMoreMovesCount { get; set; } = 0;
        public CancellationToken Token { get; set; }
        public CancellationTokenSource Cts { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
        public string Serialise()
        {
            var ret = $"P1?{IsPlayer1}" + Environment.NewLine;
            ret += "letters" + Environment.NewLine;
            foreach (var l in AlphabetWWFAvecJoker)
                ret += l.Serialize + Environment.NewLine;

            ret += "tiles" + Environment.NewLine;
            foreach (var t in Grid)
            {
                ret += t.Serialize + Environment.NewLine;
            }

            ret += "Moves" + Environment.NewLine;

            for (int i = 0; i < Math.Max(Player1.Moves.Count, Player2.Moves.Count); i++)
            {
                if (i < Player1.Moves.Count)
                    ret += $"M1{Player1.Moves[i].Serialize}";
                if (i < Player2.Moves.Count)
                    ret += $"M2{Player2.Moves[i].Serialize}";
            }

            return ret;
        }

        public void Deserialize(string txt)
        {
            InitBoard();
            var alphabet = new List<Letter>();
            var tiles = new List<VTile>();
            var lines = txt.Split(Environment.NewLine.ToCharArray());
            foreach (var l in lines)
            {
                if (l.StartsWith("P1?"))
                {
                    IsPlayer1 = bool.Parse(l.Substring(3));
                }
                else if (l.StartsWith("L"))
                {
                    alphabet.Add(l.DeserializeLetter());
                }
                else if (l.StartsWith("T"))
                {
                    tiles.Add(l.DeserializeTile(this));
                }
                else if (l.StartsWith("M1"))
                {
                    Player1.Moves.Add(l.DeserializeMove(this));
                }
                else if (l.StartsWith("M2"))
                {
                    Player2.Moves.Add(l.DeserializeMove(this));
                }
            }
            for (int idx = 0; idx < Bag.Letters.Count; idx++)
            {
                Bag.Letters[idx].Count = alphabet[idx].Count;
            }
            //foreach (var t in tiles)
            //{
            //    if (t.Ligne < Game.BoardSize && t.Col < Game.BoardSize)
            //    {
            //        if (t.Ligne == 3 && t.Col == 11)
            //        {

            //        }
            //        Grid[t.Ligne, t.Col] = t;
            //        if (!t.IsEmpty) Grid[t.Ligne, t.Col].IsValidated = true;
            //    }
            //}
            

            Player1.Points = Player1.Moves.Sum(m => m.Points);
            Player2.Points = Player2.Moves.Sum(m => m.Points);

        }
    }
}
