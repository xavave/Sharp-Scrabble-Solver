using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;


namespace DawgResolver
{
    public class Resolver
    {
        //const definition 

        const int BoardSize = 15;
        //variables definition
        public int WordCount { get; set; }
        public int NodeCount { get; private set; }
        public List<Word> LegalWords { get; set; } = new List<Word>();
        public int PlayedLetters { get; private set; }
        long[,] Dictionary { get; set; }
        public long NbPossibleMoves { get; set; }
        public long NbAcceptedMoves { get; set; }
        Game Game { get; }

        public Resolver(Game g)
        {
            Game = g;
            LoadDic();
        }

        public Dictionnaire Dictionnaire { get; private set; }
        public Noeud Noeud { get; private set; }

        /// <summary>
        /// Cette procédure génère un nouveau tirage
        /// en choisissant au hasard parmi les lettres disponibles dans le sac
        /// </summary>


        private VTile[,] DetectTiles(Player p, VTile[,] grid)
        {
            //On efface les informations précédentes en vue d'une nouvelle analyse
            foreach (var t in grid.OfType<Tile>())
            {
                t.AnchorLeftMaxLimit = t.AnchorLeftMinLimit = 0;
                t.Controlers = new Dictionary<int, int>(27);
            }

            // Le premier est un cas particulier où aucune lettre n'est encore posée sur le plateau
            // Tous les mots formés doivent toucher la case centrale qui constitue ainsi la seule ancre
            // Les mots formés au premier peuvent commencer six cases au plus vers la gauche de la case centrale
            // Comme aucune lettre n'est encore présente sur le plateau, il n'y a aucune vérification à faire
            // par rapport aux mots formés verticalement (les 26 lettres peuvent être utilisées)

            if (Game.FirstMove)
            {
                grid[7, 7].AnchorLeftMinLimit = 0;
                grid[7, 7].AnchorLeftMaxLimit = 6;

                foreach (var t in grid.OfType<Tile>())
                {
                    t.Controlers[26] = 26;
                }
            }
            else
            {
                // A partir du deuxième coup
                // Il faut recenser les cases où l'on peut commencer de nouveaux mots ou continuer des mots existants
                grid = FindAnchors(grid);

                // Et vérifier les lettres qu'on peut y placer pour que les éventuels mots formés verticalement soient valides

                grid = FindControlers(grid);
            }
            return grid;
        }

