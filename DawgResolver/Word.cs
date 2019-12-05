using Dawg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    [Serializable]
    public class Word
    {
        public Game Game { get; set; }
        public Word(Game g)
        {
            Game = g;
            StartTile = new Tile(7, 7);

        }

        public Word Suivant { get; set; }
        public bool Scramble
        {
            get
            {
                return Text.Length >= 7;
            }
        }
        public MovementDirection Direction { get; set; }
        public Tile StartTile { get; set; }
        public int Points
        {
            get; set;
        }
        public string Text { get; set; }

        public string DisplayText
        {
            get
            {
                var pos = $"{Game.Alphabet[StartTile.Ligne]}{StartTile.Col + 1}";
                if (Direction == MovementDirection.Down)
                    pos = $"{StartTile.Col + 1}{Game.Alphabet[StartTile.Ligne]}";
                return $"[{pos}] {Text} ({Points}){(Scramble?"*":"")}";
            }
        }
        public bool IsAllowed
        {
            get
            {
                return Game.Dico.MotAdmis(Text);
            }

        }
        public override string ToString()
        {
            return DisplayText;
        }
    }
}
