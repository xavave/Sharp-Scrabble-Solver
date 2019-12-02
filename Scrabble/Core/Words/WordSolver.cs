using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core.Words
{
    // Todo: Need to finish this
    public class WordSolver
    {
        public ScrabbleForm ScrabbleForm { get; set; }

        /// <summary>
        /// For all the available words in the scrabble dictionary,
        /// check which ones can be made from the provided string of letters.
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        //public List<Word> Anagrams(List<RackTile> tiles, ScrabbleTile adjacentTile = null)
        //{
        //    string letters = string.Empty;
        //    tiles.ForEach(t => letters += t.Text);
        //    // If we have been provided an adjacent tile, then we can also use that to make words.
        //    if (adjacentTile != null)
        //        letters += adjacentTile.Text;

        //    var words = new List<Word>();
        //    bool IsFirstMove = ScrabbleForm.TileManager.Tiles[7, 7].Text == "";
        //    foreach (var w in ScrabbleForm.WordValidator.ValidWords)
        //    {
        //        // If a certain letter must be in the output then ensure the word we are making contains it
        //        if (adjacentTile != null && !w.Contains(adjacentTile.Text))
        //            continue;

        //        if (CanMakeWord(w, letters))
        //        {
        //            if (adjacentTile != null)
        //            {
        //                var vertical = GenerateWordInDirection(w, adjacentTile, MovementDirection.Down);

        //                if (vertical != null && !words.Any(wo => wo.Text == vertical.Text))
        //                {
        //                    vertical.Direction = MovementDirection.Down;
        //                    vertical.AdjacentLetter = adjacentTile.Text + " (vert)";
        //                    vertical.Score = ScrabbleForm.WordScorer.RawWordScore(vertical.Text);

        //                    words.Add(vertical);
        //                }

        //                var horizontal = GenerateWordInDirection(w, adjacentTile, MovementDirection.Across);

        //                if (horizontal != null && !words.Any(wo => wo.Text == horizontal.Text))
        //                {
        //                    horizontal.Direction = MovementDirection.Across;
        //                    horizontal.AdjacentLetter = adjacentTile.Text + " (horiz)";
        //                    horizontal.Score = ScrabbleForm.WordScorer.RawWordScore(horizontal.Text);
        //                    words.Add(horizontal);
        //                }
        //            }
        //            else if (IsFirstMove)
        //            {

        //                var word = new Word
        //                {
        //                    Direction = MovementDirection.None,
        //                    Text = w,
        //                    AdjacentLetter = "",


        //                };
        //                word.Score = ScrabbleForm.WordScorer.RawWordScore(word.Text);

        //                if (!words.Contains(word))
        //                    words.Add(word);
        //            }
        //        }
        //    }

        //    return words.OrderByDescending(w => w.Score).ThenByDescending(w => w.Text.Length).ToList();
        //}


        ///// <summary>
        ///// Given a set of rack tiles, find all the possible anagrams of those tiles.
        ///// </summary>
        ///// <param name="tiles"></param>
        ///// <returns></returns>
        //public List<Word> Anagrams(List<RackTile> tiles, ScrabbleTile adjacentTile = null)
        //{
        //    string letters = string.Empty;
        //    tiles.ForEach(t => letters += t.Text);

        //    return Anagrams(letters, adjacentTile);
        //}

        ///// <summary>
        ///// Generate a Word to (potentially) be played on the board based on the anagrams available and 
        ///// the adjacent tiles.
        ///// </summary>
        ///// <param name="word"></param>
        ///// <param name="adjacentTile"></param>
        ///// <param name="direction"></param>
        ///// <returns></returns>
        //private Word GenerateWordInDirection(string word, ScrabbleTile adjacentTile, MovementDirection direction)
        //{
        //    var adjacent = adjacentTile.Text;
        //    int adjacentIndex = word.IndexOf(adjacent);
        //    int sx, ex, sy, ey = 0;

        //    switch (direction)
        //    {
        //        case MovementDirection.Down:
        //            // Vertically placing the word
        //            sx = adjacentTile.XLoc;
        //            ex = adjacentTile.XLoc;
        //            sy = adjacentTile.YLoc - adjacentIndex;
        //            ey = adjacentTile.YLoc + (word.Length - 1 - adjacentIndex);
        //            break;
        //        case MovementDirection.Across:
        //            // Horizontally placing the word
        //            sx = adjacentTile.XLoc - adjacentIndex;
        //            ex = adjacentTile.XLoc + (word.Length - 1 - adjacentIndex);
        //            sy = adjacentTile.YLoc;
        //            ey = adjacentTile.YLoc;
        //            break;
        //        default:
        //            throw new Exception($"Unsupported movement direction to generate words for {direction}");
        //    }

        //    var w = new Word
        //    {
        //        Text = word,
        //        StartX = sx,
        //        EndX = ex,
        //        StartY = sy,
        //        EndY = ey,
        //        AdjacentLetter = adjacent,
        //        Tiles = new List<ITile>()
        //    };

        //    for (int x = sx; x <= ex; x++)
        //    {
        //        for (int y = sy; y <= ey; y++)
        //        {
        //            if (x < 0 || x >= ScrabbleForm.BOARD_WIDTH - 1 || y < 0 || y >= ScrabbleForm.BOARD_HEIGHT - 1)
        //                return null;
        //            var tile = ScrabbleForm.TileManager.Tiles[x, y];
        //            //if (tile.Text == w.Text.Last().ToString() || tile.Text == "")
        //                w.Tiles.Add(tile);
        //            //else
        //            //{

        //            //}
        //        }
        //    }

        //    w.Score = ScrabbleForm.WordScorer.ScoreWord(w);
        //    //w.Score = WordScorer.RawWordScore(word);

        //    return w;
        //}

        ///// <summary>
        ///// Given a provided word, is it possible to make it using the letters provided?
        ///// </summary>
        ///// <param name="word"></param>
        ///// <param name="letters"></param>
        ///// <returns></returns>
        //public bool CanMakeWord(string word, string letters)
        //{
        //    foreach (var c in word.ToCharArray())
        //    {
        //        if (letters.Length == 0)
        //            return false;

        //        var letterIndex = letters.IndexOf(c);
        //        if (letterIndex < 0)
        //            return false;

        //        letters = letters.Remove(letterIndex, 1);
        //    }

        //    return true;
        //}
    }
}
