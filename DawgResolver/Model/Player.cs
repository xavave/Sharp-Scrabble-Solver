using System.Collections.Generic;
using System.Linq;

namespace DawgResolver.Model
{
    public class Player
    {
        public string Name { get; set; }
        Game Game { get; set; }
        public Player(Game g, string name)
        {
            Game = g;
            Name = name;
        }

        public List<Letter> Rack { get; set; } = new List<Letter>(7);
        public int Points { get; set; }
        public HashSet<Word> Moves { get; set; } = new HashSet<Word>();

        public void SetRackFromWord(string word)
        {
            Rack.Clear();
            Rack.AddRange(word.Select(s => new Letter(Game.Resolver, s, 1, 1)));
        }


        public string ToRackString()
        {
            var ret = "";
            for (int i = 0; i < Rack.Count(); i++)
            {
                ret += Rack[i].Char;
            }
            return ret;
        }

    }
}
