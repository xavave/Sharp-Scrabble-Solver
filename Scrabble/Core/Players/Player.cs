using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<RackTile> Tiles { get; set; }
    }
}
