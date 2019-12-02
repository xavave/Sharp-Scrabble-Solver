using Dawg;
using DawgResolver;
using System.Linq;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game();

            //Game.Grid[7, 7].Letter = Game.Alphabet.Find(c => c.Char == 'A');
            //Game.Grid[8, 7].Letter = Game.Alphabet.Find(c => c.Char == 'N');
            //Game.Grid[9, 7].Letter = Game.Alphabet.Find(c => c.Char == 'N');
            //Game.Grid[10, 7].Letter = Game.Alphabet.Find(c => c.Char == 'E');
            //Game.Grid[11, 7].Letter = Game.Alphabet.Find(c => c.Char == 'E');
            g.Resolver.NewDraught(g.Player1, "AR*NMES");
            //Game.Grid[7, 7].IsAnchor = true;
            //Game.Grid[8, 7].IsAnchor = true;
            //Game.Grid[9, 7].IsAnchor = true;
            //Game.Grid[10, 7].IsAnchor = true;
            //Game.Grid[11, 7].IsAnchor = true;
            g.Resolver.FindMoves(g.Player1);
        }
    }
}

