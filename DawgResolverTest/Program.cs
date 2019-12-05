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
            //Pour mes tests 
            var t = g.Grid[7, 4];
            t.SetWord(g, "famille", MovementDirection.Across);
           
            

            g.Resolver.NewDraught(g.Player1, "EUDNA*A");
            //Game.Grid[7, 7].IsAnchor = true;
            //Game.Grid[8, 7].IsAnchor = true;
            //Game.Grid[9, 7].IsAnchor = true;
            //Game.Grid[10, 7].IsAnchor = true;
            //Game.Grid[11, 7].IsAnchor = true;
            var sw = Stopwatch.StartNew();
            var ret = g.Resolver.FindMoves(g.Player1);
            sw.Stop();
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
        }
    }
}

