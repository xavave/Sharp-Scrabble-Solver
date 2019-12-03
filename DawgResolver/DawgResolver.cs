using DawgResolver;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;


namespace Dawg
{
    [Serializable]
    public class Resolver
    {
        //const definition 

        const int WildCardShift = 32;
        const int MoveLimit = 50;

        const int BoardSize = 15;
        //variables definition

        public int WordCount { get; set; }
        public int NodeCount { get; private set; }

        bool FirstMove { get; set; }
        static bool[,] Anchors { get; set; } = new bool[17, 17];
        bool PossibleMove { get; set; }

        static int[,,] AnchorLimits { get; set; } = new int[17, 17, 2];
        Tile[,] Temp1 { get; set; } = new Tile[Game.BoardSize, Game.BoardSize];
        Tile[,] Temp2 { get; set; } = new Tile[Game.BoardSize, Game.BoardSize];
        int[,,] Controlers { get; set; } = new int[17, 17, 27];
        int Direction { get; set; }

        int MoveNumber { get; set; }
        public int PlayedLetters { get; private set; }
        int MinPoint { get; set; }
        long[,] Dictionary { get; set; }
        long nbPossibleMoves { get; set; }
        long nbMoves { get; set; }
        long nbAcceptedMoves { get; set; }
        Tile[,] BoardCopy { get; set; } = new Tile[Game.BoardSize, Game.BoardSize];
        //string[,] DisplayBoard { get; set; } = new string[Game.BoardSize, Game.BoardSize];

        string[,] LegalMoves { get; set; }
        static Move Node;
        Move LegalMove = new Move();

        Game Game { get; }

        public Resolver(Game g)
        {
            Game = g;

            LoadDic();
        }

        public Dictionnaire Dictionnaire { get; private set; }
        public Noeud Noeud { get; private set; }

