using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dawg.Solver.Winform;

namespace DawgResolver.Model
{
    public class Bag
    {
       
        private Bag()
        {
            if (Letters == null)
                Letters = new List<Letter>(Game.DefaultInstance.Solver.Alphabet.Select(s => s.Clone() as Letter));
            ResetCount();
        }

        public static Bag Build()
        {
            return new Bag();
        }

        public void ResetCount()
        {
            foreach (var l in Letters)
                l.Count = l.DefaultCount;
        }

        public IList<Letter> Letters { get; }
        public string FlatList => string.Join("", Letters.Select(l => string.Join("", new string(l.Char, l.Count))));

        // On compte le nombre de lettres restantes dans le sac et on établit la liste des choix
        public int LeftLettersCount
        {
            get => Letters.Sum(t => t.Count);

        }
        public void RemoveLetterFromBag(char c)
        {
            var bagLetter = Letters.FirstOrDefault(l => l.Char == c);
            if (bagLetter.Count > 0) bagLetter.Count--;
        }

        public Letter GetLetterInFlatList(Random r)
        {
            if (!FlatList.Any())
            {
                throw new ArgumentException("No more letters in bag!");
            }

            int charIdx = r.Next(0, FlatList.Length - 1);
            var c = FlatList[charIdx];
            if (c == Game.EmptyChar) throw new ArgumentException(nameof(c));
            var alphabetLetter = Game.DefaultInstance.Solver.Find(c);

            var bagLetter = Letters.FirstOrDefault(l => l.Char == alphabetLetter.Char);
            if (bagLetter.Count > 0) bagLetter.Count--;
            return bagLetter;

        }
        public void PutBackLetterInBag(Letter l)
        {
            var letter = Game.DefaultInstance.Solver.Find(l.Char);
            letter.Count++;
        }

        public string GetBagContent(int split = 5)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{LeftLettersCount} lettres restantes");
            int idx = 0;
            foreach (var l in Letters)
            {
                sb.Append($"{l.Char}({l.Value}):{l.Count}\t");
                idx++;
                if (idx % split == 0) sb.AppendLine();
            }
            return sb.ToString();
        }

        public void GetLetters(Player p, string forcedLetters = null)
        {
            if (p.Rack.Count() > 7) return;

            if (!string.IsNullOrWhiteSpace(forcedLetters))
            {
                p.Rack.Letters = forcedLetters.Select(c => Game.DefaultInstance.Solver.Find(c)).ToList();
                return;
            }
            // Si le sac est vide
            if (LeftLettersCount == 0) return;
            //throw new ArgumentException("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes

            int lettersToTakeCount = 7 - p.Rack.Count();
            if (Math.Abs(lettersToTakeCount) > 7) lettersToTakeCount = 0;
            Random rnd = new Random();
            // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
            for (int i = 0; i <= Math.Min(FlatList.Length, lettersToTakeCount - 1); i++)
            {
                var letter = GetLetterInFlatList(rnd);

                p.Rack.Add(letter);
            }


        }
    }
}
