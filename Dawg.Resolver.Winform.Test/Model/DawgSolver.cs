﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

using DawgResolver.Model;

namespace Dawg.Solver.Winform
{
    public sealed class Solver
    {
        static readonly Solver instance = new Solver();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static Solver()
        {
        }
        public static Solver DefaultInstance
        {
            get
            {
                return instance;
            }
        }
        public char Mode { get; set; } = 'S';//S=Scrabble
        private List<Letter> AlphabetWWFAvecJoker { get; }
        private List<Letter> AlphabetScrabbleAvecJoker { get; }

        //variables definition
        public int WordCount { get; set; }
        public int NodeCount { get; private set; }
        //public HashSet<Word> LegalWords { get; set; } = new HashSet<Word>();
        public HashSet<Word> PlayedWords { get; set; } = new HashSet<Word>();
        public int PlayedLetters { get; private set; }
        long[,] Dictionary { get; set; }
        public long NbPossibleMoves { get; set; }
        public long NbAcceptedMoves { get; set; }
        public BindingList<Letter> Alphabet => Mode == 'S' ?
            new BindingList<Letter>(this.AlphabetScrabbleAvecJoker) : new BindingList<Letter>(this.AlphabetWWFAvecJoker);
        public Noeud Noeud { get; private set; }
        public Solver()
        {

            AlphabetWWFAvecJoker = new List<Letter>() {

            new Letter('A', 1, 9),
            new Letter('B', 5, 2),
            new Letter('C', 4, 2),
            new Letter('D', 3, 5),
            new Letter('E', 1, 13),
            new Letter('F', 5, 2),
            new Letter('G', 5, 3),
            new Letter('H', 5, 4),
            new Letter('I', 1, 8),
            new Letter('J', 8, 1),
            new Letter('K', 10, 1),
            new Letter('L', 2, 4),
            new Letter('M', 4, 2),
            new Letter('N', 1, 5),
            new Letter('O', 4, 8),
            new Letter('P', 4, 2),
            new Letter('Q', 10, 1),
            new Letter('R', 1, 6),
            new Letter('S', 1, 5),
            new Letter('T', 1, 7),
            new Letter('U', 2, 4),
            new Letter('V', 8, 2),
            new Letter('W', 10, 1),
            new Letter('X', 10, 1),
            new Letter('Y', 10, 1),
            new Letter('Z', 10, 1),
            new Letter(Dictionnaire.DefaultInstance.Joker, 0, 2)
            };
            AlphabetScrabbleAvecJoker = new List<Letter>() {
            new Letter('A', 1, 9),
            new Letter('B', 3, 2),
            new Letter('C', 3, 2),
            new Letter('D', 2, 3),
            new Letter('E', 1, 15),
            new Letter('F', 4, 2),
            new Letter('G', 2, 2),
            new Letter('H', 4, 2),
            new Letter('I', 1, 8),
            new Letter('J', 8, 1),
            new Letter('K', 10, 1),
            new Letter('L', 1, 5),
            new Letter('M', 2, 3),
            new Letter('N', 1, 6),
            new Letter('O', 1, 6),
            new Letter('P', 3, 2),
            new Letter('Q', 8, 1),
            new Letter('R', 1, 6),
            new Letter('S', 1, 6),
            new Letter('T', 1, 6),
            new Letter('U', 1, 6),
            new Letter('V', 4, 2),
            new Letter('W', 10, 1),
            new Letter('X', 10, 1),
            new Letter('Y', 10, 1),
            new Letter('Z', 10, 1),
            new Letter(Dictionnaire.DefaultInstance.Joker, 0, 2)
            };

            Alphabet.ListChanged += (s, e) =>
            {

            };
        }

        public Letter FindLetterByIndex(BindingList<Letter> letters, Letter letter)
        {
            var charIndex = letters.IndexOf(letter);
            if (charIndex == -1) return null;
            return letters[charIndex];
        }
        public IExtendedTile DeserializeTile(string s)
        {
            var l = s.Split(';');
            bool? isPlayedByPlayer1 = null;
            if (l.Count() > 7 && l[7] != "") isPlayedByPlayer1 = bool.Parse(l[7]);

            //IExtendedTile t = new FormTile(MainForm, int.Parse(l[0].Substring(1)), int.Parse(l[1]), bool.Parse(l[4]))
            IExtendedTile t = new FormTile(int.Parse(l[0].Substring(1)), int.Parse(l[1]))//, bool.Parse(l[4]))
            {
                LetterMultiplier = int.Parse(l[2]),
                WordMultiplier = int.Parse(l[3]),
                IsValidated = bool.Parse(l[5]),

            };
            if (l.Count() > 6 && l[6] != "") t.Letter.CopyFromOtherLetter(Solver.DefaultInstance.Find(l[6][0]));

            return t;
        }
        public Letter DeserializeLetter(string s)
        {
            var l = s.Split(';');
            return new Letter(l[0].Skip(1).First(), int.Parse(l[1]), int.Parse(l[2]));
        }