        //Tile[,] CopyBoard(Tile[,] source)
        //{
        //    var dest = new Tile[source.GetUpperBound(0) + 1, source.GetUpperBound(1) + 1];
        //    Array.Copy(source, 0, dest, 0, source.Length);
        //    return dest;
        //}
        /// <summary>
        /// Cette procédure génère un nouveau tirage
        /// en choisissant au hasard parmi les lettres disponibles dans le sac
        /// </summary>
        public List<Letter> NewDraught(Player p, string forcedLetters = null)
        {
            if (p.Rack.Count >= 7) return p.Rack;

            if (!string.IsNullOrWhiteSpace(forcedLetters))
            {
                p.Rack = forcedLetters.Select(c => Game.Alphabet.Find(a => a.Char == c)).ToList();
                return p.Rack;
            }
            // Si le sac est vide
            if (Game.Bag.LeftLetters == 0)
                throw new Exception("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes

            Random rnd = new Random();
            // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
            for (int i = 0; i < Math.Min(Game.Bag.LeftLetters, 7); i++)
            {
                int letterIdx = rnd.Next(0, Game.Alphabet.Count - 1);
                var letter = Bag.Letters.GetRandomLetter();
                p.Rack.Add(letter);

            }


            Debug.WriteLine(p.DisplayRack());

            return p.Rack;

        }

        private void DetectTiles(Player p, Tile[,] grid)
        {
            string[] Temp;
            LegalMove = new Move()
            {
                Point = 32767
            };

            MinPoint = 0;
            nbAcceptedMoves = 0;

            //Game.Grid = RealBoard.DeepClone();
            Direction = 0;
            Game.AnchorCount = 0;

            // On efface les informations précédentes en vue d'une nouvelle analyse
            foreach (var t in grid)
            {
                t.IsAnchor = false;
                t.PrefixMinSize = 0;
                t.PrefixMaxSize = 0;
                t.PossibleLetterPoints = new Dictionary<char, int>(27);
            }

            // Le premier est un cas particulier où aucune lettre n'est encore posée sur le plateau
            // Tous les mots formés doivent toucher la case centrale qui constitue ainsi la seule ancre
            // Les mots formés au premier peuvent commencer six cases au plus vers la gauche de la case centrale
            // Comme aucune lettre n'est encore présente sur le plateau, il n'y a aucune vérification à faire
            // par rapport aux mots formés verticalement (les 26 lettres peuvent être utilisées)

            if (Game.FirstMove)
            {
                grid[7, 7].IsAnchor = true;
                grid[7, 7].PrefixMinSize = 0;
                grid[7, 7].PrefixMaxSize = 6;

                //AnchorLimits[7, 7, 0] = 0;
                //AnchorLimits[7, 7, 1] = 6;
                foreach (var t in grid)
                {
                    t.PossibleLetterPoints['z'] = 26;
                }

            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                FindAnchors();
                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valide
                FindControlers();
            }
        }
        public List<Word> FindMove(Player p)
        {
            DetectTiles(p, Game.Grid);

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            FindMovesPerAnchor(p);

            // Recherche des coups verticaux
            // par transposition de la grille, on refait tout le processus
            BoardCopy = TransposeGrid(Game.Grid);
            Direction = 1;
            DetectTiles(p, BoardCopy);

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            FindMovesPerAnchor(p);

            // Mise en forme des coups enregistrés et affichage

            //LegalMoves = new string[6, nbAcceptedMoves == 0 ? 1 : nbAcceptedMoves];

            //Node = LegalMove.Next;
            //int i = 0;
            //while (Node != null)
            //{
            //    LegalMoves[0, i] = Node.Direction.ToString();
            //    LegalMoves[1, i] = Node.Direction == 0 ? Node.Line.ToString() : Node.Column.ToString();
            //    LegalMoves[2, i] = Node.Direction == 1 ? Node.Line.ToString() : Node.Column.ToString();
            //    LegalMoves[3, i] = Node.Word;
            //    LegalMoves[4, i] = Node.Point.ToString();
            //    LegalMoves[5, i] = Node.Scramble ? "*" : "";
            //    Node = Node.Next;
            //    i++;
            //}
            //ClearNode(ref LegalMove); // Pour libérer la mémoire et prévenir une erreur de gestion de pile


            //for (i = 0; i <= LegalMoves.GetUpperBound(1); i++)
            //{
            //    if (LegalMoves[0, i] != null)
            //        if (LegalMoves[0, i] == "0")
            //            Temp[i] = (char)(int.Parse(LegalMoves[1, i]) + Dictionnaire.AscShift + 1) + (int.Parse(LegalMoves[2, i]) + 1).ToString() + " - " + LegalMoves[3, i] + " = " + LegalMoves[4, i] + LegalMoves[5, i];
            //        else
            //            Temp[i] = (int.Parse(LegalMoves[2, i]) + 1).ToString() + (char)(int.Parse(LegalMoves[1, i]) + 1 + Dictionnaire.AscShift) + " - " + LegalMoves[3, i] + " = " + LegalMoves[4, i] + LegalMoves[5, i];

            //}

            // Cas où aucun coup légal n'a été trouvé
            //var ret = !LegalWords.Any() ? new string[] { "Aucun coup legal" } : LegalWords.Select(t => t.Text);

            Debug.WriteLine(string.Join(Environment.NewLine, LegalWords.Select(t => t.Text)));
            if (nbPossibleMoves > 0) PossibleMove = true;
            nbMoves = nbPossibleMoves;
            nbPossibleMoves = 0;
            return LegalWords.ToList();
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
            var myWordStart = new Word(Game);
            var myWordEnd = new Word(Game);
            foreach (var t in Game.Grid)
            {
                if (t.IsAnchor)
                {
                    int points = 0;
                    var tileCpy = new Tile(Game, t.XLoc, t.YLoc)
                    {
                        PossibleLetterPoints = t.PossibleLetterPoints,
                        Letter = t.Letter,
                        IsAnchor = t.IsAnchor,
                        TileType = t.TileType,
                        PrefixMaxSize = t.PrefixMaxSize,
                        PrefixMinSize = t.PrefixMinSize,
                    };
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
                    foreach (var c in Game.Alphabet)
                    {
                        var currentWord = new Word(Game) { Tiles = myWordStart.Tiles };
                        currentWord.Tiles.Append(t);
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


        }


        /// <summary>
        /// Cette procédure repère les ancres ligne par ligne
        /// Il s'agit des cases inocupées adjacentes à une case déjà occupée
        /// Ce recensement est utile afin de limiter les recherches aux cases où l'on peut jouer des coups
        /// </summary>
        private void FindAnchors()
        {

            foreach (var t in Game.Grid)
            {
                t.IsAnchor = (t.UpTile != null && !t.UpTile.IsEmpty) || (t.DownTile != null && !t.DownTile.IsEmpty) || (t.RightTile != null && !t.RightTile.IsEmpty) || (t.LeftTile != null && !t.LeftTile.IsEmpty);
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
                    var tileCpy = new Tile(Game, t.XLoc, t.YLoc)
                    {
                        PossibleLetterPoints = t.PossibleLetterPoints,
                        Letter = t.Letter,
                        IsAnchor = t.IsAnchor,
                        TileType = t.TileType,
                        PrefixMaxSize = t.PrefixMaxSize,
                        PrefixMinSize = t.PrefixMinSize,
                    };
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
                    while (tileCpy.LeftTile != null && tileCpy.LeftTile.IsEmpty && !tileCpy.LeftTile.IsAnchor)
                    {
                        tileCpy = tileCpy.LeftTile;
                        cptPrefix++;
                    }
                    t.PrefixMinSize = 0;
                    t.PrefixMaxSize = cptPrefix;

                }

            }
            Game.AnchorCount = Game.Grid.OfType<Tile>().Count(t => t.IsAnchor);


        }

        private Tile[,] TransposeGrid(Tile[,] source)
        {
            Tile[,] dest = new Tile[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    dest[i, j] = source[j, i];
                }
            }
            return dest;
        }

        private void FindMovesPerAnchor(Player p)
        {

            Noeud root = Game.Dico.DAWG;


            // Pour chaque ancre identifiée,
            // Cette procédure lance la recherche des différents mots formables à partir de l'ancre
            // en utilisant les mots du tirage et ceux déjà placés
            // en respectant la taille limite des préfixes précédement calculés
            foreach (var t in Game.Grid.OfType<Tile>().Where(t => t.IsAnchor).ToList())
            {
                var prefixe = "";
                // Cas où la taille du préfixe est imposée
                if (t.PrefixMinSize == t.PrefixMaxSize)
                {
                    // Cas du préfixe vide
                    // On ne peut que former un nouveau mot vers la droite
                    if (t.PrefixMinSize == 0)
                    {
                        ExtendRight("", p.Rack, 1, root, t, p.Rack.Count);
                    }
                    else
                    {
                        // Cas où le préfixe est déjà placé sur la grille
                        // On ne peut que continuer le préfixe vers la droite
                        for (int k = t.YLoc - t.PrefixMinSize; k <= t.YLoc - 1; k++)
                        {
                            if (Game.Dico.HasWordsStartingWith(prefixe + Game.Grid[t.XLoc, k].Letter))
                                prefixe += Game.Grid[t.XLoc, k].Letter;
                        }

                        ExtendRight(prefixe.ToUpper(), p.Rack, 1, root, t, p.Rack.Count);
                    }
                }
                else
                {
                    // Cas où k cases vides non identifiés comme ancres se trouvent devant l'ancre
                    // On essaie les différents préfixes possibles allant de 0 à k lettres
                    for (int minA = t.PrefixMinSize; minA <= t.PrefixMaxSize; minA++)
                    {

                        ExtendRight("", p.Rack, minA, root, t, p.Rack.Count);
                    }
                }

            }


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
        private void ExtendRight(string prefix, List<Letter> leftLetters, int minSize, Noeud node, Tile t, int nbLetters)
        {

            // If Controleurs(Ligne, Colonne, 27) = 0 And Grille(Ligne, Colonne) = "" Then Exit Sub


            //If Timer -Debut > Limite_Temps Then Exit Sub
            bool jokerInDraught = leftLetters.Take(nbLetters).Any(l => l.Char == Game.Joker);

            if (t.IsEmpty)
            {

                // Si une case vide, on peut la remplir avec une lettre du tirage sous certaines conditions
                if (Game.Dico.MotAdmis(prefix) && prefix.Length > 1 && prefix.Length > minSize && nbLetters <= leftLetters.Count)
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
                foreach (var c in Game.Alphabet.Take(26).Select(a => a.Char))
                {
                    if (Game.Dico.HasWordsStartingWith(prefix + c))
                    {
                        for (int la = 0; la < nbLetters; la++)
                        {
                            if (leftLetters[la].Char == c)
                            {
                                if ((t.PossibleLetterPoints.ContainsKey(char.ToUpper(c)) && t.PossibleLetterPoints[c] > 0) || t.PossibleLetterPoints['z'] == 26)
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
                                    ExtendRight(prefix + c, leftLetters, minSize, node, t, nbLetters);

                                    // Au retour de l'appel recursif on restitue la lettre dans le tirage
                                    nbLetters++;
                                    leftLetters[nbLetters - 1] = Game.Alphabet.Find(ch => ch.Char == c);
                                }

                                if (!jokerInDraught) continue;
                            }
                            else if (leftLetters[la].Char == Game.Joker)
                            {
                                // Cas d'un joker
                                if ((t.PossibleLetterPoints.ContainsKey(c) && t.PossibleLetterPoints[c] > 0) || t.PossibleLetterPoints['z'] == 26)
                                {
                                    // Si une lettre quelconque (représentée par le joker) permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // le joker utilisé est alors retiré du tirage
                                    leftLetters[la] = leftLetters[nbLetters - 1];
                                    nbLetters--;

                                    // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                                    ExtendRight(prefix + c, leftLetters, minSize, node, t, nbLetters);


                                    // Au retour de l'appel recursif on restitue le joker dans le tirage
                                    nbLetters++;
                                    leftLetters[nbLetters - 1] = Game.Alphabet.Find(ch => ch.Char == Game.Joker);
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                if (Game.Dico.HasWordsStartingWith(prefix + t.Letter))
                {
                    ExtendRight(prefix + t.Letter, leftLetters, minSize, node, t, nbLetters);
                }


            }


        }
        /// <summary>
        /// Recherche des différents coups admis
        /// </summary>
        public void FindMoves(Player p)
        {

            PossibleMove = false;
            Debug.WriteLine(FindLegalMoves(p));
            if (PossibleMove)
            {
                //       Bouton_Jouer_Coup.Enabled = True
                //Bouton_Chercher_Coups.Enabled = False
            }
            else
            {
                //       Bouton_Chercher_Coups.Enabled = True
                //Bouton_Jouer_Coup.Enabled = False
                if (p.Rack.Any())
                {
                    // Les lettres non utilisées sont "reversées dans le sac"
                    foreach (var l in p.Rack)
                        Bag.Letters.Add(l);
                }
            }
        }

        private string FindLegalMoves(Player p)
        {

            // Cette procédure appelle la rechercher des coups admis
            // Après vérification de la régularité du tirage
            //Start = Timer

            //if (!LoadDraught(Bag.Letters))
            //{
            //    //Bouton_Chercher_Coups.Enabled = True
            //    //Bouton_Jouer_Coup.Enabled = False
            //}

            var ret = FindMove(p);
            return ret.Count() + " coups trouvés ";// en " + (Int((Timer - Debut) * 100)) / 100 & " s."


            //If Timer -Debut > Limite_Temps Then MsgBox "Recherche intérrompue car le temps limite a été dépassé", vbInformation


        }
        public Noeud LoadDic()
        {
            Dictionnaire = new Dictionnaire();

            Noeud = Dictionnaire.ChargerFichierDAWG();

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



        //private bool LoadDraught(List<Letter> bag)
        //{
        //    // Cette fonction vérifie la régularité du tirage proposé, retourne Vrai le cas échéant et charge le tirage en mémoire

        //    bool loadDraught = false;
        //    int[] tempBag = new int[26];
        //    List<string> tempDraught = new List<string>();

        //    //Cas d'un tirage vide
        //    if (!Player.Rack.Any())
        //        throw new Exception("Le tirage est vide.");

        //    //Cas d'un tirage contenant plus de 7 lettres
        //    if (Player.Rack.Count > 7)
        //        throw new Exception("Le tirage ne doit pas compter plus de sept lettres.");

        //    // 'Comme le tirage est provisoire jusqu'à ce qu'il ait été validé
        //    //        'on travaille sur une copie du contenu du sac
        //    // ReDim Tirage_Provisoire(1)
        //    tempBag = CopyBag(bag);

        //    foreach (var l in Player.Rack)
        //    {

        //        // Contrôle de la régularité des lettres du tirage (lettres A à Z uniquement et joker noté '['
        //        if (!Char.IsLetter(l.Char) || l.Char != Game.Joker)
        //            throw new Exception($"Le tirage ne doit comporter que les lettres de A à Z ou les jokers notés '{Game.Joker}', " + l.Char + " n'est pas admis.");

        //        // Contrôle de la disponibilité des lettres du tirage dans le sac
        //        if (Bag.Letters.Where(le => le.Char == l.Char).Sum(s => s.Count) == 0)
        //            throw new Exception($"Le tirage n'est pas possible, il n'y a pas assez de " + (l.Char == Game.Joker ? "joker" : l.Char.ToString()) + " dans le sac.");
        //        else
        //            tempBag[(int)l - Dictionnaire.AscShift - 1]--;

        //        // Validation du tirage provisoire au fur et à mesure
        //        // ReDim Preserve Tirage_Provisoire(1 To UBound(Tirage_Provisoire) + 1)

        //        tempDraught.Add(l.ToString());
        //    }

        //    // Si l'on arrive ici c'est que tirage est validé
        //    // Ainsi on passe de provisoire à définitif et on peut mettre à jour l'affichage du contenu du sac
        //    Draught = CopyDraught(tempDraught);

        //    bag = CopyBag(tempBag);

        //    loadDraught = true;
        //    //for (int i = 0; i < Draught.Length - 2; i++)
        //    //{
        //    //    Draught[i] = Draught[i + 1];
        //    //}
        //    // ReDim Preserve Tirage(1 To UBound(Tirage) - 1)
        //    DisplayBagContent();
        //    return loadDraught;
        //}

        ///// <summary>
        ///// Cette procédure affiche le contenu du sac
        ///// </summary>
        //private List<string> DisplayBagContent()
        //{
        //    int total = 0;
        //    string[] temp = new string[28];

        //    for (int i = 0; i < 27; i++)
        //    {
        //        temp[i] = i == 26 ? "Joker" : (char)(i + Dictionnaire.AscShift) + " : " + BagContent[i + 1];
        //        total += BagContent[i];
        //    }
        //    temp[27] = "Lettres : " + total;
        //    return temp.ToList();
        //}
        ///// <summary>
        ///// Cette procédure copie le contenu d'un sac vers un autre
        ///// </summary>
        ///// <param name="tempBag"></param>
        ///// <param name="bag"></param>
        //private int[] CopyBag(int[] source)
        //{
        //    var dest = new List<int>();
        //    dest.AddRange(source);
        //    return dest.ToArray();

        //}
        ///// <summary>
        ///// Cette procédure copie un tirage vers un autre
        ///// </summary>
        ///// <param name="tempDraught"></param>
        ///// <param name="draught"></param>
        //private string[] CopyDraught(List<string> source)
        //{

        //    var dest = new List<string>();
        //    dest.AddRange(source);
        //    return dest.ToArray();
        //}

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
                var startTile = Game.Grid[t.XLoc, start + letterIdx];
                if (t.IsEmpty)
                {
                    if (char.IsLetter(l))
                    {
                        horizontalPoints += Game.Alphabet.Find(c => c.Char == l).Value * (startTile.TileType == TileType.DoubleLetter ? 2 : startTile.TileType == TileType.TripleLetter ? 3 : 1);
                        verticalPoints += startTile.PossibleLetterPoints.ContainsKey(l) ? startTile.PossibleLetterPoints[l] : 0;
                    }
                    else
                    {
                        wildcardIndice = (int)l - Dictionnaire.AscShift - WildCardShift;
                        if (startTile.PossibleLetterPoints[(char)wildcardIndice] > 0)
                        {
                            verticalPoints += (startTile.PossibleLetterPoints.ContainsKey((char)wildcardIndice) ? startTile.PossibleLetterPoints[(char)wildcardIndice] : 0) - Game.Alphabet.Find(c => c.Char == (char)wildcardIndice).Value
                                * (startTile.TileType == TileType.DoubleLetter ? 2 : startTile.TileType == TileType.TripleLetter ? 3 : 1);
                        }
                    }

                    multiplier *= (startTile.TileType == TileType.DoubleWord ? 2 : startTile.TileType == TileType.TripleWord ? 3 : 1);
                    UsedDraughtLetters++;
                }
                else
                {
                    if (char.IsLetter(l)) horizontalPoints += Game.Alphabet.Find(c => c.Char == l).Value;
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
        public List<Word> LegalWords = new List<Word>();
        /// <summary>
        /// La liste des coups légaux est stockée sous forme de chaine dont les maillons représentent un coup
        /// Le tri se fait par insertion : on parcourt la chaine pour repérer l'endroit où le coup doit être inséré
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="t"></param>
        /// <param name="word"></param>
        /// <param name="point"></param>
        /// <param name="scramble"></param>
        private void AddLegalMove(int direction, Tile t, string word, int point, bool scramble)
        {

            //Move PreviousNext;
            //Move NewNext;

            if (nbAcceptedMoves >= MoveLimit || point <= MinPoint) return;

            if (!LegalWords.Any(w => w.Direction == direction && w.Text == word && w.Tiles[0].XLoc == t.XLoc && w.Tiles[0].YLoc == t.YLoc))
            {
                var tiles = new List<Tile>();

                foreach (var c in word)
                {
                    var tile = t;
                    if (direction == 0)
                    {
                        tiles.Add(tile);
                        tile = tile.RightTile;
                    }
                }

                LegalWords.Add(new Word(Game)
                {
                    Tiles = tiles,
                    Direction = direction,
                    Text = word,

                });
                nbAcceptedMoves++;
            }
            //Node = LegalMove;

            //while (Node.Next != null)
            //{
            //    if (Node.Next.Direction == direction && Node.Next.Word == word && Node.Next.Line == t.XLoc && Node.Next.Column == t.YLoc)
            //    {
            //        nbPossibleMoves--;
            //        return;
            //    }

            //    if (Node.Next.Point < point)
            //    {
            //        nbAcceptedMoves++;
            //        PreviousNext = Node.Next;
            //        NewNext = new Move()
            //        {
            //            Direction = direction,
            //            Line = t.XLoc,
            //            Column = t.YLoc,
            //            Word = word,
            //            Point = point,
            //            Scramble = scramble
            //        };
            //        Node.Next = NewNext;
            //        NewNext.Next = PreviousNext;
            //        if (nbAcceptedMoves > MoveLimit)
            //        {
            //            while (Node.Next.Next != null)
            //            {
            //                MinPoint = Node.Next.Next.Point;
            //                Node = Node.Next;
            //            }
            //            Node.Next = null;
            //            nbAcceptedMoves--;
            //            return;
            //        }
            //        else
            //        {
            //            return;
            //        }

            //    }
            //    Node = Node.Next;
            //}
            //nbAcceptedMoves++;
            //MinPoint = point;
            //PreviousNext = Node.Next;
            //NewNext = new Move()
            //{
            //    Direction = direction,
            //    Line = t.XLoc,
            //    Column = t.YLoc,
            //    Word = word,
            //    Point = point,
            //    Scramble = scramble
            //};
            //Node.Next = NewNext;
            //NewNext.Next = PreviousNext;

            //if (nbAcceptedMoves > MoveLimit)
            //{
            //    while (Node.Next.Next != null)
            //    {
            //        Node = Node.Next;
            //    }
            //    Node.Next = null;
            //    nbAcceptedMoves--;
            //}
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
    [Serializable]
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
