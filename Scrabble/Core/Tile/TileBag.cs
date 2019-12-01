using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core
{
    public class TileBag
    {
        public List<char> Letters { get; set; }

        public TileBag()
        {
            SetupBag();
        }

        /// <summary>
        /// Sets up a tile bag ready for a game.
        /// </summary>
        public void SetupBag()
        {
            Letters = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int x = 0; x < LetterCount(c); x++)
                {
                    this.Letters.Add(c);
                }
            }

            Letters = Letters.OrderBy(l => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Returns how many letters are left in the rack.
        /// </summary>
        public int LetterCountRemaining()
        {
            return Letters.Count;
        }

        /// <summary>
        /// Give a letter back to the bag. This would be triggered when a user swaps a tile.
        /// </summary>
        /// <param name="letter"></param>
        public void GiveLetter(char letter)
        {
            Letters.Add(letter);
        }

        public string TakeLetters(int numLetters, bool removeLetter = true)
        {
            var letters = "";
            var random = new Random();

            while (letters.Length < numLetters)
            {
                // Ran out of letters
                if (Letters.Count == 0)
                    break;

                var randomLetter = Letters[random.Next(0, Letters.Count)];
                letters += randomLetter;
                if (removeLetter)
                    Letters.Remove(randomLetter);
            }

            return letters;
        }

        /// <summary>
        /// How many times times a provided character appear in the tile bag
        /// at the start of a game?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int LetterCount(char c)
        {
            var timesMapping = new Dictionary<char, int>()
            {
                { 'E', 13 }, { 'A', 9 }, { 'I', 8 }, { 'O', 8 },
                { 'N', 5 }, { 'R', 6 }, { 'T', 7 }, { 'L', 4  },
                { 'S', 5 }, { 'U', 4 }, { 'D', 5 }, { 'G', 3 },
                { 'B', 2 }, { 'C', 2 }, { 'M', 2 }, { 'P', 2 },
                { 'F', 2 }, { 'H', 4 }, { 'V', 2 }, { 'W', 2 },
                { 'Y', 2 }, { 'K', 1 }, { 'J', 1 }, { 'X', 1 },
                { 'Q', 1 }, { 'Z', 1 },
            };

            return timesMapping[c];
        }
    }
}
