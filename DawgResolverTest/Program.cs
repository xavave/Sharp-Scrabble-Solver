using Dawg;
using DawgResolver;
using System;
using System.Diagnostics;
using System.Linq;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game();

            g.Grid[(int)Game.BoardSize / 2, (int)Game.BoardSize / 2].Letter = g.Alphabet.Find(c => c.Char == 'A');
            g.Grid[7, 8].Letter = g.Alphabet.Find(c => c.Char == 'A');
            //g.Grid[9, 7].Letter = g.Alphabet.Find(c => c.Char == 'N');
            //g.Grid[10, 7].Letter = g.Alphabet.Find(c => c.Char == 'E');
            //g.Grid[11, 7].Letter = g.Alphabet.Find(c => c.Char == 'E');
            //g.Grid[(int)Game.BoardSize / 2, (int)Game.BoardSize / 2].Letter = g.Alphabet.Find(c => c.Char == 'A');
            //g.Grid[7, 8].Letter = g.Alphabet.Find(c => c.Char == 'N');
            //g.Grid[7, 9].Letter = g.Alphabet.Find(c => c.Char == 'N');
            //g.Grid[7, 10].Letter = g.Alphabet.Find(c => c.Char == 'E');
            //g.Grid[7, 11].Letter = g.Alphabet.Find(c => c.Char == 'E');
            g.Resolver.NewDraught(g.Player1, "EUDNA*A");
            //Game.Grid[7, 7].IsAnchor = true;
            //Game.Grid[8, 7].IsAnchor = true;
            //Game.Grid[9, 7].IsAnchor = true;
            //Game.Grid[10, 7].IsAnchor = true;
            //Game.Grid[11, 7].IsAnchor = true;
            var ret = g.Resolver.FindMoves(g.Player1);
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
        }
    }
}

