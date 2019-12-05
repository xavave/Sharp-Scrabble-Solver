using Dawg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    [Serializable]
    public class Bag
    {
        public Bag()
        {
            Letters = Game.Alphabet;
        }

        public static List<Letter> Letters { get; set; }

        // On compte le nombre de lettres restantes dans le sac et on établit la liste des choix
        public  int LeftLetters
        {
            get => Letters.Sum(t => t.Count);

        }
    }
}
