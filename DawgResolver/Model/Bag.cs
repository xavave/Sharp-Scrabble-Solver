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
            Letters = Game.AlphabetAvecJoker;
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

        public Letter GetLetterInFlatList(int charIdx, bool removeFromBag = true)
        {
            if (charIdx < FlatList.Length)
            {
                var c = FlatList[charIdx];
                if (c == char.MinValue) return null;//TODO
                var letter = Game.AlphabetAvecJoker.Find(cc => cc.Char == c);
                var le = Letters.Find(l => l == letter);
                if (removeFromBag)
                    if (le.Count <= 0) return null;
                    else le.Count -= 1;
                return le;
            }
            return null;
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

        public List<Letter> GetNewRack(Player p, string forcedLetters = null)
        {
            int lettersToTakeCount = 7 - p.Rack.Count();
            //if (p.Rack.Count >= 7) p.Rack.Clear();

            if (!string.IsNullOrWhiteSpace(forcedLetters))
            {
                p.Rack = forcedLetters.Select(c => Game.AlphabetAvecJoker.Find(a => a.Char == c)).ToList();
                return p.Rack;
            }
            // Si le sac est vide
            if (LeftLettersCount == 0)
                throw new ArgumentException("Il n'y a plus de lettres dans le sac");

            // S'il reste 7 lettres ou moins dans le sac, on n'a pas le choix, on les prend toutes

          
            int cpt = 0;
            // Sinon on tire 7 lettres du sac à condition qu'il en reste suffisament
            for (int i = 0; i < Math.Min(FlatList.Length, lettersToTakeCount); i++)
            {
                cpt++;
                Random rnd = new Random((int)DateTime.Now.Ticks);
                int charIdx = rnd.Next(0, FlatList.Length - 1);
                var letter = GetLetterInFlatList(charIdx);
                while (letter == null && LeftLettersCount > 0) letter = GetLetterInFlatList(rnd.Next(0, FlatList.Length - 1));
                if (cpt > lettersToTakeCount) break;
                p.Rack.Add(letter);
            }
            //Debug.WriteLine(p.DisplayRack());
            return p.Rack;

        }
    }
}
