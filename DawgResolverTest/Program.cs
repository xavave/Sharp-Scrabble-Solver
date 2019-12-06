using DawgResolver;
using DawgResolver.Model;
using System;
using System.Diagnostics;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game();
            //Pour mes tests 
            var t = g.Grid[7, 5];
            t.SetWord(g, "famille", MovementDirection.Across);
            t.SetWord(g, "foin", MovementDirection.Down);

            g.Bag.GetNewRack(g.Player1, "EUDNA*A");
            
            var sw = Stopwatch.StartNew();
            var ret = g.Resolver.FindMoves(g.Player1);
            sw.Stop();
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
        }
    }
}

