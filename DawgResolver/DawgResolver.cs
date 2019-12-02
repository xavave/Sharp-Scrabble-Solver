using DawgResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dawg.Word;

namespace Dawg
{
    public class Resolver
    {
        //const definition 
        const int TimeLimit = 60;
        const int TerminalNode = 27;
        const int WildCardShift = 32;
        const int MoveLimit = 50;
        const int All = 102;
        const int BoardSize = 15;
        const int NbOfLettersPlusJokerInAlphabet = 27;
        //variables definition

        public decimal[] DoubleLetters { get; set; }
        public decimal[] TripleLetters { get; set; }
        public decimal[] DoubleWords { get; set; }
        public decimal[] TripleWords { get; set; }
        public int[] LettersCount { get; set; } = new int[NbOfLettersPlusJokerInAlphabet];
        public int[] LetterPoints { get; set; }
        public int WordCount { get; set; }
        public int NodeCount { get; private set; }
        public List<string> Dawg { get; set; }

        string[,] GridCopy { get; set; }
        bool DicLoaded { get; set; }
        bool BoardDef { get; set; }
        bool[,] Connected { get; set; }
        bool FirstMove { get; set; }
        static bool[,] Anchors { get; set; } = new bool[17, 17];
        bool PossibleMove { get; set; }
        bool ValidMove { get; set; }
        int[,] LetterMultiplier { get; set; } = new int[17, 17];
        int[,] WordMultiplier { get; set; } = new int[17, 17];
        int[] LetterValue { get; set; } = new int[27];
        int[] BagContent { get; set; } = new int[27];
        int LettersPlayed { get; set; }
        static int[,,] AnchorLimits { get; set; } = new int[17, 17, 2];
        int[,] Temp1 { get; set; } = new int[17, 17];
        int[,] Temp2 { get; set; } = new int[17, 17];
        int[,,] Controlers { get; set; } = new int[17, 17, 27];
        int Direction { get; set; }

        int MoveNumber { get; set; }
        int Score { get; set; }
        public int PlayedLetters { get; private set; }
        int MinPoint { get; set; }
        long[,] Dictionary { get; set; }
        long CurrentNode { get; set; }
        long nbPossibleMoves { get; set; }
        long nbMoves { get; set; }
        long nbAcceptedMoves { get; set; }
        float Start { get; set; }
        float AnchorStart { get; set; }
        float AnchorTimeLimit { get; set; }
        Tile[,] RealBoard { get; set; } = new Tile[17, 17];
        string[,] DisplayBoard { get; set; } = new string[17, 17];

        static string[] Draught;
        public string DraughtRange { get; set; }
        string[,] LegalMoves { get; set; }
        static Move Node;
        Move LegalMove = new Move();
        public int LeftLetters { get; private set; }
        public Dictionnaire Dictionnaire { get; private set; }
        public Noeud Noeud { get; private set; }

