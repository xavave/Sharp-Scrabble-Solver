using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core.Words
{
    public class Word
    {
        public MovementDirection Direction {get;set;}
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public string AdjacentLetter { get; set; }
        public List<ITile> Tiles { get; set; }
        public bool Valid { get; set; }

        public void SetValidHighlight()
        {
            foreach (var t in Tiles)
                t.OnHighlight(Valid);
        }

        public void ClearValidHighlight()
        {
            foreach (var t in Tiles)
                t.ClearHighlight();
        }

        public override string ToString()
        {
            return $"\n{Text} {(!string.IsNullOrWhiteSpace(AdjacentLetter) ? " (" + AdjacentLetter + ")" : "")} (Score : {Score}) (Nb Let:{Text.Length}) StartX:{StartX} StartY:{StartY}";
            //return $"<Word Text={Text}, SX={StartX}, EX={EndX}, SY={StartY}, EY={EndY}, Score={Score} />";
        }

        public override bool Equals(object obj)
        {
            if (obj is Word)
            {
                var word = (Word)obj;
                return word.Text == this.Text && word.StartX == this.StartX && word.StartY == this.StartY && word.EndX == this.EndX && word.EndY == this.EndY;
            }

            return false;
        }
    }
}