        /// <summary>
        /// Cette procédure permet d'identifier les lettres qu'on peut jouer dans une case donnée
        /// en vérifiant si l'ajout d'un mot dans le sens horizontal permet de former également des mots valides
        /// dans le sens vertical. Ce travail est effectué avant la recherche proprement dite pour limiter les possibilités à tester
        /// on en profite également pour précalculer le point rapporté par les mots formés verticalement
        /// </summary>
        private VTile[,] FindControlers(VTile[,] grid)
        {
            int L = 0;

            foreach (var t in grid.OfType<VTile>())
            {
                if (t.IsAnchor)
                {
                    var wordStart = string.Empty;
                    var wordEnd = string.Empty;
                    var tileCpy = t.Copy(Game, grid);
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
                    tileCpy = t.Copy(Game, grid);

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

                    foreach (var c in Game.Alphabet.Where(c => (wordStart + c + wordEnd).Length > 1 && Game.Dico.MotAdmis((wordStart + c + wordEnd).ToUpper())))
                    {
                        grid[t.Ligne, t.Col].Controlers[((int)c.Char) - Dictionnaire.AscShiftBase0] = (points + c.Value * t.LetterMultiplier) * t.WordMultiplier;
                        L++;

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


            return grid;
        }


        /// <summary>
        /// Cette procédure repère les ancres ligne par ligne
        /// Il s'agit des cases inocupées adjacentes à une case déjà occupée
        /// Ce recensement est utile afin de limiter les recherches aux cases où l'on peut jouer des coups
        /// </summary>
        private VTile[,] FindAnchors(VTile[,] grid)
        {

            foreach (var t in grid.OfType<VTile>().Where(ti => ti.IsEmpty))
            {
                // Cas où l'ancre est à l'extrème gauche du plateau, la taille du préfixe est exactement 0
                // Cas où l'ancre est précédée d'une autre ancre, la taille du préfixe est exactement 0
                if (t.IsAnchor)
                {
                    t.IsValidated = false;
                    var tileCpy = t.Copy(Game, grid);

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

            return grid;

        }

        /// <summary>
        /// Pour chaque ancre identifiée,
        /// Cette procédure lance la recherche des différents mots formables à partir de l'ancre
        /// en utilisant les mots du tirage et ceux déjà placés
        /// en respectant la taille limite des préfixes précédement calculés
        /// </summary>
        /// <param name="p"></param>
        /// <param name="grid"></param>
        /// <param name="direction"></param>
        /// <param name="noeudActuel"></param>
        /// <returns></returns>
        private VTile[,] FindMovesPerAnchor(Player p, VTile[,] grid, int noeudActuel = 1)
        {
            var leftLetters = p.Rack;
            foreach (var t in grid.OfType<VTile>().Where(t => t.IsAnchor))
            {

                // Cas où la taille du préfixe est imposée
                if (t.AnchorLeftMinLimit == t.AnchorLeftMaxLimit)
                {
                    // Cas du préfixe vide
                    // On ne peut que former un nouveau mot vers la droite
                    if (t.AnchorLeftMinLimit == 0)
                    {
                        ExtendRight(grid, string.Empty, ref leftLetters, 1, 1, t.Ligne, t.Col);
                    }
                    else
                    {
                        // Cas où le préfixe est déjà placé sur la grille
                        // On ne peut que continuer le préfixe vers la droite
                        noeudActuel = 1;
                        string prefixe = "";
                        for (int k = t.Col - t.AnchorLeftMinLimit; k < t.Col; k++)
                        {
                            if (grid[t.Ligne, k].Letter.HasValue())
                            {
                                prefixe += grid[t.Ligne, k].Letter;
                                Trouve(grid[t.Ligne, k].Letter.Char, ref noeudActuel);
                            }
                        }
                        ExtendRight(grid, prefixe.ToUpper(), ref leftLetters, 1, noeudActuel, t.Ligne, t.Col);
                    }
                }
                else
                {
                    // Cas où k cases vides non identifiées comme ancres se trouvent devant l'ancre
                    // On essaie les différents préfixes possibles allant de 0 à k lettres
                    for (int k = 0; k <= t.AnchorLeftMaxLimit; k++)
                    {
                        ExtendRight(grid, string.Empty, ref leftLetters, k, 1, t.Ligne, t.Col - k);
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
        private bool Trouve(int lettre, ref int noeud)
        {
            if (Game.Dico.Legacy[lettre - Dictionnaire.AscShift, noeud] == 0)
                return false;
            else
            {
                noeud = Game.Dico.Legacy[lettre - Dictionnaire.AscShift, noeud];
                return true;
            }
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
        private void ExtendRight(VTile[,] grid, string partialWord, ref List<Letter> leftLetters, int minSize, int noeud, int ligne, int colonne)
        {

            bool jokerInDraught = leftLetters.Any(l => l.Char == Game.Joker);

            VTile t = null;
            if (ligne >= 0 && colonne >= 0 && colonne < 15 && ligne < 15) t = grid[ligne, colonne];
            if (noeud == 25369)
            {

            }
            if (t == null || t.IsEmpty)
            {
                // Si une case vide, on peut la remplir avec une lettre du tirage sous certaines conditions
                if (Game.Dico.Legacy[27, noeud] != 0 && partialWord.Length > minSize && partialWord.Length > 1)
                {   // Si le préfixe constitue déjà un mot valide
                    // alors on peut rajouter le préfixe dans la liste des coups admis

                    Add(grid, partialWord, ligne, colonne, Game.IsTransposed ? MovementDirection.Down : MovementDirection.Across);
                    //Add(grid, partialWord, Game.IsTransposed ? grid[t.Col - partialWord.Length, t.Ligne] : grid[t.Ligne, t.Col - partialWord.Length], Game.IsTransposed ? MovementDirection.Down : MovementDirection.Across);
                }
                if (t == null) return;
                //if (nbLetters == 0) return;
                for (int i = 1; i <= 26; i++)
                {

                    if (Game.Dico.Legacy[i, noeud] != 0)
                        for (int l = 0; l < leftLetters.Count; l++)
                        {

                            if (char.ToUpper(leftLetters[l].Char) == (char)(i + Dictionnaire.AscShift))
                            {
                                if (((t.Controlers.ContainsKey(i - 1) && t.Controlers[i - 1] > 0)) || t.Controlers[26] == 26)
                                // Pour chacune des lettres qui répondent au précédent critère dans le tirage
                                {
                                    // Si la lettre permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // la lettre utilisée est alors retirée du tirage
                                    var lettre = (char)(i + Dictionnaire.AscShift);
                                    var backupLetter = leftLetters[l];
                                    leftLetters.RemoveAt(l);

                                    if (l < leftLetters.Count() && leftLetters[l].Char == Game.Joker)
                                        lettre = char.ToLower(lettre);
                                    // De manière récursive, on essaye de continuer le nouveau préfixe vers la droite
                                    ExtendRight(grid, partialWord + lettre, ref leftLetters, minSize, Game.Dico.Legacy[i, noeud], ligne, colonne + 1);

                                    // Au retour de l'appel recursif on restitue la lettre dans le tirage

                                    leftLetters.Add(backupLetter);
                                }

                                if (!jokerInDraught)
                                    break;
                            }
                            else if (l< leftLetters.Count() && leftLetters[l].Char == Game.Joker)
                            {
                                // Cas d'un joker
                                if (((t.Controlers.ContainsKey(i - 1) && t.Controlers[i - 1] > 0)) || t.Controlers[26] == 26)
                                {
                                    // Si une lettre quelconque (représentée par le joker) permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // le joker utilisé est alors retiré du tirage
                                    var backupLetter = leftLetters[l];
                                    leftLetters.RemoveAt(l);

                                    // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                                    ExtendRight(grid, partialWord + char.ToLower((char)(i + Dictionnaire.AscShift)), ref leftLetters, minSize, Game.Dico.Legacy[i, noeud], ligne, colonne + 1);

                                    // Au retour de l'appel recursif on restitue le joker dans le tirage
                                    leftLetters.Add(backupLetter);
                                }
                            }
                        }

                }
            }
            else
            {
                if (Game.Dico.Legacy[(int)char.ToUpper(t.Letter.Char) - Dictionnaire.AscShift, noeud] != 0)
                {
                    ExtendRight(grid, partialWord + t.Letter.Char, ref leftLetters, 1, Game.Dico.Legacy[(int)char.ToUpper(t.Letter.Char) - Dictionnaire.AscShift, noeud], ligne, colonne + 1);
                }

            }


        }
        /// <summary>
        /// Recherche des différents coups admis
        /// </summary>
        public List<Word> FindMoves(Player p, int maxWordCount = 100)
        {
            NbPossibleMoves = 0;
            NbAcceptedMoves = 0;
            LegalWords.Clear();
            var backupGrid = Game.Grid.Copy();
            Game.Grid = DetectTiles(p, Game.Grid);

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            Game.Grid = FindMovesPerAnchor(p, Game.Grid);
            // Recherche des coups verticaux
            // par transposition de la grille, on refait tout le processus
            Game.Grid = backupGrid;
            Game.Grid = Game.Grid.Transpose(Game);
            Game.Grid = DetectTiles(p, Game.Grid);
            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            Game.Grid = FindMovesPerAnchor(p, Game.Grid);

            var ret = LegalWords.OrderByDescending(t => t.Points).Take(maxWordCount).ToList();

            //if (p.Rack.Any())
            //{
            //    // Les lettres non utilisées sont "reversées dans le sac"
            //    foreach (var l in p.Rack)
            //        Game.Bag.PutBackLetter(l);
            //}
            ////on remet la grille à son état initial
            Game.IsTransposed = false;
            Game.Grid = backupGrid;
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

        /// <summary>
        /// Cette procédure ajoute un coup à la liste des coups admis recensés
        /// en enregistrant la place où le coup est joué
        /// s'il est joué horizontalement ou verticalement
        /// le mot formé
        /// le point que le coup rapporte
        /// </summary>
        /// <param name="word"></param>
        /// <param name="t"></param>
        private void Add(VTile[,] grid, string word, int ligne, int colonne, MovementDirection direction)
        {
            int multiplier = 1;
            int horizontalPoints = 0;
            int verticalPoints = 0;
            int points = 0;
            int debutCol = colonne - word.Length;
            int UsedDraughtLetters = 0;


            for (int j = 0; j < word.Length; j++)
            {
                var tile = grid[ligne, debutCol + j];
                var L = word[j];
                if (tile.IsEmpty)
                {
                    // Le calcul des points prend en compte les cases bonus non encore utilisées
                    // Il faut noter que seules les lettres du tirage permettent de bénéficier des bonus
                    // Les points verticaux ont été précalculés au moment du recensement des contrôleurs
                    if (char.IsUpper(L))
                    {
                        horizontalPoints += Game.Alphabet.Find(c => c.Char == char.ToUpper(L)).Value * tile.LetterMultiplier;
                        verticalPoints += tile.Controlers.ContainsKey(L - Dictionnaire.AscShiftBase0) ? tile.Controlers[L - Dictionnaire.AscShiftBase0] : 0;
                    }

                    else
                    {
                        if (tile.Controlers.ContainsKey(((int)char.ToUpper(L)) - Dictionnaire.AscShiftBase0) && tile.Controlers[((int)char.ToUpper(L)) - Dictionnaire.AscShiftBase0] > 0)
                            verticalPoints += tile.Controlers[((int)char.ToUpper(L)) - Dictionnaire.AscShiftBase0] - Game.AlphabetAvecJoker.Find(c => c.Char == char.ToUpper(L)).Value
                                * tile.LetterMultiplier * tile.WordMultiplier;

                    }
                    multiplier *= tile.WordMultiplier;
                    UsedDraughtLetters++;
                }
                else
                {
                    if (char.IsUpper(L)) horizontalPoints += Game.Alphabet.Find(c => c.Char == char.ToUpper(L)).Value;
                }

            }
            // L'utilisation des 7 lettres du tirage rapporte en plus 50 points de bonus
            points = horizontalPoints * multiplier + verticalPoints + (UsedDraughtLetters == 7 ? 50 : 0);

            NbPossibleMoves++;
            //if (LegalWords.Any() && points < LegalWords.Min(l => l.Points)) return;
            // Tri et mise en forme des coups
            //if (points <= MinPoint) return;
            var startTile = direction == MovementDirection.Across ? grid[ligne, debutCol] : grid[debutCol, ligne];
            if (!LegalWords.Any(w => w.Direction == direction && w.Text == word && w.StartTile.Ligne == startTile.Ligne && w.StartTile.Col == startTile.Col))
            {
                LegalWords.Add(new Word(Game)
                {
                    StartTile = startTile,
                    Direction = direction,
                    Text = word,
                    Points = points,
                    Scramble = UsedDraughtLetters == 7
                }); ;
                NbAcceptedMoves++;
            }

        }

    }

}