        Tile[,] CopyBoard(Tile[,] source)
        {
            var dest = new Tile[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];
            Array.Copy(source, 0, dest, 0, source.Length);
            return dest;
        }
        /// <summary>
        /// Cette procédure génère un nouveau tirage
        /// en choisissant au hasard parmi les lettres disponibles dans le sac
        /// </summary>
        public string NewDraught(string forcedLetters = null)
        {
            string chaine = "";
            string letters = forcedLetters;
            if (!string.IsNullOrWhiteSpace(letters))
            {
                DraughtRange = letters;
                return letters;
            }
            //'Initialisation du générateur de nombre aléatoire
            //'Randomize Timer
            int j;

            // On compte le nombre de lettres restantes dans le sac et on établit la liste des choix
            for (int i = 0; i < 27; i++)
            {
                chaine += new String((char)(i + 1 + Dictionnaire.AscShift), BagContent[i]);
            }
            LeftLetters = chaine.Length;


            // Si le sac est vide
            if (LeftLetters == 0)
                throw new Exception("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes
            if (LeftLetters <= 7)
                letters = chaine;
            else
            {
                Random rnd = new Random();
                // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
                for (int i = 0; i < 7; i++)
                {
                    j = rnd.Next(1, chaine.Length);
                    letters += chaine[j];
                    chaine = chaine.Remove(j, 1);
                }
            }
            DraughtRange = letters;
            Debug.WriteLine(letters);
            return letters;
            // On affiche le tirage ainsi proposé
            // Plage_Tirage = Lettres
        }
        public void FindMove()
        {
            string[] Temp;
            LegalMove = new Move();

            LegalMove.Point = 32767;
            MinPoint = 0;
            nbAcceptedMoves = 0;

            Board.Grid = CopyBoard(RealBoard);
            Direction = 0;
            Board.AnchorCount = 0;

            // On efface les informations précédentes en vue d'une nouvelle analyse
            for (int y = 0; y < Board.BoardSize; y++)
                for (int x = 0; x < Board.BoardSize; x++)
                    Board.Grid[x, y] = new Tile();

            // Le premier est un cas particulier où aucune lettre n'est encore posée sur le plateau
            // Tous les mots formés doivent toucher la case centrale qui constitue ainsi la seule ancre
            // Les mots formés au premier peuvent commencer six cases au plus vers la gauche de la case centrale
            // Comme aucune lettre n'est encore présente sur le plateau, il n'y a aucune vérification à faire
            // par rapport aux mots formés verticalement (les 26 lettres peuvent être utilisées)
            if (Board.FirstMove)
            {
                Board.Grid[7, 7].IsAnchor = true;
                Board.Grid[7, 7].PrefixMinSize = 0;
                Board.Grid[7, 7].PrefixMaxSize = 6;

                //AnchorLimits[7, 7, 0] = 0;
                //AnchorLimits[7, 7, 1] = 6;
                foreach (var t in Board.Grid)
                {
                    t.PossibleLetterPoints['z'] = 26;
                }
                //for (int cpti = 0; cpti < 15; cpti++)
                //{
                //    for (int j = 0; j < 15; j++)
                //    {
                //        Controlers[cpti, j, 26] = 26;
                //    }
                //}

            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                FindAnchors();
                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valide
                FindControlers();
            }

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            FindMovesPerAnchor();

            // Recherche des coups verticaux
            // par transposition de la grille, on refait tout le processus
            TransposeGrid(RealBoard, Board.Grid);
            Direction = 1;

            // On efface les informations précédentes en vue d'une nouvelle analyse
            for (int cinfo = 0; cinfo < BoardSize; cinfo++)
            {
                Board.Grid[0, cinfo] = new Tile();
                Board.Grid[BoardSize - 1, cinfo] = new Tile();
                Board.Grid[cinfo, 0] = new Tile();
                Board.Grid[cinfo, BoardSize - 1] = new Tile();
                for (int j = 0; j <= 16; j++)
                {
                    // On commence la transposition des cases bonus...
                    Temp1[cinfo, j] = LetterMultiplier[j, cinfo];
                    Temp2[cinfo, j] = WordMultiplier[j, cinfo];
                    Anchors[cinfo, j] = false;
                    AnchorLimits[cinfo, j, 0] = 0;
                    AnchorLimits[cinfo, j, 1] = 0;
                    for (int k = 0; k < 27; k++)
                    {
                        Controlers[cinfo, j, k] = 0;
                    }

                }
            }

            for (int final = 0; final < BoardSize; final++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    //... et on finalise ici
                    LetterMultiplier[final, j] = Temp1[final, j];
                    WordMultiplier[final, j] = Temp2[final, j];
                }
            }

            if (FirstMove)
            {
                // Le premier est un cas particulier où aucune lettre n'est encore posée sur le plateau
                // Tous les mots formés doivent toucher la case centrale qui constitue ainsi la seule ancre
                // Les mots formés au premier peuvent commencer six cases au plus vers la gauche de la case centrale
                // Comme aucune lettre n'est encore présente sur le plateau, il n'y a aucune vérification à faire
                // par rapport aux mots formés verticalement (les 26 lettres peuvent être utilisées)
                Anchors[7, 7] = true;
                AnchorLimits[7, 7, 0] = 0;
                AnchorLimits[7, 7, 1] = 6;
                for (int contI = 1; contI <= 15; contI++)
                {
                    for (int j = 1; j <= 15; j++)
                    {
                        Controlers[contI, j, 26] = 26;
                    }
                }
                FirstMove = false;
            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                FindAnchors();
                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valide
                FindControlers();
            }

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            FindMovesPerAnchor();

            // Mise en forme des coups enregistrés et affichage

            LegalMoves = new string[6, nbAcceptedMoves == 0 ? 1 : nbAcceptedMoves];

            Node = LegalMove.Next;
            int i = 0;
            while (Node != null)
            {
                LegalMoves[0, i] = Node.Direction.ToString();
                LegalMoves[1, i] = Node.Direction == 0 ? Node.Line.ToString() : Node.Column.ToString();
                LegalMoves[2, i] = Node.Direction == 1 ? Node.Line.ToString() : Node.Column.ToString();
                LegalMoves[3, i] = Node.Word;
                LegalMoves[4, i] = Node.Point.ToString();
                LegalMoves[5, i] = Node.Scramble ? "*" : "";
                Node = Node.Next;
                i++;
            }
            ClearNode(ref LegalMove); // Pour libérer la mémoire et prévenir une erreur de gestion de pile

            Temp = new string[LegalMoves.GetUpperBound(1) + 1];// (UBound(Coups_Legaux, 2))

            for (i = 0; i <= LegalMoves.GetUpperBound(1); i++)
            {
                if (LegalMoves[0, i] == "0")
                    Temp[i] = (char)(int.Parse(LegalMoves[1, i]) + Dictionnaire.AscShift + 1) + (int.Parse(LegalMoves[2, i]) + 1).ToString() + " - " + LegalMoves[3, i] + " = " + LegalMoves[4, i] + LegalMoves[5, i];
                else
                    Temp[i] = (int.Parse(LegalMoves[2, i]) + 1).ToString() + (char)(int.Parse(LegalMoves[1, i]) + 1 + Dictionnaire.AscShift) + " - " + LegalMoves[3, i] + " = " + LegalMoves[4, i] + LegalMoves[5, i];

            }

            // Cas où aucun coup légal n'a été trouvé
            var ret = nbPossibleMoves == 0 ? new string[] { "Aucun coup legal" } : Temp;
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
            if (nbPossibleMoves > 0) PossibleMove = true;
            nbMoves = nbPossibleMoves;
            nbPossibleMoves = 0;
        }
        void ClearNode(ref Move move)
        {
            if (move.Next != null) ClearNode(ref move.Next);
            move = null;
        }

