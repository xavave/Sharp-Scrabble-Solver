using DawgResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    [Serializable]
    public class Player
    {
        Game Game { get; set; }
        public Player(Game g)
        {
            Game = g;
        }

        public List<Letter> Rack { get; set; } = new List<Letter>(7);
        public int Points { get; set; }
        public List<Word> Words { get; set; } = new List<Word>();

        public string DisplayRack()
        {
            return new string(Rack.Select(r => r.Char).ToArray());
        }


    }
}
