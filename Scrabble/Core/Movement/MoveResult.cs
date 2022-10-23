using System.Collections.Generic;

using DawgResolver.Model;

namespace Scrabble.Core.Movement
{
    public class MoveResult
    {
        public bool Valid { get; set; }
        public List<Word> Words { get; set; }
        public int TotalScore { get; set; }
    }
}