        /// <summary>
        /// Cette procédure permet d'identifier les lettres qu'on peut jouer dans une case donnée
        /// en vérifiant si l'ajout d'un mot dans le sens horizontal permet de former également des mots valides
        /// dans le sens vertical. Ce travail est effectué avant la recherche proprement dite pour limiter les possibilités à tester
        /// on en profite également pour précalculer le point rapporté par les mots formés verticalement
        /// </summary>
        private void FindControlers()
        {


            int l = 0;
            var myWordStart = new Word();
            var myWordEnd = new Word();
            foreach (var t in Board.Grid)
            {
                if (t.IsAnchor)
                {
                    int points = 0;
                    var tileCpy = t.DeepClone();
                    //On rassemble le début éventuel des mots (lettres situées au dessus de la case)
                    while (!tileCpy.UpTile.IsEmpty)
                    {
                        tileCpy = tileCpy.UpTile;
                        myWordStart.Tiles.Add(tileCpy);
                        points += tileCpy.Letter.Value;
                    }
                    myWordStart.Tiles.Reverse();
                    //On rassemble la fin éventuelle des mots (lettres situées en dessous de la case)
                    while (!tileCpy.DownTile.IsEmpty)
                    {
                        tileCpy = tileCpy.DownTile;
                        myWordEnd.Tiles.Add(tileCpy);
                        points += tileCpy.Letter.Value;
                    }
                    //On vérifie pour chaque Lettre L de A à Z si Debut+L+Fin forme un mot valide
                    //Si tel est le cas, la lettre L est jouable pour la case considérée
                    //et on précalcule le point que le mot verticalement formé permettrait de gagner si L était jouée
                    l = 0;
                    foreach (var c in Board.Alphabet)
                    {
                        var currentWord = new Word() { Tiles = myWordStart.Tiles };
                        currentWord.Tiles.Append(new Word.Tile() { Letter = c });
                        currentWord.Tiles.AddRange(myWordEnd.Tiles);
                        if (currentWord.Tiles.Count > 1 && currentWord.IsAllowed)
                        {
                            var letterMultiplier = 1;
                            if (t.TileType == TileType.DoubleLetter) letterMultiplier = 2;
                            else if (t.TileType == TileType.TripleLetter) letterMultiplier = 3;
                            var wordMultiplier = 1;
                            if (t.TileType == TileType.DoubleWord) wordMultiplier = 2;
                            else if (t.TileType == TileType.TripleWord) wordMultiplier = 3;

                            t.PossibleLetterPoints[c.Char] = points + t.Letter.Value * letterMultiplier * wordMultiplier;
                            l++;
                        }
                        else
                            t.PossibleLetterPoints[c.Char] = 0;

                    }
                    //Si aucune lettre ne se trouve ni au dessus ni en dessous de la case, il n'y aucune contrainte à respecter
                    //toutes les lettres peuvent être placées dans la case considérée.
                    if (!myWordStart.Tiles.Any() && !myWordEnd.Tiles.Any())
                        t.PossibleLetterPoints['z'] = 26;
                    else
                        t.PossibleLetterPoints['z'] = l;

                }
                else
                    t.PossibleLetterPoints['z'] = 26;
            }

            //for (int i = 1; i <= 15; i++)
            //    for (int j = 1; j <= 15; i++)
            //    {
            //        if (Anchors[i, j])
            //        {
            //            //On rassemble le début éventuel des mots (lettres situées au dessus de la case)
            //            points = 0;
            //            k = i;
            //            start = "";
            //            while (k - 1 > 0 && Grid[k - 1, j] != "")
            //            {
            //                k--;
            //                if (Grid[k, j] != "")
            //                {
            //                    start = Grid[k, j] + start;
            //                    if (((int)Grid[k, j].First()) < 91) points += LetterValue[(int)Grid[k, j].First() - AscShift];

            //                }
            //            }


            //            //On rassemble la fin éventuelle des mots (lettres situées en dessous de la case)
            //            k = i;
            //            end = "";
            //            while (k + 1 < 16 && Grid[k + 1, j] != "")
            //            {
            //                k++;
            //                if (Grid[k, j] != "")
            //                {
            //                    end = end + Grid[k, j];
            //                    if (((int)Grid[k, j].First()) < 91) points += LetterValue[(int)Grid[k, j].First() - AscShift];
            //                }
            //            }

            //            //On vérifie pour chaque Lettre L de A à Z si Debut+L+Fin forme un mot valide
            //            //Si tel est le cas, la lettre L est jouable pour la case considérée
            //            //et on précalcule le point que le mot verticalement formé permettrait de gagner si L était jouée
            //            l = 0;
            //            for (k = 0; k < 27; k++)
            //            {

            //                word = (start + (char)(k + AscShift) + end).ToUpper();
            //                if (PossibleWord(word) && word.Length > 1)
            //                {
            //                    Controlers[i, j, k] = (points + LetterValue[k] * LetterMultiplier[i, j]) * WordMultiplier[i, j];
            //                    l++;
            //                }
            //                else
            //                    Controlers[i, j, k] = 0;

            //            }

            //            //Si aucune lettre ne se trouve ni au dessus ni en dessous de la case, il n'y aucune contrainte à respecter
            //            //toutes les lettres peuvent être placées dans la case considérée.
            //            if (start + end == "")
            //                Controlers[i, j, 26] = 26;
            //            else
            //                Controlers[i, j, 26] = l;

            //        }
            //        else if (Grid[i, j] == "") Controlers[i, j, 26] = 26;
            //    }
        }
        /// <summary>
        /// Cette fonction teste si un mot est présent dans le dictionnaire
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        //private bool PossibleWord(string word)
        //{

        //    int i;
        //    CurrentNode = 0; //Noeud Racine

        //    // La fonction utilise le mot à tester comme un chemin dans le DAWG à partir du noeud racine
        //    foreach (var l in word)
        //        if (!Found(CurrentNode, l.ToString()))
        //            return false;

        //    //Si le chemin est valide et s'il aboutit à un noeud terminal c'est que le mot existe dans le dictionnaire
        //    return Dictionary[TerminalNode, CurrentNode] == 1;
        //}

