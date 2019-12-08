using DawgResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver.Model
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    public class Word
    {
        public Game Game { get; set; }
        public Word(Game g)
        {
            Game = g;
            StartTile = new Tile(Game, 7, 7);
        }
        public void SetWord(Player p,bool validate)
        {
            this.StartTile.SetWord(p, Text, Direction, validate);
        }
        public bool Scramble { get; set; }
        
        public MovementDirection Direction { get; set; }
        public VTile StartTile { get; set; }
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
                return $"[{pos}] {Text} ({Points}){(Scramble ? "*" : "")}";
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
