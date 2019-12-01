using Scrabble.Core.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core.Movement
{
    public class MoveResult
    {
        public bool Valid { get; set; }
        public List<Word> Words { get; set; }
        public int TotalScore { get; set; }
    }
}