        private void FindAnchors()
        {
            int k = 0;
            // Cette procédure repère les ancres ligne par ligne
            // Il s'agit des cases inoccupées adjacentes à une case déjà occupée
            // Ce recensement est utile afin de limiter les recherches aux cases où l'on peut jouer des coups

            foreach (var t in Board.Grid)
            {

                // Cas où l'ancre est à l'extrème gauche du plateau, la taille du préfixe est exactement 0
                // Cas où l'ancre est précédée d'une autre ancre, la taille du préfixe est exactement 0
                if (t.YLoc == 0 || t.IsAnchor && t.LeftTile.IsAnchor)
                {
                    t.PrefixMinSize = 0;
                    t.PrefixMaxSize = 0;
                }

                // Cas où l'ancre est précédée d'une case occupée, la taille du préfixe est exactement également la taille du mot
                // ou de la chaîne qui précède l'ancre
                if (t.IsAnchor)
                {
                    // Cas où l'ancre est précédée d'une case occupée, la taille du préfixe est exactement également la taille du mot
                    // ou de la chaîne qui précède l'ancre
                    int cptPrefix = 0;
                    var tileCpy = t.DeepClone();
                    while (!tileCpy.LeftTile.IsEmpty)
                    {
                        tileCpy = tileCpy.LeftTile;
                        cptPrefix++;
                    }
                    t.PrefixMinSize = cptPrefix;
                    t.PrefixMaxSize = cptPrefix;
                    // Cas où l'ancre est précédée par un case vide,
                    // la taille du préfixe varie de 0 à k où k représente le nombre de cases vides non identifiées comme des ancres
                    cptPrefix = 0;
                    while (tileCpy.LeftTile.IsEmpty && !tileCpy.LeftTile.IsAnchor)
                    {
                        tileCpy = tileCpy.LeftTile;
                        cptPrefix++;
                    }
                    t.PrefixMinSize = 0;
                    t.PrefixMaxSize = cptPrefix;

                }

            }
            Board.AnchorCount = Board.Grid.OfType<Tile>().Count(t => t.IsAnchor);



            //for (int i = 0; i < 15; i++)
            //    for (int j = 0; j < 15; i++)
            //    {

            //        // cas d'une cellule avec une case occupée au dessus
            //        if (i - 1 > 0)
            //            if (Grid[i - 1, j].Letter != '')
            //                Anchors[i, j] = true;


            //        // cas d'une cellule avec une case occupée à gauche
            //        if (j - 1 > 0)
            //            if (Grid[i, j - 1] != "")
            //                Anchors[i, j] = true;


            //        // cas d'une cellule avec une case occupée à droite
            //        if (j + 1 < 15)
            //            if (Grid[i, j + 1] != "")
            //                Anchors[i, j] = true;


            //        // cas d'une cellule avec une case occupée en dessous
            //        if (i + 1 < 15)
            //            if (Grid[i + 1, j].Letter != '')
            //                Anchors[i, j] = true;

            //        // Elimination des cases ocupées
            //        if (Grid[i, j] != '')
            //            Anchors[i, j] = false;

            //        // Pour chaque ancre, déterminer la taille du préfixe qu'on peut rajouter
            //        // La règle est que le développement du préfixe ne doit pas empieter sur une autre ancre
            //        if (Anchors[i, j])
            //            AnchorCount++;
            //        if (j == 1)
            //        {
            //            // Cas où l'ancre est à l'extrème gauche du plateau, la taille du préfixe est exactement 0
            //            AnchorLimits[i, j, 0] = 0;
            //            AnchorLimits[i, j, 1] = 0;
            //        }
            //        else if (Anchors[i, j - 1])
            //        {
            //            // Cas où l'ancre est précédée d'une autre ancre, la taille du préfixe est exactement 0
            //            AnchorLimits[i, j, 0] = 0;
            //            AnchorLimits[i, j, 1] = 0;
            //        }
            //        else if (Grid[i, j - 1] != '')
            //        {
            //            // Cas où l'ancre est précédée d'une case occupée, la taille du préfixe est exactement également la taille du mot
            //            // ou de la chaîne qui précède l'ancre
            //            k = 1;
            //            while (Grid[i, j - k] != "")
            //            {
            //                k++;
            //            }
            //            AnchorLimits[i, j, 0] = k - 1;
            //            AnchorLimits[i, j, 1] = k - 1;
            //        }
            //        else if (Grid[i, j - 1] == '')
            //        {
            //            // Cas où l'ancre est précédée par un case vide,
            //            // la taille du préfixe varie de 0 à k où k représente le nombre de cases vides non identifiées comme des ancres
            //            k = 1;
            //            while (j - k > 0 && k < (Draught.Length + 1) && !Anchors[i, j - k])
            //            {
            //                if (Grid[i, j - k].Letter == '') k++;
            //            }
            //            AnchorLimits[i, j, 0] = 0;
            //            AnchorLimits[i, j, 1] = k - 1;
            //        }
            //        // If Sens = 0 Then Plage_Plateau(i, j) = Limites_Ancres(i, j, 1) & Limites_Ancres(i, j, 2)
            //    }
            //// If Sens = 0 Then MsgBox "ancres recensées"

        }

