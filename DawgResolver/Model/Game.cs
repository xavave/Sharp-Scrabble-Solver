using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using Dawg;

namespace DawgResolver.Model
{
    public class Game
    {
        protected Dictionnaire Dico { get; private set; }
        public static char Joker;
        public bool IsPlayer1 { get; set; } = true;
        public bool EndGame { get; set; } = false;
        public Resolver Resolver { get; private set; }
        public Bag Bag { get; private set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public bool Stop { get; set; } = false;
        public int MoveIndex { get; set; } = 1;
        public static char EmptyChar { get; } = ' ';
        public static MovementDirection CurrentWordDirection { get; set; } = MovementDirection.Across;
        public static bool HintSortByBestScore { get; set; } = true;
        public Player CurrentPlayer => IsPlayer1 ? this.Player1 : this.Player2;

        public int BoardSize { get; set; } = 15;

        public string GenerateHtml(IExtendedTile[,] Tiles)
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

        public string GenerateTextGrid(CustomExtendedTilesGrid Tiles, bool? printAnchor = false, bool printLetterValue = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("123456789012345");
            sb.AppendLine("_______________");
            for (int ligne = 0; ligne < Tiles.GetLength(0); ligne++)
            {
                sb.Append($"{Resolver.Alphabet.ElementAt(ligne).Char}|");
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
        public void TransposeGameGrid()
        {
            var backupGrid = this.BackupGameGrid();
            for (int ligne = 0; ligne < backupGrid.GetLength(0); ligne++)
                for (int col = 0; col < backupGrid.GetLength(1); col++)
                {
                    var source = backupGrid[col, ligne];
                    if (source == null) continue;
                    this.Grid[ligne, col] = new BaseVirtualTile(this.Resolver, col, ligne);
                    this.Grid[ligne, col].Ligne = ligne;
                    this.Grid[ligne, col].Col = col;
                    this.Grid[ligne, col].WordMultiplier = source.WordMultiplier;
                    this.Grid[ligne, col].LetterMultiplier = source.LetterMultiplier;
                    this.Grid[ligne, col].Letter = backupGrid[col, ligne].Letter;
                    this.Grid[ligne, col].CopyControllers(backupGrid[col, ligne].Controlers);
                    this.Grid[ligne, col].IsValidated = backupGrid[col, ligne].IsValidated;
                }
            this.IsTransposed = !this.IsTransposed;

        }

        /// <summary>
        /// Cette procédure repère les ancres ligne par ligne
        /// Il s'agit des cases inocupées adjacentes à une case déjà occupée
        /// Ce recensement est utile afin de limiter les recherches aux cases où l'on peut jouer des coups
        /// </summary>
        public void FindAnchors(Resolver r)
        {

            foreach (var t in this.Grid.OfType<IExtendedTile>().Where(ti => ti.IsAnchor))
            {
                // Cas où l'ancre est à l'extrème gauche du plateau, la taille du préfixe est exactement 0
                // Cas où l'ancre est précédée d'une autre ancre, la taille du préfixe est exactement 0

                t.IsValidated = false;
                IExtendedTile tileCpy = t.Copy(r);

                int cptPrefix = 0;

                if (t.Col == 0 || t.LeftTile.IsAnchor)
                {
                    t.AnchorLeftMinLimit = 0;
                    t.AnchorLeftMaxLimit = 0;
                }
                else if (!t.LeftTile.IsEmpty)
                {
                    // Cas où l'ancre est précédée d'une case occupée, la taille du préfixe est exactement également la taille du mot
                    // ou de la chaîne qui précède l'ancre
                    cptPrefix = 0;
                    while (tileCpy != null && tileCpy.LeftTile != null && !tileCpy.LeftTile.IsEmpty)
                    {
                        cptPrefix++;
                        tileCpy = tileCpy.LeftTile;

                    }
                    t.AnchorLeftMinLimit = cptPrefix;
                    t.AnchorLeftMaxLimit = cptPrefix;
                }
                else if (t.LeftTile.IsEmpty)
                {
                    // Cas où l'ancre est précédée par un case vide,
                    // la taille du préfixe varie de 0 à k où k représente le nombre de cases vides non identifiées comme des ancres
                    cptPrefix = 0;
                    while (tileCpy != null && tileCpy.LeftTile != null && tileCpy.LeftTile.IsEmpty)
                    {
                        cptPrefix++;
                        tileCpy = tileCpy.LeftTile;


                    }
                    t.AnchorLeftMinLimit = 0;
                    t.AnchorLeftMaxLimit = cptPrefix;
                }
            }
        }

        /// <summary>
        /// Cette procédure génère un nouveau tirage
        /// en choisissant au hasard parmi les lettres disponibles dans le sac
        /// </summary>
        public void DetectTiles()
        {
            //On efface les informations précédentes en vue d'une nouvelle analyse
            CleanTiles();

            // Le premier est un cas particulier où aucune lettre n'est encore posée sur le plateau
            // Tous les mots formés doivent toucher la case centrale qui constitue ainsi la seule ancre
            // Les mots formés au premier peuvent commencer six cases au plus vers la gauche de la case centrale
            // Comme aucune lettre n'est encore présente sur le plateau, il n'y a aucune vérification à faire
            // par rapport aux mots formés verticalement (les 26 lettres peuvent être utilisées)

            if (IsFirstMove)
            {
                Grid[BoardSize / 2, BoardSize / 2].AnchorLeftMinLimit = 0;
                Grid[7, 7].AnchorLeftMaxLimit = 6;

                foreach (var t in Grid.OfType<IExtendedTile>())
                {
                    t.Controlers[26] = 26;
                }
            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                FindAnchors(Resolver);

                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valides
                FindControlers(Resolver);
            }

        }

        public void CleanTiles()
        {
            foreach (var t in Grid.OfType<IExtendedTile>().ToList())
            {
                if (t.AnchorLeftMaxLimit != 0) t.AnchorLeftMaxLimit = 0;
                if (t.AnchorLeftMinLimit != 0) t.AnchorLeftMinLimit = 0;
                t.Controlers.Clear();
                t.Letter.Char = Game.EmptyChar;
                t.Letter.Value = 0;
                t.Letter.Count = 0;
                t.LetterMultiplier = 1;
                t.WordMultiplier = 1;
            }
        }
        /// <summary>
        /// Cette procédure permet d'identifier les lettres qu'on peut jouer dans une case donnée
        /// en vérifiant si l'ajout d'un mot dans le sens horizontal permet de former également des mots valides
        /// dans le sens vertical. Ce travail est effectué avant la recherche proprement dite pour limiter les possibilités à tester
        /// on en profite également pour précalculer le point rapporté par les mots formés verticalement
        /// </summary>
        public void FindControlers(Resolver r)
        {
            int L = 0;

            foreach (var t in Grid.OfType<IExtendedTile>())
            {
                if (t.IsAnchor)
                {
                    var wordStart = string.Empty;
                    var wordEnd = string.Empty;
                    var tileCpy = t.Copy(r);
                    int points = 0;

                    //On rassemble le début éventuel des mots (lettres situées au dessus de la case)
                    while (tileCpy.UpTile != null && !tileCpy.UpTile.IsEmpty)
                    {
                        tileCpy = tileCpy.UpTile;
                        wordStart += tileCpy.Letter.Char;
                        points += tileCpy.Letter.Value;

                    }
                    wordStart = string.Join("", wordStart.Reverse());
                    wordEnd = "";
                    tileCpy = t.Copy(r);

                    //On rassemble la fin éventuelle des mots (lettres situées en dessous de la case)
                    while (tileCpy.DownTile != null && !tileCpy.DownTile.IsEmpty)
                    {
                        tileCpy = tileCpy.DownTile;
                        wordEnd += tileCpy.Letter.Char;
                        points += tileCpy.Letter.Value;

                    }
                    //TODO test avec "AINEE" puis "SURPAYA"

                    //On vérifie pour chaque Lettre L de A à Z si Debut+L+Fin forme un mot valide
                    //Si tel est le cas, la lettre L est jouable pour la case considérée
                    //et on précalcule le point que le mot verticalement formé permettrait de gagner si L était jouée
                    L = 0;
                    foreach (var c in r.Alphabet)
                    {
                        var mot = wordStart + c.Char + wordEnd;
                        if (mot.Length > 1 && r.Dico.MotAdmis(mot))
                        {
                            Grid[t.Ligne, t.Col].Controlers[((int)c.Char) - Dictionnaire.AscShift] = (points + c.Value * t.LetterMultiplier) * t.WordMultiplier;
                            L++;
                        }
                        else
                            Grid[t.Ligne, t.Col].Controlers[((int)c.Char) - Dictionnaire.AscShift] = 0;
                    }

                    //Si aucune lettre ne se trouve ni au dessus ni en dessous de la case, il n'y aucune contrainte à respecter
                    //toutes les lettres peuvent être placées dans la case considérée.
                    if (string.IsNullOrWhiteSpace(wordStart + wordEnd))
                        t.Controlers[26] = 26;
                    else
                        t.Controlers[26] = L;
                }
                else
                {
                    if (t.IsEmpty) t.Controlers[26] = 26;
                }
            }

        }
        public CustomExtendedTilesGrid BackupGameGrid()
        {
            var ret = new CustomExtendedTilesGrid(this, BoardSize);
            for (int ligne = 0; ligne < Grid.GetLength(0); ligne++)
                for (int col = 0; col < Grid.GetLength(1); col++)
                {
                    ret[ligne, col] = Grid[ligne, col];

                }
            return ret;
        }
        public void RestoreGameGridFrom(CustomExtendedTilesGrid backTiles)
        {
            for (int ligne = 0; ligne < backTiles.GetLength(0); ligne++)
                for (int col = 0; col < backTiles.GetLength(1); col++)
                {
                    Grid[ligne, col] = backTiles[ligne, col];
                }
        }

        public Game(string nomDico = null)
        {
           
            Dico = new Dictionnaire(nomDico);
            Joker = Dico.Joker;
            Resolver = new Resolver(this, nomDico);
            Player1 = new Player(this, "Player 1");
            Player2 = new Player(this, "Player 2");
            Grid = new CustomExtendedTilesGrid(this, BoardSize);
            //InitBoard();
            InitGameProperties();
        }

        public void InitGameProperties()
        {
            Bag = Bag.Build(Resolver);
            IsTransposed = false;
            NoMoreMovesCount = 0;
            EndGame = false;
            IsPlayer1 = true;
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

        public IExtendedTile[,] InitGameBoardTiles()
        {
            IExtendedTile[,] vTiles = new BaseVirtualTile[this.BoardSize, this.BoardSize];

            // Définition des cases bonus
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith($"initial_board{BoardSize}{Resolver.Mode}.txt"));
            if (string.IsNullOrWhiteSpace(resourceName)) return null;
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
                        if (vTiles[row, col] == null) vTiles[row, col] = new BaseVirtualTile(Resolver, row, col);
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
                                    vTiles[row, col].WordMultiplier = 1;
                                    vTiles[row, col].LetterMultiplier = 1;
                                    break;
                                case "CE":
                                    vTiles[row, col].WordMultiplier = 1;
                                    vTiles[row, col].LetterMultiplier = 1;
                                    break;
                                case "TW":
                                case "3W":
                                    vTiles[row, col].WordMultiplier = 3;
                                    vTiles[row, col].LetterMultiplier = 1;
                                    break;
                                case "TL":
                                case "3L":
                                    vTiles[row, col].LetterMultiplier = 3;
                                    vTiles[row, col].WordMultiplier = 1;
                                    break;
                                case "DW":
                                case "2W":
                                    vTiles[row, col].WordMultiplier = 2;
                                    vTiles[row, col].LetterMultiplier = 1;
                                    break;
                                case "DL":
                                case "2L":
                                    vTiles[row, col].LetterMultiplier = 2;
                                    vTiles[row, col].WordMultiplier = 1;
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
                return vTiles;
            }
        }
        //protected static VTile[,] StaGrid { get; }
        //{
        //    get => grid; set { grid = value; }
        //}
        public bool IsTransposed { get; set; } = false;
        public bool FirstMove
        {
            get
            {
                return Grid.OfType<IExtendedTile>().First(t => t.TileType == TileType.Center).IsEmpty;
            }
        }
        public CustomExtendedTilesGrid Grid { get; }
        public int NoMoreMovesCount { get; set; } = 0;
        public CancellationToken Token { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
        public bool IsFirstMove { get; set; }

        public string Serialise()
        {
            var ret = $"P1?{IsPlayer1}" + Environment.NewLine;
            ret += "letters" + Environment.NewLine;
            foreach (var l in Resolver.Alphabet)
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
                    ret += $"M1{Player1.Moves.ElementAt(i).Serialize}";
                if (i < Player2.Moves.Count)
                    ret += $"M2{Player2.Moves.ElementAt(i).Serialize}";
            }

            return ret;
        }

        public void Deserialize(string txt)
        {
            //InitBoard();
            var alphabet = new List<Letter>();
            var tiles = new List<IExtendedTile>();
            var lines = txt.Split(Environment.NewLine.ToCharArray());
            foreach (var l in lines)
            {
                if (l.StartsWith("P1?"))
                {
                    IsPlayer1 = bool.Parse(l.Substring(3));
                }
                else if (l.StartsWith("L"))
                {
                    alphabet.Add(l.DeserializeLetter(Resolver));
                }
                else if (l.StartsWith("T"))
                {
                    tiles.Add(l.DeserializeTile(Resolver));
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
                Bag.Letters.ElementAt(idx).Count = alphabet[idx].Count;
            }
            foreach (var t in tiles)
            {
                if (t.Ligne < this.BoardSize && t.Col < this.BoardSize)
                {

                    Grid[t.Ligne, t.Col] = t;
                    if (!t.IsEmpty) Grid[t.Ligne, t.Col].IsValidated = true;
                }
            }


            Player1.Points = Player1.Moves.Sum(m => m.Points);
            Player2.Points = Player2.Moves.Sum(m => m.Points);

        }
    }
}
