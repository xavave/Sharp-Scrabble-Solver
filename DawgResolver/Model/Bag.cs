using DawgResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver.Model
{
    public class Bag
    {
        public Bag()
        {
            Letters = new List<Letter>(Game.AlphabetAvecJoker);
        }

        public static List<Letter> Letters { get; set; }
        public string FlatList
        {
            get
            {
                var ret = "";
                foreach (var l in Letters)
                {
                    ret += new string(l.Char, l.Count);
                }
                return ret;
            }
        }
        // On compte le nombre de lettres restantes dans le sac et on établit la liste des choix
        public int LeftLettersCount
        {
            get => Letters.Sum(t => t.Count);

        }
        public void RemoveLetterFromBag(char c)
        {
            var letter = Game.AlphabetAvecJoker.Find(cc => cc.Char == c);
            if (letter.Count > 0) letter.Count = --letter.Count;
        }
        Random rnd = new Random();
        public Letter GetLetterInFlatList()
        {
            if (!FlatList.Any())
            {
                throw new ArgumentException("No more letters in bag!");
            }

            int charIdx = rnd.Next(0, FlatList.Length - 1);
            var c = FlatList[charIdx];
            var letter = Game.AlphabetAvecJoker.Find(cc => cc.Char == c);
            var le = Letters.Find(l => l == letter);

            return le;

        }
        public void PutBackLetter(Letter l)
        {
            var letter = Game.AlphabetAvecJoker.Find(cc => cc == l);
            letter.Count++;
        }

        public string GetBagContent(int split = 5)
        {
            var sb = new StringBuilder();
            int idx = 0;
            foreach (var l in Letters)
            {
                sb.Append($"{l.Char}:{l.Count}_______");
                idx++;
                if (idx % split == 0) sb.AppendLine();
            }
            return sb.ToString();
        }

        public List<Letter> GetLetters(Player p, string forcedLetters = null)
        {


            if (!string.IsNullOrWhiteSpace(forcedLetters))
            {
                p.Rack = forcedLetters.Select(c => Game.AlphabetAvecJoker.Find(a => a.Char == c)).ToList();
            }
            // Si le sac est vide
            if (LeftLettersCount == 0)
                throw new ArgumentException("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes

            int lettersToTakeCount = 7 - p.Rack.Count();

            // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
            for (int i = 0; i < Math.Min(FlatList.Length, lettersToTakeCount); i++)
            {
                var letter = GetLetterInFlatList();
                p.Rack.Add(letter);
            }
            return p.Rack;

        }
    }
}