        private void TransposeGrid(Tile[,] realBoard, Tile[,] board)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = realBoard[j, i];
                }
            }
        }

        private void FindMovesPerAnchor()
        {

            string root = "";

            bool b;
            // Pour chaque ancre identifiée,
            // Cette procédure lance la recherche des différents mots formables à partir de l'ancre
            // en utilisant les mots du tirage et ceux déjà placés
            // en respectant la taille limite des préfixes précédement calculés
            foreach (var t in Board.Grid.OfType<Tile>().Where(t => t.IsAnchor))
            {

                // Cas où la taille du préfixe est imposée
                if (t.PrefixMinSize == t.PrefixMaxSize)
                {
                    // Cas du préfixe vide
                    // On ne peut que former un nouveau mot vers la droite
                    if (t.PrefixMinSize == 0)
                    {
                        //TODO
                    }
                    else
                    {
                        // Cas où le préfixe est déjà placé sur la grille
                        // On ne peut que continuer le préfixe vers la droite

                        //TODO
                    }
                }
                else
                {
                    // Cas où k cases vides non identifiés comme ancres se trouvent devant l'ancre
                    // On essaie les différents préfixes possibles allant de 0 à k lettres
                    for (int minA = 0; minA < t.PrefixMaxSize; minA++)
                    {
                        var len = DraughtRange.Length;
                        GoOn("", ref Draught, minA, 1, t, ref len);
                    }
                }

            }

            //for (int i = 1; i <= 15; i++)
            //    for (int j = 1; j <= 15; j++)
            //        if (Anchors[i, j])
            //        {
            //            //if (Timer - Debut > TimeLimit)  break;
            //            //AnchorStart = Timer
            //            if (AnchorLimits[i, j, 0] == AnchorLimits[i, j, 1])
            //                // Cas où la taille du préfixe est imposée
            //                if (AnchorLimits[i, j, 0] == 0)
            //                {
            //                    // Cas du préfixe vide
            //                    // On ne peut que former un nouveau mot vers la droite
            //                    var len = DraughtRange.Length;
            //                    GoOn("", ref Draught, 1, 1, i, j, ref len);
            //                }
            //                else
            //                {
            //                    // Cas où le préfixe est déjà placé sur la grille
            //                    // On ne peut que continuer le préfixe vers la droite
            //                    root = "";
            //                    CurrentNode = 1;
            //                    for (int k = j - AnchorLimits[i, j, 0]; k <= j - 1; k++)
            //                    {
            //                        root += Grid[i, k];
            //                        b = Found(CurrentNode, Grid[i, k]);
            //                    }
            //                    var len = DraughtRange.Length;
            //                    GoOn(root.ToUpper(), ref Draught, 1, CurrentNode, i, j, ref len);
            //                }

            //            else
            //            {
            //                // Cas où k cases vides non identifiés comme ancres se trouvent devant l'ancre
            //                // On essaie les différents préfixes possibles allant de 0 à k lettres
            //                for (int kk = 0; kk <= AnchorLimits[i, j, 1]; kk++)
            //                {
            //                    var len = DraughtRange.Length;
            //                    GoOn("", ref Draught, kk, 1, i, j - kk, ref len);
            //                }
            //            }
            //        }
        }
        /// <summary>
        /// Cette procédure teste les différentes combinaisons possibles
        /// pour continuer un préfixe vers la droite
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="leftLetters"></param>
        /// <param name="minSize"></param>
        /// <param name="node"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="nbLetters"></param>
        private void GoOn(string prefix, ref string[] leftLetters, int minSize, int node, Tile t, ref int nbLetters)
        {

            // If Controleurs(Ligne, Colonne, 27) = 0 And Grille(Ligne, Colonne) = "" Then Exit Sub


            //If Timer -Debut > Limite_Temps Then Exit Sub
            bool wildcardInDraught = leftLetters.Contains("[");

            if (t.IsEmpty)
            {

                // Si une case vide, on peut la remplir avec une lettre du tirage sous certaines conditions
                if (Board.Dico.dawg.Find(n => n.Numero == node).IsTerminal && prefix.Length > 1 && prefix.Length > minSize && nbLetters < Draught.Length)
                {
                    // Si le préfixe constitue déjà un mot valide
                    // alors on peut rajouter le préfixe dans la liste des coups admis
                    Add(prefix, t);
                }

                //if (Dictionary[TerminalNode, node] == 1 && root.Length > 1 && root.Length > minSize && nbLetters < Draught.Length) //  Or Lettres_Restantes(1) = "") Then
                //{// Si le préfixe constitue déjà un mot valide
                // // alors on peut rajouter le préfixe dans la liste des coups admis
                //    Add(root, line, column);
                //}
                foreach (var c in Board.Alphabet.Select(a => a.Char))
                {
                    if (Board.Dico.dawg.Find(n => n.Numero == node).IsTerminal)
                    {
                        for (int la = 0; la < nbLetters; la++)
                        {
                            if (leftLetters[la] == c.ToString())
                            {
                                if (t.PossibleLetterPoints[c] > 0 || t.PossibleLetterPoints['z'] == 26)
                                // Pour chacune des lettres qui répondent au précédent critère dans le tirage
                                //if (Controlers[line, column, letterIdx] > 0 || Controlers[line, column, 26] == 26)
                                {
                                    // Si la lettre permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // la lettre utilisée est alors retirée du tirage
                                    leftLetters[la] = leftLetters[nbLetters - 1];
                                    nbLetters--;
                                    // De manière récursive, on essaye de continuer le nouveau préfixe vers la droite
                                    //TODO GoOn(root + ((char)(letterIdx + AscShift)), ref leftLetters, minSize, Dictionary[letterIdx, node], t, ref nbLetters);

                                    // Au retour de l'appel recursif on restitue la lettre dans le tirage
                                    nbLetters++;
                                    //TODO leftLetters[nbLetters - 1] = ((char)(letterIdx + AscShift)).ToString();
                                }

                                if (!wildcardInDraught) break;
                            }
                            else if (leftLetters[la] == "[")
                            {
                                // Cas d'un joker
                                if (t.PossibleLetterPoints[(char)la] > 0 || t.PossibleLetterPoints['z'] == 26)
                                {
                                    // Si une lettre quelquonque (représentée par le joker) permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // le joker utilisé est alors retiré du tirage
                                    leftLetters[la] = leftLetters[nbLetters - 1];
                                    nbLetters--;

                                    // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                                    //TODO GoOn(root + ((char)(la + Dictionnaire.AscShift)).ToString().ToLower(), ref leftLetters, minSize, Dictionary[la, node], line, column, ref nbLetters);


                                    // Au retour de l'appel recursif on restitue le joker dans le tirage
                                    nbLetters++;
                                    leftLetters[nbLetters - 1] = "[";
                                }
                            }
                        }
                    }

                }

                //    for (int letterIdx = 1; letterIdx <= 26; letterIdx++)
                //    {
                //        if (Dictionary[letterIdx, node] != 0)
                //        {
                //            // On recherche les lettres qui rajoutées au préfixe permettrait d'aboutir à des mots dans le dictionnaire
                //            for (int la = 0; la < nbLetters; la++)
                //            {
                //                //if (leftLetters[la] == ((char)(letterIdx + AscShift)).ToString())
                //                //{
                //                //    // Pour chacune des lettres qui répondent au précédent critère dans le tirage
                //                //    if (Controlers[line, column, letterIdx] > 0 || Controlers[line, column, 26] == 26)
                //                //    {
                //                //        // Si la lettre permet également de former des mots verticalement
                //                //        // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                //                //        // alors on peut essayer de continuer le préfixe avec cette lettre
                //                //        // la lettre utilisée est alors retirée du tirage
                //                //        leftLetters[la] = leftLetters[nbLetters - 1];
                //                //        nbLetters--;
                //                //        // De manière récursive, on essaye de continuer le nouveau préfixe vers la droite
                //                //        GoOn(root + ((char)(letterIdx + AscShift)), ref leftLetters, minSize, Dictionary[letterIdx, node], line, column, ref nbLetters);

                //                //        // Au retour de l'appel recursif on restitue la lettre dans le tirage
                //                //        nbLetters++;
                //                //        leftLetters[nbLetters - 1] = ((char)(letterIdx + AscShift)).ToString();
                //                //    }

                //                //    if (!wildcardInDraught) break;
                //                //}
                //                //else if (leftLetters[la] == "[")
                //                //{
                //                //    // Cas d'un joker
                //                //    if (Controlers[line, column, la] > 0 || Controlers[line, column, 26] == 26)
                //                //    {
                //                //        // Si une lettre quelquonque (représentée par le joker) permet également de former des mots verticalement
                //                //        // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                //                //        // alors on peut essayer de continuer le préfixe avec cette lettre
                //                //        // le joker utilisé est alors retiré du tirage
                //                //        leftLetters[la] = leftLetters[nbLetters - 1];
                //                //        nbLetters--;

                //                //        // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                //                //        GoOn(root + ((char)(la + AscShift)).ToString().ToLower(), ref leftLetters, minSize, Dictionary[la, node], line, column, ref nbLetters);


                //                //        // Au retour de l'appel recursif on restitue le joker dans le tirage
                //                //        nbLetters++;
                //                //        leftLetters[nbLetters - 1] = "[";
                //                //    }
                //                //}
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    // Si la case n'est pas vide, on fait une concatenation du préfixe avec la lettre de la case courante
                //    if (Dictionary[(int)((Grid[line, column].ToUpper())[0] - Dictionnaire.AscShift), node] != 0)
                //    { // Si un mot du dictionnaire est susceptible de débuter par le nouveau préfixe ainsi obtenu
                //      // alors on peut essayer de continuer le préfixe avec cette lettre vers la droite
                //        GoOn(root + Grid[line, column], ref leftLetters, 0, Dictionary[((int)(Grid[line, column].ToUpper()[0]) - AscShift), node], line, column, ref nbLetters);
                //    }
            }
        }
        /// <summary>
        /// Recherche des différents coups admis
        /// </summary>
        public void FindMoves()
        {

            PossibleMove = false;
            Debug.WriteLine(FindLegalMoves());
            if (PossibleMove)
            {
                //       Bouton_Jouer_Coup.Enabled = True
                //Bouton_Chercher_Coups.Enabled = False
            }
            else
            {
                //       Bouton_Chercher_Coups.Enabled = True
                //Bouton_Jouer_Coup.Enabled = False
                if (DraughtRange.Any())
                {
                    // Les lettres non utilisées sont "reversées dans le sac"
                    foreach (var l in Draught)
                        BagContent[(int)l.First() - Dictionnaire.AscShift - 1]++;
                }
            }
        }

        private string FindLegalMoves()
        {

            // Cette procédure appelle la rechercher des coups admis
            // Après vérification de la régularité du tirage
            //Start = Timer

            if (!LoadDraught(BagContent))
            {
                //Bouton_Chercher_Coups.Enabled = True
                //Bouton_Jouer_Coup.Enabled = False
            }

            FindMove();
            return nbMoves + " coups trouvés ";// en " + (Int((Timer - Debut) * 100)) / 100 & " s."


            //If Timer -Debut > Limite_Temps Then MsgBox "Recherche intérrompue car le temps limite a été dépassé", vbInformation


        }
        public Noeud LoadDic()
        {
            Dictionnaire = new Dictionnaire();

            Noeud = Dictionnaire.ChargerFichierDAWG();
            DicLoaded = true;

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("dico_dawg.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();
                List<string> dic = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                WordCount = int.Parse(dic[0].Replace("NBMOTS : ", ""));
                NodeCount = dic.Count - 2;// int.Parse(dic[1].Replace("NBNOEUDS : ", ""));
                Dictionary = new long[28, NodeCount + 2];
                for (int lineIdx = 2; lineIdx < dic.Count; lineIdx++)
                {
                    var line = dic[lineIdx];
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
            //DicLoaded = true;
            return Noeud;
            //return DicLoaded;
        }
        /// <summary>
        /// Lancement d'une nouvelle partie
        /// </summary>
        public void NewGame()
        {

            if (!DicLoaded)
                LoadDic();
            //throw new Exception("Vous devez d'abord charger un dictionnaire");

            //if (RealBoard[8, 8] != "" )
            //    Reponse = MsgBox("Une partie est en cours, êtes-vous sur de vouloir d'en démarrer une autre?", vbYesNo)
            //    If Reponse = vbNo Then Exit Sub
            //End If
            //Definir_Plages
            InitBoard();
            //Bouton_Chercher_Coups.Enabled = True
            //Bouton_Jouer_Coup.Enabled = False
            //Historique.AddItem "Debut de la partie : " & Time()
        }
        /// <summary>
        /// Cette procédure initialise ou réinitialise le plateau de jeu ainsi que différentes variables
        /// </summary>
        private void InitBoard()
        {
            // Définition des cases bonus
            DoubleLetters = new decimal[] { 2.03M, 2.13M, 3.02M, 3.05M, 3.11M, 3.14M, 5.03M, 5.07M, 5.09M, 5.13M, 7.05M, 7.11M, 9.05M, 9.11M, 11.03M, 11.07M, 11.09M, 11.13M, 13.02M, 13.05M, 13.11M, 13.14M, 14.03M, 14.13M };
            TripleLetters = new decimal[] { 1.07M, 1.09M, 4.04M, 4.12M, 6.06M, 6.1M, 7.01M, 7.15M, 9.01M, 9.15M, 10.06M, 10.1M, 12.04M, 12.12M, 15.07M, 15.09M };
            DoubleWords = new decimal[] { 2.06M, 2.1M, 4.08M, 6.02M, 6.14M, 8.04M, 8.12M, 10.02M, 10.14M, 12.08M, 14.06M, 14.1M };
            TripleWords = new decimal[] { 1.04M, 1.12M, 4.01M, 4.15M, 12.01M, 12.15M, 15.04M, 15.12M };

            // Définition des lettres disponibles au début d'une partie et des points associés à chaque lettre
            LettersCount = new int[] { 9, 2, 2, 3, 15, 2, 2, 2, 8, 1, 1, 5, 3, 6, 6, 2, 1, 6, 6, 6, 6, 2, 1, 1, 1, 1, 2 };
            LetterPoints = new int[] { 1, 5, 4, 3, 1, 5, 5, 5, 1, 10, 10, 2, 4, 1, 4, 4, 10, 1, 1, 1, 2, 8, 10, 10, 10, 10, 0 };
            MoveNumber = 0;
            Score = 0;
            PlayedLetters = 0;

            // Initialisation du plateau
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    RealBoard[i, j] = new Tile();
                    DisplayBoard[i, j] = "";
                    LetterMultiplier[i, j] = 1;
                    WordMultiplier[i, j] = 1;

                }
            //        Plage_Plateau = Grille_Affichage
            //Plage_Copie = Grille_Affichage

            //         for (i = 1; i <= 15; i++)
            //             for (j = 1; j <= 15; j++)
            //             {
            //                 Plage_Plateau(i, j).Borders.Color = RGB(255, 255, 255)
            //  Plage_Plateau(i, j).Interior.Color = RGB(246, 245, 238)
            //}
            // Initialisation des cases bonus
            //For i = 1 To UBound(Lettre_Compte_Double)
            //    Ligne = Int(Lettre_Compte_Double(i))
            //    Colonne = 100 * (Lettre_Compte_Double(i) - Ligne)
            //    Multiplicateur_Lettre(Ligne, Colonne) = 2
            //    Plage_Plateau(Ligne, Colonne).Interior.Color = RGB(51, 153, 255) 'bleu
            //Next i


            //For i = 1 To UBound(Lettre_Compte_Triple)
            //    Ligne = Int(Lettre_Compte_Triple(i))
            //    Colonne = 100 * (Lettre_Compte_Triple(i) - Ligne)
            //    Multiplicateur_Lettre(Ligne, Colonne) = 3
            //    Plage_Plateau(Ligne, Colonne).Interior.Color = RGB(102, 255, 102) 'vert
            //Next i

            //For i = 1 To UBound(Mot_Compte_Double)
            //    Ligne = Int(Mot_Compte_Double(i))
            //    Colonne = 100 * (Mot_Compte_Double(i) - Ligne)
            //    Multiplicateur_Mot(Ligne, Colonne) = 2
            //    Plage_Plateau(Ligne, Colonne).Interior.Color = RGB(255, 0, 0) 'rouge
            //Next i

            //For i = 1 To UBound(Mot_Compte_Triple)
            //    Ligne = Int(Mot_Compte_Triple(i))
            //    Colonne = 100 * (Mot_Compte_Triple(i) - Ligne)
            //    Multiplicateur_Mot(Ligne, Colonne) = 3
            //    Plage_Plateau(Ligne, Colonne).Interior.Color = RGB(255, 102, 0) 'orange


            //Next i


            //'Initialisation du contenu du sac d'où sont tirées les lettres
            for (int nl = 0; nl < 27; nl++)
            {
                BagContent[nl] = LettersCount[nl];
                LetterValue[nl] = LetterPoints[nl];
            }
            //Afficher_Contenu_Sac

            //'Gestion du cas particulier du premier coup
            FirstMove = true;


            //'Remise à zéro de la liste des coups admis et de l'historique
            //Liste_Coups_Legaux.List = Array()
            //Historique.List = Array()
        }

        private bool LoadDraught(int[] bag)
        {
            // Cette fonction vérifie la régularité du tirage proposé, retourne Vrai le cas échéant et charge le tirage en mémoire

            bool loadDraught = false;
            int[] tempBag = new int[26];
            List<string> tempDraught = new List<string>();

            //Cas d'un tirage vide
            if (DraughtRange == "")
                throw new Exception("Le tirage est vide.");

            //Cas d'un tirage contenant plus de 7 lettres
            if (DraughtRange.Length > 7)
                throw new Exception("Le tirage ne doit pas compter plus de sept lettres.");

            // 'Comme le tirage est provisoire jusqu'à ce qu'il ait été validé
            //        'on travaille sur une copie du contenu du sac
            // ReDim Tirage_Provisoire(1)
            tempBag = CopyBag(bag);

            foreach (var l in DraughtRange)
            {

                // Contrôle de la régularité des lettres du tirage (lettres A à Z uniquement et joker noté '['
                if (!((int)l >= 65 && (int)l <= 91))
                    throw new Exception("Le tirage ne doit comporter que les lettres de A à Z ou les jokers notés '[', " + l + " n'est pas admis.");

                // Contrôle de la disponibilité des lettres du tirage dans le sac
                if (tempBag[(int)l - Dictionnaire.AscShift - 1] == 0)
                    throw new Exception("Le tirage n'est pas possible, il n'y a pas assez de " + (l == '[' ? "joker" : l.ToString()) + " dans le sac.");
                else
                    tempBag[(int)l - Dictionnaire.AscShift - 1]--;

                // Validation du tirage provisoire au fur et à mesure
                // ReDim Preserve Tirage_Provisoire(1 To UBound(Tirage_Provisoire) + 1)

                tempDraught.Add(l.ToString());
            }

            // Si l'on arrive ici c'est que tirage est validé
            // Ainsi on passe de provisoire à définitif et on peut mettre à jour l'affichage du contenu du sac
            Draught = CopyDraught(tempDraught);

            bag = CopyBag(tempBag);

            loadDraught = true;
            //for (int i = 0; i < Draught.Length - 2; i++)
            //{
            //    Draught[i] = Draught[i + 1];
            //}
            // ReDim Preserve Tirage(1 To UBound(Tirage) - 1)
            DisplayBagContent();
            return loadDraught;
        }

        /// <summary>
        /// Cette procédure affiche le contenu du sac
        /// </summary>
        private List<string> DisplayBagContent()
        {
            int total = 0;
            string[] temp = new string[28];

            for (int i = 0; i < 27; i++)
            {
                temp[i] = i == 26 ? "Joker" : (char)(i + Dictionnaire.AscShift) + " : " + BagContent[i + 1];
                total += BagContent[i];
            }
            temp[27] = "Lettres : " + total;
            return temp.ToList();
        }
        /// <summary>
        /// Cette procédure copie le contenu d'un sac vers un autre
        /// </summary>
        /// <param name="tempBag"></param>
        /// <param name="bag"></param>
        private int[] CopyBag(int[] source)
        {
            var dest = new List<int>();
            dest.AddRange(source);
            return dest.ToArray();

        }
        /// <summary>
        /// Cette procédure copie un tirage vers un autre
        /// </summary>
        /// <param name="tempDraught"></param>
        /// <param name="draught"></param>
        private string[] CopyDraught(List<string> source)
        {

            var dest = new List<string>();
            dest.AddRange(source);
            return dest.ToArray();
        }

        /// <summary>
        /// Cette procédure ajoute un coup à la liste des coups admis recensés
        /// en enregistrant la place où le coup est joué
        /// s'il est joué horizontalement ou verticalement
        /// le mot formé
        /// le point que le coup rapporte
        /// </summary>
        /// <param name="move"></param>
        /// <param name="t"></param>
        private void Add(string move, Tile t)
        {
            int start = t.YLoc - move.Length;
            int multiplier = 1;
            int horizontalPoints = 0;
            int verticalPoints = 0;
            int points = 0;
            int UsedDraughtLetters = 0;
            int letterIdx = 0;
            int wildcardIndice = 0;
            foreach (var l in move)
            {
                letterIdx++;
                // Le calcul des points prend en compte les cases bonus non encore utilisées
                // Il faut noter que seules les lettres du tirage permettent de bénéficier des bonus
                // Les points verticaux ont été précalculés au moment du recensement des contrôleurs
                var startTile = Board.Grid[t.XLoc, start - letterIdx];
                if (t.IsEmpty)
                {
                    if (char.IsLetter(l))
                    {
                        horizontalPoints += Board.Alphabet.Find(c => c.Char == l).Value * (startTile.TileType == TileType.DoubleLetter ? 2 : startTile.TileType == TileType.TripleLetter ? 3 : 1);
                        verticalPoints += startTile.PossibleLetterPoints[l];
                    }
                    else
                    {
                        wildcardIndice = (int)l - Dictionnaire.AscShift - WildCardShift;
                        if (startTile.PossibleLetterPoints[(char)wildcardIndice] > 0)
                        {
                            verticalPoints += startTile.PossibleLetterPoints[(char)wildcardIndice] - Board.Alphabet.Find(c => c.Char == (char)wildcardIndice).Value
                                * (startTile.TileType == TileType.DoubleLetter ? 2 : startTile.TileType == TileType.TripleLetter ? 3 : 1);
                        }
                    }

                    multiplier *= (startTile.TileType == TileType.DoubleWord ? 2 : startTile.TileType == TileType.TripleWord ? 3 : 1);
                    UsedDraughtLetters++;
                }
                else
                {
                    if (char.IsLetter(l)) horizontalPoints += Board.Alphabet.Find(c => c.Char == l).Value;
                }

            }
            // L'utilisation des 7 lettres du tirage rapporte en plus 50 points de bonus
            points = horizontalPoints * multiplier + verticalPoints + (UsedDraughtLetters == 7 ? 50 : 0);


            nbPossibleMoves++;
            // Tri et mise en forme des coups
            AddLegalMove(Direction, t, move, points, UsedDraughtLetters == 7);

        }
        ///// <summary>
        ///// Cette procédure ajoute un coup à la liste des coups admis recensés
        ///// en enregistrant la place où le coup est joué
        ///// s'il est joué horizontalement ou verticalement
        ///// le mot formé
        ///// le point que le coup rapporte
        ///// </summary>
        ///// <param name="move"></param>
        ///// <param name="line"></param>
        ///// <param name="column"></param>
        //private void Add(string move, int line, int column)
        //{



        //    int start = column + move.Length;
        //    int multiplier = 1;
        //    int horizontalPoints = 0;
        //    int verticalPoints = 0;
        //    int points = 0;
        //    int UsedDraughtLetters = 0;
        //    int letterIdx = 0;
        //    int wildcardIndice = 0;
        //    foreach (var l in move)
        //    {
        //        letterIdx++;
        //        // Le calcul des points prend en compte les cases bonus non encore utilisées
        //        // Il faut noter que seules les lettres du tirage permettent de bénéficier des bonus
        //        // Les points verticaux ont été précalculés au moment du recensement des contrôleurs
        //        if (Grid[line, start - letterIdx] == "")
        //        {
        //            if ((int)l < 91)
        //            {
        //                horizontalPoints += LetterValue[(int)l - AscShift] * LetterMultiplier[line, start - letterIdx];
        //                verticalPoints += Controlers[line, start - letterIdx, (int)l - AscShift];
        //            }
        //            else
        //            {
        //                wildcardIndice = (int)l - AscShift - WildCardShift;
        //                if (Controlers[line, start - letterIdx, wildcardIndice] > 0)
        //                {
        //                    verticalPoints += Controlers[line, start + letterIdx, wildcardIndice] - LetterValue[wildcardIndice]
        //                        * LetterMultiplier[line, start - letterIdx] * WordMultiplier[line, start - letterIdx];
        //                }
        //            }

        //            multiplier *= WordMultiplier[line, start - letterIdx];
        //            UsedDraughtLetters++;
        //        }
        //        else
        //        {
        //            if ((int)l < 91) horizontalPoints += LetterValue[(int)l - AscShift];
        //        }


        //    }


        //    // L'utilisation des 7 lettres du tirage rapporte en plus 50 points de bonus
        //    points = horizontalPoints * multiplier + verticalPoints + (UsedDraughtLetters == 7 ? 50 : 0);


        //    nbPossibleMoves++;
        //    // Tri et mise en forme des coups
        //    AddLegalMove(Direction, line, start, move, points, UsedDraughtLetters == 7);

        //}

        private void AddLegalMove(int direction, Tile t, string word, int point, bool scramble)
        {
            // La liste des coups légaux est stockée sous forme de chaine dont les maillons représentent un coup
            // Le tri se fait par insertion : on parcourt la chaine pour repérer l'endroit où le coup doit être inséré

            Move PreviousNext;
            Move NewNext;

            if (nbAcceptedMoves >= MoveLimit && point <= MinPoint) return;

            Node = LegalMove;

            while (Node.Next != null)
            {
                if (Node.Next.Direction == direction && Node.Next.Word == word && Node.Next.Line == t.XLoc && Node.Next.Column == t.YLoc)
                {
                    nbPossibleMoves--;
                    return;
                }

                if (Node.Next.Point < point)
                {
                    nbAcceptedMoves++;
                    PreviousNext = Node.Next;
                    NewNext = new Move()
                    {
                        Direction = direction,
                        Line = t.XLoc,
                        Column = t.YLoc,
                        Word = word,
                        Point = point,
                        Scramble = scramble
                    };
                    Node.Next = NewNext;
                    NewNext.Next = PreviousNext;
                    if (nbAcceptedMoves > MoveLimit)
                    {
                        while (Node.Next.Next != null)
                        {
                            MinPoint = Node.Next.Next.Point;
                            Node = Node.Next;
                        }
                        Node.Next = null;
                        nbAcceptedMoves--;
                        return;
                    }
                    else
                    {
                        return;
                    }

                }
                Node = Node.Next;
            }
            nbAcceptedMoves++;
            MinPoint = point;
            PreviousNext = Node.Next;
            NewNext = new Move()
            {
                Direction = direction,
                Line = t.XLoc,
                Column = t.YLoc,
                Word = word,
                Point = point,
                Scramble = scramble
            };
            Node.Next = NewNext;
            NewNext.Next = PreviousNext;

            if (nbAcceptedMoves > MoveLimit)
            {
                while (Node.Next.Next != null)
                {
                    Node = Node.Next;
                }
                Node.Next = null;
                nbAcceptedMoves--;
            }
        }

        //private bool Found(long node, string letter)
        //{
        //    //Cette fonction vérifie si un arc représentant une lettre sort d'un noeud
        //    //Si l'arc existe, la fonction retourne vrai comme valeur et la variable Noeud_Actuel pointe sur le noeud où atteint par l'arc
        //    bool found;

        //    if (Dictionary[((int)letter.ToUpper()[0]) - AscShift, node] == 0)
        //        found = false;
        //    else
        //    {
        //        found = true;
        //        CurrentNode = Dictionary[((int)letter.ToUpper()[0]) - AscShift, node];
        //    }
        //    return found;
        //}


    }

    public class Move
    {
        public Move()
        {
        }
        public Move Next;
        public int Direction { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Word { get; set; }
        public int Point { get; set; }
        public bool Scramble { get; set; }
    }
}
