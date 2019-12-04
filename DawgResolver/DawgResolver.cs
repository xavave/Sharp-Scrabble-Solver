﻿using DawgResolver;

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

        MovementDirection Direction { get; set; }


        public int PlayedLetters { get; private set; }
        int MinPoint { get; set; }
        long[,] Dictionary { get; set; }
        long nbPossibleMoves { get; set; }

        long nbAcceptedMoves { get; set; }
        Tile[,] BoardCopy { get; set; } = new Tile[Game.BoardSize, Game.BoardSize];
        //string[,] DisplayBoard { get; set; } = new string[Game.BoardSize, Game.BoardSize];



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

        private Tile[,] DetectTiles(Player p, Tile[,] grid)
        {
            //Game.Grid = RealBoard.DeepClone();

            //On efface les informations précédentes en vue d'une nouvelle analyse
            foreach (var t in grid.OfType<Tile>().Where(ti => ti != null))
            {
                t.IsAnchor = false;
                t.AnchorLimit1 = 0;
                t.AnchorLimit2 = 0;
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
                grid[7, 7].AnchorLimit1 = 0;
                grid[7, 7].AnchorLimit2 = 6;

                foreach (var t in grid.OfType<Tile>().Where(ti => ti != null))
                {
                    t.PossibleLetterPoints[Game.Joker] = 26;
                }
                Game.PrintGrid(grid, true);
            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                grid = FindAnchors(grid);
                //Game.PrintGridConsole(grid, true);
                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valide

                grid = FindControlers(grid);
                Game.PrintGrid(grid, true);
            }
            return grid;
        }
        private List<Word> FindMove(Player p, MovementDirection direction)
        {

            var grid = DetectTiles(p, Game.Grid);

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            grid = FindMovesPerAnchor(p, grid, direction);
            Game.PrintGrid(grid, true);
            //// Recherche des coups verticaux
            //// par transposition de la grille, on refait tout le processus
            //grid = TransposeGrid(grid);
            //Direction = MovementDirection.Down;
            //grid = DetectTiles(p, grid);

            ////// Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            //grid = FindMovesPerAnchor(p, grid);

            return LegalWords.OrderByDescending(t => t.Points).ToList();
        }
        //void ClearNode(ref Move move)
        //{
        //    if (move.Next != null) ClearNode(ref move.Next);
        //    move = null;
        //}

        /// <summary>
        /// Cette procédure permet d'identifier les lettres qu'on peut jouer dans une case donnée
        /// en vérifiant si l'ajout d'un mot dans le sens horizontal permet de former également des mots valides
        /// dans le sens vertical. Ce travail est effectué avant la recherche proprement dite pour limiter les possibilités à tester
        /// on en profite également pour précalculer le point rapporté par les mots formés verticalement
        /// </summary>
        private Tile[,] FindControlers(Tile[,] grid)
        {
            int L = 0;

            foreach (var t in grid.OfType<Tile>())
            {
                if (t.IsAnchor)
                {
                    var wordStart = "";
                    var wordEnd = "";
                    int points = 0;
                    var tileCpy = t.Copy();

                    //On rassemble le début éventuel des mots (lettres situées au dessus de la case)
                    while (tileCpy.UpTile != null && !tileCpy.UpTile.IsEmpty)
                    {
                        tileCpy = tileCpy.UpTile;
                        wordStart += tileCpy.Letter.Char;
                        points += tileCpy.Letter.Value;

                    }
                    wordStart = string.Join("", wordStart.Reverse());
                    wordEnd = "";
                    //On rassemble la fin éventuelle des mots (lettres situées en dessous de la case)
                    while (tileCpy.DownTile != null && !tileCpy.DownTile.IsEmpty)
                    {
                        tileCpy = tileCpy.DownTile;
                        wordEnd += tileCpy.Letter.Char;
                        points += tileCpy.Letter.Value;

                    }
                    //On vérifie pour chaque Lettre L de A à Z si Debut+L+Fin forme un mot valide
                    //Si tel est le cas, la lettre L est jouable pour la case considérée
                    //et on précalcule le point que le mot verticalement formé permettrait de gagner si L était jouée
                    L = 0;
                    foreach (var c in Game.Alphabet.Take(26))
                    {
                        var currentWord = (wordStart + c + wordEnd).ToUpper();
                        if (currentWord.Length > 1 && Game.Dico.MotAdmis(currentWord))
                        {
                            t.PossibleLetterPoints[c.Char] = points + c.Value * t.LetterMultiplier * t.WordMultiplier;
                            L++;
                        }
                        else
                            t.PossibleLetterPoints[c.Char] = 0;

                    }
                    //Si aucune lettre ne se trouve ni au dessus ni en dessous de la case, il n'y aucune contrainte à respecter
                    //toutes les lettres peuvent être placées dans la case considérée.
                    if (string.IsNullOrWhiteSpace(wordStart + wordEnd))
                        t.PossibleLetterPoints[Game.Joker] = 26;
                    else
                        t.PossibleLetterPoints[Game.Joker] = L;
                }
                else
                    if (t.IsEmpty)
                    t.PossibleLetterPoints[Game.Joker] = 26;
            }

            return grid;
        }


        /// <summary>
        /// Cette procédure repère les ancres ligne par ligne
        /// Il s'agit des cases inocupées adjacentes à une case déjà occupée
        /// Ce recensement est utile afin de limiter les recherches aux cases où l'on peut jouer des coups
        /// </summary>
        private Tile[,] FindAnchors(Tile[,] grid)
        {

            foreach (var t in grid.OfType<Tile>().Where(ti => ti.IsEmpty))
            {
                t.IsAnchor = (t.UpTile != null && !t.UpTile.IsEmpty) || (t.DownTile != null && !t.DownTile.IsEmpty) || (t.RightTile != null && !t.RightTile.IsEmpty) || (t.LeftTile != null && !t.LeftTile.IsEmpty);
                // Cas où l'ancre est à l'extrème gauche du plateau, la taille du préfixe est exactement 0
                // Cas où l'ancre est précédée d'une autre ancre, la taille du préfixe est exactement 0
                if (t.IsAnchor)
                {
                    var tileCpy = t.Copy();

                    int cptPrefix = 0;

                    if (t.Col == 0 || t.LeftTile.IsAnchor)
                    {
                        t.AnchorLimit2 = 0;
                        t.AnchorLimit1 = 0;
                    }
                    else if (!t.LeftTile.IsEmpty)
                    {
                        // Cas où l'ancre est précédée d'une case occupée, la taille du préfixe est exactement également la taille du mot
                        // ou de la chaîne qui précède l'ancre

                        while (tileCpy != null && tileCpy.LeftTile != null && !tileCpy.LeftTile.IsEmpty)
                        {
                            tileCpy = tileCpy.UpTile;
                            cptPrefix++;
                        }
                        t.AnchorLimit2 = cptPrefix;
                        t.AnchorLimit1 = cptPrefix;
                    }
                    else if (t.LeftTile.IsEmpty)
                    {
                        // Cas où l'ancre est précédée par un case vide,
                        // la taille du préfixe varie de 0 à k où k représente le nombre de cases vides non identifiées comme des ancres
                        cptPrefix = 0;
                        while (tileCpy != null && tileCpy.LeftTile != null && !tileCpy.LeftTile.IsAnchor)
                        {
                            tileCpy = tileCpy.LeftTile;
                            if (tileCpy.IsEmpty) cptPrefix++;
                        }
                    }
                    t.AnchorLimit1 = 0;
                    t.AnchorLimit2 = cptPrefix;
                }

            }

            return grid;

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

        int NoeudActuel = 1;
        /// <summary>
        /// Pour chaque ancre identifiée,
        /// Cette procédure lance la recherche des différents mots formables à partir de l'ancre
        /// en utilisant les mots du tirage et ceux déjà placés
        /// en respectant la taille limite des préfixes précédement calculés
        /// </summary>
        /// <param name="p"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private Tile[,] FindMovesPerAnchor(Player p, Tile[,] grid, MovementDirection direction)
        {
            var leftLetters = p.Rack;
            var nbLetters = leftLetters.Count;

            foreach (var t in grid.OfType<Tile>().Where(t => t.IsAnchor).OrderBy(o => o.Ligne))
            {

                // Cas où la taille du préfixe est imposée
                if (t.AnchorLimit2 == t.AnchorLimit1)
                {
                    // Cas du préfixe vide
                    // On ne peut que former un nouveau mot vers la droite
                    if (t.AnchorLimit1 == 0)
                    {
                        ExtendRight(grid, "", ref leftLetters, 1, 1, t.Ligne, t.Col, ref nbLetters, direction);
                    }
                    else
                    {
                        // Cas où le préfixe est déjà placé sur la grille
                        // On ne peut que continuer le préfixe vers la droite
                        NoeudActuel = 1;
                        string prefixe = "";
                        for (int k = t.Col - t.AnchorLimit1; k < t.Col; k++)
                        {
                            prefixe += Game.Grid[t.Ligne, k].Letter;
                            var trouv = Trouve(Game.Grid[t.Ligne, k].Letter.Char, NoeudActuel);
                        }
                        ExtendRight(grid, prefixe.ToUpper(), ref leftLetters, 1, NoeudActuel, t.Ligne, t.Col, ref nbLetters, direction);
                    }
                }
                else
                {
                    // Cas où k cases vides non identifiées comme ancres se trouvent devant l'ancre
                    // On essaie les différents préfixes possibles allant de 0 à k lettres
                    for (int k = 0; k <= t.AnchorLimit2; k++)
                    {
                        ExtendRight(grid, "", ref leftLetters, k, 1, t.Ligne, t.Col - k, ref nbLetters, direction);
                    }
                }

            }
            return grid;

        }
        /// <summary>
        /// Cette fonction vérifie si un arc représenant une lettre sort d'un noeud
        /// Si l'arc existe, la fonction retourne vrai comme valeur et la variable Noeud_Actuel pointe sur le noeud où atteint par l'arc
        /// </summary>
        /// <param name="noeud_Actuel"></param>
        /// <param name="tile"></param>
        private bool Trouve(int lettre, int noeud)
        {
            var trouve = false;
            if (Game.Dico.Legacy[lettre - Dictionnaire.AscShift, noeud] == 0)
                return trouve;
            else
                trouve = true;
            NoeudActuel = Game.Dico.Legacy[lettre - Dictionnaire.AscShift, noeud];
            return trouve;
        }

        /// <summary>
        /// Cette procédure teste les différentes combinaisons possibles
        /// pour continuer un préfixe vers la droite
        /// </summary>
        /// <param name="partialWord"></param>
        /// <param name="leftLetters"></param>
        /// <param name="minSize"></param>
        /// <param name="node"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="nbLetters"></param>
        private void ExtendRight(Tile[,] grid, string partialWord, ref List<Letter> leftLetters, int minSize, int noeud, int ligne, int colonne, ref int nbLetters, MovementDirection direction)
        {
            bool jokerInDraught = leftLetters.Take(nbLetters).Any(l => l.Char == Game.Joker);

            Tile t = grid[ligne, colonne];

            if (t.IsEmpty)
            {
                // Si une case vide, on peut la remplir avec une lettre du tirage sous certaines conditions
                if (Game.Dico.Legacy[27, noeud] != 0 && partialWord.Length > minSize && partialWord.Length > 1 && nbLetters < leftLetters.Count())
                {   // Si le préfixe constitue déjà un mot valide
                    // alors on peut rajouter le préfixe dans la liste des coups admis
                    //t.Letter = Game.Alphabet.Find(l => l.Char == prefix[0]);
                    Add(grid, partialWord, t, direction);
                }

                for (int i = 1; i <= 26; i++)
                {

                    if (Game.Dico.Legacy[i, noeud] != 0)
                        for (int j = 0; j < nbLetters; j++)
                        {
                            if (char.ToUpper(leftLetters[j].Char) == (char)(i + Dictionnaire.AscShift))
                            {
                                if (((t.PossibleLetterPoints.ContainsKey((char)(i + Dictionnaire.AscShift)) && t.PossibleLetterPoints[(char)(i + Dictionnaire.AscShift)] > 0)) || t.PossibleLetterPoints[Game.Joker] == 26)
                                // Pour chacune des lettres qui répondent au précédent critère dans le tirage
                                //if (Controlers[line, column, letterIdx] > 0 || Controlers[line, column, 26] == 26)
                                {
                                    // Si la lettre permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // la lettre utilisée est alors retirée du tirage
                                    leftLetters[j] = leftLetters[nbLetters - 1];
                                    nbLetters--;
                                    // De manière récursive, on essaye de continuer le nouveau préfixe vers la droite
                                    ExtendRight(grid, partialWord + (char)(i + Dictionnaire.AscShift), ref leftLetters, minSize, Game.Dico.Legacy[i, noeud], ligne, colonne + 1, ref nbLetters, direction);

                                    // Au retour de l'appel recursif on restitue la lettre dans le tirage
                                    nbLetters++;
                                    leftLetters[nbLetters - 1] = Game.Alphabet.Find(ch => ch.Char == (char)(i + Dictionnaire.AscShift));
                                }

                                if (!jokerInDraught)
                                    break;
                            }
                            else if (leftLetters[j].Char == Game.Joker)
                            {
                                // Cas d'un joker
                                if (((t.PossibleLetterPoints.ContainsKey((char)(i + Dictionnaire.AscShift)) && t.PossibleLetterPoints[(char)(i + Dictionnaire.AscShift)] > 0)) || t.PossibleLetterPoints[Game.Joker] == 26)
                                {
                                    // Si une lettre quelconque (représentée par le joker) permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // le joker utilisé est alors retiré du tirage
                                    leftLetters[j] = leftLetters[nbLetters - 1];
                                    nbLetters--;

                                    // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                                    ExtendRight(grid, partialWord + char.ToLower((char)(i + Dictionnaire.AscShift)), ref leftLetters, minSize, Game.Dico.Legacy[i, noeud], ligne, colonne + 1, ref nbLetters, direction);


                                    // Au retour de l'appel recursif on restitue le joker dans le tirage
                                    nbLetters++;
                                    leftLetters[nbLetters - 1] = Game.Alphabet.Find(ch => ch.Char == Game.Joker);
                                }
                            }
                        }

                }
            }
            else
            {
                if (Game.Dico.Legacy[(int)char.ToUpper(t.Letter.Char) - Dictionnaire.AscShift, noeud] != 0)
                {
                    ExtendRight(grid, partialWord + t.Letter.Char, ref leftLetters, 1, Game.Dico.Legacy[(int)char.ToUpper(t.Letter.Char) - Dictionnaire.AscShift, noeud], ligne, colonne + 1, ref nbLetters, direction);
                }

            }


        }
        /// <summary>
        /// Recherche des différents coups admis
        /// </summary>
        public List<Word> FindMoves(Player p)
        {

            var ret = FindMove(p, MovementDirection.Across);

            if (p.Rack.Any())
            {
                // Les lettres non utilisées sont "reversées dans le sac"
                foreach (var l in p.Rack)
                    Bag.Letters.Add(l);
            }
            return ret;
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
        /// <param name="word"></param>
        /// <param name="t"></param>
        private void Add(Tile[,] grid, string word, Tile t, MovementDirection direction)
        {
            //Debug.WriteLine(word);
            int wordStartY = t.Col - word.Length;
            //int wordStartX = t.Ligne;
            int multiplier = 1;
            int horizontalPoints = 0;
            int verticalPoints = 0;
            int points = 0;
            int UsedDraughtLetters = 0;
            int letterIdx = 0;

            foreach (var L in word.ToUpper())
            {
                letterIdx++;
                var tile = grid[t.Ligne, wordStartY + letterIdx - 1];
                if (tile.IsEmpty)
                {
                    // Le calcul des points prend en compte les cases bonus non encore utilisées
                    // Il faut noter que seules les lettres du tirage permettent de bénéficier des bonus
                    // Les points verticaux ont été précalculés au moment du recensement des contrôleurs
                    if (char.IsLetter(L))
                    {
                        horizontalPoints += Game.Alphabet.Find(c => c.Char == L).Value * tile.LetterMultiplier;
                        verticalPoints += tile.PossibleLetterPoints.ContainsKey(L) ? tile.PossibleLetterPoints[L] : 0;
                    }
                    else
                    {
                        if (tile.PossibleLetterPoints[Game.Joker] > 0)
                        {
                            verticalPoints += (tile.PossibleLetterPoints.ContainsKey(Game.Joker) ? tile.PossibleLetterPoints[Game.Joker] : 0) - Game.Alphabet.Find(c => c.Char == Game.Joker).Value
                                * tile.LetterMultiplier * tile.WordMultiplier;
                        }
                    }
                    multiplier *= tile.WordMultiplier;
                    UsedDraughtLetters++;
                }
                else
                {
                    if (char.IsLetter(L)) horizontalPoints += Game.Alphabet.Find(c => c.Char == L).Value;
                }

            }
            // L'utilisation des 7 lettres du tirage rapporte en plus 50 points de bonus
            points = horizontalPoints * multiplier + verticalPoints + (UsedDraughtLetters == 7 ? 50 : 0);

            nbPossibleMoves++;
            // Tri et mise en forme des coups
            if (nbAcceptedMoves >= MoveLimit || points <= MinPoint) return;

            if (!LegalWords.Any(w => w.Direction == direction && w.Text.Equals(word) && w.StartTile.Ligne == t.Ligne && w.StartTile.Col == t.Col))
            {
                LegalWords.Add(new Word(Game)
                {
                    StartTile = t,
                    Direction = Direction,
                    //Text = word,
                    Points = points,
                    Text = word,
                });
                nbAcceptedMoves++;
            }

        }

        public List<Word> LegalWords = new List<Word>();



    }
    //[Serializable]
    //public class Move
    //{
    //    public Move()
    //    {
    //    }
    //    public Move Next;
    //    public int Direction { get; set; }
    //    public int Line { get; set; }
    //    public int Column { get; set; }
    //    public string Word { get; set; }
    //    public int Point { get; set; }
    //    public bool Scramble { get; set; }
    //}
}