        public Letter FindLetterByChar(BindingList<Letter> letters, Char c)
        {
            var foundLetter = letters.FirstOrDefault(l => l.Char == c);
            return FindLetterByIndex(letters, foundLetter);
        }

        public Letter Find(char c)
        {
            return FindLetterByChar(Alphabet, c);
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
        public HashSet<Word> FindMovesPerAnchor(int noeudActuel = 1)
        {
            var newWords = new HashSet<Word>();

            var leftLetters = Game.DefaultInstance.CurrentPlayer.Rack.Letters;
            var anchorTiles = Game.DefaultInstance.Grid.Where(t => t.IsAnchor);
            foreach (var t in anchorTiles)
            {
                // Cas où la taille du préfixe est imposée
                if (t.AnchorLeftMinLimit == t.AnchorLeftMaxLimit)
                {
                    // Cas du préfixe vide
                    // On ne peut que former un nouveau mot vers la droite
                    if (t.AnchorLeftMinLimit == 0)
                    {
                        newWords.UnionWith(ExtendRight(string.Empty, ref leftLetters, 1, 1, t.Ligne, t.Col));
                    }
                    else
                    {
                        // Cas où le préfixe est déjà placé sur la grille
                        // On ne peut que continuer le préfixe vers la droite
                        noeudActuel = 1;
                        string prefixe = "";
                        for (int k = t.Col - t.AnchorLeftMinLimit; k < t.Col; k++)
                        {
                            if (Game.DefaultInstance.Grid[t.Ligne, k].Letter.HasValue())
                            {
                                prefixe += Game.DefaultInstance.Grid[t.Ligne, k].Letter;
                                Trouve(Game.DefaultInstance.Grid[t.Ligne, k].Letter.Char, ref noeudActuel);
                            }
                        }
                        newWords.UnionWith(ExtendRight(prefixe.ToUpper(), ref leftLetters, 1, noeudActuel, t.Ligne, t.Col));
                    }
                }
                else
                {
                    // Cas où k cases vides non identifiées comme ancres se trouvent devant l'ancre
                    // On essaie les différents préfixes possibles allant de 0 à k lettres
                    for (int k = 0; k <= t.AnchorLeftMaxLimit; k++)
                    {
                        newWords.UnionWith(ExtendRight(string.Empty, ref leftLetters, k, 1, t.Ligne, t.Col - k));
                    }
                }
            }
            return newWords;
        }
        /// <summary>
        /// Cette fonction vérifie si un arc représenant une lettre sort d'un noeud
        /// Si l'arc existe, la fonction retourne vrai comme valeur et la variable Noeud_Actuel pointe sur le noeud où atteint par l'arc
        /// </summary>
        /// <param name="noeud_Actuel"></param>
        /// <param name="tile"></param>
        public bool Trouve(int lettre, ref int noeud)
        {
            if (((char)lettre == ' ')) return false;
            if (Dictionnaire.DefaultInstance.Legacy[lettre - Dictionnaire.AscShift, noeud] == 0)
                return false;
            else
            {
                noeud = Dictionnaire.DefaultInstance.Legacy[lettre - Dictionnaire.AscShift, noeud];
                return true;
            }
        }
        public bool Trouve(char lettre, ref int noeud)
        {
            return Trouve((int)lettre, ref noeud);
        }


        /// <summary>
        /// Cette procédure teste les différentes combinaisons possibles
        /// pour continuer un préfixe vers la droite
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="partialWord"></param>
        /// <param name="leftLetters"></param>
        /// <param name="minSize"></param>
        /// <param name="noeud"></param>
        /// <param name="ligne"></param>
        /// <param name="colonne"></param>
        /// <param name="boardSize"></param>
        /// <param name="isTransposed"></param>
        private HashSet<Word> ExtendRight(string partialWord, ref List<Letter> leftLetters, int minSize, int noeud, int ligne, int colonne)
        {
            bool jokerInDraught = leftLetters.Any(l => l.Char == Dictionnaire.DefaultInstance.Joker);
            var newWords = new HashSet<Word>();
            IExtendedTile t = null;
            if (ligne >= 0 && colonne >= 0 && colonne < Game.DefaultInstance.BoardSize && ligne < Game.DefaultInstance.BoardSize && leftLetters.Count() <= 7
                && leftLetters.Count() >= 0) t = Game.DefaultInstance.Grid[ligne, colonne];

            if (t != null && t.IsEmpty)
            {
                // Si une case vide, on peut la remplir avec une lettre du tirage sous certaines conditions
                if (Dictionnaire.DefaultInstance.Legacy[27, noeud] != 0 && partialWord.Length > minSize && partialWord.Length > 1)
                {
                    // Si le préfixe constitue déjà un mot valide
                    // alors on peut rajouter le préfixe dans la liste des coups admis
                    newWords.UnionWith(Add(partialWord, ligne, colonne, Game.DefaultInstance.IsTransposed ? MovementDirection.Down : MovementDirection.Across));
                    //Add(grid, partialWord, Game.IsTransposed ? grid[t.Col - partialWord.Length, t.Ligne] : grid[t.Ligne, t.Col - partialWord.Length], Game.IsTransposed ? MovementDirection.Down : MovementDirection.Across);
                }
                //if (t == null) return;
                //if (nbLetters == 0) return;
                for (int i = 1; i <= 26; i++)
                {

                    if (Dictionnaire.DefaultInstance.Legacy[i, noeud] != 0)
                        for (int l = 0; l < leftLetters.Count; l++)
                        {

                            if (char.ToUpper(leftLetters[l].Char) == (char)(i + Dictionnaire.AscShift))
                            {
                                if (((t.Controllers.ContainsKey(i - 1) && t.Controllers[i - 1] > 0)) || t.Controllers[26] == 26)
                                // Pour chacune des lettres qui répondent au précédent critère dans le tirage
                                {
                                    // Si la lettre permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // la lettre utilisée est alors retirée du tirage
                                    var lettre = char.ToUpper((char)(i + Dictionnaire.AscShift));
                                    var backupLetter = leftLetters[l];
                                    leftLetters.RemoveAt(l);

                                    //if (l < leftLetters.Count() && leftLetters[l].Char == Game.Joker)
                                    //    lettre = char.ToLower(lettre);
                                    // De manière récursive, on essaye de continuer le nouveau préfixe vers la droite
                                    newWords.UnionWith(ExtendRight(partialWord + lettre, ref leftLetters, minSize, Dictionnaire.DefaultInstance.Legacy[i, noeud], ligne, colonne + 1));

                                    // Au retour de l'appel recursif on restitue la lettre dans le tirage

                                    leftLetters.Add(backupLetter);
                                }

                                if (!jokerInDraught)
                                    break;
                            }
                            else if (l < leftLetters.Count() && leftLetters[l].Char == Dictionnaire.DefaultInstance.Joker)
                            {
                                // Cas d'un joker
                                if (((t.Controllers.ContainsKey(i - 1) && t.Controllers[i - 1] > 0)) || t.Controllers[26] == 26)
                                {
                                    // Si une lettre quelconque (représentée par le joker) permet également de former des mots verticalement
                                    // (cette information a été préalablement déterminée dans la procédure Determiner_Controleurs)
                                    // alors on peut essayer de continuer le préfixe avec cette lettre
                                    // le joker utilisé est alors retiré du tirage
                                    var backupLetter = leftLetters[l];
                                    leftLetters.RemoveAt(l);

                                    // De manière récursive, on essayer de continuer le nouveau préfixe vers la droite
                                    newWords.UnionWith(ExtendRight(partialWord + char.ToLower((char)(i + Dictionnaire.AscShift)), ref leftLetters, minSize, Dictionnaire.DefaultInstance.Legacy[i, noeud], ligne, colonne + 1));

                                    // Au retour de l'appel recursif on restitue le joker dans le tirage
                                    leftLetters.Add(backupLetter);
                                }
                            }
                        }

                }
            }
            else
            {
                if (colonne < Game.DefaultInstance.BoardSize && ligne < Game.DefaultInstance.BoardSize)
                    t = Game.DefaultInstance.Grid[ligne, colonne];
                if (t != null && !t.IsEmpty && Dictionnaire.DefaultInstance.Legacy[(int)char.ToUpper(t.Letter.Char) - Dictionnaire.AscShift, noeud] != 0)
                {
                    newWords.UnionWith(ExtendRight(partialWord + t.Letter.Char, ref leftLetters, 1, Dictionnaire.DefaultInstance.Legacy[(int)char.ToUpper(t.Letter.Char)
                        - Dictionnaire.AscShift, noeud], ligne, colonne + 1));
                }

            }
            return newWords;

        }
        /// <summary>
        /// Recherche des différents coups admis
        /// </summary>
        public List<Word> FindMoves(int maxWordCount = 120, bool sortByBestScore = true, string customRack = null)
        {

            var foundWords = new HashSet<Word>();
            Game.DefaultInstance.IsTransposed = false;
            var backUpCurrentRack = Game.DefaultInstance.CurrentPlayer.Rack.Backup();
            if (!string.IsNullOrWhiteSpace(customRack)) Game.DefaultInstance.CurrentPlayer.SetRackFromWord(customRack);

            NbPossibleMoves = 0;
            NbAcceptedMoves = 0;
            //LegalWords.Clear();
            //LegalWords.ToList().AddRange(g.Player1.Moves);
            //LegalWords.ToList().AddRange(g.Player2.Moves);

            var backupGrid = Game.DefaultInstance.BackupGameGrid();
            //var vTiles = g.Grid;
            Game.DefaultInstance.DetectTiles();

            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            foundWords.UnionWith(FindMovesPerAnchor());
            // Recherche des coups verticaux
            // par transposition de la grille, on refait tout le processus
            Game.DefaultInstance.RestoreGameGridFrom(backupGrid);
            Game.DefaultInstance.CurrentPlayer.Rack.Restore(backUpCurrentRack);

            Game.DefaultInstance.TransposeGameGrid();
            Game.DefaultInstance.DetectTiles();
            // Rechercher pour chaque case précédemment identifiée les différents coups possibles et les enregistrer
            foundWords.UnionWith(FindMovesPerAnchor());

            //on remet la grille à son état initial
            Game.DefaultInstance.RestoreGameGridFrom(backupGrid);
            Game.DefaultInstance.IsTransposed = false;

            if (sortByBestScore)
                return foundWords.OrderByDescending(t => t.Points).Take(maxWordCount).ToList();
            else
                return foundWords.OrderByDescending(t => t.Text.Length).Take(maxWordCount).ToList();

            //if (p.Rack.Any())
            //{
            //    // Les lettres non utilisées sont "reversées dans le sac"
            //    foreach (var l in p.Rack)
            //        Game.Bag.PutBackLetter(l);
            //}

        }

        /// <summary>
        /// Vérifie la présence d'un mot dans le dictionnaire.
        /// </summary>
        /// <param name="mot">Mot à vérifier</param>
        /// <returns></returns>
        public List<string> AllMatchingWords(string mot)
        {
            List<string> ret = new List<string>();

            Noeud noeudEnCours = Dictionnaire.DefaultInstance.DAWG;
            List<Arc> arcs = new List<Arc>();
            var end = false;
            for (int i = 0; i < mot.Length; i++)
            {
                var c = mot[i];

                end = false;
                if (c == Dictionnaire.DefaultInstance.Joker)
                {
                    var tmpArcs = noeudEnCours.Sortants;
                    end = InternalLoop(ref noeudEnCours, tmpArcs, c);
                    arcs.AddRange(tmpArcs);
                }
                else
                {
                    end = InternalLoop(ref noeudEnCours, arcs, c);

                }

            }
            if (end) return ret;
            Dictionnaire.DefaultInstance.ParcoursDAWG(arcs, ref ret);
            return ret;

        }

        bool InternalLoop(ref Noeud enCours, List<Arc> arcs, char c)
        {
            var arc = Dictionnaire.DefaultInstance.BoucleDawg(c, ref enCours);
            if (arc != null) arcs.Add(arc);
            else if (!enCours.IsTerminal)
                return true;

            return false;
        }
        public Noeud LoadDic(string nomDico)
        {
            Noeud = Dictionnaire.DefaultInstance.DAWG;

            //var assembly = Assembly.GetExecutingAssembly();
            //string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(Dictionnaire.NomDicoDawgODS7));
            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //using (StreamReader reader = new StreamReader(stream, true))
            //{
            //    string content = reader.ReadToEnd();
            //    List<string> dic = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //    WordCount = int.Parse(dic[0].Replace("NBMOTS : ", ""));
            //    NodeCount = dic.Count - 2;// int.Parse(dic[1].Replace("NBNOEUDS : ", ""));
            //    Dictionary = new long[28, NodeCount + 2];
            //    for (int lineIdx = 2; lineIdx < dic.Count; lineIdx++)
            //    {
            //        var line = dic[lineIdx];
            //        //Remplacement du marqueur de noeud terminal "#" utilisé à la place de "[1" lors de la compilation du dico
            //        //Ce remplacement avait deux objectifs : diminuer la taille du fichier tout en améliorant la lisibilité du dawg
            //        if (line == "#")
            //            line = line.Replace("#", "[1");
            //        else
            //            line = line.Replace("#", "-[1");

            //        // chaque ligne est décomposée pour en extraire les arcs sortants du noeud
            //        // un arc est noté par une lettre de A à Z
            //        // et par la référence du noeud atteint par l'arc
            //        // les arcs sont séparés par le marqueur "-"

            //        foreach (var arc in line.Split('-'))
            //        {
            //            var partie = arc;
            //            var letterCode = (int)partie[0] - Dictionnaire.AscShift;
            //            var node = int.Parse(partie.Substring(1));
            //            Dictionary[letterCode, lineIdx - 1] = node;
            //        }

            //    }
            //}
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
        private HashSet<Word> Add(string word, int ligne, int colonne, MovementDirection direction)
        {
            int multiplier = 1;
            int horizontalPoints = 0;
            int verticalPoints = 0;
            int points = 0;
            int debutCol = colonne - word.Length;
            int UsedDraughtLetters = 0;
            var retWords = new HashSet<Word>();
            var startTile = direction == MovementDirection.Across ? Game.DefaultInstance.Grid[ligne, debutCol] : Game.DefaultInstance.Grid[debutCol, ligne];
            var newWord = new Word()
            {
                Direction = direction,
                Text = word,
                StartTile = startTile
            };
            //if (!referenceWords.Any(pw => pw.Equals(newWord)))
            //{
            for (int j = 0; j < word.Length; j++)
            {
                var tile = Game.DefaultInstance.Grid[ligne, debutCol + j];
                var L = word[j];
                if (tile.IsEmpty)
                {
                    // Le calcul des points prend en compte les cases bonus non encore utilisées
                    // Il faut noter que seules les lettres du tirage permettent de bénéficier des bonus
                    // Les points verticaux ont été précalculés au moment du recensement des contrôleurs
                    if (char.IsUpper(L))
                    {
                        horizontalPoints += this.Alphabet.First(c => c.Char == L).Value * tile.LetterMultiplier;
                        verticalPoints += tile.Controllers.ContainsKey(L - Dictionnaire.AscShift) ? tile.Controllers[L - Dictionnaire.AscShift] : 0;
                    }

                    else
                    {
                        if (tile.Controllers.ContainsKey(((int)char.ToUpper(L)) - Dictionnaire.AscShift) && tile.Controllers[((int)char.ToUpper(L)) - Dictionnaire.AscShift] > 0)
                            verticalPoints += tile.Controllers[((int)char.ToUpper(L)) - Dictionnaire.AscShift] - this.Alphabet.First(c => c.Char == char.ToUpper(L)).Value
                                * tile.LetterMultiplier * tile.WordMultiplier;

                    }
                    multiplier *= tile.WordMultiplier;
                    UsedDraughtLetters++;
                }
                else
                {
                    if (char.IsUpper(L)) horizontalPoints += this.Alphabet.First(c => c.Char == char.ToUpper(L)).Value;
                }

            }
            // L'utilisation des 7 lettres du tirage rapporte en plus 50 points de bonus
            points = horizontalPoints * multiplier + verticalPoints + (UsedDraughtLetters == 7 ? 50 : 0);

            NbPossibleMoves++;
            //if (LegalWords.Any() && points < LegalWords.Min(l => l.Points)) return;
            // Tri et mise en forme des coups
            //if (points <= MinPoint) return;
            newWord.Points = points;

            newWord.Scramble = UsedDraughtLetters == 7;
            //if (!LegalWords.Any(w => w.Equals(newWord)) && !PlayedWords.Any(pw => pw.Equals(newWord)))
            NbAcceptedMoves++;
            retWords.Add(newWord);

            //}
            return retWords;
        }

    }

}