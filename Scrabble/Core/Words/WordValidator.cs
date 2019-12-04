using DawgResolver;
using DawgSharp;
using Scrabble.Core.Movement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core.Words
{
    public class WordValidator
    {
        public List<string> ValidWords { get; set; }
        public ScrabbleForm ScrabbleForm { get; set; }

        Dawg<bool> Dawg { get; set; }
        public WordValidator()
        {
            this.LoadWords();
        }

        /// <summary>
        /// Loads the list of valid words from the input file.
        /// These words are from the Collin's dictionary of valid scrabble words.
        /// </summary>
        private void LoadWords()
        {
            ValidWords = new List<string>();

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\ods4.txt");
            foreach (var w in File.ReadAllLines(path))
            {
                ValidWords.Add(w);
            }
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("ODS4_DAWG.bin"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                Dawg = Dawg<bool>.Load(reader.BaseStream);
            }

        }

        /// <summary>
        /// Check if a provided word is present in the list of known valid words.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool CheckWord(Word word)
        {
            if (Dawg == null)
                LoadWords();

            // Todo: maybe not all 1 length words should be valid???
            if (word.Text.Length == 1)
                return true;

            return Dawg[word.Text];
        }

        /// <summary>
        /// Validate all the words on the board.
        /// </summary>
        /// <returns></returns>
        //public MoveResult ValidateWordsInPlay(ITile[,] scrabbleTile = null, Word word = null)
        //{
        //    var words = new List<Word>();
        //    if (scrabbleTile == null) scrabbleTile = ScrabbleForm.TileManager.Tiles;
        //    int startX = word?.Tiles[0].XLoc ?? 0;
        //    int startY = word?.Tiles[0].YLoc ?? 0;
        //    int endX = ScrabbleForm.BOARD_WIDTH;
        //    int endY = ScrabbleForm.BOARD_HEIGHT;
        //    if (word != null)
        //    {
        //        if (word.Direction == MovementDirection.Across)
        //        {
        //            endX = startX + word.Text.Length;
        //            endY = startY + 1;
        //        }
        //        else if (word.Direction == MovementDirection.Down)
        //        {
        //            endX = startX + 1;
        //            endY = startY + word.Text.Length;
        //        }
        //    }
        //    for (int x = startX; x < endX; x++)
        //    {
        //        for (int y = startY; y < endY; y++)
        //        {
        //            if (!string.IsNullOrEmpty(scrabbleTile[x, y].Text))// && scrabbleTile[x, y].TileInPlay)
        //            {
        //                foreach (var w in GetSurroundingWords(x, y, scrabbleTile))
        //                {
        //                    if (word != null)
        //                    {
        //                        if (CheckWord(w) && !words.Contains(w))
        //                            words.Add(word);
        //                    }
        //                    else
        //                        if (!words.Contains(w))
        //                        words.Add(w);
        //                    // Todo: need to allow duplicated words if the word actually has been played twice
        //                    // Think this is sorted, just need to test it.

        //                }
        //            }
        //        }
        //    }

        //    foreach (var w in words)
        //    {
        //        w.Tiles = GetWordTiles(w, scrabbleTile);
        //        w.Points = ScrabbleForm.WordScorer.ScoreWord(w);


        //        //w.SetValidHighlight();
        //        //MessageBox.Show($"{w} valid: {w.Valid}");
        //    }

        //    return new MoveResult
        //    {
        //        TotalScore = words.Sum(w => w.Points),
        //        Words = words,
        //        Valid = ScrabbleForm.TileManager.Tiles[7, 7].Text == "" || words.Any() && words.All(w => w.IsAllowed)
        //    };
        //}

        /// <summary>
        /// Traverse the board horizontally and vertically from a given point (x, y)
        /// to find the full word in play in both the horizontal and vertical direction.
        /// These words are then validated to ensure that the move is valid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        //public List<Word> GetSurroundingWords(int x, int y, ITile[,] scrabbleTile = null)
        //{
        //    var words = new List<Word>();
        //    if (scrabbleTile == null) scrabbleTile = ScrabbleForm.TileManager.Tiles;
        //    string horizontal = string.Empty;
        //    string vertical = string.Empty;

        //    // Start/End location for the horizonal word
        //    var tx = x;
        //    while (tx >= 0 && !string.IsNullOrEmpty(scrabbleTile[tx, y].Text))
        //        tx -= 1;
        //    tx += 1;

        //    var tx2 = x;
        //    while (tx2 < ScrabbleForm.BOARD_WIDTH && !string.IsNullOrEmpty(scrabbleTile[tx2, y].Text))
        //        tx2 += 1;
        //    tx2 -= 1;

        //    for (var i = Math.Max(tx, 0); i <= Math.Min(tx2, ScrabbleForm.BOARD_WIDTH - 1); i++)
        //        horizontal += scrabbleTile[i, y].Text;

        //    // Start/End location for the vertical word
        //    var ty = y;
        //    while (ty >= 0 && !string.IsNullOrEmpty(scrabbleTile[x, ty].Text))
        //        ty -= 1;
        //    ty += 1;

        //    var ty2 = y;
        //    while (ty2 < ScrabbleForm.BOARD_WIDTH && !string.IsNullOrEmpty(scrabbleTile[x, ty2].Text))
        //        ty2 += 1;
        //    ty2 -= 1;

        //    for (var i = Math.Max(ty, 0); i <= Math.Min(ty2, ScrabbleForm.BOARD_HEIGHT - 1); i++)
        //        vertical += scrabbleTile[x, i].Text;

        //    if (!string.IsNullOrEmpty(horizontal) && horizontal.Length > 1)
        //    {
        //        var w = new Word
        //        {
        //            StartX = tx,
        //            EndX = tx2,
        //            StartY = y,
        //            EndY = y,
        //            Text = horizontal
        //        };
        //        //w.Valid = CheckWord(w);
        //        //if (w.Valid)
        //        //{
        //        words.Add(w);
        //        w.Score = ScrabbleForm.WordScorer.ScoreWord(w);
        //        //}

        //    }
        //    if (!string.IsNullOrEmpty(vertical) && vertical.Length > 1)
        //    {
        //        var w = new Word
        //        {
        //            StartX = x,
        //            EndX = x,
        //            StartY = ty,
        //            EndY = ty2,
        //            Text = vertical,

        //        };
        //        //w.Valid = CheckWord(w);
        //        //if (w.Valid)
        //        //{
        //        words.Add(w);
        //        w.Score = ScrabbleForm.WordScorer.ScoreWord(w);
        //        //}

        //    }
        //    return words.ToList();
        //}
    }
}
