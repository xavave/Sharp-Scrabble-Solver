using DawgResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver.Model
{
    public class Player
    {
        public string Name { get; set; }
        Game Game { get; set; }
        public Player(Game g,string name)
        {
            Game = g;
            Name = name;
        }

        public List<Letter> Rack { get; set; } = new List<Letter>(7);
        public int Points { get; set; }
        public List<Word> Moves { get; set; } = new List<Word>();



    }
}
